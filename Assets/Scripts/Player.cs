using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // se quita para interactuar con NPC
    //[SerializeField] private LayerMask queEsColisionable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    private bool interactuando;

    [SerializeField] private GameManagerSO gameManager;
    public bool Interactuando { get => interactuando; set => interactuando = value; }
    public float VelocidadMovimiento { get => velocidadMovimiento; set => velocidadMovimiento = value; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        transform.position = gameManager.SpawnPoint.transform.position;

        GameObject[] jugadores = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject jugador in jugadores)
        {
            if (jugador != this.gameObject)
            {
                Destroy(jugador);
            }
        }
        StopAllCoroutines();
        inputH = 0;
        inputV = 0;
        puntoDestino = transform.position;
        puntoInteraccion = transform.position;
        ultimoInput = Vector3.zero;
        moviendo = false;
        animator.SetBool("Move", false);
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
        
        //Se agrega tecla 'E' para interactuar con NPC
        if (Input.GetKeyDown(KeyCode.E))
        {
            LanzarInteraccion();
        }
        
        //Ejecutar movimiento solo si estoy en una casilla y solo si hay input
        if(!interactuando && !moviendo && (inputH!= 0 || inputV != 0))
        {
            //actualizar cual fue mi ultimo input, cual va a ser mi puntoDestino y cual es mi puntoInteraccion.
            // 1. Registrar input y posiciones de chequeo
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            // 2. Mirar qué hay justo delante
            colliderDelante = LanzarCheck();

            // 3.A Nada delante → avanza normal
            if (!colliderDelante || colliderDelante.CompareTag("Entrada"))
            {
                StartCoroutine(Mover());
            }
            // 3.B Hay algo y es Pushable → intenta empujarlo
            else if (colliderDelante.CompareTag("Pushable"))
            {
                Vector3 destinoBloque = colliderDelante.transform.position + ultimoInput;

                // ¿Está libre la casilla más allá del bloque?
                if (!Physics2D.OverlapCircle(destinoBloque, radioInteraccion))
                {
                    // Mueve bloque y luego jugador, a la misma velocidad
                    Pushable pushable = colliderDelante.GetComponent<Pushable>();
                    pushable.moveSpeed = velocidadMovimiento;

                    StartCoroutine(pushable.MoveTo(destinoBloque));
                    StartCoroutine(Mover());
                }
            }
            // 3.C Cualquier otra cosa (muro, NPC, límite) → no te mueves

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
        // se quita por ajuste de NPC
        //return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion, queEsColisionable);
        return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
    
    //Metodo para dialogos con NPC
    private void LanzarInteraccion()
    {
        colliderDelante = LanzarCheck();
        if (colliderDelante)
        {
            if (colliderDelante.TryGetComponent(out Interactuable interactuable))
            {
                interactuable.Interactuar();
            }
        }
    }
}
