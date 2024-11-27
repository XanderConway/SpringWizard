using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    private Animator animator;
    private PlayerInputActions playerInputActions;
    private Vector2 leanInputVector;
    private PogoControls pogoControlsScript;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        pogoControlsScript = GetComponentInParent<PogoControls>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Lean.Enable();
    }

    private void Update() {
        if (pogoControlsScript.IsDead()) {
            animator.enabled = false;
            return;
        } else {
            animator.enabled = true;
        }

        leanInputVector = playerInputActions.Player.Lean.ReadValue<Vector2>();
        float forwardInput = leanInputVector.y;
        float sideInput = leanInputVector.x;

        float truncatedForward = (float)Math.Truncate(forwardInput * 10) / 10;
        float truncatedSide = (float)Math.Truncate(sideInput * 10) / 10;

        animator.SetBool("IsLeaningBackAndRight", truncatedForward == -0.7f && truncatedSide == 0.7f);
        animator.SetBool("IsLeaningBackAndLeft", truncatedForward == -0.7f && truncatedSide == -0.7f);

        animator.SetBool("IsLeaningRight", sideInput == 1);
        animator.SetBool("IsLeaningLeft", sideInput == -1);
        animator.SetBool("IsLeaningFront", forwardInput == 1);
        animator.SetBool("IsLeaningBack", forwardInput == -1);
        Debug.Log(animator.GetBool("IsLeaningBack"));
    }

    private void PerformTrick1() {
        animator.SetTrigger("Hands Off Trigger");
    }

    private void PerformTrick2() {
        animator.SetTrigger("Kickflip Trigger");
    }

    private void PerformTrick3() {
        animator.SetTrigger("Scissor Kick Right Trigger");
    }

    private void PerformTrick4() {
        animator.SetTrigger("Handless Bar Spin Trigger");
    }

    private void PerformTrick5() {
        animator.SetTrigger("NoHandsTrick1Trigger");
    }

    private void PerformTrick6() {
        animator.SetTrigger("PogoKickFlipTrigger");
    }

    private void PlayRailGrindingAnimation(bool isRailGrinding) {
        if (isRailGrinding) {
            int randomIndex = UnityEngine.Random.Range(0, 2);
            if (randomIndex == 0){
                animator.Play("Rail Grinding Start 0");
            } else {
                animator.Play("Rail Grinding Start 1");
            }
            animator.SetBool("IsRailGrinding", true);
        } else {
            animator.SetBool("IsRailGrinding", false);
        }
    }

    private void PerformChargedJump(bool isChargingJump) {
        if (isChargingJump) {
            animator.SetBool("IsChargingJump", true);
            animator.Play("Charging Jump");
        } else {
            animator.SetBool("IsChargingJump", false);
        }
    }
}