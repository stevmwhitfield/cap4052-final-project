using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private const int MAX_HP = 1;

    private const float SPEED = 6.0f;
    private const float ACCELERATION = 12.0f;

    private const float RATE_OF_ATTACK = 3.0f;

    public CapsuleCollider hurtbox; // detects incoming combat collisions
    public CapsuleCollider hitbox;  // detects outgoing combat collisions

    private SphereCollider innerAggroRadius; // triggers aggro
    private SphereCollider outerAggroRadius; // detriggers aggro

    private int currentHp;
    private bool isDead;

    private bool canAttack;
    private bool isAttacking;

    private bool isAggro;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
