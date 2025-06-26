using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;
using System.Collections;

/// <summary>
/// Detecta la celda del Tilemap donde entra el jugador y carga la escena indicada
/// (una fila por puerta para mayor flexibilidad).
/// </summary>
public class TilemapSceneTransition : MonoBehaviour
{

    private void Start()
    {
        
    }

    [SerializeField]  private string sceneName;
    [SerializeField] private string spawnPoint;
    [SerializeField] private GameManagerSO gameManager;

    Tilemap tmap;

    void Awake() => tmap = GetComponent<Tilemap>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DoTransition(other.gameObject, sceneName, spawnPoint));
        }

                

    }



    IEnumerator DoTransition(GameObject player, string scene, string spawn)
    {


        gameManager.SpawnName = spawn;

        // Carga la escena nueva
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        while (!op.isDone) yield return null;

    }
}

