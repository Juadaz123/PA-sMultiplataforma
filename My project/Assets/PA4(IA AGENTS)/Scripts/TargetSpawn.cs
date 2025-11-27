using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour {

    [SerializeField] private Grid gridReference;
    [SerializeField] private float timeInterval = 5f;
    public Transform targetToMove;

    void Start() {
        StartCoroutine(MoveTargetRoutine());
    }

    IEnumerator MoveTargetRoutine() {
        while (true) {
            yield return new WaitForSeconds(timeInterval);

            if (gridReference != null && gridReference.grid != null) {
                
                Node randomNode;
                int maxX = gridReference.grid.GetLength(0); // Tamaño X del grid
                int maxY = gridReference.grid.GetLength(1); // Tamaño Y del grid
                int attempts = 0;

                do {
                    int randomX = Random.Range(0, maxX);
                    int randomY = Random.Range(0, maxY);
                    randomNode = gridReference.grid[randomX, randomY];
                    
                    attempts++;
                    if (attempts > 100) break; 

                } while (!randomNode.walkable);

                if (randomNode.walkable) {
                    Vector3 newPosition = randomNode.worldPosition;
                    newPosition.y = targetToMove.position.y; 
                    targetToMove.position = newPosition;
                }
            }
        }
    }
}