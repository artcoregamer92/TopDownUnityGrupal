using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private  GameObject MarcoInventario;
    [SerializeField] private Button[] Botones;

    private int itemsDisponibles = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Botones.Length; i++) {
            int indiceBoton = i;
            Botones[i].onClick.AddListener(()=> BotonClickado(indiceBoton));  
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            MarcoInventario.SetActive(!MarcoInventario.activeSelf);
        }
    }

    public void BotonClickado(int index)
    {
        Debug.Log("Boton Clickado" + index);
    }

    public void NuevoItem(ItemSO item)
    {
        //activar item
        Botones[itemsDisponibles].gameObject.SetActive(true);
        //le pongo icono del SO
        Botones[itemsDisponibles].GetComponent<Image>().sprite = item.icono;
        itemsDisponibles++;
    }
}
