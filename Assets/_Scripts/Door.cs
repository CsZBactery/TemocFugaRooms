using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [Tooltip("Ángulo hacia el que girará la puerta al abrirse (ej: 90 o -90)")]
    public float openAngle = 90f;

    [Tooltip("Velocidad de movimiento al abrir o cerrar")]
    public float speed = 3f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine animateCoroutine;

    void Start()
    {
        // Guarda la rotación inicial cerrada
        closedRotation = transform.localRotation;
        // Calcula la rotación destino sumando el ángulo en el eje Y
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    // Esta función la invoca PlayerInteraction cuando presionas 'E'
    public void Interact()
    {
        isOpen = !isOpen;

        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
        }

        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        animateCoroutine = StartCoroutine(AnimateDoor(targetRotation));

        Debug.Log(isOpen ? "Puerta Abierta" : "Puerta Cerrada");
    }

    public void AbrirPorGuardia()
    {
        if (!isOpen)
        {
            Interact(); // Solo la abre si está cerrada
        }
    }

    private IEnumerator AnimateDoor(Quaternion target)
    {
        while (Quaternion.Angle(transform.localRotation, target) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * speed);
            yield return null;
        }

        transform.localRotation = target;
    }
}