using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de UI")]
    // Ahora tenemos dos variables: una para tu menú de inicio y otra para las opciones
    public GameObject menuPrincipal;
    public GameObject panelOpciones;

    void Start()
    {
        // Al entrar al menú, nos aseguramos de liberar el ratón para poder hacer clic
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Le decimos a Unity que al darle Play, el menú principal DEBE estar encendido...
        if (menuPrincipal != null)
        {
            menuPrincipal.SetActive(true);
        }

        // ...y el panel de opciones DEBE estar apagado
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void AbrirOpciones()
    {
        // Al abrir opciones, apagamos el menú principal para que no estorbe
        if (menuPrincipal != null) menuPrincipal.SetActive(false);

        // Y prendemos el panel de opciones
        if (panelOpciones != null) panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        // Al cerrar opciones, hacemos lo contrario: apagamos opciones
        if (panelOpciones != null) panelOpciones.SetActive(false);

        // Y volvemos a prender el menú principal
        if (menuPrincipal != null) menuPrincipal.SetActive(true);
    }

    public void Salir()
    {
        Debug.Log("El jugador ha salido del juego.");
        Application.Quit();
    }
}