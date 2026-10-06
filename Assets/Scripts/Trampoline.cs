using UnityEngine;

public class Trampoline : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite[] tramSprites;


    public int pressed;
    private float openTime;
    public float openDelay; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        openTime = 0f;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/6f, 1f/6f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Update()
    {
        if (pressed == 1)
        {
            openTime += Time.deltaTime;

            if (openTime >= openDelay) {
                pressed = 0;
            }
        } else {
            openTime = 0f;
        }
        // Debug.Log(openTime);
        
    }

    private void AnimateSprite()
    {
        spriteRenderer.sprite = tramSprites[pressed];
        
    }
}
