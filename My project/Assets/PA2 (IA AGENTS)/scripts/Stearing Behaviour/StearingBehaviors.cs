using UnityEngine;

namespace IA.Stearing_Behaviours
{
    public class StearingBehaviors
    {
        public static Vector3 Seek(IAgent agent, IAgent target)
        {
            Vector3 desired = (target.Position - agent.Position).normalized * agent.MaxSpeed;
            Vector3 steering = Vector3.ClampMagnitude(desired - agent.Velocity, agent.MaxForce);
            
            return steering;
        }
        
        public static Vector3 Flee(IAgent agent, IAgent target)
        {
            Vector3 desired = (agent.Position - target.Position).normalized * agent.MaxSpeed;
            Vector3 steering = Vector3.ClampMagnitude(desired - agent.Velocity, agent.MaxForce);
            
            return steering;
        }
        public static Vector3 Arrive(IAgent agent, IAgent target, float arriveRadius)
        {
            Vector3 desired = (target.Position - agent.Position).normalized * agent.MaxSpeed;
            if (Vector3.Distance(agent.Position, target.Position) < arriveRadius)
            {
                desired *= Vector3.Distance(agent.Position, target.Position) / arriveRadius;
            }
            Vector3 steering = Vector3.ClampMagnitude(desired - agent.Velocity, agent.MaxForce);
            
            return steering;
        }
        
    }
}