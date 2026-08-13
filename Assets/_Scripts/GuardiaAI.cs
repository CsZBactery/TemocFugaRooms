using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardiaAI : MonoBehaviour
{
    [Header("Puntos de Patrullaje")]
    [Tooltip("Arrastra aquí objetos vacíos colocados en los extremos del pasillo")]
    public Transform[] puntosPatrulla;

    [Header("Configuración de Visión y Jugador")]
    public Transform jugador;
    [Tooltip("Distancia máxima a la que el guardia puede verte")]
    public float rangoVision = 10f;
    [Tooltip("Ángulo del cono de visión frontal (ejemplo: 90 grados)")]
    [Range(0, 360)] public float anguloVision = 90f;

    [Header("Velocidades del Guardia")]
    [Tooltip("Velocidad al dar sus rondines normales")]
    public float velocidadPatrulla = 2f;
    [Tooltip("Velocidad de persecución (Un poco más lento que la carrera del jugador)")]
    public float velocidadPersecucion = 3.2f;

    [Header("Tiempos de Búsqueda")]
    [Tooltip("Segundos que busca en tu última posición vista antes de rendirse")]
    public float tiempoBuscandoPerdido = 4f;
    public float tiempoInvestigandoRuido = 3f;

    [Header("Interacción con Puertas")]
    public float distanciaAperturaPuertas = 2.5f;

    private NavMeshAgent agent;
    private int indiceActual = 0;

    // Máquina de estados interna
    private enum EstadoGuardia { Patrullando, InvestigandoRuido, Persiguiendo, BuscandoPerdido }
    private EstadoGuardia estadoActual = EstadoGuardia.Patrullando;

    private Vector3 ultimaPosicionJugador;
    private float timerBusqueda = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidadPatrulla;
        IrAlSiguientePunto();
    }

    void Update()
    {
        // 1. Abrir puertas automáticamente frente a él
        AbrirPuertasCercanas();

        // 2. Verificar en cada cuadro si el Guardia TIENE LÍNEA DE VISIÓN DIRECTA
        bool veAlJugador = DetectarJugadorConVision();

        if (veAlJugador)
        {
            // Te está viendo: Inicia / Mantiene Persecución
            estadoActual = EstadoGuardia.Persiguiendo;
            timerBusqueda = 0f;
            ultimaPosicionJugador = jugador.position;
            agent.speed = velocidadPersecucion;
            agent.destination = jugador.position;
        }
        else if (estadoActual == EstadoGuardia.Persiguiendo)
        {
            // Te dejó de ver (doblaste la esquina): Pasa a buscar en la última posición conocida
            estadoActual = EstadoGuardia.BuscandoPerdido;
            agent.destination = ultimaPosicionJugador;
            Debug.Log("👮 Guardia: ¡Lo perdí de vista! Investigando su última posición...");
        }

        // 3. Manejo de estados de navegación
        switch (estadoActual)
        {
            case EstadoGuardia.Patrullando:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    IrAlSiguientePunto();
                }
                break;

            case EstadoGuardia.InvestigandoRuido:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    timerBusqueda += Time.deltaTime;
                    if (timerBusqueda >= tiempoInvestigandoRuido)
                    {
                        VolverAPatrullar();
                    }
                }
                break;

            case EstadoGuardia.BuscandoPerdido:
                // Llegó a la última posición donde te vio
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    timerBusqueda += Time.deltaTime;
                    if (timerBusqueda >= tiempoBuscandoPerdido)
                    {
                        VolverAPatrullar(); // Se rinde y vuelve a sus rondines
                    }
                }
                break;

            case EstadoGuardia.Persiguiendo:
                // Se actualiza en la sección de visión
                break;
        }
    }

    // --- SISTEMA DE VISIÓN (Cono de Visión y Raycast) ---
    private bool DetectarJugadorConVision()
    {
        if (jugador == null) return false;

        Vector3 direccionHaciaJugador = (jugador.position - transform.position).normalized;
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // A) ¿Está dentro del rango de distancia?
        if (distancia <= rangoVision)
        {
            // B) ¿Está dentro del cono de visión frontal?
            float angulo = Vector3.Angle(transform.forward, direccionHaciaJugador);
            if (angulo <= anguloVision / 2f)
            {
                // C) Lanzar Raycast para verificar que no haya paredes tapando la vista
                RaycastHit hit;
                Vector3 origenOjos = transform.position + Vector3.up * 1.5f; // Altura de los ojos
                Vector3 destinoJugador = jugador.position + Vector3.up * 1.0f; // Altura del torso

                if (Physics.Raycast(origenOjos, (destinoJugador - origenOjos).normalized, out hit, rangoVision))
                {
                    // Si el rayo choca con el jugador o uno de sus componentes
                    if (hit.transform == jugador || hit.transform.IsChildOf(jugador))
                    {
                        return true; // ¡Confirmado: Te está viendo!
                    }
                }
            }
        }
        return false;
    }

    private void VolverAPatrullar()
    {
        estadoActual = EstadoGuardia.Patrullando;
        timerBusqueda = 0f;
        agent.speed = velocidadPatrulla;
        IrAlSiguientePunto();
        Debug.Log("👮 Guardia: Sin novedades, volviendo al patrullaje.");
    }

    void IrAlSiguientePunto()
    {
        if (puntosPatrulla == null || puntosPatrulla.Length == 0) return;

        agent.destination = puntosPatrulla[indiceActual].position;
        indiceActual = (indiceActual + 1) % puntosPatrulla.Length;
    }

    // Escuchar el rechinido de loseta
    public void EscucharRuido(Vector3 posicionDelRuido)
    {
        // Solo investiga si no está persiguiendo activamente al jugador
        if (estadoActual != EstadoGuardia.Persiguiendo)
        {
            Debug.Log("👮 Guardia: ¡Escuché un rechinido!");
            estadoActual = EstadoGuardia.InvestigandoRuido;
            timerBusqueda = 0f;
            agent.speed = velocidadPatrulla;
            agent.destination = posicionDelRuido;
        }
    }

    // Apertura automática de puertas
    private void AbrirPuertasCercanas()
    {
        Vector3 centroDeteccion = transform.position + transform.forward * 1f;
        Collider[] objetosDetectados = Physics.OverlapSphere(centroDeteccion, distanciaAperturaPuertas);

        foreach (Collider col in objetosDetectados)
        {
            SlidingDoor puertaCorrediza = col.GetComponentInParent<SlidingDoor>();
            if (puertaCorrediza != null)
            {
                puertaCorrediza.AbrirPuerta();
            }

            Door puertaNormal = col.GetComponentInParent<Door>();
            if (puertaNormal != null)
            {
                puertaNormal.AbrirPorGuardia();
            }
        }
    }

    // Gizmos visuales para calibrar en el editor
    private void OnDrawGizmosSelected()
    {
        // Esfera de apertura de puertas (Azul)
        Gizmos.color = Color.blue;
        Vector3 centroDeteccion = transform.position + transform.forward * 1f;
        Gizmos.DrawWireSphere(centroDeteccion, distanciaAperturaPuertas);

        // Rango de visión (Rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoVision);
    }
}