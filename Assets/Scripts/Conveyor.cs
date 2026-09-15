using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] moveSprites;
    private int spriteIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= moveSprites.Length) {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = moveSprites[spriteIndex];
        
    }
}
