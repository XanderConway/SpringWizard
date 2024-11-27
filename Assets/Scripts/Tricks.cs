using System;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class Tricks : MonoBehaviour {
    [SerializeField] private float backForce = 40f;
    [SerializeField] private float forwardForce = 40f;
    [SerializeField] private float rightForce = 40f;
    [SerializeField] private float leftForce = 40f;
    [SerializeField] private float upForce = 50f;
    [SerializeField] private float dragForce = 10f;
    [SerializeField] private float baseTrickVelocityMax = 75f;
    [SerializeField] private float baseTrickVelocityMin = -75f;
    [SerializeField] private float specialTrickVelocityMax = 75f;
    [SerializeField] private float specialTrickVelocityMin = -30f;
    private PlayerInputActions playerInputActions;
    private Rigidbody player;
    private bool baseTrickCharged = false;
    private bool specialTrickCharged = false;
    private Coroutine dragResetCoroutine;

    private void Start() {
        player = GetComponent<Rigidbody>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Trick1.Enable();
        playerInputActions.Player.Trick1.performed += OnPerformTrick1;
        playerInputActions.Player.Trick2.Enable();
        playerInputActions.Player.Trick2.performed += OnPerformTrick2;
        playerInputActions.Player.Trick3.Enable();
        playerInputActions.Player.Trick3.performed += OnPerformTrick3;
        playerInputActions.Player.Trick4.Enable();
        playerInputActions.Player.Trick4.performed += OnPerformTrick4;
        playerInputActions.Player.Trick5.Enable();
        playerInputActions.Player.Trick5.performed += OnPerformTrick5;
        playerInputActions.Player.Trick6.Enable();
        playerInputActions.Player.Trick6.performed += OnPerformTrick6;
    }

    private void OnPerformTrick1(InputAction.CallbackContext context) {
        if (CanPerformBaseTrick()) {
            BroadcastMessage("PerformTrick1");
            TriggerBaseTrickSideEffects();
        }
    }

    private void OnPerformTrick2(InputAction.CallbackContext context) {
        if (CanPerformBaseTrick()) {
            BroadcastMessage("PerformTrick2");
            TriggerBaseTrickSideEffects();
        }
    }

    private void OnPerformTrick3(InputAction.CallbackContext context) {
        if (CanPerformBaseTrick()) {
            BroadcastMessage("PerformTrick3");
            TriggerBaseTrickSideEffects();
        }
    }

    private void OnPerformTrick4(InputAction.CallbackContext context) {
        if (CanPerformBaseTrick()) {
            BroadcastMessage("PerformTrick4");
            TriggerBaseTrickSideEffects();
        }
    }

    private void OnPerformTrick5(InputAction.CallbackContext context) {
        if (CanPerformSpecialTrick()) {
            BroadcastMessage("PerformTrick5");
            TriggerSpecialTrickSideEffects();
        }
    }

    private void OnPerformTrick6(InputAction.CallbackContext context) {
        if (CanPerformSpecialTrick()) {
            BroadcastMessage("PerformTrick6");
            TriggerSpecialTrickSideEffects();
        }
    }

    /// <summary>
    /// TODO: disabled for final Demo
    /// </summary>
    // // dash back
    // private void PerformTrick1() {
    //     player.AddForce(player.transform.forward * forwardForce, ForceMode.Impulse);
    // }

    // // dash forward
    // private void PerformTrick2() {
    //     player.AddForce(player.transform.forward * -backForce, ForceMode.Impulse);
    // }

    // // dash right
    // private void PerformTrick3() {
    //     player.AddForce(player.transform.right * rightForce, ForceMode.Impulse);
    // }

    // // dash left
    // private void PerformTrick4() {
    //     player.AddForce(player.transform.right * -leftForce, ForceMode.Impulse);
    // }

    // float up
    private void PerformTrick5() {
        player.AddForce(player.transform.up * upForce, ForceMode.Impulse);
    }

    // slow fall
    private void PerformTrick6() {
        player.drag = dragForce;
        player.AddForce(player.transform.up * upForce, ForceMode.Force);
        dragResetCoroutine = StartCoroutine(ResetDrag());
    }

    private void PerformChargedJump(bool jumpStarted) {
        if (jumpStarted) {
            baseTrickCharged = true;
            specialTrickCharged = true;
        }
    }

    private void PerformGroundedAction() {
        baseTrickCharged = false;
        specialTrickCharged = false;
    }

    private void ActivateSpringboard() {
        baseTrickCharged = true;
        specialTrickCharged = true;
    }

    private bool CanPerformBaseTrick() {
        bool velocityWithinThreshold = baseTrickVelocityMin < player.velocity.y && player.velocity.y < baseTrickVelocityMax;
        bool canPerformTrick = baseTrickCharged && velocityWithinThreshold;
        //return canPerformTrick;
        // TODO
        // disabled for final demo
        return true;
    }

    private bool CanPerformSpecialTrick() {
        bool velocityWithinThreshold = specialTrickVelocityMin < player.velocity.y && player.velocity.y < specialTrickVelocityMax;
        bool canPerformTrick = specialTrickCharged && velocityWithinThreshold;
        return canPerformTrick;
    }

    private void TriggerBaseTrickSideEffects() {
        baseTrickCharged = false;
        if (dragResetCoroutine != null) {
            StopCoroutine(dragResetCoroutine);
            player.drag = 0;
            dragResetCoroutine = null;
        }
    }

    private void TriggerSpecialTrickSideEffects() {}

    private IEnumerator ResetDrag() {
        yield return new WaitForSeconds(2f);
        player.drag = 0;
    }
}