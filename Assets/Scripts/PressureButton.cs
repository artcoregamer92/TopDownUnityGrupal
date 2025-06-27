using UnityEngine;
using UnityEngine.Events;

/// <summary>Placa que detecta peso y lanza eventos de abrir/cerrar puerta.</summary>
public class PressureButton : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite upSprite;      // sin peso
    public Sprite downSprite;    // con peso

    [Header("A qui�n notifica")]
    public Door targetDoor;              // arrastra la puerta aqu�

    [Header("Qui�nes activan la placa")]
    public LayerMask activatorMask;      // Player y Pushable

    int objectsOnTop = 0;                // cu�ntos activadores pisan
    SpriteRenderer sr;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsActivator(other)) return;
        objectsOnTop++;
        if (objectsOnTop == 1) Press();  // primer objeto
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!IsActivator(other)) return;
        objectsOnTop = Mathf.Max(0, objectsOnTop - 1);
        if (objectsOnTop == 0) Release(); // se fue el �ltimo
    }

    bool IsActivator(Collider2D col) =>(activatorMask.value & (1 << col.gameObject.layer)) != 0;

    void Press()
    {
        sr.sprite = downSprite;
        targetDoor.Open();
    }

    void Release()
    {
        sr.sprite = upSprite;
        targetDoor.Close();
    }
}

