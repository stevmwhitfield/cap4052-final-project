using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {


    #region Fields
    private const int MAX_HP = 10;

    private const float WALK_SPEED = 7.0f;
    private const float RUN_SPEED = 11.0f;
    private const float ACCELERATION = 50.0f;
    private const float JUMP_FORCE = 9.0f;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform attackPoint;

    [SerializeField] private AudioClip basicAttackSfx;
    [SerializeField] private AudioClip hurtSfx;
    [SerializeField] private AudioClip deathSfx;

    [SerializeField] private CapsuleCollider hitbox;

    [SerializeField] private LayerMask enemyLayers;

    private AudioSource audioSource;

    private Rigidbody rb;

    private CapsuleCollider hurtbox;

    private Animator animator;

    private InputController input;

    private int currentHp = MAX_HP;

    private float followCameraYaw = 0f;
    private float followCameraPitch = 0f;
    private float verticalMinClamp = -45f;
    private float verticalMaxClamp = 60f;
    private float cameraSensitivity = 0.3f;
    private float cameraTargetDirection = 0f;
    private float rotationVelocity = 0f;
    private float smoothTime = 0.1f;

    //private bool isDead = false;
    private bool isGrounded = false;
    private bool canAttack = true;
    private bool blastRuneCollected = false;
    private bool barrierRuneCollected = false;
    #endregion

    #region UnityMethods
    private void Awake() {
        input = GetComponent<InputController>();
        cameraSensitivity = GameManager.playerSettings.sensitivity;
    }

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

    }

    private void Update() {
        if (!PauseController.IsPaused) {
            if (GameManager.playerSettings.sensitivity == 0) {
                cameraSensitivity = 0.3f;
            }
            else {
                cameraSensitivity = GameManager.playerSettings.sensitivity;
            }

            HealthCheck();

        }

        if (input.IsAttacking) {
            AttackCallback(input.CurrentAbility);
        }
    }

    private void FixedUpdate() {
        if (!PauseController.IsPaused) {
            animator.SetFloat("Speed", new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude);
            animator.SetFloat("UpVelocity", rb.velocity.y);

            MoveCallback();

            if (input.IsJumping) {
                GroundCheck();
                JumpCallback();
            }
        }
    }

    private void LateUpdate() {
        RotateCamera();
    }
    #endregion

    #region MovementMethods
    // Use velocity and acceleration to move the player in a direction
    private void MoveCallback() {
        // set speed based on movement input
        float moveSpeed = input.IsRunning ? RUN_SPEED : WALK_SPEED;
        if (input.MovementDirection == Vector3.zero) {
            moveSpeed = 0f;
        }

        // rotate player to match movement
        if (input.MovementDirection != Vector3.zero) {
            cameraTargetDirection = Mathf.Atan2(input.MovementDirection.x, input.MovementDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
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
            //if (input.IsRunning) {
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
        if (input.LookDirection.sqrMagnitude >= 0.01f) {
            followCameraYaw += input.LookDirection.x * cameraSensitivity;
            followCameraPitch -= input.LookDirection.y * cameraSensitivity;
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
    private IEnumerator AttackBuffer(float duration) {
        yield return new WaitForSeconds(duration);
        canAttack = true;
    }

    // Activate an attack or ability based on the active ability
    private void AttackCallback(AbilityType ability) {
        // do nothing if the player cannot attack
        if (!canAttack || input.CurrentAbility == AbilityType.None) {
            return;
        }

        float buffer = 1.5f;
        string animation = "";
        AudioClip sfx = null;
        float attackRange = 0.5f;
        Vector3 attackPosition = attackPoint.position;

        if (input.CurrentAbility == AbilityType.Basic) {
            animation = "BasicAttack";
            sfx = basicAttackSfx;
            attackRange = 1.0f;
            attackPosition = attackPoint.position;
        }

        if (input.CurrentAbility == AbilityType.Blast && blastRuneCollected) {
            animation = "BlastAbility";
            sfx = basicAttackSfx;
            attackRange = 2.0f;
            attackPosition = attackPoint.position + attackPoint.forward * 1.5f;
        }

        UseAttack(buffer, animation, sfx, attackPosition, attackRange);
    }

    private void UseAttack(float attackTimer, string animation, AudioClip sfx, Vector3 position, float range) {
        canAttack = false;

        StartCoroutine(AttackBuffer(attackTimer));
        animator.SetTrigger(animation);

        if (sfx != null) {
            audioSource.PlayOneShot(sfx);
        }
        else {
            Debug.LogWarning("Warning! " + name + ": is missing " + animation + " sfx.");
        }

        Collider[] enemiesHit = Physics.OverlapSphere(position, range, enemyLayers);

        foreach (Collider enemy in enemiesHit) {
            enemy.gameObject.GetComponent<BaseEnemy>().Die();
        }
    }

    private void HealthCheck() {
        if (currentHp <= 0) {
            Die();
        }
    }

    private void Die() {
        animator.SetBool("IsDead", true);
        if (deathSfx != null) {
            audioSource.PlayOneShot(deathSfx);
        }
        else {
            Debug.LogWarning("Warning! " + name + ": is missing deathSfx.");
        }
        // GameManager.RespawnPlayer();
    }

    public void TakeDamage() {
        currentHp -= 1;
        animator.SetTrigger("TakeDamage");

        if (hurtSfx != null) {
            audioSource.PlayOneShot(hurtSfx);
        }
        else {
            Debug.LogWarning("Warning! " + name + ": is missing hurtSfx.");
        }
    }
    #endregion
}
