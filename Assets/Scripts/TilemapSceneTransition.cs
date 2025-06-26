using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

/// <summary>
/// Detecta la celda del Tilemap donde entra el jugador y carga la escena indicada
/// (una fila por puerta para mayor flexibilidad).
/// </summary>
public class TilemapSceneTransition : MonoBehaviour
{
    [System.Serializable]
    public struct PortalInfo
    {
        public TileBase tile;          // El tile gráfico que identifica la puerta
        public string targetScene;     // Escena a la que saltar (debe estar en Build Settings)
        public string spawnPointName;  // Nombre del GameObject donde aterrizará el jugador
    }

    public PortalInfo[] portals;       // Lista configurable en el Inspector

    Tilemap tmap;

    void Awake() => tmap = GetComponent<Tilemap>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1. ¿En qué celda del tilemap está el jugador?
        Vector3Int cell = tmap.WorldToCell(other.transform.position);
        TileBase currentTile = tmap.GetTile(cell);
        if (!currentTile) return;

        // 2. ¿Esa celda corresponde a alguno de nuestros portales?
        foreach (var p in portals)
        {
            if (p.tile == currentTile)
            {
                StartCoroutine(DoTransition(other.gameObject, p.targetScene, p.spawnPointName));
                break;
            }
        }
    }

    System.Collections.IEnumerator DoTransition(GameObject player, string scene, string spawn)
    {
        // (opcional) Desactiva input para evitar movimiento durante la carga
        player.GetComponent<MonoBehaviour>().enabled = false;

        // Carga asincrónica para evitar congelación
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        while (!op.isDone) yield return null;

        // 1 frame extra para que todo despierte
        yield return null;

        // Reubica al jugador en el punto de aparición
        Transform target = GameObject.Find(spawn)?.transform;
        if (target) player.transform.position = target.position;

        // Reactiva el control
        player.GetComponent<MonoBehaviour>().enabled = true;
    }
}

