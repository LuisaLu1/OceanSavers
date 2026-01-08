using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasingEnemyController : MonoBehaviour
{
    public GameObject player;
    public float detectionRadius = 4f;

    public GameObject enemy;
    public Material idleMaterial;
    public Material chasingMaterial;

    private NavMeshAgent nav;
    private Vector3 initialPosition = Vector3.zero;

    protected virtual Vector3 GetNextDestination()
    {
        return initialPosition;
    }

    private void Start()
    {
        nav = gameObject.GetComponent<NavMeshAgent>();
        initialPosition = gameObject.transform.position;
    }

    private void Update()
    {
        // Distance from player to enemy is smaller than the given detection radius -> start chasing the player 
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < detectionRadius)
        {
            enemy.GetComponent<Renderer>().material = chasingMaterial;

            nav.SetDestination(player.transform.position);

            return;
        }

        // Enemy is in idle mode
        enemy.GetComponent<Renderer>().material = idleMaterial;

        if (nav.remainingDistance <= float.Epsilon)
        {
            nav.SetDestination(GetNextDestination());
        }
    }



}
