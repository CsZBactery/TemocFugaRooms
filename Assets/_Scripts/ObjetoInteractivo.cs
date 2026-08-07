using UnityEngine;

public class ObjetoInteractivo : MonoBehaviour
{
    [Tooltip("El texto que aparecerá en el panel para este objeto específico")]
    public string mensajePersonalizado = "Abrir Puerta";

    // Esta función se llamará cuando el jugador presione la 'E'
    public void Interactuar()
    {
        // Aquí irá la lógica de abrir la puerta, agarrar un objeto, etc.
        Debug.Log("Has interactuado con: " + gameObject.name);
    }
}