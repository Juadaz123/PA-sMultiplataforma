using System.Collections;
using UnityEngine;

public class MovementPattern : MonoBehaviour
{
    [Header("movement parameters 3D")]
    private Vector3 _originalPosition;
    private Vector3 _endPosition;
    public bool iLikeMoveUp = true; //true = arriba / false = abajo

    [SerializeField] private float duration = 2f, limitedForY = 15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _originalPosition = transform.position;
        _endPosition = new Vector3(_originalPosition.x, _originalPosition.y + limitedForY, _originalPosition.z);
    }

    // Ahora este método es público para que el botón pueda acceder a él.
    public void MoveObject()
    {
        if (iLikeMoveUp)
        {
            StartCoroutine(MovementObject(_endPosition));
            return;
        }
        StartCoroutine(MovementObject(_originalPosition));
    }

    private IEnumerator MovementObject(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float t = 0f;
        while (t < duration)
        {
            //progrseo de interpolacion 
            float p = t / duration;
            p = Mathf.SmoothStep(0f, 1f, p);
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, p);
            
            t += Time.deltaTime;
            yield return null;
        }

        if (!iLikeMoveUp) iLikeMoveUp = true;
    }
}