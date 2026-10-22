using UnityEngine;

public class Barrel2 : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;

    public AudioClip destroySFX;

    public float speed = 1f;

    public bool descending;
    public float ladderCooldown;

    private SpriteRenderer spriteRenderer;
    private int spriteIndex;
    public Sprite rollSprite;
    public Sprite[] descSprites;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
        descending = false;
        ladderCooldown = 0.25f;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateBarrel), 1f/12f, 1f/12f);
    }

    private void Update()
    {
        CheckCollision();
        CheckLadder();
        ladderCooldown  += Time.deltaTime;
    }

    private void CheckCollision()
    {
        Vector2 size = collider.bounds.size;
        size.x /= 2f;
        size.y /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Oil"))
            {
                if (Mathf.Abs(hit.transform.position.x - transform.position.x) < 0.15f)
                {
                    Destroy(this.gameObject);
                }
            } else if (hit.layer == LayerMask.NameToLayer("Hitbox"))
            {
                AudioSource.PlayClipAtPoint(destroySFX, transform.position, 1f);
                Destroy(this.gameObject);
            } else if (hit.layer == LayerMask.NameToLayer("DeathPlane"))
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CheckLadder()
    {
        Vector2 size = collider.bounds.size;
        size.x /= 2f;
        size.y /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position - new Vector3 (0, 2, 0), size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                int randomType = Random.Range(0,2);
                if (randomType == 1 && ladderCooldown > 0.25f) {
                    LadderDescend();
                } else
                {
                    ladderCooldown = 0f;
                }
            }
        }

    }

    private void LadderDescend()
    {
        descending = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = 0f;
        transform.rotation = Quaternion.identity;
        transform.position = transform.position - new Vector3 (0,0.6f,0);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        descending = false;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = 0f;
            rigidbody.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Conveyor1")) {
            rigidbody.AddForce(collision.transform.right * (speed - 1.25f), ForceMode2D.Impulse);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Conveyor2")) {
            rigidbody.AddForce(collision.transform.right * (speed + 1.25f), ForceMode2D.Impulse);
        }
    }

    private void AnimateBarrel()
    {
        if (descending == true) {
                spriteIndex++;

                if (spriteIndex >= descSprites.Length) {
                    spriteIndex = 0;
                }

                spriteRenderer.sprite = descSprites[spriteIndex];
            }
        else
        {
            spriteRenderer.sprite = rollSprite;
        }
    }
}