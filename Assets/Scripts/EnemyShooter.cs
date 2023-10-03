using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyShooter : MonoBehaviour, IDamageable
{
    [Header("Detection")]
    [SerializeField]
    private float detectionRange = 40f;
    [SerializeField]
    private Rig rig;

    [Header("Shooting")]
    [SerializeField]
    private float fireRate = 10f;
    [SerializeField]
    private Transform muzzleTrans;
    [SerializeField] 
    private ParticleSystem muzzleFlash;
    [SerializeField] 
    AudioSource shotSound;
    [SerializeField]
    private float damage = 10f;
    [SerializeField] 
    TrailRenderer bulletTrail;
    [SerializeField]
    private float accuracy = 0.8f;
    [SerializeField]
    private AudioSource shootSFX;

    private float timeSinceLastShot = 0;
    private Transform player;
    private Animator animator;

    private bool isDead = false;

    private DissolvingController dc;


    private bool CanShoot() => timeSinceLastShot > 1f / (fireRate / 60f);


    [SerializeField] private float health = 100f;

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject, 3f);
            isDead = true;
            animator.SetBool("isDead", isDead);
            dc.Dissolve();
        }
    }


    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        dc = GetComponent<DissolvingController>();

        rig.weight = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void Update()
    {

        if (!isDead)
        {
            timeSinceLastShot += Time.deltaTime;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                // Face towards the player
                transform.LookAt(player);
                animator.SetBool("isAiming", true);
                rig.weight = 1;

                if (CanShoot())
                {
                    DoShoot();
                }

            } else
            {
                animator.SetBool("isAiming", false);
                rig.weight = 0;
            }

        } else
        {
            rig.weight = 0; 
        }
    }


    void DoShoot()
    {
        // Debug.DrawRay(muzzleTrans.position, muzzleTrans.forward * detectionRange, Color.blue, 2);

        Vector3 playerPositionWithOffset = player.position + new Vector3(0f, 1.5f, 0f);

        Vector3 dir = (playerPositionWithOffset - muzzleTrans.position).normalized;
        dir += Random.insideUnitSphere * (1 - accuracy);
        dir.Normalize();

        StartCoroutine(SendRay(dir));
        timeSinceLastShot = 0;
    }

    private IEnumerator SendRay(Vector3 dir)
    {

        yield return new WaitForSeconds(0.1f);

        shootSFX.Play();

        if (Physics.Raycast(muzzleTrans.position, dir, out RaycastHit hitInfo, detectionRange))
        {
            IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
            damageable?.Damage(damage);

            TrailRenderer trail = Instantiate(bulletTrail, muzzleTrans.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitInfo.point));
        }
        else
        {
            // add trail for missed shots
            TrailRenderer trail = Instantiate(bulletTrail, muzzleTrans.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hitInfo.point));
        }

        
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 point)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        // isShooting = false
        trail.transform.position = point;
        // instanciate hit particle
        Destroy(trail.gameObject, trail.time);
    }
}
