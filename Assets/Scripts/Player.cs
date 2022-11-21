using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    private const int MAX_HP = 5;

    private const float WALK_SPEED = 7.0f;
    private const float RUN_SPEED = 11.0f;
    private const float ACCELERATION = 14.0f;
    private const float GROUND_DRAG = 5.0f;
    private const float JUMP_FORCE = 6.0f;
    private const float BARRIER_TIMER = 1.0f;

    [SerializeField] private CapsuleCollider hurtbox; // detects incoming combat collisions
    [SerializeField] private CapsuleCollider hitbox;  // detects outgoing combat collisions

    [SerializeField] private Camera followCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform orientation;

    private Rigidbody rb;

    private Vector3 movementDirection;

    private int currentHp = MAX_HP;

    private float cameraRotationSpeed = 7.0f;

    private bool isPaused = false;
    private bool isDead = false;

    // private bool isWalking;
    // private bool isRunning;
    private bool isGrounded = false;
    private bool isJumping = false;

    private bool canAttack = false;
    private bool isAttacking = false;

    private bool blastRuneCollected;
    private bool smashRuneCollected;
    private bool barrierRuneCollected;

    // move to game manager
    // private int totalArtifactsCollected;
    // private int currentLevelArtifactsCollected;
    // private int totalManaCrystalsCollected;
    // private int currentLevelManaCrystalsCollected;

    private enum AbilityTypes {
        Basic, Blast, Smash, Barrier
    }
    private AbilityTypes abilityType;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (rb == null) {
            throw new System.Exception(name + " does not have Rigidbody!");
        }
    }

    // Update is called once per frame
    void Update() {
        if (!isPaused) {
            MoveCallback();
            if (isJumping) {
                JumpCallback();
            }
            if (isAttacking) {
                AttackCallback();
            }
        }

        Debug.DrawRay(transform.position, transform.forward * 15, Color.red);
        Debug.DrawRay(playerTransform.transform.position, playerTransform.transform.forward * 10, Color.green);
        Debug.DrawRay(rb.position, movementDirection * 5, Color.blue);
    }

    // Read movement input to get the direction the player is trying to move in
    public void OnMove(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started || c.phase == InputActionPhase.Performed) {
            Vector2 input = c.ReadValue<Vector2>();
            movementDirection = new Vector3(input.x, 0, input.y) + transform.forward;
        }
        if (c.phase == InputActionPhase.Canceled) {
            movementDirection = Vector3.zero;
        }
    }

    // Read jump input to determine if the player is attempting to jump
    public void OnJump(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            isJumping = true;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            isJumping = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext c) {
        // get key input
        // call AttackCallback
    }

    // Use velocity and acceleration to move the player in a direction
    private void MoveCallback() {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 newVelocity = movementDirection * WALK_SPEED;
        Vector3 deltaVelocity = newVelocity - currentVelocity;

        if (newVelocity.magnitude < 0.1f && currentVelocity.magnitude < 0.1f) {
            rb.velocity = Vector3.zero + new Vector3(0, rb.velocity.y, 0);
        }
        else {
            rb.velocity += deltaVelocity.normalized * ACCELERATION * Time.deltaTime;

            float currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
            if (currentSpeed > WALK_SPEED) {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * WALK_SPEED + new Vector3(0, rb.velocity.y, 0);
            }

            Vector3 lookDirection = transform.position - new Vector3(followCamera.transform.position.x, transform.position.y, followCamera.transform.position.z);
            orientation.forward = lookDirection.normalized;

            Vector3 inputDirection = orientation.forward * movementDirection.z + orientation.right * movementDirection.x;
            inputDirection = inputDirection.normalized;

            if (inputDirection != Vector3.zero) {
                rb.transform.forward = inputDirection;
                playerTransform.forward = Vector3.Slerp(playerTransform.forward, inputDirection, Time.deltaTime * cameraRotationSpeed);
            }
        }

        
    }

    // Check if the player is grounded and jump if they are
    private void JumpCallback() {
        if (!isGrounded && rb.velocity.y <= 0) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up * -1, out hit)) {
                // test and adjust distance
                if (hit.distance < 1.1) {
                    isGrounded = true;
                }
            }
        }

        if (isGrounded) {
            rb.velocity += new Vector3(0, JUMP_FORCE, 0);
            isGrounded = false;
        }
    }

    private void AttackCallback() {
        // implement attacking
    }

    private void OnCollisionEnter(Collision collision) {
        // check if player hurtbox collides with item
        // check if player hurtbox collides with enemy
        if (collision.gameObject.CompareTag("Item")) {
            ItemCollisionCallback(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy")) {
            EnemyCollisionCallback(collision.gameObject);
        }
    }

    private void ItemCollisionCallback(GameObject item) {
        // if (item.prefab == "Artifact") {
        //   GameManager.CollectArtifact(item);
        // }

        // if (item.prefab == "ManaCrystal") {
        //   GameManager.CollectManaCrystal(item);
        // }

        // if (item.name == "BlastRune") {
        //   GameManager.CollectBlastRune(item);
        // }
        // if (item.name == "SmashRune") {
        //   GameManager.CollectBlastRune(item);
        // }
        // if (item.name == "BarrierRune") {
        //   GameManager.CollectBlastRune(item);
        // }

        // Destroy(item);
    }

    private void EnemyCollisionCallback(GameObject enemy) {
        // kill enemy
        // enemy.die();
    }
}
