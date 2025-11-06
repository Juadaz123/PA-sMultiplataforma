using UnityEngine;

namespace  IA.Stearing_Behaviours
{
    [RequireComponent(typeof(TargetAgent))]
public class AgentSeek : MonoBehaviour,  IAgent
{
    [Header(("Agent"))]
    [SerializeField] private TargetAgent target;
    [SerializeField] private TargetAgent seekTarrget;
    [SerializeField] private float maxForce = 10f, maxSpeed = 5f;

    private void Start()
    {    if (target == null)
        {
           
            Debug.Log($"No Target Found {target.name} no es igual a Player");
            return;
        }
        //target = GameObject.Find("Player").GetComponent<TargetAgent>(); 
        Position = transform.position;
        MaxForce = maxForce;
        MaxSpeed = maxSpeed;
       
    }

    public virtual void Update()
    {
        SeekBehaviour(target);

    }

    private void SeekBehaviour(TargetAgent thistarget)
    {
        Position = transform.position;
        Velocity += StearingBehaviors.Seek(this, thistarget).normalized;
        transform.position += Velocity * Time.deltaTime;
        
        
    }


    public float MaxSpeed { get; set; }
    public float MaxForce { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
}
    
}
