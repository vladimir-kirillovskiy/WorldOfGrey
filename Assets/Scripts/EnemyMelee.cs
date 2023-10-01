using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float detectionRange = 40f;
    [SerializeField]
    private float minDistance = 1f;
    
    [SerializeField] 
    private float health = 100f;

    private NavMeshAgent enemyNMA;

    private Transform player;
    private Animator animator;
    private bool isDead = false;
    private Collider col;
    private DissolvingController dc;

    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        enemyNMA = gameObject.GetComponent<NavMeshAgent>();
        col = gameObject.GetComponent<Collider>();
        dc = GetComponent<DissolvingController>();
    }

    private void Update()
    {
        if(!isDead) 
        {
            ///
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {

                // move tovards the players
                enemyNMA.SetDestination(player.position);

                // check if moving - start walk animation
                if (enemyNMA.velocity.magnitude > 0)
                {
                    animator.SetBool("isRunning", true);
                } else
                {
                    animator.SetBool("isRunning", false);
                }

                if (distanceToPlayer < minDistance && !isAttacking)
                {
                    isAttacking= true;
                    transform.LookAt(player.position);
                    Debug.Log("Trigger Melee");
                    animator.SetBool("MeleeHit", true);
                    StartCoroutine(resetAttack());
                } else
                {
                    animator.SetBool("MeleeHit", false);
                }
                // get to the min distance 
                // attack player
            }
        }
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject, 3f);
            isDead = true;
            animator.SetBool("isDead", isDead);
            col.enabled = false;
            enemyNMA.enabled = false;
            dc.Dissolve();
        }
    }


    private void AttackStart()
    {
        isAttacking = true;
    }

    private void AttackEnd()
    {
        isAttacking = false;
    }


    private IEnumerator resetAttack()
    {
        yield return new WaitForSeconds(3);
        isAttacking = false;
    }

}
