using System.Collections;
using UnityEngine;

public class ProjectorInteractable : MonoBehaviour
{
    [Header("Efectos de Proyección")]
    public GameObject mensajePizarron;
    public Light luzProyector;

    [Header("Efecto Terror (Parpadeo)")]
    [Tooltip("¿Quieres que el mensaje parpadee de forma terrorífica?")]
    public bool activarParpadeo = true;
    public float minFlickerDelay = 0.05f;
    public float maxFlickerDelay = 0.35f;

    [Header("Audio del Proyector")]
    public AudioSource audioSource;

    private bool estaEncendido = false;
    private Coroutine corrutinaParpadeo;

    void Start()
    {
        // Si no asignaste el AudioSource manualmente, intenta buscarlo en el proyector
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        estaEncendido = !estaEncendido;

        if (estaEncendido)
        {
            // 1. Reproducir sonido en bucle
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.loop = true; // Para que suene contínuo mientras esté encendido
                audioSource.Play();
            }

            // 2. Iniciar el parpadeo de terror o encendido fijo
            if (activarParpadeo)
            {
                if (corrutinaParpadeo != null) StopCoroutine(corrutinaParpadeo);
                corrutinaParpadeo = StartCoroutine(FlickerRoutine());
            }
            else
            {
                SetEstadoProyeccion(true);
            }
        }
        else
        {
            // 1. Detener el sonido
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            // 2. Detener el parpadeo y apagar todo
            if (corrutinaParpadeo != null)
            {
                StopCoroutine(corrutinaParpadeo);
            }

            SetEstadoProyeccion(false);
        }

        Debug.Log(estaEncendido ? "📽️ Proyector Encendido" : "📽️ Proyector Apagado");
    }

    private void SetEstadoProyeccion(bool estado)
    {
        if (mensajePizarron != null) mensajePizarron.SetActive(estado);
        if (luzProyector != null) luzProyector.enabled = estado;
    }

    private IEnumerator FlickerRoutine()
    {
        while (estaEncendido)
        {
            // Parpadeo con 75% de probabilidades de estar encendido y 25% de apagado rápido
            bool estadoTemporal = Random.value > 0.25f;
            SetEstadoProyeccion(estadoTemporal);

            // Espera un tiempo aleatorio entre destellos
            yield return new WaitForSeconds(Random.Range(minFlickerDelay, maxFlickerDelay));
        }
    }
}