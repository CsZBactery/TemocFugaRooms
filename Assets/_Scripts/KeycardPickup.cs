using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [Header("Icono para el Inventario")]
    public Sprite iconoTarjeta; // Arrastra el dibujo de la tarjeta transparente aquí en el Inspector

    // Función que llamará PlayerInteraction al presionar E
    public void Interact(PlayerInventory playerInv)
    {
        if (playerInv != null)
        {
            // Le dice al inventario del jugador que ya tiene la tarjeta Y le manda la imagen
            playerInv.RecogerCredencial(iconoTarjeta);

            // Desaparece la tarjeta física del mundo
            Destroy(gameObject);
        }
    }
}