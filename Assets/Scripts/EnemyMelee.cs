using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField]
    private float destroyTime = 1.5f;

    [HideInInspector] public UnityEvent DamageDealer;

    private NavMeshAgent enemyNMA;

    private Transform player;
    private PlayerHealth playerHealth;
    private Animator animator;
    private bool isDead = false;
    private Collider col;
    private DissolvingController dc;

    private bool isAttacking = false;
    private bool isDamageDone = false;

    private void Start()
    {
        DamageDealer.AddListener(DealDamage);
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyNMA = gameObject.GetComponent<NavMeshAgent>();
        col = gameObject.GetComponent<Collider>();
        dc = GetComponent<DissolvingController>();
    }

    private void OnDestroy()
    {
        DamageDealer.RemoveListener(DealDamage);
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
                    PerformMeleeAttack();
                } 
                else
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
            Destroy(gameObject, destroyTime);
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
        isDamageDone = false;
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
        isAttacking = true;
        animator.SetBool("MeleeHit", true);
        transform.LookAt(player.position);
        StartCoroutine(resetAttack());
    }

    private void DealDamage()
    {
        if(isAttacking && !isDamageDone)
        {
            playerHealth.Damage(damage);
            isDamageDone = true;
        }
    }


}
