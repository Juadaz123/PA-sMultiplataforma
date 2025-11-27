using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
   public enum EnemyState
   {
      Idle,
      Walking,
      Patrolling,
      Attacking,
   }
   
   [Header("Enemy Settings")]
   [SerializeField] private float walkSpeed = 2f;
   [SerializeField] private float idleDuration = 3f;
   [SerializeField] private float attackDuration = 1.5f;
   [SerializeField] private float patrolDuration = 1.5f;
   [SerializeField] private float walkDuration = 1.5f;
   [SerializeField] private float nextWaypointDistance = 0.1f;
   
   [Header("Debug")]
   [SerializeField] private EnemyState currentState;
   [SerializeField] private Pathfinding pathfinding;
   [SerializeField] private TargetSpawner targetSpawner;

   private Transform _player;
   private float _startTimer;
   private int _currentPathIndex;

   private void Awake()
   {
      GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
      if (playerObj != null)
      {
         _player = playerObj.transform;
      }
   }

   private void Start()
   {
      ChangeState(EnemyState.Idle);
   }

   private void Update()
   {
      if (_player == null) return;

      float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

      if (distanceToPlayer < 0.5f)
      {
         ChangeState(EnemyState.Idle);
         return;
      }
   
      if (distanceToPlayer < 3f)
      {
         if (currentState != EnemyState.Walking)
         {
            pathfinding.ChangeTarget(_player);
            ChangeState(EnemyState.Walking);
         }
      }
      else
      {
         if (currentState != EnemyState.Patrolling)
         {
            pathfinding.ChangeTarget(targetSpawner.targetToMove);
            ChangeState(EnemyState.Patrolling);
         }
      }

      switch (currentState)
      {
         case EnemyState.Walking:
         case EnemyState.Patrolling:
            MoveAlongPath();
            break;
         case EnemyState.Idle:
            break;
      }

      _startTimer -= Time.deltaTime;
   }

   private void MoveAlongPath()
   {
      // 1. Validación básica: ¿Existe el camino?
      if (pathfinding.finalPath == null || pathfinding.finalPath.Count == 0) return;
      if (_currentPathIndex >= pathfinding.finalPath.Count)
      {
         return; 
      }
      Vector3 targetPosition = pathfinding.finalPath[_currentPathIndex];
      targetPosition.y = transform.position.y; 

      transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, targetPosition) < nextWaypointDistance)
      {
         _currentPathIndex++;
        
         if (_currentPathIndex >= pathfinding.finalPath.Count)
         {
            _currentPathIndex = 0;
            ChangeState(EnemyState.Idle);
         }
      }
   }
   private void ChangeState(EnemyState newState)
   {
      if (currentState == newState) return;

      currentState = newState;
      _currentPathIndex = 0;

      switch (currentState)
      {
         case EnemyState.Idle:
            Debug.Log("Idle State");
            _startTimer = idleDuration;
            break;
         case EnemyState.Walking:
            Debug.Log("Walking State");
            _startTimer = walkDuration;
            break;
         case EnemyState.Patrolling:
            Debug.Log("Patrolling State");
            _startTimer = patrolDuration;
            break;
         case EnemyState.Attacking:
            _startTimer = attackDuration;
            break;
      }
   }
}