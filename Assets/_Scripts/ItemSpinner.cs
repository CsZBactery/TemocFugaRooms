using UnityEngine;

public class ItemSpinner : MonoBehaviour
{
    [Header("Configuración del Giro")]
    [Tooltip("Velocidad a la que girará la tarjeta")]
    public float rotationSpeed = 60f;

    [Tooltip("Eje sobre el que va a girar (Eje Y es el vertical)")]
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        // Gira el objeto suavemente en cada frame
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.World);
    }
}