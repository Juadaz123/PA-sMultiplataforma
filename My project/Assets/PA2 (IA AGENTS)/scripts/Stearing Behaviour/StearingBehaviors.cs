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
        
        public static  Vector3 Cohesion(IAgent boid, IAgent[] neighbours, float radius)
        {
            Vector3 center = Vector3.zero;
            int count = 0;
            int neighboursCount = neighbours.Length;

            for (int i = 0; i < neighboursCount; i++)
            {
                IAgent neighbour = neighbours[i];
                float distance = Vector3.Distance(neighbour.Position, boid.Position);
                if(distance < radius && distance > Mathf.Epsilon)
                {
                    center += neighbour.Position;
                    count++;
                }
            }
            if(count == 0) return Vector3.zero;
                
            center = center/ count;
            
            return Seek(boid, new SimpleAgent(center));
            
            
        }

        public static Vector3 Separation(IAgent boid, IAgent[] neighbours, float separationRadius)
        {
            //acumulador del vector deseado
            Vector3 steer = Vector3.zero;
            //vecinos cerca
            int count = 0;
            
            float sqrRadius = separationRadius * separationRadius;
            
            int neighboursCount = neighbours.Length;

            for (int i = 0; i < neighboursCount; i++)
            {
                
                IAgent neighbour = neighbours[i];
                Vector3 offset = boid.Position - neighbour.Position;
                float sqrDistance = offset.sqrMagnitude;
                if (sqrDistance < sqrRadius && sqrDistance > Mathf.Epsilon)
                {
                    //necesitamos la distancia real
                    float distance = Mathf.Sqrt(sqrDistance);
                    Vector3 direction = offset / distance;
                    steer += direction/ Mathf.Max(distance, Mathf.Epsilon);
                    count++;
                }
            }

            if (count == 0)
            {
                steer = steer.normalized * boid.MaxSpeed;
            }


            return steer;
        }

        public static Vector3 Alignment(IAgent boid, IAgent[] neighbours, float alignmentRadius)
        {
            Vector3 averageVelocity = Vector3.zero;
            float sqrRadius = alignmentRadius * alignmentRadius;
            int count = 0;
            int neighboursCount = neighbours.Length;

            for (int i = 0; i < neighboursCount; i++)
            {
                IAgent neighbour = neighbours[i];
                float sqrDistance = (boid.Position - neighbour.Position).sqrMagnitude;
                if (sqrDistance < sqrRadius && sqrDistance > Mathf.Epsilon)
                {
                    averageVelocity += boid.Velocity;
                    count++;
                }
            }
            if(count == 0) return Vector3.zero;
            
            averageVelocity /= count;
            
            return averageVelocity.normalized * boid.MaxSpeed -  boid.Velocity;;
        }
        
        public static Vector3 InsideSphere(IAgent agent, Vector3 sphereCenter, float sphereRadius)
        {
            Vector3 toCenter = sphereCenter - agent.Position;
            float distance = toCenter.magnitude;

            if (distance > sphereRadius)
            {
                Vector3 desired = toCenter.normalized * agent.MaxSpeed;
                Vector3 steering = Vector3.ClampMagnitude(desired - agent.Velocity, agent.MaxForce);
                return steering;
            }
            
            return Vector3.zero;
        }
        
        private class SimpleAgent : IAgent
        {
            public float MaxSpeed { get; set; }
            public float MaxForce { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Velocity { get; set; }

            public SimpleAgent(Vector3 position)
            {
                Position = position;
                Velocity = Vector3.zero;
                MaxForce = 0;
                MaxSpeed = 0;
            }
        }
        
    }
}