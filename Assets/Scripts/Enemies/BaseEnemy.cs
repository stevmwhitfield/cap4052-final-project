using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour {

    #region Fields
    //private const float SPEED = 4.0f;
    //private const float ACCELERATION = 36.0f;
    //private const float RATE_OF_ATTACK = 3.0f;
    private const float VERTICAL_LIMIT = 1.8f;

    private NavMeshAgent agent;

    [SerializeField] private Transform playerTransform;

    private CapsuleCollider hurtbox;
    //private Vector3 spawnLocation;

    private float gainAggroRange = 8f;
    private float loseAggroRange = 16f;
    private float attackRange = 2f;

    private bool canDropItem = true;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isAggro = false;
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
    }

    private void Update() {
        CheckAggro();

        if (isAggro) {
            Move();
        }
    }
    #endregion

    #region CombatMethods
    private void CheckAggro() {
        Vector2 target = new Vector2(playerTransform.position.x, playerTransform.position.z);
        Vector2 self = new Vector2(transform.position.x, transform.position.z);
        float xzDistance = Vector2.Distance(target, self);
        float yDistance = playerTransform.position.y - transform.position.y;

        if (xzDistance < gainAggroRange && yDistance < VERTICAL_LIMIT) {
            isAggro = true;
        }

        if (xzDistance < attackRange && yDistance < VERTICAL_LIMIT) {
            canAttack = true;
        }
        else {
            canAttack = false;
        }

        if (xzDistance > loseAggroRange) {
            isAggro = false;
        }
    }
    #endregion

    #region MovementMethods
    private void Move() {
        Vector3 lookDirection = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(lookDirection.x, 0f, lookDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        agent.destination = playerTransform.position;
        //agent.SetDestination(playerTransform.position);
    }
    #endregion

    #region CollisionMethods
    private void OnCollisionEnter(Collision collision) {

    }
    #endregion
}
