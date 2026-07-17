using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Necesario para el Slider

public class PauseManager : MonoBehaviour
{
    [Header("UI de Pausa")]
    public GameObject panelPausa;
    public GameObject panelAjustes;

    [Header("Configuración de Audio")]
    public Slider audioSlider;

    private bool juegoPausado = false;

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelAjustes != null) panelAjustes.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }
    }

    public void Pausar()
    {
        juegoPausado = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (panelPausa != null) panelPausa.SetActive(true);

        // Actualizamos el slider al valor actual cuando pausamos
        if (audioSlider != null && AudioManager.Instancia != null)
        {
            audioSlider.value = AudioManager.Instancia.volumenGuardadoEnSesion;
        }
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelAjustes != null) panelAjustes.SetActive(false);
    }

    public void AbrirAjustes()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelAjustes != null) panelAjustes.SetActive(true);

        if (audioSlider != null && AudioManager.Instancia != null)
        {
            audioSlider.value = AudioManager.Instancia.volumenGuardadoEnSesion;
        }
    }

    public void CerrarAjustes()
    {
        // Si el jugador cancela, regresamos el slider al último guardado
        if (audioSlider != null && AudioManager.Instancia != null)
        {
            audioSlider.value = AudioManager.Instancia.volumenGuardadoEnSesion;
            AudioManager.Instancia.AplicarVolumen(audioSlider.value);
        }

        if (panelAjustes != null) panelAjustes.SetActive(false);
        if (panelPausa != null) panelPausa.SetActive(true);
    }

    // --- FUNCIONES DE AUDIO PARA EL MENÚ DE PAUSA ---

    public void AplicarAjustes()
    {
        if (audioSlider != null && AudioManager.Instancia != null)
        {
            AudioManager.Instancia.volumenGuardadoEnSesion = audioSlider.value;
        }
    }

    public void RestaurarPorDefecto()
    {
        if (audioSlider != null) audioSlider.value = 0.75f;
    }

    public void CambiarVolumenEnTiempoReal(float valorEnVivo)
    {
        if (AudioManager.Instancia != null)
        {
            AudioManager.Instancia.AplicarVolumen(valorEnVivo);
        }
    }

    public void SalirAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}