using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instancia;

    [Header("Configuración")]
    public AudioMixer mainMixer;
    private string parametroVolumen = "VolumenMaestro";

    // Esta variable guarda el volumen SOLO mientras el juego está abierto.
    // Siempre que inicies el juego, empezará en 0.75f (75%).
    public float volumenGuardadoEnSesion = 0.75f;

    void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            // Se eliminó la línea DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Al iniciar el juego, aplica el volumen por defecto
        AplicarVolumen(volumenGuardadoEnSesion);
    }

    public void AplicarVolumen(float valorSlider)
    {
        float volumenDecibelios;

        // CORRECCIÓN DE SILENCIO: Si la barra está casi en cero, forzamos el muteo absoluto (-80dB)
        if (valorSlider <= 0.001f)
        {
            volumenDecibelios = -80f;
        }
        else
        {
            // Matemática logarítmica estándar
            volumenDecibelios = Mathf.Log10(valorSlider) * 20f;
        }

        if (mainMixer != null)
        {
            mainMixer.SetFloat(parametroVolumen, volumenDecibelios);
        }
    }
}