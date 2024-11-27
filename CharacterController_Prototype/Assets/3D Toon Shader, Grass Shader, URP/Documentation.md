3D Toon Shader, Grass Shader, URP
=================================

diff

This version is for URP V14.0.10+ (Unity 2022.3+).

### Overview:

This package provides shaders for achieving stylized 3D effects, including a toon shader, grass shader, and various sub shaders, compatible with Unity's Universal Render Pipeline (URP).

### Package contents:

*   **GrassShader**: A shader for creating realistic grass effects.
*   **InvisibleShader**: A shader for rendering invisible objects with dynamic effects.
*   **Original**: A shader utilizing Unity Render Pipeline Lit for basic rendering.
*   **ToonShaderLit**: A toon shader for lit rendering.
*   **ToonShaderUnlit**: A toon shader for unlit rendering.
*   **ColorPrecision**: A sub shader for adjusting color precision.
*   **SubToonShader**: A sub shader for additional toon shader effects.
*   **CustomOutline**: A render pass Outline Edge Detection URP.
*   **NoirShader**: A shader with noise and hard shadows.

### Installation instructions:

*   Install via Unity Package Manager.

### Requirements:

*   Unity version 2022.3 or higher.
*   Universal Render Pipeline (URP) version 14.0.10 or higher.

### Limitations:

*   CustomOutline does not work with GrassShader


### Workflows:

*   **GrassShader Workflow**:
    
    1.  Apply the GrassShader to desired objects.
    2.  Adjust properties such as texture, alpha clip, and wind strength for desired effect.
*   **ToonShader Workflow**:
    
    1.  Choose either ToonShaderLit or ToonShaderUnlit based on rendering needs.
    2.  Apply the shader to objects requiring toon shading.
    3.  Adjust properties like base color, smoothness, and outline width to achieve desired stylized effect.

*   **CustomOutline Workflow**:
    
    1.  Enable depth texture in the Universal Render Pipeline Asset
    2.  Add the 'Screen Space Outlines' renderer feature to the Universal Renderer Data
    3.  Go to Project Settings > Graphics and add the "CustomOutline" and "ViewSpaceNormals" to the list of Always Included Shaders.

### Advanced topics:

*   Advanced users can explore the customization options within each shader.
*   Sub shaders like ColorPrecision and SubToonShader offer additional control over specific rendering aspects.

### Reference:

*   For detailed property descriptions, refer to the shader properties listed in each shader's section.

### Samples:

*   This package does include sample ExampleScene, with one example of each shader

### Tutorials:

*   Currently, no tutorials are available. However, the provided workflows should guide users in utilizing the shaders effectively.