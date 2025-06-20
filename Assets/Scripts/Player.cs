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

    [SerializeField] private Animator animator;

    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float radioInteraccion;
    [SerializeField] private LayerMask queEsColisionable;
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
            animator.SetFloat("VelX", inputH);
            animator.SetFloat("VelY", 0);
        }

        if(inputH == 0)
        {
            inputV = Input.GetAxisRaw("Vertical");
            animator.SetFloat("VelY", inputV);
            animator.SetFloat("VelX", 0);
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

    IEnumerator Mover()
    {
        moviendo = true;

        animator.SetBool("Move", true);

        while (transform.position != puntoDestino)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
            yield return null;
        }

        //Ante un nuevo destino, necesito refrescar de nuevo puntoInteraccion.
        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;

        animator.SetBool("Move", false);
    }

    private Collider2D LanzarCheck()
    {
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion, queEsColisionable);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
}
