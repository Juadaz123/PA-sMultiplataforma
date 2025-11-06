
    using UnityEngine;

    public class EnemyBehaviour : MonoBehaviour, IDamageable
        
    {
        public float CurrentLife { get; set; }
        public float MaxLife { get; set; }
        
        private Boid _boid;
        private ScoreManager _scoreManager;
        
        [Header("Enemy Settings")]
        [SerializeField] private float health;
        [SerializeField] private string tagColliderTriiger, tagDamage;

        private void Start()
        {
            _scoreManager  = FindFirstObjectByType<ScoreManager>();
            _boid = GetComponent<Boid>();
            MaxLife = health;
            CurrentLife = MaxLife;
            
            _scoreManager.SubmitScore();
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(tagColliderTriiger))
            {
                _boid.GetMoreCohesion(true);
            }
            _boid.GetMoreCohesion(false);

            if (other.gameObject.CompareTag(tagDamage))
            {
                CurrentLife -= 1f;
                
                Debug.Log($"La vida es {CurrentLife}");
                if (CurrentLife <= 0)
                {
                    _scoreManager.AddScore(10);
                    Debug.Log($"El enemigo tiene {CurrentLife} va a ser destruido ");
                    Destroy(gameObject);
                }
            }
        }

    
    }