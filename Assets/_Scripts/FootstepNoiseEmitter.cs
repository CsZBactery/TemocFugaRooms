using UnityEngine;

public class FootstepNoiseEmitter : MonoBehaviour
{
    [Header("Configuración del Rechinido")]
    public float radioDeRuido = 12f; // Distancia máxima a la que el Guardia te escuchará
    public float tiempoEntrePasos = 0.4f; // Qué tan rápido suenan las pisadas al correr
    public AudioSource audioSource;
    public AudioClip sfxRechinidoTenis; // Tu sonido de rechinido

    // Reemplaza esto con la referencia o variable que ya uses para saber si corres
    // (Por ejemplo, puedes conectarla con tu script de movimiento actual)
    [HideInInspector] public bool estaCorriendo = false;
    [HideInInspector] public bool estaAgachado = false;

    private CharacterController controller;
    private float timerPasos;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Verificar si el jugador se está moviendo físicamente en el suelo
        bool seEstaMoviendo = controller != null && controller.velocity.magnitude > 0.1f && controller.isGrounded;

        if (seEstaMoviendo && estaCorriendo && !estaAgachado)
        {
            timerPasos += Time.deltaTime;
            if (timerPasos >= tiempoEntrePasos)
            {
                EmitirRechinido();
                timerPasos = 0f;
            }
        }
        else
        {
            timerPasos = tiempoEntrePasos; // Resetea el timer si se detiene
        }
    }

    void EmitirRechinido()
    {
        // 1. Reproducir el efecto de sonido agudo de tenis
        if (audioSource != null && sfxRechinidoTenis != null)
        {
            audioSource.PlayOneShot(sfxRechinidoTenis);
        }

        // 2. Alerta invisible para la Inteligencia Artificial en el área
        Collider[] enemigosCercanos = Physics.OverlapSphere(transform.position, radioDeRuido);
        foreach (Collider col in enemigosCercanos)
        {
            // Busca si hay un enemigo en el radio que tenga el script de oído de IA
            GuardiaAI oidoEnemigo = col.GetComponentInParent<GuardiaAI>();
            if (oidoEnemigo != null)
            {
                // Le pasa la posición exacta de tus tenis a la IA
                oidoEnemigo.EscucharRuido(transform.position);
            }
        }
    }

    // Dibujar el círculo de ruido en la pestaña Scene para que puedas calibrar el tamaño
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioDeRuido);
    }
}