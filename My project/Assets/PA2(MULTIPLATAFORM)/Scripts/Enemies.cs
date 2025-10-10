using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Enemies : MonoBehaviour
{
    public IObjectPool<Enemies> objectPool;
    public IObjectPool<Enemies> ObjectPool { set => objectPool = value; }

    [SerializeField] private float moveSpeed = 5f;
    
    private Transform _playerTransform; 

    public void SetTarget(Transform target)
    {
        _playerTransform = target;
    }

    private void OnEnable()
    {
        if (_playerTransform != null)
        {
            StartCoroutine(MoveToPlayerCoroutine());
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (objectPool != null)
        {
            objectPool.Release(this);
        }
    }

    private IEnumerator MoveToPlayerCoroutine()
    {
        while (true)
        {
            if (_playerTransform)
            {
                gameObject.SetActive(false); 
                yield break;
            }

            Vector3 direction = _playerTransform.position - transform.position;
            direction.Normalize();

            transform.position += direction * (moveSpeed * Time.deltaTime);
            
            yield return null; 
        }
    }
}