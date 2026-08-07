using System.Collections;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Paneles de Cristal")]
    public Transform panelIzquierdo;
    public Transform panelDerecho;

    [Header("Distancia de Desplazamiento Local")]
    [Tooltip("Hacia dónde y cuánto se mueve el panel izquierdo (ejemplo: X = -1.2)")]
    public Vector3 izqOffset = new Vector3(-1.2f, 0f, 0f);

    [Tooltip("Hacia dónde y cuánto se mueve el panel derecho (ejemplo: X = 1.2)")]
    public Vector3 derOffset = new Vector3(1.2f, 0f, 0f);

    public float velocidad = 3f;

    private Vector3 izqPosInicial;
    private Vector3 derPosInicial;
    private bool estaAbierta = false;

    void Start()
    {
        if (panelIzquierdo != null) izqPosInicial = panelIzquierdo.localPosition;
        if (panelDerecho != null) derPosInicial = panelDerecho.localPosition;
    }

    public void AbrirPuerta()
    {
        if (estaAbierta) return;
        estaAbierta = true;
        StartCoroutine(MoverPuerta());
    }

    private IEnumerator MoverPuerta()
    {
        Vector3 targetIzq = izqPosInicial + izqOffset;
        Vector3 targetDer = derPosInicial + derOffset;

        float tiempo = 0f;
        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidad;

            if (panelIzquierdo != null)
                panelIzquierdo.localPosition = Vector3.Lerp(izqPosInicial, targetIzq, tiempo);

            if (panelDerecho != null)
                panelDerecho.localPosition = Vector3.Lerp(derPosInicial, targetDer, tiempo);

            yield return null;
        }
    }
}