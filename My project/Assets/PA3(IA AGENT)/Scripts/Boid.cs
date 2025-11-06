using System;
using IA.Stearing_Behaviours;
using UnityEngine;

public class Boid : MonoBehaviour, IAgent
{

    public float MaxSpeed { get; set; }
    public float MaxForce { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
    
    private FlockManager _manager;
    private float _cohesionWeight;
    private float _separationWeight;
    private float _alignmentWeight;

    private void Start()
    {
        SetValuesBoids();
    }

    private void SetValuesBoids()
    {
        _manager = FindAnyObjectByType<FlockManager>();
        Position = transform.position;
        MaxSpeed = _manager.maxSpeed;
        MaxForce = _manager.maxForce;
        
        _cohesionWeight = _manager.cohesionWeight;
        _separationWeight = _manager.separationWeight;
        _alignmentWeight = _manager.alignmentWeight;
    }

    private void Update()
    {
        Vector3 cohesionPos = StearingBehaviors.Cohesion(this, _manager.Boids, _manager.cohesionRadius) * _cohesionWeight;
        Vector3 separation = StearingBehaviors.Separation(this, _manager.Boids, _manager.separationRadius) * _separationWeight;
        Vector3 alignment = StearingBehaviors.Alignment(this, _manager.Boids, _manager.alignmentRadius) * _alignmentWeight; 
        if (_manager.enableWrapping)
        {
            Vector3 wrappedPos = _manager.GetWrapedPos(Position);
            if (wrappedPos != Position)
            {
                Position = wrappedPos;
            }
            
            Velocity += (cohesionPos + separation + alignment) * Time.deltaTime;
            Position += Velocity * Time.deltaTime;
            transform.up = Velocity.normalized;
            transform.position = Position;
        }
        
    }
}
