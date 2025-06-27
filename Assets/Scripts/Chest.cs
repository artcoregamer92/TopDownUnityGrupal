using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour, Interactuable
{
    [SerializeField] GameManagerSO gameManager;
    [SerializeField] Animator animator;
    [SerializeField] GameObject chestItem;
    [SerializeField] float chestDelay;
    private bool itemDropped = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interactuar()
    {
        animator.SetBool("Open", true);
        StartCoroutine(GetChestItem());
    }

    IEnumerator GetChestItem()
    {
        // valida si ya se creó el item antes de instanciarlo
        if (itemDropped == false)
        {
            yield return new WaitForSeconds(chestDelay);
            Instantiate(chestItem, transform.position, Quaternion.identity);
            itemDropped = true;
        }
    }
}
