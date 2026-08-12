using UnityEngine;

public class KeycardReader : MonoBehaviour
{
    [Header("Referencia a la Puerta")]
    public SlidingDoor puertaCorrediza;

    [Header("Estado de la Credencial")]
    [Tooltip("Marca esto si el jugador ya recogió la tarjeta para probar")]
    public bool tieneTarjeta = true;

    public void EscanearTarjeta(PlayerInventory inventory)
    {
        // Verifica si el objeto inventario existe Y si tiene la tarjeta
        if (inventory != null && inventory.tieneCredencialEscolar)
        {
            Debug.Log("💳 ¡Credencial Aceptada! Abriendo puerta de acceso...");
            if (puertaCorrediza != null)
            {
                puertaCorrediza.AbrirPuerta();
            }
        }
        else
        {
            Debug.Log("❌ Acceso Denegado: Requiere Credencial Escolar Física.");
            // Aquí podrías poner un sonido de error
        }
    }

}