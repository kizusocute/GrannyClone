using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Granny : MonoBehaviour
{
    public enum GrannyState
    {
        Idle,
        HeardSound,
        Waiting,
        Returning,
        Chasing,
        Attacking,
        Dead
    }

    [Header("References")]
    private NavMeshAgent navMeshAgent;
    public Transform startPosition;
    public Transform player;

    [Header("Settings")]
    public float moveSpeed = 2.5f;
    public float hitDamage = 100f;
    public float waittingTime = 5f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private float lastAttackTime = 0f;
    private Vector3 soundLocation;

    private GrannyState currentState = GrannyState.Idle;

    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;

    [Header("Footstep")]
    public AudioClip[] footstepSounds;
    AudioSource audioSource;
    float footstepInterval = .5f;
    float nextFootstepTime = 0f;


    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
        audioSource = GetComponent<AudioSource>();
        InvokeRepeating(nameof(LookForPlayer), 0f, 0.1f);
    }

    void Update()
    {
        UpdateAnimations();
        switch (currentState)
        {
            case GrannyState.Dead:
                
                break;

            case GrannyState.Idle:

                break;

            case GrannyState.HeardSound:
                MoveToSound();
                break;

            case GrannyState.Waiting:
                
                break;

            case GrannyState.Returning:
                ReturnToStart();
                break;

            case GrannyState.Chasing:
                ChasePlayer();
                break;

            case GrannyState.Attacking:
                AttackPlayer();
                break;
        }
        PlayFootstepSounds();
    }

    
    public void OnSoundHeard(Vector3 location)
    {
        if (currentState == GrannyState.Dead) return;

        soundLocation = location;
        currentState = GrannyState.HeardSound;
    }

    void MoveToSound()
    {
        navMeshAgent.SetDestination(soundLocation);

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentState = GrannyState.Waiting;
            StartCoroutine(WaitBeforeReturning());
        }
    }

    IEnumerator WaitBeforeReturning()
    {
        yield return new WaitForSeconds(waittingTime);
        currentState = GrannyState.Returning;
    }

    void ReturnToStart()
    {
        navMeshAgent.SetDestination(startPosition.position);

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentState = GrannyState.Idle;
        }
    }

    void LookForPlayer()
    {
        if (currentState == GrannyState.Chasing || currentState == GrannyState.Attacking || currentState == GrannyState.Dead) return;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                currentState = GrannyState.Chasing;
                break;
            }
        }
    }

    void ChasePlayer()
    {
        if (navMeshAgent.isStopped) navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            navMeshAgent.isStopped = true;
            currentState = GrannyState.Attacking;
        }
        else if (distanceToPlayer > detectionRange)
        {
            navMeshAgent.isStopped = false;
            currentState = GrannyState.Returning;
        }
    }

    void AttackPlayer()
    {
        if (isAttacking || currentState != GrannyState.Attacking) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            navMeshAgent.isStopped = false;
            currentState = GrannyState.Chasing;
            return;
        }

        StartCoroutine(PerformAttack());
    }


    public void Die()
    {
        currentState = GrannyState.Dead;
        navMeshAgent.isStopped = true;
        
    }
    void UpdateAnimations()
    {
        bool isActuallyWalking = navMeshAgent.velocity.magnitude > 0.1f;
        bool isWalking = isActuallyWalking && currentState != GrannyState.Waiting;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isAttacking", currentState == GrannyState.Attacking || isAttacking);
        animator.SetBool("isDead", currentState == GrannyState.Dead);

        bool isIdle = !isWalking && (currentState == GrannyState.Idle || currentState == GrannyState.Waiting);

        animator.SetBool("isIdle", isIdle);
    }
    IEnumerator PerformAttack()
    {
        isAttacking = true;
        if (Time.time < lastAttackTime + attackCooldown)
        {
            isAttacking = false;
            navMeshAgent.isStopped = false;
            currentState = GrannyState.Attacking;
            yield break;
        }

        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(0.5f); 

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (distance <= attackRange && angle <= 60f) 
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(hitDamage);
                StartCoroutine(Respawn(5));
                Debug.Log("Granny Hit!");
            }
        }
        else
        {
            Debug.Log("Miss");
        }

        lastAttackTime = Time.time;

        yield return new WaitForSeconds(0.5f); // delay anim attack

        navMeshAgent.isStopped = false;
        isAttacking = false;

        float dist = Vector3.Distance(transform.position, player.position);
        currentState = (dist <= attackRange) ? GrannyState.Attacking : GrannyState.Chasing;
    }
    void PlayFootstepSounds()
    {
        if (navMeshAgent.velocity.magnitude > 0.1f && Time.time >= nextFootstepTime)
        {
            if (footstepSounds.Length > 0)
            {
                int index = Random.Range(0, footstepSounds.Length);
                AudioClip footstepClip = footstepSounds[index];
                audioSource.PlayOneShot(footstepClip);

                
                nextFootstepTime = Time.time + footstepInterval ;
            }
        }
    }

    IEnumerator Respawn(float delay)
    {
        yield return new WaitForSeconds(delay - 3f);
        isDead = false;
        transform.position = startPosition.position;
        navMeshAgent.Warp(startPosition.position);
        currentState = GrannyState.Idle;
    }

}
