using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 40f;
    public float graceTime = 1f;
    public float pushDistance = 2f;
    public float pushForce = 10f;
    public int killStreak = 0;
    public float drag = 2f;


    public static bool Attacking = false;
    public string hand;
    public GameObject objHand;

    public static bool facing;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;
    private float graceTimer = 0f;
    private bool inGracePeriod = false;
    private bool isBeingPushed = false;
    private Vector2 pushDirection;
    private float pushTimer = 0.2f; // duración del empuje
    private float pushTimeRemaining = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (objHand != null ) {
            if (objHand.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                facing = true;
            }

            else { spriteRenderer.flipX = false; facing = false; }
        }
        if (inGracePeriod)
        {
            graceTimer -= Time.deltaTime;
            if (graceTimer <= 0f)
            {
                inGracePeriod = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z)) { hand = "Rock";  }
        if (Input.GetKeyDown(KeyCode.C)) { hand = "Paper";  }
        if (Input.GetKeyDown(KeyCode.X)) { hand = "Scissors"; }

        if (Input.GetMouseButtonDown(0) && !isMoving && !isBeingPushed)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            moveDirection = (mouseWorldPos - transform.position).normalized;
            rb.velocity = moveDirection * speed;
            isMoving = true;
            Attacking = true;
        }

        if (isBeingPushed)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * drag);
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
                isBeingPushed = false;
                isMoving = false;
            }
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si está siendo empujado, no puede ser atacado pero empuja al enemigo
            if (isBeingPushed)
            {
                Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 enemyPushDirection = (collision.transform.position - transform.position).normalized;
                    enemyRb.AddForce(enemyPushDirection * pushForce, ForceMode2D.Impulse);
                }
                return;
            }

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                string enemyHand = enemy.enemyHand;

                Vector2 recoilDirection = (transform.position - collision.transform.position).normalized;

                if (Attacking)
                {
                    if (Beats(hand, enemyHand))
                    {
                        // ✅ Ataque exitoso → rebote suave
                        //rb.velocity = recoilDirection * 5f;
                        killStreak++;
                        Destroy(collision.gameObject);
                    }
                    else
                    {
                        // ❌ Ataque fallido → rebote fuerte
                        rb.velocity = recoilDirection * 40f;
                        killStreak = 0;
                    }

                    Attacking = false;
                    isMoving = false;
                }
                else
                {
                    // 🛡️ Recibe daño → rebote fuerte
                    rb.velocity = recoilDirection * 40f;
                    killStreak = 0;
                }

                // Activar empuje
                graceTimer = graceTime;
                inGracePeriod = true;

                pushDirection = recoilDirection;
                isBeingPushed = true;
                pushTimeRemaining = pushTimer;
            }

            return;
        }
        else if (collision.gameObject.CompareTag("Limits"))
        {
            Debug.Log("Muerto");
        }

        if (!isBeingPushed)
        {
            rb.velocity = Vector2.zero;
            isMoving = false;
        }
    }


    bool Beats(string playerHand, string enemyHand)
    {
        return (playerHand == "Rock" && enemyHand == "Scissors") ||
               (playerHand == "Paper" && enemyHand == "Rock") ||
               (playerHand == "Scissors" && enemyHand == "Paper");
    }
    
}
