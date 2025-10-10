using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public IObjectPool<Bullet> objectPool;
    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    
    private Rigidbody _rb;
    private float _timeActive;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _timeActive = 0f;
    }



    private void Update()
    {
        _timeActive += Time.deltaTime;

        if (_timeActive >= lifeTime)
        {
            ReturnToPool();
        }
    }

    private void OnDisable()
    {
        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    public void SetMovementTarget(Vector3 startPosition, Vector3 direction)
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        _rb.linearVelocity = direction * speed;
    }
    
    public void ReturnToPool()
    {
        if (objectPool != null)
        {
            objectPool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Target"))
        {
            ReturnToPool();
        }
    }
}