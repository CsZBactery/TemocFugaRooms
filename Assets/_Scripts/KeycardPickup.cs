using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    // Función que llamará PlayerInteraction al presionar E
    public void Interact(PlayerInventory playerInv)
    {
        if (playerInv != null)
        {
            // Le dice al inventario del jugador que ya tiene la tarjeta
            playerInv.RecogerCredencial();

            // Desaparece la tarjeta física del mundo
            Destroy(gameObject);
        }
    }
}