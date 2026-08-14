using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerController playerController;
    public Image iconoEstado;

    [Tooltip("Arrastra aquí la imagen RellenoEstamina")]
    public Image barraEstamina; // NUEVA REFERENCIA

    [Header("Sprites de Estado")]
    public Sprite iconoCorriendo;
    public Sprite iconoAgachado;

    [Header("Colores Visuales de la Barra")]
    public Color colorEstaminaLlena = new Color(0.2f, 0.8f, 0.2f); // Verde
    public Color colorEstaminaBaja = new Color(0.8f, 0.1f, 0.1f);  // Rojo

    private Color colorTransparente = new Color(1f, 1f, 1f, 0f);
    private Color colorVisible = new Color(1f, 1f, 1f, 1f);

    void Update()
    {
        // ----------------------------------------------------
        // 1. LÓGICA DE LOS ICONOS (Ya la tenías)
        // ----------------------------------------------------
        if (playerController.currentState == PlayerController.MovementState.Running)
        {
            iconoEstado.sprite = iconoCorriendo;
            iconoEstado.color = colorVisible;
        }
        else if (playerController.currentState == PlayerController.MovementState.Crouching)
        {
            iconoEstado.sprite = iconoAgachado;
            iconoEstado.color = colorVisible;
        }
        else
        {
            iconoEstado.color = colorTransparente;
        }

        // ----------------------------------------------------
        // 2. LÓGICA DE LA BARRA DE ESTAMINA
        // ----------------------------------------------------
        if (barraEstamina != null)
        {
            // Calculamos el porcentaje de estamina (ej. 50 / 100 = 0.5f)
            float porcentaje = playerController.currentStamina / playerController.maxStamina;

            // Llenamos la barra según el porcentaje
            barraEstamina.fillAmount = porcentaje;

            // Efecto visual: Cambia a color rojo si la estamina baja del 25%
            if (porcentaje <= 0.25f)
            {
                barraEstamina.color = colorEstaminaBaja;
            }
            else
            {
                barraEstamina.color = colorEstaminaLlena;
            }
        }
    }
}