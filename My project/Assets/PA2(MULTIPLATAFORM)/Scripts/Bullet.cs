using System;
using UnityEngine;
using UnityEngine.Pool;


    public class Bullet : MonoBehaviour
    {
        public IObjectPool<Bullet> objectPool;
        public IObjectPool<Bullet> ObjectPool { set => objectPool = value; }
        
        // ------------------ Bullet Properties ------------------
        [SerializeField] private float speed = 10f; // Speed of the bullet
        [SerializeField] private float lifeTime = 3f; // Time before returning to pool
        
        // ------------------ Private Members ------------------
        private Rigidbody _rb;
        private float _timeActive; // Timer for tracking active time
        private Vector3 _targetDirection; // Direction to move towards

        // ------------------ Unity Methods ------------------
        private void Awake()
        {
            // Get the Rigidbody component on Awake
            _rb = GetComponent<Rigidbody>();
            if (_rb == null)
            {
                Debug.LogError("Bullet object requires a Rigidbody component.");
            }
        }

        private void OnEnable()
        {
            // Reset timer when the object is taken from the pool
            _timeActive = 0f; 
            
            // NOTE: The position and direction should be set by the GameManagerBasket 
            // or the Spawner method before calling Get() or right after.
            // For now, we'll assume they are set externally right before OnEnable runs.
            
            // To apply movement using Rigidbody, we set its velocity
            // This is cleaner than manipulating transform.
            if (_rb != null)
            {
                // Ensure physics are active (optional, but good practice)
                _rb.isKinematic = false;
                
                // Set the velocity to move in the stored direction
                // Assuming _targetDirection is set externally
                _rb.linearVelocity = _targetDirection * speed;
            }
        }

        private void Update()
        {
            // Increment the active timer
            _timeActive += Time.deltaTime;

            // Check if the life time has expired
            if (_timeActive >= lifeTime)
            {
                // Return the bullet to the pool
                ReturnToPool();
            }
        }

        private void OnDisable()
        {
            // Stop movement when returning to the pool
            if (_rb != null)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
            
            // Optional: Reset position to a safe location if needed, 
            // but usually setting Active(false) is enough.
        }

        // Public method to be called from the spawner (GameManagerBasket)
        public void SetMovementTarget(Vector3 startPosition, Vector3 direction)
        {
            // Set the bullet's starting position
            transform.position = startPosition;
            
            // Store the direction for OnEnable and Rigidbody
            _targetDirection = direction;
            
          
        }

        // Method to return the bullet to the pool
        public void ReturnToPool()
        {
            if (objectPool != null)
            {
                objectPool.Release(this);
            }
            else
            {
                // Fallback: if pool reference is lost, destroy the object
                Destroy(gameObject);
            }
        }

    }
        

