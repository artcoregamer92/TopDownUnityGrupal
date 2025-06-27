using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Cambia a la escena especificada
    public void Jugar(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Cierra el juego (solo funciona en el ejecutable)
    public void Salir()
    {
        Application.Quit();

#if UNITY_EDITOR
        // Para que también funcione al probar en el editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
