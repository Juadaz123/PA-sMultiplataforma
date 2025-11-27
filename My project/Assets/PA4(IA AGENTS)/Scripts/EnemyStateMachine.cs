using System;
using UnityEditor;
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
   
   [Header("Debug")]
   [SerializeField] private EnemyState currentState;
   [SerializeField] private Pathfinding pathfinding;
   [SerializeField] private TargetSpawner targetSpawner;

   private Vector3 _targetPos;
   private Vector3 _startPos;
   private float _startTimer;
   private float _isMoving;
   private Vector3 _enemy; 
   private Transform _player;
   private void Awake()
   {
      GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
      if (playerObj != null)
      {
         _player = playerObj.transform;
      }
      _enemy = transform.position;
      
      
   }

   private void Start()
   {
      _startPos = transform.position;
      ChangeState(EnemyState.Idle);
   }

   private void Update()
   {
      if (_player == null) return;

      float distanceToPlayer = Vector3.Distance(_enemy, _player.position);
   
      if (distanceToPlayer < 3f)
      {
         if (currentState != EnemyState.Walking)
         {
            ChangeState(EnemyState.Walking);
            pathfinding.ChangeTarget(_player);
         }
      }
      else
      {
         if (currentState != EnemyState.Patrolling)
         {
            ChangeState(EnemyState.Patrolling);
            pathfinding.ChangeTarget(targetSpawner.targetToMove);
         }
      }

      if (_targetPos == _startPos)
      {
         ChangeState(EnemyState.Idle);
      }

      _startTimer -= Time.deltaTime;
   }

   private void ChangeState(EnemyState newState)
   {
      currentState = newState;
      switch (currentState)
      {
         case EnemyState.Idle:
            _startTimer = idleDuration;
            Debug.Log("Enemy is Idle State");
            break;
         case EnemyState.Walking:
            _startTimer = walkDuration;
            Debug.Log("Enemy is Walking State");
            break;
         case EnemyState.Patrolling:
            _startTimer = patrolDuration;
            Debug.Log("Enemy  is Patrolling State");
            break;
         case EnemyState.Attacking:
            _startTimer = attackDuration;
            Debug.Log("Enemy is Attacking State");
            break;
            
      }
   }
   
   
}
