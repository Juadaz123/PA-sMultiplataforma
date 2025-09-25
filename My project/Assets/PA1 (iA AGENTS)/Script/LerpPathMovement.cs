using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpPathMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private List<Vector3> waypoint = new List<Vector3>();
    [SerializeField, Range(1, 10)] private int speed = 5, waitedTime = 0;
    [SerializeField, Range(1, 4)] private int patternNumber;

    private int _currentSegment;
    private bool _iFinishHim, isWaiting;
    private float _progress;
    private float _waitTimer = 0f;

    // --- NUEVA VARIABLE ---
    // Agregamos una bandera para asegurarnos de que la corutina solo se inicie una vez.
    private bool _coroutineStarted = false;

    private void Start()
    {
        Debug.Log($"Patron seleccionado {patternNumber}");
        if (waypoint.Count < 2)
        {
            waypoint.Add(transform.position);
            waypoint.Add(transform.position + Vector3.forward * 10);
        }
        transform.position = waypoint[0];
    }

    private void Update()
    {
        switch (patternNumber)
        {
            case 1:
                Lerp1();
                break;
            case 2:
                // --- CORRECCIÓN PRINCIPAL ---
                // Solo iniciamos la corutina si no ha sido iniciada antes.
                if (!_coroutineStarted)
                {
                    StartCoroutine(LerpPath());
                    _coroutineStarted = true; // Marcamos que ya se inició.
                }
                break;
            case 3:
                Lerp3();
                break;
            
            case 4:
                Lerp4();
                break;
        }
    }

    
    private IEnumerator LerpPath()
    {
        while (true)
        {
            for (int i = 0; i < waypoint.Count - 1; i++)
            {
                yield return StartCoroutine(MoveToWaypoint(waypoint[i], waypoint[i + 1]));
            }
            
            // Recorrido de VUELTA (del final al inicio)
            for (int i = waypoint.Count - 1; i > 0; i--)
            {
                yield return StartCoroutine(MoveToWaypoint(waypoint[i], waypoint[i - 1]));
            }
            // Ya no es necesario StopAllCoroutines() porque el bucle while(true) se encarga de repetir.
        }
    }

    // --- FUNCIÓN CORREGIDA ---
    private IEnumerator MoveToWaypoint(Vector3 startPoint, Vector3 endPoint)
    {
        float journeyLength = Vector3.Distance(startPoint, endPoint);
        if (journeyLength <= 0)
        {
            yield break;
        }

        float startTime = Time.time;
        float progress = 0;

        while (progress < 1)
        {
            float timeSinceStarted = Time.time - startTime;
            progress = (timeSinceStarted * speed) / journeyLength;
            
            transform.position = Vector3.Lerp(startPoint, endPoint, progress);


            yield return null;
        }
    
        transform.position = endPoint;
    }

    private void Lerp3()
    {
        _iFinishHim  = false;
        if (_currentSegment >= waypoint.Count - 1)
        {
            FInalMessage();   
            _iFinishHim = true;
            return;
        }
        float segmentLength = Vector3.Distance(waypoint[_currentSegment], waypoint[_currentSegment + 1]);
        _progress += (Time.deltaTime * speed) / segmentLength;

        if (_progress >= 1f)
        {
            transform.position = waypoint[_currentSegment + 1];
            _currentSegment++;

            _progress = 0f;
            
        }
        
        transform.position = Vector3.Lerp(waypoint[_currentSegment], waypoint[_currentSegment + 1], _progress);
         
    }
    
    private void Lerp4()
    {
        if (_iFinishHim) return;

        if (isWaiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= waitedTime)
            {
                isWaiting = false;
                _waitTimer = 0f;
                _progress = 0f;
                _currentSegment++;
            }
        }
        else
        {
            if (_currentSegment >= waypoint.Count - 1)
            {
                _iFinishHim = true;
                FInalMessage();
                return;
            }
            
            float segmentLength = Vector3.Distance(waypoint[_currentSegment], waypoint[_currentSegment + 1]);
            _progress += (Time.deltaTime * speed) / segmentLength;

            transform.position = Vector3.Lerp(waypoint[_currentSegment], waypoint[_currentSegment + 1], _progress);

            if (_progress >= 1f)
            {
                transform.position = waypoint[_currentSegment + 1];
                isWaiting = true;
            }
        }
    }
 
    private void Lerp1()
    {
        _iFinishHim = false;
        if (_currentSegment >= waypoint.Count - 1)
        {
            _iFinishHim = true;
            _currentSegment = 0;
            return;
        }
        float segmentLength = Vector3.Distance(waypoint[_currentSegment], waypoint[_currentSegment + 1]);
        _progress += (Time.deltaTime * speed) / segmentLength;

        if (_progress >= 1f)
        {
            transform.position = waypoint[_currentSegment + 1];
            _currentSegment++;
            _progress = 0f;
        }

        if (_currentSegment >= waypoint.Count - 1)
        {
            _iFinishHim = true;
            _currentSegment = 0;
            return;
        }
        
        transform.position = Vector3.Lerp(waypoint[_currentSegment], waypoint[_currentSegment + 1], _progress);
    }


    private void FInalMessage()
    {
        print("Patrón terminado reinicia la escena, por favor. :u");
    }

    private void OnDrawGizmos()
    {
        if (waypoint.Count == 0) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < waypoint.Count; i++)
        {
            Gizmos.DrawSphere(waypoint[i], 0.2f);
            if (i < waypoint.Count - 1) Gizmos.DrawLine(waypoint[i], waypoint[i + 1]);
        }
    }
}