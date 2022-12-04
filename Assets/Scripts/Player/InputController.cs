using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {

    #region Fields
    public AbilityType CurrentAbility { get; private set; }

    public Vector3 LookDirection { get; private set; }
    public Vector3 MovementDirection { get; private set; }

    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsAttacking { get; private set; }
    #endregion

    #region Public Methods
    // Read mouse input for camera direction
    public void OnLook(InputAction.CallbackContext c) {
        LookDirection = c.ReadValue<Vector2>();
    }

    // Read movement input to get the direction the player is trying to move in
    public void OnMove(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started || c.phase == InputActionPhase.Performed) {
            Vector2 input = c.ReadValue<Vector2>();
            MovementDirection = new Vector3(input.x, 0, input.y);
        }
        if (c.phase == InputActionPhase.Canceled) {
            MovementDirection = Vector3.zero;
        }
    }

    // Read mapping input to check if the player is sprinting
    public void OnSprint(InputAction.CallbackContext c) {
        IsRunning = c.ReadValueAsButton();
    }

    // Read mapping input to check if the player is jumping
    public void OnJump(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            IsJumping = true;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            IsJumping = false;
        }
    }

    // Read attack input to determine which attack or ability the player is using
    public void OnBasicAttack(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            IsAttacking = true;
            CurrentAbility = AbilityType.Basic;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            IsAttacking = false;
            CurrentAbility = AbilityType.None;
        }
    }

    public void OnBlastAbility(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            IsAttacking = true;
            CurrentAbility = AbilityType.Blast;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            IsAttacking = false;
            CurrentAbility = AbilityType.None;
        }
    }

    public void OnBarrierAbility(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            IsAttacking = true;
            CurrentAbility = AbilityType.Barrier;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            IsAttacking = false;
            CurrentAbility = AbilityType.None;
        }
    }

    // Read pause input to pause or resume game
    public void OnPause(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            if (PauseController.IsPaused) {
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<PauseController>().ResumeGame();
            }
            if (!PauseController.IsPaused) {
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<PauseController>().PauseGame();
            }
        }
    }
    #endregion
}
