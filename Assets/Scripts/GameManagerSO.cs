using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Scriptable Objects/GameManager")]

public class GameManagerSO : ScriptableObject
{
    [ContextMenu("Reiniciar �tems recogidos")]

    public void ReiniciarRecogidos()
    {
        foreach (var estado in estadoItems)
        {
            estado.recogido = false;
        }
    }
    [SerializeField] private bool modoTesteo = false;

    [System.Serializable]
    public class EstadoItem
    {
        public ItemSO item;
        public bool recogido;
    }

    [SerializeField] private List<EstadoItem> estadoItems;

    public bool EstaRecogido(ItemSO item)
    {
        foreach (var e in estadoItems)
        {
            if (e.item == item)
                return e.recogido;
        }
        return false;
    }

    public void MarcarComoRecogido(ItemSO item)
    {
        foreach (var e in estadoItems)
        {
            if (e.item == item)
            {
                e.recogido = true;
                return;
            }
        }

        // Si no existe, lo crea
        estadoItems.Add(new EstadoItem { item = item, recogido = true });
    }

    private Player player;
    private SistemaInventario inventario;
    public string SpawnName = "Spawn";
    public GameObject SpawnPoint;

    public SistemaInventario Inventario { get => inventario; }
    public bool ModoTesteo { get => modoTesteo; set => modoTesteo = value; }

    private void OnEnable() //Llamadas por EVENTO.
    {
            SceneManager.sceneLoaded += NuevaEscenaCargada;
    }
    private void NuevaEscenaCargada(Scene arg0, LoadSceneMode arg1)
    {
        player = UnityEngine.Object.FindFirstObjectByType<Player>();
        inventario = UnityEngine.Object.FindFirstObjectByType<SistemaInventario>();
        SpawnPoint = UnityEngine.GameObject.Find(SpawnName);
    }
    
    public void CambiarEstadoPlayer(bool estado)
        {
            player.Interactuando = !estado;
        }
    public void UpgradeVelocidad()
    {
        player.VelocidadMovimiento = 5;
    }
    
    //metodo para consultar si se recogio llave en el inventario
    public bool validateKey()
    {
        return inventario.GetHaveKey();
    }

}