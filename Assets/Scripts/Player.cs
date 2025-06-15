using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float inputH;
    private float inputV;
    private bool moviendo;
    private Vector3 puntoDestino;
    private Vector3 ultimoInput;
    private Vector3 puntoInteraccion; 
    private Collider2D colliderDelante; //indica el collider que tenemos por delante.
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float radioInteraccion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(inputV == 0)
        {
            inputH = Input.GetAxisRaw("Horizontal");
        }

        if(inputH == 0)
        {
            inputV = Input.GetAxisRaw("Vertical");
        }
        
        //Se agrega tecla 'E' para interactuar
        if (Input.GetKeyDown(KeyCode.E))
        {
            LanzarInteraccion();
        }
        
        //Ejecutar movimiento solo si estoy en una casilla y solo si hay input
        if(!moviendo && (inputH!= 0 || inputV != 0))
        {
            //actualizar cual fue mi ultimo input, cual va a ser mi puntoDestino y cual es mi puntoInteraccion.
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();

            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }            
        }             
    }
    
    //Metodo para dialogos
    private void LanzarInteraccion()
    {
        colliderDelante = LanzarCheck();
        if (colliderDelante)
        {
            if (colliderDelante.gameObject.CompareTag("NPC"))
            {
                NPC npcScript = colliderDelante.gameObject.GetComponent<NPC>();
                npcScript .Interactuar();
            }
        }
    }

    IEnumerator Mover()
    {
        moviendo = true;
        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        //Ante un nuevo destino, necesito refrescar de nuevo puntoInteraccion.
        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;
    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
}
