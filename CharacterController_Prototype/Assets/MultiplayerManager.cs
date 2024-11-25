using Cinemachine;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    public List<PlayerInput> players = new List<PlayerInput>();
    public GameObject[] playerPrefabs = new GameObject[4];


    LayerMask p1OcclusionMask;
    LayerMask p2OcclusionMask;
    LayerMask p3OcclusionMask;
    LayerMask p4OcclusionMask;

    private void Start()
    {
        p1OcclusionMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Item", "Water", "UI", "Pole", "Obstacle", "Vehicle", "Ignore", "Player1", "Player1UI");
        p2OcclusionMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Item", "Water", "UI", "Pole", "Obstacle", "Vehicle", "Ignore", "Player2", "Player2UI");
        p3OcclusionMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Item", "Water", "UI", "Pole", "Obstacle", "Vehicle", "Ignore", "Player3", "Player3UI");
        p4OcclusionMask = LayerMask.GetMask("Default", "TransparentFX", "Ignore Raycast", "Item", "Water", "UI", "Pole", "Obstacle", "Vehicle", "Ignore", "Player4", "Player4UI");
    }

    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
    }

    private void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        PlayerInputManager.instance.onPlayerLeft -= OnPlayerLeft;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        players.Add(playerInput);

        GameObject player = playerInput.gameObject;

        player = cullingMaskApplier(player, playerInput.playerIndex);

        playerInputManager.playerPrefab = playerPrefabs[playerInput.playerIndex + 1];

        Debug.Log($"Player joined: {playerInput.playerIndex}");
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        players.Remove(playerInput);
        Debug.Log($"Player left: {playerInput.playerIndex}");
    }

    private GameObject cullingMaskApplier(GameObject player, int playerId)
    {

        GameObject playerCam = player.GetComponent<CameraHolders>().carCamera;
        GameObject playerCine = player.GetComponent<CameraHolders>().carCinemachine;

        if (playerId == 0)
        {
            playerCam.GetComponent<Camera>().cullingMask = p1OcclusionMask;
            playerCam.layer = 10;
            playerCine.layer = 10;
        }

        if(playerId == 1) 
        {
            playerCam.GetComponent<Camera>().cullingMask = p2OcclusionMask;
            playerCam.layer = 11;
            playerCine.layer = 11;
        }

        if (playerId == 2)
        {
            playerCam.GetComponent<Camera>().cullingMask = p3OcclusionMask;
            playerCam.layer = 12;
            playerCine.layer = 12;
        }

        if (playerId == 3)
        {
            playerCam.GetComponent<Camera>().cullingMask = p4OcclusionMask;
            playerCam.layer = 14;
            playerCine.layer = 14;
        }

        return player;
    }
}