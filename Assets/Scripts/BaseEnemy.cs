using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    #region Fields
    private const int MAX_HP = 1;

    private const float SPEED = 6.0f;
    private const float ACCELERATION = 12.0f;
    private const float RATE_OF_ATTACK = 3.0f;

    private SphereCollider innerAggroRadius; // triggers aggro
    private SphereCollider outerAggroRadius; // detriggers aggro
    private CapsuleCollider hurtbox; // detects incoming combat collisions
    private Vector3 spawnLocation;

    [SerializeField] private CapsuleCollider hitbox;  // detects outgoing combat collisions
    [SerializeField] private Vector3 playerPosition;

    private int currentHp = MAX_HP;

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
    }

    private void Update() {

    }
    #endregion

    #region CombatMethods
    #endregion

    #region CollisionMethods
    private void OnCollisionEnter(Collision collision) {

    }
    #endregion
}
