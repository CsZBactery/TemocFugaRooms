using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public static bool JuegoPausado = false;
    public GameObject menuPausaUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        menuPausaUI.SetActive(false);
        Time.timeScale = 1f;
        JuegoPausado = false;

        // Ocultar y bloquear el cursor de nuevo al jugar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pausar()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        JuegoPausado = true;

        // Liberar y mostrar el cursor para poder usar el menú
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        JuegoPausado = false;

        // Asegurarnos de que el cursor esté libre al volver al menú principal
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Menu");
    }
}