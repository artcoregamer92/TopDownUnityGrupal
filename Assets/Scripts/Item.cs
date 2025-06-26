using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, Interactuable
{
    [SerializeField] ItemSO misDatos;
    [SerializeField] GameManagerSO gameManager;

    private void Start()
    {
        if (gameManager.ModoTesteo == false && gameManager.EstaRecogido(misDatos))
        {
            Destroy(gameObject);
        }
    }

    public void Interactuar()
    {
        gameManager.Inventario.NuevoItem(misDatos);
        gameManager.MarcarComoRecogido(misDatos);
        Destroy(this.gameObject);
    }
}
