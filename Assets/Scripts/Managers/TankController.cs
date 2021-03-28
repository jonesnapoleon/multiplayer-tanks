using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TankController : NetworkBehaviour
{
    // Server and Client Side
    [Header("Server and Client Side")]
    [SyncVar] public Color m_PlayerColor;                             // This is the color this tank will be tinted.
    [HideInInspector] [SyncVar(hook = nameof(Setup))] public int m_PlayerNumber;  // This specifies which player this the manager for.
    
    // Server Side
    [Header("Server Side")]
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.

    
    // Client Side
    [Header("Client Side")]
    [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
    public Vector3 m_SpawnPosition;                         // Received from server
    public Quaternion m_SpawnRotation;                      // Received from server
    [SyncVar(hook = nameof(SetControl))] public bool m_ControlEnabled = false;                   // 


    private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
    private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
    private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.


    // Get spawn point from server
    [ClientRpc]
    public void RpcSetSpawnPoint(Vector3 position, Quaternion rotation)
    {
        m_SpawnPosition = position;
        m_SpawnRotation = rotation;
    }

    [Client]
    public void Setup(int oldPlayerNumber, int newPlayerNumber)
    {
        // Get references to the components.
        m_Movement = GetComponent<TankMovement>();
        m_Shooting = GetComponent<TankShooting>();
        m_CanvasGameObject = GetComponentInChildren<Canvas>().gameObject;

        // Set the player numbers to be consistent across the scripts.
        m_Movement.m_PlayerNumber = newPlayerNumber;
        m_Shooting.m_PlayerNumber = newPlayerNumber;

        // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + newPlayerNumber + "</color>";

        // Get all of the renderers of the tank.
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        // Go through all the renderers...
        for (int i = 0; i < renderers.Length; i++)
        {
            // ... set their material color to the color specific to this tank.
            renderers[i].material.color = m_PlayerColor;
        }

        // Set default controls
        m_Movement.enabled = false;
        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    // Used during the phases of the game where the player should or shouldn't be able to control their tank.
    [Client]
    public void SetControl(bool oldControl, bool newControl)
    {
        m_Movement.enabled = newControl;
        m_Shooting.enabled = newControl;

        m_CanvasGameObject.SetActive(newControl);
    }

    // Used at the start of each round to put the tank into it's default state.
    [ClientRpc]
    public void RpcReset()
    {
        gameObject.transform.position = m_SpawnPosition;
        gameObject.transform.rotation = m_SpawnRotation;

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    [ClientRpc]
    public void RpcSetCamera()
    {
        // camera for local player
        if (!isLocalPlayer) return;

        CameraControl camera = ((GameManager)NetworkManager.singleton).m_CameraControl;

        Transform[] targets = { transform };
        camera.m_Targets = targets;
    }
}
