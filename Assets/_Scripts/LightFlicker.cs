using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("El foco (Light) que va a parpadear")]
    public Light focoLuz;

    [Tooltip("Opcional: El objeto del foco físico si tiene un material que brille (emisión)")]
    public MeshRenderer mallaFoco;

    [Header("Configuración del Parpadeo")]
    [Tooltip("Probabilidad de que la luz se quede encendida (0.0 a 1.0). Mayor valor = más tiempo prendida.")]
    [Range(0.1f, 0.9f)] public float probabilidadEncendido = 0.8f;

    [Tooltip("Tiempo mínimo que dura un estado (prendido o apagado)")]
    public float tiempoMinimo = 0.05f;

    [Tooltip("Tiempo máximo que dura un estado (prendido o apagado)")]
    public float tiempoMaximo = 0.4f;

    private Material materialFoco;
    private bool estaParpadeando = true;

    void Start()
    {
        // Si no asignaste el Light en el inspector, intenta buscarlo en el mismo objeto
        if (focoLuz == null) focoLuz = GetComponent<Light>();
        if (focoLuz == null) focoLuz = GetComponentInChildren<Light>();

        // Si hay un MeshRenderer asignado, extraemos su material para controlar el brillo visual
        if (mallaFoco != null)
        {
            materialFoco = mallaFoco.material;
        }

        // Iniciamos el ciclo infinito de parpadeo aleatorio
        StartCoroutine(FlicekerRoutine());
    }

    private IEnumerator FlicekerRoutine()
    {
        while (estaParpadeando)
        {
            // Calculamos un estado aleatorio basado en la probabilidad
            bool encender = Random.value < probabilidadEncendido;

            // Cambiamos el estado del componente Light
            if (focoLuz != null) focoLuz.enabled = encender;

            // Controlamos el brillo del material (Emisión) para que combine con el foco
            if (materialFoco != null)
            {
                if (encender)
                    materialFoco.EnableKeyword("_EMISSION");
                else
                    materialFoco.DisableKeyword("_EMISSION");
            }

            // Esperamos un tiempo completamente aleatorio antes del siguiente cambio
            float tiempoEspera = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(tiempoEspera);
        }
    }
}