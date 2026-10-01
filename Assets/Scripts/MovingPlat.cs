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

    void Awake()
    {
        collider = GetComponent<Collider2D>();

        transform.position = posA;
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
}
