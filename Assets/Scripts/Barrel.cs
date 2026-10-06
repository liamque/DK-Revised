using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;

    public float speed = 1f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
    }

    private void Update()
    {
        CheckCollision();
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
                Debug.Log("destroy barrel");
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            rigidbody.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Conveyor1")) {
            rigidbody.AddForce(collision.transform.right * (speed - 1.25f), ForceMode2D.Impulse);
        } else if (collision.gameObject.layer == LayerMask.NameToLayer("Conveyor2")) {
            rigidbody.AddForce(collision.transform.right * (speed + 1.25f), ForceMode2D.Impulse);
        }
    }
}
