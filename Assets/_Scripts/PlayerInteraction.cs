using UnityEngine;
using UnityEngine.UI; // Importante para controlar la UI

public class PlayerInteraction : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float interactionDistance = 3f;
    public LayerMask interactableLayer = ~0;

    [Header("UI de Interacción")]
    [Tooltip("Arrastra aquí el objeto 'InteractionPrompt' del Canvas")]
    public GameObject uiPrompt; // La referencia al círculo con la E

    private void Update()
    {
        // --------------------------------------------------------
        // FIX DE PAUSA: Si el juego está pausado, apagamos la "E"
        // y nos salimos del Update para no detectar clics ni rayos.
        // --------------------------------------------------------
        if (MenuPausa.JuegoPausado)
        {
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(false); // Forzamos que se apague
            }
            return;
        }

        // 1. SIEMPRE VERIFICAMOS SI ESTAMOS MIRANDO ALGO INTERACTUABLE
        CheckForInteractable();

        // Al presionar la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Lanzamos el rayo para ver si hay algo
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            // Verificamos si el objeto tiene CUALQUIERA de nuestros scripts interactuables
            bool esInteractuable = hit.collider.GetComponentInParent<Door>() != null ||
                       hit.collider.GetComponentInParent<KeycardReader>() != null ||
                       hit.collider.GetComponentInParent<KeycardPickup>() != null ||
                       hit.collider.GetComponentInParent<ProjectorInteractable>() != null;

            // Si es interactuable, mostramos la UI. Si no, la ocultamos.
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(esInteractuable);
            }
        }
        else
        {
            // Si el rayo no choca con nada, ocultamos la UI
            if (uiPrompt != null)
            {
                uiPrompt.SetActive(false);
            }
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

            Debug.Log("El rayo golpeó a: " + hit.collider.name);

            // 1. Obtener el inventario del jugador (asumiendo que este script está en el hijo de la cámara o el padre del jugador)
            PlayerInventory inventory = GetComponentInParent<PlayerInventory>();
            if (inventory == null) inventory = GetComponentInChildren<PlayerInventory>();


            // NUEVO: Detección para recoger la Credencial Física
            KeycardPickup pickup = hit.collider.GetComponentInParent<KeycardPickup>();
            if (pickup != null)
            {
                // Llama a la función de interacción de la tarjeta y le pasa el inventario
                pickup.Interact(inventory);
                return; // Terminamos la interacción aquí para no intentar abrir puertas al mismo tiempo
            }

            // Detección de Lector de Tarjeta
            KeycardReader lector = hit.collider.GetComponentInParent<KeycardReader>();
            if (lector != null)
            {
                // Pasa el inventario que obtuvimos al principio de TryInteract
                lector.EscanearTarjeta(inventory);
            }

            // Detección del Proyector
            ProjectorInteractable proyector = hit.collider.GetComponentInParent<ProjectorInteractable>();
            if (proyector != null)
            {
                proyector.Interact();
                return;
            }
        }
    }
}