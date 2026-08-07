using UnityEngine;

public class KeycardReader : MonoBehaviour
{
    [Header("Referencia a la Puerta")]
    public SlidingDoor puertaCorrediza;

    [Header("Estado de la Credencial")]
    [Tooltip("Marca esto si el jugador ya recogió la tarjeta para probar")]
    public bool tieneTarjeta = true;

    public void EscanearTarjeta()
    {
        if (tieneTarjeta)
        {
            Debug.Log("💳 ¡Credencial Aceptada! Abriendo puerta de acceso...");
            if (puertaCorrediza != null)
            {
                puertaCorrediza.AbrirPuerta();
            }
        }
        else
        {
            Debug.Log("❌ Acceso Denegado: Requiere Credencial Escolar.");
        }
    }
}