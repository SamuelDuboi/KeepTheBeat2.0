﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/StayInRadius")]
public class StayInRadiusBehavior : FlockBehaviour
{
    public Vector3 center;
    public float radius = 15f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 centerOffSet = center - agent.transform.position;
        float t = centerOffSet.magnitude / radius;
        if(t < 0.9f)
        {
            return Vector3.zero;
        }

        return centerOffSet * t * t;
    }
}
