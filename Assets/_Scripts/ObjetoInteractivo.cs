using UnityEngine;
using UnityEngine.Events;

public class ObjetoInteractivo : MonoBehaviour
{
    [Header("Mensaje UI")]
    [Tooltip("El texto que saldrá en pantalla. Ej: 'Abrir Puerta'")]
    public string mensajePersonalizado = "Interactuar";

    [Header("Evento de Interacción")]
    [Tooltip("La función que se ejecutará al presionar E")]
    public UnityEvent alInteractuar;

    public void Interactuar()
    {
        // Llama a la función asignada desde el Inspector
        if (alInteractuar != null)
        {
            alInteractuar.Invoke();
        }
    }
}