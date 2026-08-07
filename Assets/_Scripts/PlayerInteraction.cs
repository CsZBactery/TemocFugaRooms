using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    [Tooltip("Distancia máxima en metros a la que el jugador puede interactuar")]
    public float interactionDistance = 3f;

    [Tooltip("Capa de objetos con los que puede interactuar")]
    public LayerMask interactableLayer = ~0; // Todo por defecto

    void Update()
    {
        // Al presionar la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        // Rayo desde el centro de la cámara mirando hacia adelante
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Dibuja una línea roja en la pestaña Scene para ver el rayo
        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red, 2f);

        // Si el rayo choca con algo dentro del rango
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            Debug.Log("El rayo golpeó a: " + hit.collider.name);

            // 1. Busca el script 'Door' hacia arriba (Padre)
            Door door = hit.collider.GetComponentInParent<Door>();

            // 2. Si no lo encuentra arriba, busca hacia abajo (Hijo)
            if (door == null)
            {
                door = hit.collider.GetComponentInChildren<Door>();
            }

            // 3. Si lo encuentra en cualquier lado, abre la puerta
            if (door != null)
            {
                door.Interact();
            }

            // Detección de Lector de Tarjeta
            KeycardReader lector = hit.collider.GetComponentInParent<KeycardReader>();
            if (lector != null)
            {
                lector.EscanearTarjeta();
            }
        }
    }
}