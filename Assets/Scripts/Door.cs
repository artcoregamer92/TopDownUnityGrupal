using UnityEngine;

/// <summary> Puerta que puede abrirse/cerrarse visual y físicamente. </summary>
public class Door : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    [Header("Opciones")]
    public bool startsOpen = false;     // Por si la puerta ya está abierta al inicio
    public bool staysOpen = false;      // Si es true, no vuelve a cerrarse

    BoxCollider2D col;
    SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        SetState(startsOpen);
    }

    /// <param name="open">true = abrir, false = cerrar</param>
    public void SetState(bool open)
    {
        sr.sprite = open ? openSprite : closedSprite;
        col.enabled = !open;                         // Desactiva collider al abrir
    }

    public void Open()
    {
        if (staysOpen) { SetState(true); return; }
        SetState(true);
        StopAllCoroutines();
    }

    public void Close()
    {
        if (staysOpen) return;
        SetState(false);
    }
}

