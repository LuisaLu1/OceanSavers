using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemyController : ChasingEnemyController
{
    public Transform[] waypoints;

    private int waypointIndex = 0;

    protected override Vector3 GetNextDestination()
    {
        Vector3 destination = waypoints[waypointIndex].position;

        waypointIndex = (waypointIndex + 1) % waypoints.Length;

        return destination;
    }
}
