using UnityEngine;

public class Spray : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    private new Collider2D collider;

    private float sprayTime; // Time since spray has been fired
    public float sprayDuration; // Duration of time spray lasts
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        sprayTime = 0f;
        spriteRenderer.sprite = sprites[0];
    }

    private void Update()
    {
        sprayTime += Time.deltaTime;

        transform.position += transform.up * Time.deltaTime * 3.5f;

        if (sprayTime >= 0.25f) {
            spriteRenderer.sprite = sprites[1];
        }
        if (sprayTime >= sprayDuration) {
            Destroy(this.gameObject);
        }
    } 
}
