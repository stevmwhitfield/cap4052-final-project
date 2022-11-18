using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour {

    private const int MAX_HP = 5;

    private const float WALK_SPEED = 7.0f;
    private const float RUN_SPEED = 11.0f;
    private const float ACCELERATION = 14.0f;
    private const float JUMP_FORCE = 6.0f;

    public CapsuleCollider hurtbox; // detects incoming combat collisions
    public CapsuleCollider hitbox;  // detects outgoing combat collisions

    public int currentLevelArtifactsCollected;
    public int currentLevelManaCrystalsCollected;

    public bool isPaused;

    private int currentHp;
    private bool isDead;

    private Vector2 movementInput;
    private bool canJump;
    private bool isJumping;

    private bool canAttack;
    private bool isAttacking;

    private float barrierTimer;
    private bool blastRuneCollected;
    private bool smashRuneCollected;
    private bool barrierRuneCollected;

    private int totalArtifactsCollected;
    private int totalManaCrystalsCollected;

    private enum AbilityTypes {
        Basic, Blast, Smash, Barrier
    }
    private AbilityTypes abilityType;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void OnMove(InputAction.CallbackContext c) {
        // get direction vector input
        // call MovementCallback
    }

    public void OnJump(InputAction.CallbackContext c) {
        // get boolean input
        // call JumpCallback
    }

    public void OnAttack(InputAction.CallbackContext c) {
        // get key input
        // call AttackCallback
    }

    private void MoveCallback() {
        // implement moving
    }

    private void JumpCallback() {
        // implement jumping
    }

    private void AttackCallback() {
        // implement attacking
    }

    private void OnCollisionEnter(Collision collision) {
        // check if player hurtbox collides with item
        // check if player hurtbox collides with enemy
    }

    private void ItemCollisionCallback() {
        // determine item type
        // increment number of item type collected
        // delete item instance
    }

    private void EnemyCollisionCallback() {
        // kill enemy
    }
}
