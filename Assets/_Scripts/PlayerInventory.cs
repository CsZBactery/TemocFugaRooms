using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Esta variable guardará si tenemos la tarjeta o no
    public bool tieneCredencialEscolar = false;

    // Función para darnos la tarjeta (la llamará la tarjeta misma)
    public void RecogerCredencial()
    {
        tieneCredencialEscolar = true;
        Debug.Log("💳 ¡Credencial escolar recogida!");

        // Aquí podrías reproducir un sonido de 'pickup' si quieres
    }
}