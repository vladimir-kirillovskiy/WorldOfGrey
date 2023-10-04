using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float detectionRange = 40f;
    [SerializeField]
    private float minDistance = 1f;
    [SerializeField]
    private float attackRange = 2f;

    [SerializeField] 
    private float health = 100f;

    [SerializeField]
    private float damage = 20f;

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
                    animator.SetBool("MeleeHit", true);
                    transform.LookAt(player.position);
                    PerformMeleeAttack();
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


    void PerformMeleeAttack()
    {
        RaycastHit hit;

        // Calculate direction from enemy to player
        Vector3 direction = (transform.position - Camera.main.transform.position).normalized;

        // Perform a SphereCast to check if the player is within attack range
        if (Physics.SphereCast(transform.position, attackRange, direction, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Player")) // Assuming the player has the "Player" tag
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                damageable?.Damage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
    }
}
