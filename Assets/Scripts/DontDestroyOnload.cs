using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class DontDestroyOnload : MonoBehaviour
{
    public static DontDestroyOnload Instance;

    [Header("Referencias del HUD")]
    public GameObject cuadroDialogo;
    public TextMeshProUGUI textoDialogo;

    private void Awake()
    {
        // Si ya hay una instancia y no somos nosotros, nos destruimos
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        GameObject[] canvaas = GameObject.FindGameObjectsWithTag("Canvas");

        foreach (GameObject canva in canvaas)
        {
            if (canva != this.gameObject)
            {
                Destroy(canva);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
