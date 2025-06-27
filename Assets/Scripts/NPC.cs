using System.Collections;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour, Interactuable
{
    [SerializeField] private GameManagerSO gameManager;
    [SerializeField, TextArea(1, 5)] private string[] frases;
    [SerializeField] private float tiempoDialogo;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TextMeshProUGUI textoDialogo;
    private bool hablando = false;
    private int fraseActual = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (DontDestroyOnload.Instance != null)
        {
            cuadroDialogo = DontDestroyOnload.Instance.cuadroDialogo;
            textoDialogo = DontDestroyOnload.Instance.textoDialogo;
        }
    }

    public void Interactuar()
    {
        gameManager.CambiarEstadoPlayer(false);
        cuadroDialogo.SetActive(true);
        if (!hablando)
        {
            SiguienteFrase();
        }
        else
        {
            CompletarFrase();
        }
        hablando = false;
    }

    private IEnumerator EscribirFrase()
    {
        hablando = true;
        // Limpia el cuadro de texto para que no se solapen las frases
        textoDialogo.text = "";
        // Divide la frase en caracteres para que se pueda escribir por letras
        char[] caracteresFrase = frases[fraseActual].ToCharArray();
        foreach (char caracter in caracteresFrase)
        {
            textoDialogo.text += caracter;
            yield return new WaitForSeconds(tiempoDialogo);
        }
    }

    private void CompletarFrase()
    {
        StopAllCoroutines();
        textoDialogo.text = frases[fraseActual];
        hablando = false;
    }

    private void SiguienteFrase()
    {
        fraseActual++;
        if (fraseActual >= frases.Length)
        {
            TerminarDialogo();
        }
        else
        {
            StartCoroutine(EscribirFrase());
        }
    }

    private void TerminarDialogo()
    {
        hablando = false;
        textoDialogo.text = "";
        fraseActual = -1;
        cuadroDialogo.SetActive(false);
        gameManager.CambiarEstadoPlayer(true);
    }
    
}
