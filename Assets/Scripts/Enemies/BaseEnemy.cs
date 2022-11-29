using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {

    #region Fields
    private const float VERTICAL_LIMIT = 1.8f;

    [SerializeField] private Transform playerTransform;

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

        spawnLocation = transform.position;
    }

    private void Update() {
        CheckAggro();

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

    private void Attack() {
        // todo
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

    }
    #endregion
}
