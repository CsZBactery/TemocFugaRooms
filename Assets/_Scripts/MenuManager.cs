using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de UI")]
    public GameObject menuPrincipal;
    public GameObject panelOpciones;

    [Header("Configuración de Audio")]
    public Slider audioSlider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (menuPrincipal != null) menuPrincipal.SetActive(true);
        if (panelOpciones != null) panelOpciones.SetActive(false);

        // Al entrar al menú, el slider toma el valor actual de la sesión
        if (audioSlider != null && AudioManager.Instancia != null)
        {
            audioSlider.value = AudioManager.Instancia.volumenGuardadoEnSesion;
        }
    }

    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void AbrirOpciones()
    {
        if (menuPrincipal != null) menuPrincipal.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(true);

        // Aseguramos que muestre el valor guardado de esta partida
        if (audioSlider != null && AudioManager.Instancia != null)
        {
            audioSlider.value = AudioManager.Instancia.volumenGuardadoEnSesion;
        }
    }

    public void CerrarOpciones()
    {
        // Si cancela (BACK), regresamos el slider y el sonido al último guardado de esta partida
        if (audioSlider != null && AudioManager.Instancia != null)
        {
            audioSlider.value = AudioManager.Instancia.volumenGuardadoEnSesion;
            AudioManager.Instancia.AplicarVolumen(audioSlider.value);
        }

        if (panelOpciones != null) panelOpciones.SetActive(false);
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
    }

    public void AplicarAjustes()
    {
        if (audioSlider != null && AudioManager.Instancia != null)
        {
            // Guardamos el cambio SOLAMENTE en la memoria temporal de la partida
            AudioManager.Instancia.volumenGuardadoEnSesion = audioSlider.value;
            Debug.Log("Ajustes aplicados temporalmente. Volumen actual: " + audioSlider.value);
        }
    }

    public void RestaurarPorDefecto()
    {
        if (audioSlider != null)
        {
            audioSlider.value = 0.75f;
        }
    }

    public void CambiarVolumenEnTiempoReal(float valorEnVivo)
    {
        if (AudioManager.Instancia != null)
        {
            AudioManager.Instancia.AplicarVolumen(valorEnVivo);
        }
    }

    public void Salir()
    {
        Debug.Log("El jugador ha salido del juego.");
        Application.Quit();
    }
}