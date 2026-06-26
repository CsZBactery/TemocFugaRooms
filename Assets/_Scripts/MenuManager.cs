using UnityEngine;
using UnityEngine.SceneManagement; // Librería obligatoria para cambiar de escena

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        // Al entrar al menú, nos aseguramos de liberar el ratón para poder hacer clic
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Este método lo llamará el botón "Jugar"
    public void Jugar()
    {
        // Opción A: Carga la escena que esté justo después del menú en la lista de compilación
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        // Opción B: También puedes cargarla directamente por su nombre exacto descomentando la línea de abajo:
        // SceneManager.LoadScene("NombreDeTuEscenaDeJuego");
    }

    // Este método lo llamará el botón "Salir"
    public void Salir()
    {
        Debug.Log("El jugador ha salido del juego.");
        Application.Quit(); // Cierra el juego (solo funciona en el juego ya exportado/.exe)
    }
}