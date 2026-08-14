using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Iconos de los Recuadros")]
    [Tooltip("Arrastra aquí los 3 objetos 'IconoSlot' (los hijos que tienen la imagen transparente)")]
    public Image[] iconosSlots;

    // Esta función se llamará desde tu script PlayerInventory cuando recojas algo
    public void AgregarIcono(Sprite spriteObjeto)
    {
        // Recorremos los 3 slots para buscar el primero que esté vacío
        for (int i = 0; i < iconosSlots.Length; i++)
        {
            // Si el slot actual no tiene un sprite asignado (está vacío)
            if (iconosSlots[i].sprite == null)
            {
                iconosSlots[i].sprite = spriteObjeto; // Le ponemos la imagen del objeto
                iconosSlots[i].color = new Color(1f, 1f, 1f, 1f); // Lo hacemos 100% visible
                return; // Terminamos la función para que no llene todos los slots a la vez
            }
        }

        Debug.Log("El inventario visual ya está lleno. No hay más espacio en la UI.");
    }
}