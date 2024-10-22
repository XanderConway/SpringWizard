using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RespawnSystem: MonoBehaviour
{
    public PogoControls player;

    public Transform respawnPointParent;

    private PlayerInputActions playerInputActions;

    public GameObject respawnText;

    public bool enableRespawn;

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

    void Update()
    {
        if (enableRespawn)
        {
            respawnTimer += Time.deltaTime;

            if(respawnTimer > respawnDelay)
            {
                if(respawnText != null)
                {
                    respawnText.SetActive(true);
                }
            }

            // Choose the closest respawn point to the players last grounded position
            if(resPressed && respawnPointParent && respawnPointParent.childCount > 0)
            {

                Transform closestPoint = respawnPointParent.GetChild(0);
                float minDistance = (currentDeathData.lastGroundedPosition - closestPoint.position).magnitude;

                for(int i = 1; i < respawnPointParent.childCount; i++)
                {
                    float distance = (currentDeathData.lastGroundedPosition - respawnPointParent.GetChild(i).position).magnitude;
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        closestPoint = respawnPointParent.GetChild(i);
                    }
                }

                player.setDead(false);
                player.transform.position = closestPoint.position;
                player.transform.rotation = closestPoint.rotation;
                player.pogoStick.rotation = Quaternion.identity;

                respawnText.SetActive(false);
                enableRespawn = false;

            }

 
        }
    }




}
