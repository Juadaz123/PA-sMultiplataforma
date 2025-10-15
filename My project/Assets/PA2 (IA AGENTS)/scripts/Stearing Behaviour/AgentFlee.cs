using System;
using System.Collections.Generic;
using UnityEngine;

namespace IA.Stearing_Behaviours
{
    public class AgentFlee: MonoBehaviour, IAgent

    {
        public float MaxSpeed { get; set; }
         public float MaxForce { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        [Header("Agent")] 
        [SerializeField] private TargetAgent target;
        [SerializeField] private TargetAgent arriveTarget;

        [SerializeField] private float maxForce = 10f, maxSpeed = 5f;
        
        [SerializeField] private List<TargetAgent> walls;

        private Vector3 _blockY;
        private bool _playerBool;


        private void Start()
        {
            _playerBool = true;
            target = GameObject.Find("Player").GetComponent<TargetAgent>(); 
            Position = transform.position;
            MaxForce = maxForce;
            MaxSpeed = maxSpeed;
            
            Walls_Checker();
            
        }

        private void Update()
        {
            if (_playerBool)
            {
                
                Flee_Player(target, 3f);
            }

            //List Targets
            Flee_Target(walls[0], 1f);
            Flee_Target(walls[1], 1f);
            Flee_Target(walls[2], 1f);
            Flee_Target(walls[3], 1f);

            Vector3 blockY = transform.position;
            blockY.y = -3.8f;
            transform.position = blockY;;
            
            ArriveAgent(arriveTarget, 8f);

      
        }   

        // ReSharper disable Unity.PerformanceAnalysis
        private void Flee_Player(TargetAgent targetPlayer, float distance)
        {
            Position = transform.position;

            if(Vector3.Distance(Position, targetPlayer.transform.position) < distance) 
            {
                Velocity += StearingBehaviors.Flee(this, targetPlayer);
                transform.position += Velocity * Time.deltaTime;
                //Debug.Log("Cerca");
            }
            if(Vector3.Distance(Position, targetPlayer.transform.position) > distance)
            {
                Velocity = Vector3.zero;
                //Debug.Log("Lejos");
            }
        }
        private void Flee_Target(TargetAgent targetWalls, float distance)
        {
            Position = transform.position;

            if(Vector3.Distance(Position, targetWalls.transform.position) < distance) 
            {
                Velocity += StearingBehaviors.Flee(this, targetWalls);
                transform.position += Velocity * Time.deltaTime;
                Debug.Log($"Cerca {targetWalls.name}");
            }
        }

        private void ArriveAgent(TargetAgent aTarget, float distance)
        {
            if (Vector3.Distance(transform.position, arriveTarget.transform.position) < distance)
            {
                Position = transform.position;
                Velocity += StearingBehaviors.Arrive(this, aTarget, distance);
                transform.position += Velocity * Time.deltaTime;
                //Debug.Log("Arrive");
            }
        }
        

        private void Walls_Checker()
        {
            if (walls.Count > 0 && walls != null)
            {
                foreach (TargetAgent wall in walls)
                {
                    TargetAgent targetwall = wall.GetComponent<TargetAgent>();
                    Debug.Log(targetwall.name);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Redill")
            {
                
               _playerBool = false;
            }
            
        }
    }
}