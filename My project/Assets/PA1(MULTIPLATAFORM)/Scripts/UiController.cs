using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [Header("elementos a Mover")]
    public List<GameObject> Buttons;
    public List<GameObject> Objects;
    
    [Space(2)]
    
    [Header("Return Button reference")]
    [SerializeField] private Button retturnButton;
    [SerializeField] private Vector2 targetPos;
    
    void Start()
    {
        // Verificamos si las listas tienen el mismo número de elementos para evitar errores.
        if (Buttons.Count != Objects.Count)
        {
            Debug.LogWarning("¡Advertencia! El número de botones y objetos no coincide. Asegúrate de que las listas tengan el mismo tamaño.");
            return; 
        }

        // Asignamos el evento de clic a cada botón de la lista.
        for (int i = 0; i < Buttons.Count; i++)
        {
            // Usamos una variable temporal para el índice en la lambda.
            int index = i;
            Button button = Buttons[i].GetComponent<Button>();
            
            if (button != null)
            {
                // El clic solo llamará al método para mover el objeto 3D correspondiente.
                button.onClick.AddListener(() => MoveCorrespondingObject(index));
            }
        }
        
        // Asignamos el evento de clic al botón de "regresar".
        if (retturnButton != null)
        {
            retturnButton.onClick.AddListener(ReturnAllObjects);
        }
    }

    /// <summary>
    /// Mueve el objeto 3D correspondiente al botón que fue presionado.
    /// </summary>
    /// <param name="index">El índice del objeto en la lista.</param>
    private void MoveCorrespondingObject(int index)
    {
        MovementPattern movementPattern = Objects[index].GetComponent<MovementPattern>();
        
        if (movementPattern != null)
        {
            movementPattern.MoveObject();
        }
    }

    /// <summary>
    /// Mueve todos los objetos de la lista a su posición original.
    /// </summary>
    public void ReturnAllObjects()
    {
        // Recorremos todos los objetos en la lista de móviles
        foreach (GameObject obj in Objects)
        {
            MovementPattern movementPattern = obj.GetComponent<MovementPattern>();
            
            if (movementPattern != null)
            {
                // Llamamos a MoveObject() para que el objeto se mueva a su posición original.
                // El script MovementPattern.cs maneja la lógica para saber si el objeto está arriba o abajo.
                movementPattern.iLikeMoveUp = false;
                movementPattern.MoveObject();
            }
        }

        foreach (GameObject obj in Buttons)
        {
            ButtonAnimator buttonAnimator = obj.GetComponent<ButtonAnimator>();
            if (buttonAnimator != null)
            {
                buttonAnimator.AMove(buttonAnimator.originalPosition, 1f);

            }
        }
    }    
}
