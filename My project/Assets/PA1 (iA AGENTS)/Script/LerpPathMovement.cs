using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

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
                StartCoroutine(LerpPath(0));
                break;
            case 3:
                Lerp3();
                break;
            
            case 4:
                Lerp4();
                break;
        }
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
    // Si ya terminamos todo el recorrido, no hacemos nada más.
    if (_iFinishHim) return;

    // --- ESTADO 1: ESPERANDO EN UN WAYPOINT ---
    if (isWaiting)
    {
        // Acumulamos el tiempo transcurrido en nuestro temporizador.
        _waitTimer += Time.deltaTime;

        // Comprobamos si ya hemos esperado el tiempo suficiente.
        if (_waitTimer >= waitedTime)
        {
            // Si ya esperamos, reiniciamos todo para el siguiente movimiento.
            isWaiting = false; // Dejamos de esperar.
            _waitTimer = 0f;    // Reiniciamos el temporizador.
            _progress = 0f;     // Reiniciamos el progreso para el nuevo segmento.
            _currentSegment++;  // Apuntamos al siguiente waypoint.
        }
    }
    // --- ESTADO 2: MOVIÉNDONOS HACIA EL SIGUIENTE WAYPOINT ---
    else
    {
        // Primero, verificamos si aún quedan segmentos por recorrer.
        if (_currentSegment >= waypoint.Count - 1)
        {
            _iFinishHim = true; // Marcamos que hemos terminado.
            FInalMessage();
            return;
        }
        
        // Calculamos la distancia del segmento actual para mantener una velocidad constante.
        float segmentLength = Vector3.Distance(waypoint[_currentSegment], waypoint[_currentSegment + 1]);
        // Incrementamos el progreso del movimiento.
        _progress += (Time.deltaTime * speed) / segmentLength;

        // Movemos el objeto usando Lerp.
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
        // Verificamos si hemos llegado al final de la ruta.
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
    
    
    private IEnumerator LerpPath(float waitDuration)
    {
        for (int i = 0; i < waypoint.Count - 1; i++)
        {
            yield return StartCoroutine(MoveToWaypoint(waypoint[i], waypoint[i + 1]));
        }
        
        for (int i = waypoint.Count - 1; i > 0; i--)
        {
            yield return StartCoroutine(MoveToWaypoint(waypoint[i], waypoint[i - 1]));
        }
        FInalMessage(); 
        StopAllCoroutines();
    }

    private IEnumerator MoveToWaypoint(Vector3 startPoint, Vector3 endPoint)
    {
        float journeyLength = Vector3.Distance(startPoint, endPoint);
        float startTime = Time.deltaTime;
        float progress = 0;

        while (progress < 1)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            progress = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(startPoint, endPoint, progress);
            yield return null;
        }
    
        transform.position = endPoint; 
    }

}