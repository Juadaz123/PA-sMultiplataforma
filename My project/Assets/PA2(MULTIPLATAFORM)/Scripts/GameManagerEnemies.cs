using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class GameManagerEnemies : MonoBehaviour
{
    public static GameManagerEnemies Instance {get; private set;}
    
    [Header("Pooling Parameters")]
    [SerializeField] private Enemies enemies;
    [SerializeField] private int defaultPoolSize = 5, maxPoolSize = 20;
    
    [Header("Spawn and Target")]
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private Transform playerTransform;
    
    private bool _collectionCheck = true;
    
    private IObjectPool<Enemies> _enemiesPool;
    
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

        _enemiesPool = new ObjectPool<Enemies>(
            CreateEnemies,
            OnGetFromPool,
            onRelasePool,
            onDestroyPoolObject,
            _collectionCheck,
            defaultPoolSize,
            maxPoolSize
        );
    }

    private Enemies CreateEnemies()
    {
        Enemies enemiesInstance = Instantiate(enemies);
        enemiesInstance.ObjectPool = _enemiesPool;
        return enemiesInstance;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            return Vector3.zero;
        }

        int randomIndex = Random.Range(0, spawnPositions.Count);
        return spawnPositions[randomIndex].position;
    }

    private void OnGetFromPool(Enemies obj)
    {
        obj.gameObject.SetActive(true);
        
        obj.transform.position = GetRandomSpawnPosition(); 

        if (playerTransform != null)
        {
            obj.SetTarget(playerTransform); 
        }
    }

    private void onRelasePool(Enemies obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void onDestroyPoolObject(Enemies obj)
    {
        Destroy(obj.gameObject);
    }
}