using UnityEngine;

public class MovingPlat : MonoBehaviour
{
    private new Collider2D collider;

    public float speed = 1f;
    public float direction = 1f;

    public float platMotion = 0f;

    public int target = 0;

    public Vector3 posA;
    public Vector3 posB;

    private SpriteRenderer spriteRenderer;
    public Sprite[] moveSprites;
    private int spriteIndex;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.position = posA;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/6f, 1f/6f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void FixedUpdate()
    {
        if (target == 0)
        {
            direction = -1f;
        } else
        {
            direction = 1f;
        }

        platMotion = speed * direction;

        transform.Translate(platMotion * Time.fixedDeltaTime,0,0);

        if (transform.position.x < posB.x)
        {
            target = 1;
        } else if (transform.position.x > posA.x)
        {
            target = 0;
        }
        
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
