using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RespawnSystem: MonoBehaviour
{
    public PogoControls player;

    public Rotate camControls;

    public Transform respawnPointParent;

    private PlayerInputActions playerInputActions;

    public bool drawRespawnLocations;
    public float respawnRad = 5f;

    private bool enableRespawn;

    public float respawnDelay = 1.0f;
    private float respawnTimer = 0f;
    private bool resPressed = false;

    private DeathData currentDeathData = null;

    public void Start()
    {
        player.getDeathEvent().AddListener(setEnableRepsawnUI);

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Respawn.performed += respawnPressed;
        playerInputActions.Player.Respawn.Enable();
    }

    private void respawnPressed(InputAction.CallbackContext obj)
    {
        if(enableRespawn && respawnTimer > respawnDelay)
        {
            resPressed = true;
        }
    }

    private void setEnableRepsawnUI(DeathData data)
    {
        resPressed = false;
        enableRespawn = true;
        currentDeathData = data;
        respawnTimer = 0.0f;
    }

    void LateUpdate()
    {
        if (enableRespawn)
        {
            respawnTimer += Time.deltaTime;

            if(respawnTimer > respawnDelay)
            {
                //if(respawnText != null)
                //{
                //    respawnText.SetActive(true);
                //}
            }

            // Choose the closest respawn point to the players last grounded position
            if(respawnTimer > respawnDelay && respawnPointParent && respawnPointParent.childCount > 0)
            {

                Vector3 respawnPos = currentDeathData.deathPosition;
                // If they fall of the map, bring them to a checkpoint
                if (currentDeathData.fellToDeath)
                {
                    Transform closestPoint = respawnPointParent.GetChild(0);
                    //float minDistance = (currentDeathData.lastGroundedPosition - closestPoint.position).magnitude;
                    float minDistance = (currentDeathData.deathPosition - closestPoint.position).magnitude;

                    for (int i = 1; i < respawnPointParent.childCount; i++)
                    {
                        float distance = (currentDeathData.deathPosition - respawnPointParent.GetChild(i).position).magnitude;
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestPoint = respawnPointParent.GetChild(i);
                        }
                    }

                    respawnPos = closestPoint.transform.position;
                }
                player.setDead(false);
                player.transform.position = respawnPos;
                player.transform.rotation = Quaternion.identity;

                player.pogoStick.rotation = Quaternion.identity;

                camControls.updateCameraPosition();
                CinemachineCore.Instance.GetActiveBrain(0).ManualUpdate();

                //respawnText.SetActive(false);
                enableRespawn = false;

            }

 
        }
    }


    private void OnDrawGizmos()
    {
        if(drawRespawnLocations)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < respawnPointParent.childCount; i++)
            {
                Transform pos = respawnPointParent.GetChild(i);
                Gizmos.DrawSphere(pos.position, respawnRad);
            }
        }
    }
}
