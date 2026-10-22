using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private new Collider2D collider;

    private Collider2D[] results;

    public AudioClip destroySFX;

    private SpriteRenderer spriteRenderer;
    public Sprite[] burnSprites;
    private int spriteIndex;

    public Vector3 spawnPoint;

    public float duration;
    public float offset;
    public float traveltime;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        results = new Collider2D[6];
        spriteRenderer = GetComponent<SpriteRenderer>();
        duration = 0f;
        spawnPoint = transform.position;
        traveltime = 1;
        offset = Mathf.Asin(spawnPoint.x/5.8f) - traveltime;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/6f, 1f/6f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void CheckCollision()
    {
        Vector2 size = collider.bounds.size;
        size.x /= 2f;
        size.y /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);
        Debug.Log(size);
        Debug.Log(results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Hitbox"))
            {
                AudioSource.PlayClipAtPoint(destroySFX, transform.position, 1f);
                Destroy(this.gameObject);
            }
        }
    }

    private void Update()
    {
        if (duration < 1f)
        {
            transform.position = new Vector3 (spawnPoint.x, spawnPoint.y + 0.7f * duration, spawnPoint.z);
        } else
        {
            transform.position = new Vector3 (5.8f * Mathf.Sin(traveltime * duration + offset), spawnPoint.y + 0.7f + 0.25f * (duration - 1f), spawnPoint.z);
            
        }
        duration = duration + Time.deltaTime;

        CheckCollision();
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= burnSprites.Length) {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = burnSprites[spriteIndex];
    }
}