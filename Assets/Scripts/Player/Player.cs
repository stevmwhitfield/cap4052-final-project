using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {


    #region Fields
    private const int MAX_HP = 5;

    private const float WALK_SPEED = 7.0f;
    private const float RUN_SPEED = 11.0f;
    private const float ACCELERATION = 20.0f;
    private const float JUMP_FORCE = 6.0f;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform cameraTarget;

    //[SerializeField] private AudioClip walkSfx;
    //[SerializeField] private AudioClip runSfx;
    //[SerializeField] private AudioClip jumpSfx;
    //[SerializeField] private AudioClip landSfx;
    [SerializeField] private AudioClip basicAttackSfx;
    //[SerializeField] private AudioClip blastAbilitySfx;
    //[SerializeField] private AudioClip barrierAbilitySfx;
    [SerializeField] private AudioClip hurtSfx;
    [SerializeField] private AudioClip deathSfx;

    private AudioSource audioSource;

    private Rigidbody rb;

    private CapsuleCollider hurtbox; // detects incoming combat collisions
    [SerializeField] private CapsuleCollider hitbox;  // detects outgoing combat collisions

    private Animator animator;

    private Vector3 movementDirection;
    private Vector3 lookDirection;

    private int currentHp = MAX_HP;

    private float followCameraYaw = 0f;
    private float followCameraPitch = 0f;
    private float verticalMinClamp = -45f;
    private float verticalMaxClamp = 60f;
    private float cameraSensitivity = 0.1f;
    private float cameraTargetDirection = 0f;
    private float rotationVelocity = 0f;
    private float smoothTime = 0.1f;

    private bool isDead = false;
    private bool isRunning = false;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool blastRuneCollected = false;
    private bool barrierRuneCollected = false;

    private enum AbilityType {
        None, Basic, Blast, Barrier
    }
    private AbilityType currentAbility;
    #endregion

    #region UnityMethods
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (rb == null) {
            throw new System.Exception("Error! " + name + ": missing Rigidbody.");
        }

        animator = GetComponent<Animator>();
        if (animator == null) {
            throw new System.Exception("Error! " + name + ": missing Animator.");
        }

        hurtbox = GetComponent<CapsuleCollider>();
        if (hurtbox == null) {
            throw new System.Exception("Error! " + name + ": missing CapsuleCollider.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            throw new System.Exception("Error! " + name + ": missing AudioSource.");
        }

        if (mainCamera == null) {
            throw new System.Exception("Error! " + name + ": missing MainCamera.");
        }

        animator.SetInteger("Health", currentHp);
    }

    private void Update() {
        if (!GameManager.isPaused) {
            HealthCheck();

            // Animator parameters
            animator.SetFloat("Speed", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
            animator.SetFloat("UpVelocity", rb.velocity.y);
        }
        if (isDead) {
            if (deathSfx != null) {
                audioSource.PlayOneShot(deathSfx);
            }
            else {
                Debug.LogWarning("Warning! " + name + ": is missing deathSfx.");
            }
            // GameManager.RespawnPlayer();
        }
    }

    private void FixedUpdate() {
        if (!GameManager.isPaused && !isDead) {
            MoveCallback();
            if (isJumping) {
                GroundCheck();
                JumpCallback();
            }
            if (isAttacking) {
                //AttackCallback(currentAbility);
            }
        }
    }

    private void LateUpdate() {
        RotateCamera();
    }
    #endregion

    #region InputMethods
    // Read mouse input for camera direction
    public void OnLook(InputAction.CallbackContext c) {
        lookDirection = c.ReadValue<Vector2>();
    }

    // Read movement input to get the direction the player is trying to move in
    public void OnMove(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started || c.phase == InputActionPhase.Performed) {
            Vector2 input = c.ReadValue<Vector2>();
            movementDirection = new Vector3(input.x, 0, input.y);
        }
        if (c.phase == InputActionPhase.Canceled) {
            movementDirection = Vector3.zero;
        }
    }

    // Read mapping input to check if the player is sprinting
    public void OnSprint(InputAction.CallbackContext c) {
        isRunning = c.ReadValueAsButton();
    }

    // Read mapping input to check if the player is jumping
    public void OnJump(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            isJumping = true;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            isJumping = false;
        }
    }

    // Read attack input to determine which attack or ability the player is using
    public void OnBasicAttack(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            isAttacking = true;
            currentAbility = AbilityType.Basic;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            isAttacking = false;
            currentAbility = AbilityType.None;
        }
    }

    public void OnBlastAbility(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            isAttacking = true;
            currentAbility = AbilityType.Blast;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            isAttacking = false;
            currentAbility = AbilityType.None;
        }
    }

    public void OnBarrierAbility(InputAction.CallbackContext c) {
        if (c.phase == InputActionPhase.Started) {
            isAttacking = true;
            currentAbility = AbilityType.Barrier;
        }
        else if (c.phase == InputActionPhase.Canceled) {
            isAttacking = false;
            currentAbility = AbilityType.None;
        }
    }
    #endregion

    #region MovementMethods
    // Use velocity and acceleration to move the player in a direction
    private void MoveCallback() {
        // set speed based on movement input
        float moveSpeed = isRunning ? RUN_SPEED : WALK_SPEED;
        if (movementDirection == Vector3.zero) {
            moveSpeed = 0f;
        }

        // rotate player to match movement
        if (movementDirection != Vector3.zero) {
            cameraTargetDirection = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraTargetDirection, ref rotationVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        // move player with acceleration
        Vector3 direction = Quaternion.Euler(0f, cameraTargetDirection, 0f) * Vector3.forward;
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 newVelocity = direction * moveSpeed;
        Vector3 deltaVelocity = newVelocity - currentVelocity;

        if (newVelocity.magnitude < 0.1f && currentVelocity.magnitude < 0.1f) {
            rb.velocity = Vector3.zero + new Vector3(0, rb.velocity.y, 0);
        }
        else {
            rb.velocity += deltaVelocity.normalized * ACCELERATION * Time.deltaTime;

            // prevent going over move speed when going in diagonal directions
            float currentSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
            if (currentSpeed > moveSpeed) {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized * moveSpeed + new Vector3(0, rb.velocity.y, 0);
            }

            // play walk or run audio
            //if (isRunning) {
            //    if (runSfx != null) {
            //        audioSource.PlayOneShot(runSfx);
            //    }
            //    else {
            //        Debug.LogWarning("Warning! " + name + ": is missing runSfx.");
            //    }
            //}
            //else {
            //    if (walkSfx != null) {
            //        audioSource.PlayOneShot(walkSfx);
            //    }
            //    else {
            //        Debug.LogWarning("Warning! " + name + ": is missing walkSfx.");
            //    }
            //}
        }
    }

    // Use look direction to rotate player's camera
    private void RotateCamera() {
        // get direction for player camera
        if (lookDirection.sqrMagnitude >= 0.01f) {
            followCameraYaw += lookDirection.x * cameraSensitivity;
            followCameraPitch -= lookDirection.y * cameraSensitivity;
        }

        // clamp rotations to values between 0 and 360 degrees
        followCameraYaw = ClampAngle(followCameraYaw, float.MinValue, float.MaxValue);
        followCameraPitch = ClampAngle(followCameraPitch, verticalMinClamp, verticalMaxClamp);

        // rotate camera in desired direction
        cameraTarget.transform.rotation = Quaternion.Euler(followCameraPitch, followCameraYaw, 0);
    }

    // Clamp angles to between 0 and 360 degrees
    private float ClampAngle(float angle, float min, float max) {
        if (angle < -360f) {
            angle += 360f;
        }
        if (angle > 360f) {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }

    // Check if the player is grounded and jump if they are
    private void JumpCallback() {
        if (isGrounded) {
            rb.velocity += new Vector3(0, JUMP_FORCE, 0);
            isGrounded = false;

            animator.SetTrigger("Jump");

            //if (jumpSfx != null) {
            //    audioSource.PlayOneShot(jumpSfx);
            //}
            //else {
            //    Debug.LogWarning("Warning! " + name + ": is missing jumpSfx.");
            //}
        }
    }

    // Cast a ray downwards to check if the player is close to the ground
    private void GroundCheck() {
        if (!isGrounded && rb.velocity.y <= 0) {
            Vector3 origin = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Vector3 direction = transform.up * -1;
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit)) {
                if (hit.distance < 1) {
                    isGrounded = true;
                }
            }
        }
    }
    #endregion

    #region CombatMethods
    // Attack timer to prevent attacking again instantly
    private IEnumerator TimerCoroutine(float duration) {
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    // Activate an attack or ability based on the active ability
    private void AttackCallback(AbilityType ability) {
        // do nothing if the player cannot attack
        if (!canAttack || currentAbility == AbilityType.None) {
            return;
        }

        if (currentAbility == AbilityType.Basic) {
            UseAttack(1.2f, "BasicAttack", basicAttackSfx);
        }

        //if (currentAbility == AbilityType.Blast && blastRuneCollected) {
        //    UseAttack(1.0f, "BlastAbility", blastAbilitySfx);
        //}

        //if (currentAbility == AbilityType.Barrier && barrierRuneCollected) {
        //    UseAttack(1.0f, "BarrierAbility", barrierAbilitySfx);
        //}
    }

    private void UseAttack(float attackTimer, string animation, AudioClip sfx) {
        canAttack = false;

        StartCoroutine(TimerCoroutine(attackTimer));
        animator.SetTrigger(animation);

        if (sfx != null) {
            audioSource.PlayOneShot(sfx);
        }
        else {
            Debug.LogWarning("Warning! " + name + ": is missing " + animation + " sfx.");
        }
    }

    private void HealthCheck() {
        if (currentHp > 0) {
            isDead = false;
        }
        else {
            isDead = true;
        }
    }

    public void TakeDamage() {
        currentHp -= 1;
        animator.SetTrigger("TakeDamage");
        animator.SetInteger("Health", currentHp);

        if (hurtSfx != null) {
            audioSource.PlayOneShot(hurtSfx);
        }
        else {
            Debug.LogWarning("Warning! " + name + ": is missing hurtSfx.");
        }
    }
    #endregion

    #region CollisionMethods
    private void OnCollisionEnter(Collision collision) {
        if (isAttacking) {
            Ray ray = new Ray(hitbox.transform.position, transform.forward);
            RaycastHit hit;
            if (hitbox.Raycast(ray, out hit, 1.5f)) {
                if (hit.collider.gameObject.CompareTag("Enemy")) {
                    EnemyCollisionCallback(hit.collider.gameObject);
                }
            }
        }
    }

    private void EnemyCollisionCallback(GameObject enemy) {
        enemy.GetComponent<Animator>().SetTrigger("TakeDamage");
        enemy.GetComponent<AudioSource>().Play();
        Destroy(enemy);
    }
    #endregion

}
