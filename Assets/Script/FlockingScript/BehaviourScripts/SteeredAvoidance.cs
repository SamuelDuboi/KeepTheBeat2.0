using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/SteeredAvoidance")]
public class SteeredAvoidance : FilteredFlockBehaviour
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;
    public float avoidanceMult;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (context.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius * avoidanceMult)
            {
                nAvoid++;
                avoidanceMove += agent.transform.position - item.position;
            }
        }

        if (nAvoid > 0)
        {
            avoidanceMove /= nAvoid;
        }
        avoidanceMove= Vector3.SmoothDamp(agent.transform.forward, avoidanceMove, ref currentVelocity, agentSmoothTime);

        return avoidanceMove;
    }
}
