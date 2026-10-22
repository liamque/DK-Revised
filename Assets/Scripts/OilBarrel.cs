using UnityEngine;

public class OilBarrel : MonoBehaviour
{
    public bool onFire;

    private SpriteRenderer spriteRenderer;
    public Sprite[] burnSprites;
    public Sprite startSprite;
    private int spriteIndex;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        onFire = false;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/6f, 1f/6f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void AnimateSprite()
    {
        if (onFire) {
            spriteIndex++;

            if (spriteIndex >= burnSprites.Length) {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = burnSprites[spriteIndex];
        } else
        {
            spriteRenderer.sprite = startSprite;
        }
        
    }
}