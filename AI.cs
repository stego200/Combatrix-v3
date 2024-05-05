using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
   public NavMeshAgent agent;
   public Transform player;
   public LayerMask whatIsGround, whatIsPayer;

   //Patroling
   public Vector3 walkPoint;
   bool walkPointSet;
   public float walkPointRange;

    //Attacking
    public float timrOfAttck;
    bool hasAttacked;

    //States
    public float sightRange, attackRange;
    public bool isPlayerInRange, isPlayerInAttRange;

    private void awake(){
        player = GameObject.Find("Main_Camera").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update(){
        //Check for sight and attack range
        isPlayerInRange = Physics.CheckSphere(transform.position, sightRange, whatIsPayer);
        isPlayerInAttRange = Physics.CheckSphere(transform.position, attackRange, whatIsPayer);

        if (!isPlayerInRange && !isPlayerInAttRange) Patrolling();
        if  (isPlayerInRange && !isPlayerInAttRange) ChasePlayer();
        if  (isPlayerInRange && isPlayerInAttRange) AttackPlayer();

        
    }

    private void Patrolling(){
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) {
            agent.SetDestination(walkPoint);
        }

        Vector3 distyanceToWalkPoint = transform.position - walkPoint;

        if (distyanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint(){
        //Calculate random poit in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }

    }
    private void ChasePlayer(){
        agent.SetDestination(player.position);
    }
    private void AttackPlayer(){
        
   }







}
