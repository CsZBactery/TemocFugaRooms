using UnityEngine;
using TMPro; // Borra esta línea si usas Text normal de Unity (Legacy)
// using UnityEngine.UI; // Descomenta esta línea si usas Text normal (Legacy)

public class DetectorInteraccion : MonoBehaviour
{
    [Header("Configuración del Raycast")]
    public float distanciaInteraccion = 3f; // Qué tan cerca debes estar de la puerta

    [Header("Referencias de UI")]
    public GameObject panelInteraccion;
    public TextMeshProUGUI textoInteraccion; // Cambia a "public Text textoInteraccion;" si usas el texto viejo de Unity

    private ObjetoInteractivo objetoActual;

    void Start()
    {
        // Asegurarnos de que el panel empiece apagado
        if (panelInteraccion != null)
        {
            panelInteraccion.SetActive(false);
        }
    }

    void Update()
    {
        // Disparamos un rayo desde el centro de la cámara hacia adelante
        Ray rayo = new Ray(transform.position, transform.forward);
        RaycastHit golpe;

        // Si el rayo choca con algo a nuestra distancia máxima de interacción...
        if (Physics.Raycast(rayo, out golpe, distanciaInteraccion))
        {
            // Verificamos si lo que golpeamos tiene el script de "ObjetoInteractivo"
            ObjetoInteractivo interactuable = golpe.collider.GetComponent<ObjetoInteractivo>();

            if (interactuable != null)
            {
                // Si encontramos uno, lo guardamos y mostramos el UI
                objetoActual = interactuable;
                panelInteraccion.SetActive(true);

                // Actualizamos el texto con el mensaje de ese objeto (Ej: "[E] Abrir Puerta")
                if (textoInteraccion != null)
                {
                    textoInteraccion.text = "[E] " + interactuable.mensajePersonalizado;
                }

                // Detectamos si presiona la tecla E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    objetoActual.Interactuar();
                }
            }
            else
            {
                // Si chocamos con algo que NO es interactuable (ej. una pared normal)
                ApagarUI();
            }
        }
        else
        {
            // Si no estamos mirando a nada de cerca
            ApagarUI();
        }
    }

    // Función auxiliar para apagar el UI y limpiar la referencia
    private void ApagarUI()
    {
        if (objetoActual != null)
        {
            panelInteraccion.SetActive(false);
            objetoActual = null;
        }
    }
}