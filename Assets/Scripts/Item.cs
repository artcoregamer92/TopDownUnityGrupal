using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, Interactuable
{
    [SerializeField] ItemSO misDatos;
    [SerializeField] GameManagerSO gameManager;
    public void Interactuar()
    {
        gameManager.Inventario.NuevoItem(misDatos);
        Destroy(this.gameObject);
    }
}
