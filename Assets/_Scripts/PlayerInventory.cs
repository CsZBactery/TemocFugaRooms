using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Conexión con UI")]
    [Tooltip("Arrastra aquí tu objeto UIManager de la jerarquía")]
    public InventoryUIManager uiManager;

    [Header("Objetos en Inventario")]
    // Esta variable guardará si tenemos la tarjeta o no (Se mantiene igual)
    public bool tieneCredencialEscolar = false;

    // Modificamos la función para que ahora reciba la imagen (Sprite) de la tarjeta
    public void RecogerCredencial(Sprite iconoCredencial)
    {
        tieneCredencialEscolar = true;
        Debug.Log("💳 ¡Credencial escolar recogida!");

        // Le mandamos la imagen a la UI para que la ponga en el cuadrito
        if (uiManager != null && iconoCredencial != null)
        {
            uiManager.AgregarIcono(iconoCredencial);
        }
        else
        {
            Debug.LogWarning("Falta asignar el UIManager en el PlayerInventory o el Sprite no se envió.");
        }

        // Aquí podrías reproducir un sonido de 'pickup' si quieres
    }
}