using System.Data.Common;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite[] hammerUpSprites;
    public Sprite[] hammerDownSprites;
    public Sprite[] hoseDownSprites;
    public Sprite[] hoseUpSprites;
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

    public int item; // 0 = none, 1 = hammer, 2 = hose

    public GameObject hammerHitbox; // GameObject used for barrel destroying hitbox
    public bool hammerDown; // true = hammer lowered, false = hammer raised
    private int hammerDownTime; // current animation frame of the hammer swing cycle

    public GameObject hoseSpray; //GameObject fired by hose
    public int hoseDown; // 1 = hose pointed down, 0 = hose pointed up

    private float hammerTime; // Time since an item (hammer/hose) was collected
    public float hammerDuration; // Maximum duration a hammer item can be held
    public float hoseDuration; // Maximum duration a hose item can be held

    public AudioClip jumpSFX;
    public AudioClip deathSFX;
    public AudioClip stepSFX;

    private float stepCount;
    public float stepDuration;

    public Transform conveyTransform;

    private GameObject bgm;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        bgm = GameObject.Find("MusicPlayer");
        results = new Collider2D[4];
        item = 0;
        hammerDown = false;
        hammerDownTime = 0;
        hoseDown = 0;
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
        if (item == 1) {
            hammerHitbox.SetActive(true);
            hammerTime += Time.deltaTime;

            if (hammerTime >= hammerDuration) {
                item = 0;
                bgm.GetComponent<MusicController>().PlayMusic(0);
            }
        } else if (item == 2) {
            hammerTime += Time.deltaTime;

            if (Input.GetButtonDown("Fire1"))
            {
                hoseDown = 0;
            } else if (Input.GetButtonDown("Fire2"))
            {
                hoseDown = 1;
            }

            if (hammerTime >= hoseDuration) {
                item = 0;
                bgm.GetComponent<MusicController>().PlayMusic(0);
            }
        }
        else {
            hammerHitbox.SetActive(false);
            hammerTime = 0f;
            hammerDown = false;
            hammerDownTime = 0;
        }
    }

    private void Spray()
    {
        Instantiate(hoseSpray, transform.position, new Quaternion (0,0,180*hoseDown,1));
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
                if (hit.GetComponent<Trampoline>().pressed == 0) {
                    bgm.GetComponent<MusicController>().PlayClip2D(jumpSFX);
                    hit.GetComponent<Trampoline>().pressed = 1;
                }
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder") && item == 0)
            {
                climbing = true;
            } else if (hit.layer == LayerMask.NameToLayer("Item"))
            {
                if (hit.CompareTag("Hammer"))
                {
                    item = 1;
                } else if (hit.CompareTag("Washer"))
                {
                    item = 2;
                }
                hammerTime = 0f;
                bgm.GetComponent<MusicController>().PlayMusic(1);
                Destroy(hit);
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
        } else if (grounded && Input.GetButtonDown("Jump") && item != 1) {
            direction = Vector2.up * jumpStrength;
            bgm.GetComponent<MusicController>().PlayClip2D(jumpSFX);

        } else {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        if (item == 2 && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")))
        {
            Spray();
        }

        if (Input.GetButton("Horizontal") && grounded && stepCount > stepDuration)
        {
            bgm.GetComponent<MusicController>().PlayClip2D(stepSFX);
            stepCount = 0f;
        } else
        {
            stepCount += Time.deltaTime;
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
        
        if (item == 1)
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
        } 
        else if (item == 2)
        {
            if (hoseDown == 0) {
                spriteRenderer.sprite = hoseUpSprites[spriteIndex];
            } else
            {
                spriteRenderer.sprite = hoseDownSprites[spriteIndex];
            }
        }
        else if (item == 0)
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
            bgm.GetComponent<MusicController>().StopMusic();
            bgm.GetComponent<MusicController>().PlayClip2D(deathSFX);
            enabled = false;
            FindObjectOfType<GameManager>().LevelFailed();
        }
    }
}
