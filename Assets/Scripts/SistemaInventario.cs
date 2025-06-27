using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SistemaInventario : MonoBehaviour
{
    [SerializeField] private GameObject MarcoInventario;
    [SerializeField] private Button[] Botones;

    private int itemsDisponibles = 0;


    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private GameObject panelCombinacion;
    [SerializeField] private GameObject panelPapiro;
    [SerializeField] private Button slot1;
    [SerializeField] private Button slot2;
    [SerializeField] private TextMeshProUGUI mensajeCombinacion;
    [SerializeField] private ItemSO llaveSO, orbeSO, llaveVerdeSO;


    private ItemSO objetoSeleccionado1 = null;
    private ItemSO objetoSeleccionado2 = null;
    private ItemSO[] inventarioActual = new ItemSO[10]; // si tienes 10 slots
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Botones.Length; i++)
        {
            int indiceBoton = i;
            Botones[i].onClick.AddListener(() => BotonClickado(indiceBoton));
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
        Botones[itemsDisponibles].gameObject.SetActive(true);
        Botones[itemsDisponibles].GetComponent<Image>().sprite = item.icono;

        // Guardar el item en el array
        inventarioActual[itemsDisponibles] = item;

        if(item.nombre == "Botas")
        {
            gameManager.UpgradeVelocidad();
        }

        // Añadir callback para selección
        int index = itemsDisponibles;
        Botones[itemsDisponibles].onClick.AddListener(() => SeleccionarItemParaCombinar(index));

        if (item.nombre == "Paper")
        {
            //AbrirPanelPapiro();
            Botones[itemsDisponibles].onClick.AddListener(() => AbrirPanelPapiro());
        }


        itemsDisponibles++;
    }

    public void AbrirPanelPapiro()
    {
        panelPapiro.SetActive(true);
    }

    public void AbrirPanelDeCombinacion()
    {
        objetoSeleccionado1 = null;
        objetoSeleccionado2 = null;

        slot1.image.sprite = null;
        slot2.image.sprite = null;
        mensajeCombinacion.text = "";


        if(panelCombinacion.activeSelf == false)
        {
            panelCombinacion.SetActive(true);
        }
        else
        {
            panelCombinacion.SetActive(false);
        }
        
    }

    void SeleccionarItemParaCombinar(int index)
    {
        ItemSO item = inventarioActual[index];
        if (objetoSeleccionado1 == null)
        {
            objetoSeleccionado1 = item;
            slot1.image.sprite = item.icono;
        }
        else if (objetoSeleccionado2 == null && item != objetoSeleccionado1)
        {
            objetoSeleccionado2 = item;
            slot2.image.sprite = item.icono;

            // Ya hay dos objetos seleccionados → intentar combinar
            IntentarCombinar();
        }
    }

    void IntentarCombinar()
    {
        if (
            (objetoSeleccionado1 == llaveSO && objetoSeleccionado2 == orbeSO) ||
            (objetoSeleccionado1 == orbeSO && objetoSeleccionado2 == llaveSO)
        )
        {
            //  Combinación correcta → eliminar objetos y agregar nueva llave verde
            mensajeCombinacion.text = "¡COMBINADOS!";

            EliminarItemDelInventario(objetoSeleccionado1);
            EliminarItemDelInventario(objetoSeleccionado2);
            NuevoItem(llaveVerdeSO);
        }
        else
        {
            mensajeCombinacion.text = "INCOMPATIBLES";
        }

        // Opcional: cerrar panel después de un tiempo
        Invoke("CerrarPanelCombinacion", 2f);
    }

    void EliminarItemDelInventario(ItemSO item)
    {
        for (int i = 0; i < inventarioActual.Length; i++)
        {
            if (inventarioActual[i] == item)
            {
                inventarioActual[i] = null;
                Botones[i].gameObject.SetActive(false);
                Botones[i].onClick.RemoveAllListeners();
                break;
            }
        }
    }

    void CerrarPanelCombinacion()
    {
        panelCombinacion.SetActive(false);
    }

}