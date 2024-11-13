using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerInputActions playerInputActions;
    private Vector2 leanInputVector;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Lean.Enable();
    }

    private void Update() {
        leanInputVector = playerInputActions.Player.Lean.ReadValue<Vector2>();
        float forwardInput = leanInputVector.y;
        float sideInput = leanInputVector.x;

        // Truncate inputs once for repeated comparisons
        float truncatedForward = (float)Math.Truncate(forwardInput * 10) / 10;
        float truncatedSide = (float)Math.Truncate(sideInput * 10) / 10;

        animator.SetBool("IsLeaningBackAndRight", truncatedForward == -0.7f && truncatedSide == 0.7f);
        animator.SetBool("IsLeaningBackAndLeft", truncatedForward == -0.7f && truncatedSide == -0.7f);

        animator.SetBool("IsLeaningRight", sideInput == 1);
        animator.SetBool("IsLeaningLeft", sideInput == -1);
        animator.SetBool("IsLeaningFront", forwardInput == 1);
        animator.SetBool("IsLeaningBack", forwardInput == -1);
    }

    void PlayTrick1Animation() {
        animator.SetTrigger("NoHandsTrick2Trigger");
    }

    void PlayTrick2Animation() {
        animator.SetTrigger("PogoKickFlipTrigger");
    }

    void PlayTrick3Animation() {
        animator.SetTrigger("ScissorKickTrigger");
    }

    void PlayTrick4Animation() {
        animator.SetTrigger("NoHandsTrick1Trigger");
    }

    void PlayRailGrindingAnimation(bool isRailGrinding) {
        if (isRailGrinding) {
            animator.SetBool("IsRailGrinding", true);
            animator.Play("rail grind part 1");
        } else {
            animator.SetBool("IsRailGrinding", false);
        }
    }

    void PlayChargingJumpAnimation(bool isChargingJump) {
        if (isChargingJump) {
            animator.SetBool("IsChargingJump", true);
            animator.Play("Charging Jump");
        } else {
            animator.SetBool("IsChargingJump", false);
        }
    }
}
