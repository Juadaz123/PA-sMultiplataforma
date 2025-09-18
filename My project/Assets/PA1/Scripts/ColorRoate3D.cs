using System.Collections;
using UnityEngine;

public class ColorRoate3D : MonoBehaviour
{
    
    [Header("3d parammeters")]
    private Renderer _rendererObject;
    [SerializeField] private Color startColor;
    [SerializeField] private float speedRotation = 20f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //inicializa la rotacion y cambia el color al iniciar la escena
        _rendererObject = gameObject.GetComponent<Renderer>();
        _rendererObject.material.color = startColor;

        StartCoroutine(ObjectRotation());

    }

    private IEnumerator ObjectRotation()
    {
        while (true)
        {
            transform.RotateAround(transform.position, Vector3.up, speedRotation * Time.deltaTime);
            yield return null;  
        }
    }

    
}
