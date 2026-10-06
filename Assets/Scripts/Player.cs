using System.Data.Common;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite[] hammerUpSprites;
    public Sprite[] hammerDownSprites;
    public Sprite climbSprite;
    private int spriteIndex;
    
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;
    public float conveySpeed = 1.75f;
    public float trampStrength = 1f;

    private bool grounded;
    private bool climbing;
    private bool conveyagainst;
    private bool conveywith;

    public float platMotion = 0f;

    public GameObject hammerHitbox;
    public bool hammer;
    public bool hammerDown;
    private int hammerDownTime;
    private float hammerTime;
    public float hammerDuration;

    public Transform conveyTransform;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
        hammer = false;
        hammerDown = false;
        hammerDownTime = 0;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void EquipItem()
    {
        if (hammer) {
            hammerHitbox.SetActive(true);
            hammerTime += Time.deltaTime;

            if (hammerTime >= hammerDuration) {
                hammer = false;
            }
        } else {
            hammerHitbox.SetActive(false);
            hammerTime = 0f;
            hammerDown = false;
            hammerDownTime = 0;
        }
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;
        conveyagainst = false;
        conveywith = false;


        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f; 
        
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground") || hit.layer == LayerMask.NameToLayer("MovingGround") || hit.layer == LayerMask.NameToLayer("Conveyor1") || hit.layer == LayerMask.NameToLayer("Conveyor2"))
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);
                Physics2D.IgnoreCollision(collider, results[i], !grounded);
                if (hit.layer == LayerMask.NameToLayer("Conveyor1") && grounded == true)
                {
                    conveyagainst = true;
                    conveyTransform = hit.transform;
                } else if (hit.layer == LayerMask.NameToLayer("Conveyor2") && grounded == true)
                {
                    conveywith = true;
                    conveyTransform = hit.transform;
                }

                if (hit.layer == LayerMask.NameToLayer("MovingGround") && grounded == true)
                {
                    platMotion = hit.GetComponent<MovingPlat>().platMotion;
                } else {
                    platMotion = 0f;
                }

            } 
            else if (hit.layer == LayerMask.NameToLayer("Trampoline"))
            {
                direction = Vector2.up * trampStrength;
                hit.GetComponent<Trampoline>().pressed = 1;
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder") && hammer == false)
            {
                climbing = true;
            } else if (hit.layer == LayerMask.NameToLayer("Item"))
            {
                if (hit.CompareTag("Hammer"))
                {
                    hammer = true;
                    hammerTime = 0f;
                    Destroy(hit);
                }
            }
        } 
    }

    private void Update()
    {
        EquipItem();
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        } else if (grounded && Input.GetButtonDown("Jump") && hammer == false) {
            direction = Vector2.up * jumpStrength;
        } else {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed + platMotion * 1.4f;

        if (grounded) {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if (conveyagainst) {
            direction.x -= conveyTransform.right.x * conveySpeed;
        } else if (conveywith) {
            direction.x += conveyTransform.right.x * conveySpeed;
        }

        if (Input.GetAxis("Horizontal") > 0) {
            transform.eulerAngles = Vector3.zero;
        } else if (Input.GetAxis("Horizontal") < 0) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else 
        {
            if (Input.GetAxis("Horizontal") != 0) {
                spriteIndex++;

                if (spriteIndex >= runSprites.Length) {
                    spriteIndex = 0;
                }
            }
        }
        
        if (hammer)
        {
            if (hammerDownTime == 2) {
                hammerDown = true;
            } else
            if (hammerDownTime == 4){
                hammerDown = false;
                hammerDownTime = 0;
            }

            if (hammerDown) {
                spriteRenderer.sprite = hammerDownSprites[spriteIndex];
            } else
            {
                spriteRenderer.sprite = hammerUpSprites[spriteIndex];
            }

            hammerDownTime++;
        } else
        {
            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelFailed();
        }
    }
}
