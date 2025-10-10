using UnityEngine;
using UnityEngine.Pool;


    public class GameManagerBasket : MonoBehaviour
    {
        public static GameManagerBasket Instance {get; private set;}
        
        [Header("Pooling Parameters")]
        [SerializeField] private Bullet bullet;
        [SerializeField] private Transform bulletSpawn;
        [SerializeField] private int defaultPoolSize = 5, maxPoolSize = 20;
        
        private bool _collectionCheck = true;
        
        private IObjectPool<Bullet> _bulletPool;
        
        private Camera _mainCamera;
        
        //Singelton
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            _bulletPool = new ObjectPool<Bullet>(
                CreateBullet,
                OnGetFromPool,
                onRelasePool,
                onDestroyPoolObject,
                _collectionCheck,
                defaultPoolSize,
                maxPoolSize
            );
            
            _mainCamera = Camera.main;
        }

        private Bullet CreateBullet()
        {
                Bullet bulletInstance = Instantiate(bullet);
                bulletInstance.objectPool = _bulletPool;
                return bulletInstance;
        }

        private void OnGetFromPool(Bullet pooledBullet)
        {
            pooledBullet.gameObject.SetActive(true);
        }

        private void onRelasePool(Bullet pooledBullet)
        {
            pooledBullet.gameObject.SetActive(false);
        }

        private void onDestroyPoolObject(Bullet pooledBullet)
        {
            Destroy(pooledBullet.gameObject);
        }

        public void SpawnBullet()
        {
            if (bulletSpawn == null || _mainCamera == null)
            {
                Debug.LogError("Player Transform or Main Camera is not set.");
                return;
            }
            
            //calculate direction to mouse
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 targetPoint = hit.point;
                
                Vector3 direction = (targetPoint - _mainCamera.transform.position).normalized;
                
                Bullet bulletInstance = _bulletPool.Get();
                bulletInstance.SetMovementTarget(bulletSpawn.position, direction);

            }
        }
    }
