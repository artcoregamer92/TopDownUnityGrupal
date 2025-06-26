using System.Collections;
using UnityEngine;

/// <summary>Movimiento de 1 casilla para bloques empujables.</summary>
public class Pushable : MonoBehaviour
{
    /// Velocidad a la que se desliza el bloque (la igualamos a la del jugador desde fuera).
    public float moveSpeed = 6f;

    /// Lanza la corrutina de movimiento externo:
    public IEnumerator MoveTo(Vector3 destino)
    {
        while (transform.position != destino)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
