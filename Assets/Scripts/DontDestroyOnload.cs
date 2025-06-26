using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class DontDestroyOnload : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
