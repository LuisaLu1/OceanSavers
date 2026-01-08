using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent nav;

    public float walkRadius = 1f;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;

        nav.SetDestination(finalPosition);
    }

    void Update()
    {
        if (nav.remainingDistance <= float.Epsilon)
        {
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;

            nav.SetDestination(finalPosition);
        }
    }




}
