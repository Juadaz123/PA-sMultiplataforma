using UnityEngine;
using UnityEngine.Pool;
using System.Collections;

public class Enemies : MonoBehaviour
{
    public IObjectPool<Enemies> ObjectPool { get; set; }

    [Header("Movement Parameters")]
    [SerializeField]
    private float speed = 2.0f; 

    [Header("Spawn Settings")]
    [SerializeField]
    private float initialDelay = 3.0f; 

    private Transform _target;
    private Coroutine _movementCoroutine;
    private ScoreManager _scoreManager;

    private void OnEnable()
    {
        _scoreManager = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
        
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        
        if (playerGO != null)
        {
            _target = playerGO.transform;
            _movementCoroutine = StartCoroutine(MovementToTarget(_target));
        }
        else
        {
            Debug.LogError("Player con la etiqueta 'Player' no encontrado. El enemigo no se moverá.");
        }
    }

    private void OnDisable()
    {
        if (_movementCoroutine != null)
        {
            StopCoroutine(_movementCoroutine);
        }
        _target = null;
        
        if (ObjectPool != null)
        {
            ObjectPool.Release(this);
        }
    }

    private IEnumerator MovementToTarget(Transform targetTransform)
    {
        if (initialDelay > 0)
        {
            yield return new WaitForSeconds(initialDelay);
        }
        
        while (targetTransform && gameObject.activeInHierarchy)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetTransform.position, 
                speed * Time.deltaTime
            );
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null )
        {
            gameObject.SetActive(false);
        }

        if (other.GetComponent<Bullet>() != null)
        {
            gameObject.SetActive(false);
            _scoreManager.AddScore(10);
        }
        
    }
}