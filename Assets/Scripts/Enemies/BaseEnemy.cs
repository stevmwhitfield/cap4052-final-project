using System.Collections;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {

    #region Fields
    private const float VERTICAL_LIMIT = 1.8f;

    [SerializeField] private AudioClip attackSfx;
    private AudioSource audioSource;

    private Transform playerTransform;

    private Animator animator;
    private CapsuleCollider hurtbox;
    private NavMeshAgent agent;
    private Vector3 spawnLocation;

    private float gainAggroRange = 10f;
    private float loseAggroRange = 15f;
    private float attackRange = 2f;

    private bool canDropItem = true;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isAggro = false;
    private bool isReturning = false;
    #endregion

    #region UnityMethods
    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null) {
            throw new System.Exception("Error! " + name + ": cannot find Player transform.");
        }

        hurtbox = GetComponent<CapsuleCollider>();
        if (hurtbox == null) {
            throw new System.Exception("Error! " + name + ": is missing CapsuleCollider.");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null) {
            throw new System.Exception("Error! " + name + ": is missing NavMeshAgent.");
        }

        animator = GetComponent<Animator>();
        if (animator == null) {
            throw new System.Exception("Error! " + name + ": is missing Animator.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            throw new System.Exception("Error! " + name + ": is missing AudioSource.");
        }

        spawnLocation = transform.position;
    }

    private void Update() {
        CheckAggro();

        animator.SetFloat("Speed", new Vector3(agent.velocity.x, 0, agent.velocity.z).magnitude);
        animator.SetBool("IsAggro", isAggro);

        if (isAggro) {
            MoveTowardsPlayer();
        }

        if (canAttack) {
            Attack();
        }
    }
    #endregion

    #region CombatMethods
    private void CheckAggro() {
        if (Vector3.Distance(transform.position, spawnLocation) < 0.1f) {
            isReturning = false;
        }

        Vector2 target = new Vector2(playerTransform.position.x, playerTransform.position.z);
        Vector2 self = new Vector2(transform.position.x, transform.position.z);
        float xzDistance = Vector2.Distance(target, self);
        float yDistance = playerTransform.position.y - transform.position.y;

        if (yDistance < VERTICAL_LIMIT && !isReturning) {
            if (xzDistance < gainAggroRange) {
                isAggro = true;
            }

            if (xzDistance < attackRange) {
                canAttack = true;
            }
            else {
                canAttack = false;
            }
        }

        Vector2 xzSpawnPosition = new Vector2(spawnLocation.x, spawnLocation.z);
        if (Vector2.Distance(self, xzSpawnPosition) > loseAggroRange) {
            isAggro = false;
            MoveTowardsSpawn();
        }
    }

    private IEnumerator AttackBuffer() {
        yield return new WaitForSeconds(2.0f);
        canAttack = true;
    }

    private void Attack() {
        canAttack = false;
        animator.SetTrigger("Attack");
        if (attackSfx != null) {
            audioSource.PlayOneShot(attackSfx);
        }
        else {
            Debug.LogWarning("Warning! " + name + ": is missing attackSfx.");
        }
    }
    #endregion

    #region MovementMethods
    private void MoveTowardsPlayer() {
        Vector3 lookDirection = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0f, lookDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        agent.SetDestination(playerTransform.position);
    }

    private void MoveTowardsSpawn() {
        isReturning = true;
        agent.SetDestination(spawnLocation);
    }
    #endregion

    #region CollisionMethods
    private void OnCollisionEnter(Collision collision) {
        if (isAttacking && collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Player>().TakeDamage();
        }
    }
    #endregion
}
