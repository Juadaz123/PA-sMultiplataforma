using System;
using UnityEngine;

namespace IA.Stearing_Behaviours
{
    public class TargetAgent : MonoBehaviour, IAgent
    {
        public float MaxSpeed { get; set; }
        public float MaxForce { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        private void Start()
        {
            Position = transform.position;
            
        }

        private void Update()
        {
            Position =  transform.position;
        }
    }
}