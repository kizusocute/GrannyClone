using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Granny : MonoBehaviour
{
    [Header("Sound Detection")]
    NavMeshAgent navMeshAgent;
    public float moveSpeed = 2.5f;
    public float waittingTime = 10f;
    public Transform startPosition;

    private Vector3 soundLocation;
    public bool isReturning = false;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isWaiting = false;
    private bool isDead = false;
    //bool soundHeard = false;

    [Header("Granny States")]
    public Transform player;
    public float detectionRange = 15f;
    private float attackRange = 2f;
    private float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        if (isDead) return;

        if (isReturning)
        {
            ReturnToStart();
        }
        else if (isChasing )
        {
            ChasePlayer();
        }
        else if (!isWaiting)
        {
            LookForPlayer();
        }
        else if (isAttacking)
        {
            AttackPlayer();
        }
    }
    public void OnSoundHeard(Vector3 location)
    {
        if(isDead) return;

        //soundHeard = true;
        soundLocation = location;
        isReturning = false;
        isChasing = false;
        isAttacking = false;
        isWaiting = false;
        MoveToSoundLocation();
    }

    void MoveToSoundLocation()
    {
        navMeshAgent.SetDestination(soundLocation);
        if(Vector3.Distance(transform.position, soundLocation) <= navMeshAgent.stoppingDistance)
        {
            StartCoroutine(WaitBeforeReturning());
        }
    }

    IEnumerator WaitBeforeReturning()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waittingTime);
        isWaiting = false;
        isReturning = true;
        //soundHeard = false;
    }
    
    void ReturnToStart()
    {
        navMeshAgent.SetDestination(startPosition.position);
        if (Vector3.Distance(transform.position, startPosition.position) <= navMeshAgent.stoppingDistance)
        {
            isReturning = false;
        }
    }

    void LookForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                isChasing = true;
                isReturning = false;
                //Debug.Log(isChasing);
                break;
            }
        }
    }

    void ChasePlayer()
    {
        navMeshAgent.SetDestination(player.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            navMeshAgent.isStopped = true;
            isChasing = false;
            isAttacking = true;
        }
        else if (distanceToPlayer > detectionRange)
        {
            isChasing = false;
            isReturning = true;
        }
        
    }

    void AttackPlayer()
    {
        if(Time.time > lastAttackTime + attackCooldown)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if(playerController != null)
            {
                Debug.Log("Die");
            }
            lastAttackTime = Time.time;
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if(distanceToPlayer > attackRange)
            {
                navMeshAgent.isStopped = false;
                isChasing = true;
                isAttacking = false;
            }
        }
    }
}
