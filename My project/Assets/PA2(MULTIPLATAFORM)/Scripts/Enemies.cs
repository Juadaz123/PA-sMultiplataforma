using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Enemies : MonoBehaviour
{
    public IObjectPool<Enemies> ObjectPool { get; set; }

    [Header("Movement Parameters")]
    [SerializeField]
    private float duration = 2.0f; 

    [SerializeField] private Transform _target;
    
    private Coroutine _movementCoroutine; 

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    private void OnEnable()
    {
        StartCoroutine(MovementObject(_target.position));
    }

    private void OnDisable()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
            _movementCoroutine = null;
        }
    }

    private IEnumerator MovementObject(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float t = 0f;

        while (t < duration)
        {
            float p = t / duration;
            
            p = Mathf.SmoothStep(0f, 1f, p);
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            
            t += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition; 

        if (ObjectPool != null)
        {
            ObjectPool.Release(this);
        }
    }
}