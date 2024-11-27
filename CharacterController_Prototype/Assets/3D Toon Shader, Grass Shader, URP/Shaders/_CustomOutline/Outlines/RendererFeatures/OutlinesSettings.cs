// This shader is based on the original shader by Robin Seibold, modified by [Bruno do Carmo Melicio].
// The original version can be found at [https://github.com/Robinseibold/Unity-URP-Outlines/tree/main].

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace OutlineShader
{
    public class OutlinesSettings : ScriptableRendererFeature
    {

        [System.Serializable]
        private class OutlineConfig
        {

            [Header("Outline")]
            public bool outlineLines;
            public bool isMoving;

            [Header("Outline")]
            public Color outlineColor = Color.black;
            [Range(0.0f, 20.0f)]
            public float outlineThickness = 1.0f;

            [Header("Outline Moving")]
            [Range(0.1f, 150f)]
            public float outlineMoveScale = 34f;

            [Range(0.0f, 1f)]
            public float outlineSpeed = 0.08f;

            [Header("Normal")]
            [Range(0.0f, 2.0f)]
            public float normalEdgeThreshold = 0.3f;

            [Header("Scene View")]
            public RenderTextureFormat textureFormat;
            public int depthBits;
            public FilterMode textureFilterMode;
            public Color clearColor = Color.clear;

            public PerObjectData objectData;
            public bool dynamicBatching;
            public bool instancing;
        }

        private class OutlineRenderPass : ScriptableRenderPass
        {

            private readonly Material outlineMaterial;
            private OutlineConfig config;

            private FilteringSettings filterSettings;

            private readonly List<ShaderTagId> shaderTagIds;
            private readonly Material normalMaterial;

            private RTHandle normalTexture;
            private RendererList normalRendererList;

            RTHandle tempTexture;

            public OutlineRenderPass(RenderPassEvent passEvent, LayerMask layerMask,
                OutlineConfig config)
            {
                this.config = config;
                this.renderPassEvent = passEvent;

                outlineMaterial = new Material(Shader.Find("Hidden/CustomOutline"));
                outlineMaterial.SetColor("_OutlineColor", config.outlineColor);
                outlineMaterial.SetFloat("_OutlineThickness", config.outlineThickness);

                outlineMaterial.SetFloat("_NormalEdgeThreshold", config.normalEdgeThreshold);
                outlineMaterial.SetFloat("_OutlineSpeed", config.outlineSpeed);
                outlineMaterial.SetFloat("_OutlineMoveScale", config.outlineMoveScale);
                outlineMaterial.SetInt("_IsMoving", System.Convert.ToInt32(config.isMoving));
                outlineMaterial.SetInt("_OutlineLines", System.Convert.ToInt32(config.outlineLines));

                filterSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);

                shaderTagIds = new List<ShaderTagId> {
                new ShaderTagId("UniversalForward"),
                new ShaderTagId("UniversalForwardOnly"),
                new ShaderTagId("LightweightForward"),
                new ShaderTagId("SRPDefaultUnlit")
            };

                normalMaterial = new Material(Shader.Find("Hidden/ViewSpaceNormals"));
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                RenderTextureDescriptor textureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                textureDescriptor.colorFormat = config.textureFormat;
                textureDescriptor.depthBufferBits = config.depthBits;
                RenderingUtils.ReAllocateIfNeeded(ref normalTexture, textureDescriptor, config.textureFilterMode);

                textureDescriptor.depthBufferBits = 0;
                RenderingUtils.ReAllocateIfNeeded(ref tempTexture, textureDescriptor, FilterMode.Bilinear);

                ConfigureTarget(normalTexture, renderingData.cameraData.renderer.cameraDepthTargetHandle);
                ConfigureClear(ClearFlag.Color, config.clearColor);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (!outlineMaterial || !normalMaterial ||
                    renderingData.cameraData.renderer.cameraColorTargetHandle.rt == null || tempTexture.rt == null)
                    return;

                CommandBuffer cmd = CommandBufferPool.Get();
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                DrawingSettings drawSettings = CreateDrawingSettings(shaderTagIds, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
                drawSettings.perObjectData = config.objectData;
                drawSettings.enableDynamicBatching = config.dynamicBatching;
                drawSettings.enableInstancing = config.instancing;
                drawSettings.overrideMaterial = normalMaterial;

                RendererListParams normalRendererParams = new RendererListParams(renderingData.cullResults, drawSettings, filterSettings);
                normalRendererList = context.CreateRendererList(ref normalRendererParams);
                cmd.DrawRendererList(normalRendererList);

                cmd.SetGlobalTexture(Shader.PropertyToID("_SceneViewSpaceNormals"), normalTexture.rt);

                using (new ProfilingScope(cmd, new ProfilingSampler("ScreenSpaceOutlines")))
                {
                    Blitter.BlitCameraTexture(cmd, renderingData.cameraData.renderer.cameraColorTargetHandle, tempTexture, outlineMaterial, 0);
                    Blitter.BlitCameraTexture(cmd, tempTexture, renderingData.cameraData.renderer.cameraColorTargetHandle);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            public void DisposeResources()
            {
                CoreUtils.Destroy(outlineMaterial);
                CoreUtils.Destroy(normalMaterial);
                normalTexture?.Release();
                tempTexture?.Release();
            }
        }

        [SerializeField] private RenderPassEvent passEvent = RenderPassEvent.BeforeRenderingSkybox;
        [SerializeField] private LayerMask layerMask;

        [SerializeField] private OutlineConfig outlineConfig = new OutlineConfig();

        private OutlineRenderPass outlineRenderPass;

        public override void Create()
        {
            if (passEvent < RenderPassEvent.BeforeRenderingPrePasses)
                passEvent = RenderPassEvent.BeforeRenderingPrePasses;

            outlineRenderPass = new OutlineRenderPass(passEvent, layerMask, outlineConfig);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(outlineRenderPass);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                outlineRenderPass?.DisposeResources();
            }
        }
    }
}