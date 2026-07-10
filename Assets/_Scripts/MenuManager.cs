using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de UI")]
    // Esta variable nos permitirá encender y apagar el panel visual de opciones
    public GameObject panelOpciones;

    void Start()
    {
        // Al entrar al menú, nos aseguramos de liberar el ratón para poder hacer clic
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Aseguramos que el panel de opciones empiece apagado al cargar el menú
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    // Este método lo llamará el botón "Jugar"
    public void Jugar()
    {
        // Opción A (Activada): Carga la escena que esté justo después del menú.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Este método lo llamará el botón "Opciones"
    public void AbrirOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true); // Muestra el panel
        }
    }

    // Este método lo llamará un botón de "Volver" o "Cerrar" dentro del panel de opciones
    public void CerrarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false); // Oculta el panel
        }
    }

    // Este método lo llamará el botón "Salir"
    public void Salir()
    {
        Debug.Log("El jugador ha salido del juego.");
        Application.Quit(); // Cierra el juego en la build final
    }
}