using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementSlap : MonoBehaviour
{
    public float speed = 40f;
    public float maxRange = 3f;
    public float graceTime = 1f;
    public float pushDistance = 2f;
    public float pushForce = 10f;
    public float drag = 5f;
    public int killStreak = 0;
    private float comboTimer = 0f;
    public float comboResetTime = 3f; // tiempo en segundos para reiniciar el combo

    private Rigidbody2D rb;
    private Vector3 targetPosition;
    public bool isMoving = false;
    public static bool Attacking = false;
    public string hand;

    private Color c;
    private SpriteRenderer spriteRenderer;

    public GameController gameController;
    public GameObject objHand;
    public static bool facing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        c = spriteRenderer.color;
    }

    void Update()
    {
        if(killStreak > 0)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer >= comboResetTime)
            {
                killStreak = 0;
                comboTimer = 0f;
            }
        }
        if (objHand != null)
        {
            spriteRenderer.flipX = objHand.transform.position.x < transform.position.x;
            facing = spriteRenderer.flipX;
        }

        if (!isMoving)
        {
            rb.velocity = Vector2.zero;
            
            c.a = 1f;
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 direction = (mouseWorldPos - transform.position).normalized;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                hand = "Rock";
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                hand = "Paper";
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                hand = "Scissors";
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.X))
            {
                targetPosition = mouseWorldPos;
                isMoving = true;
                Attacking = true;
            }
        }
        
       
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(targetPosition);
            isMoving = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            string enemyHand = enemy.enemyHand;
            Vector2 recoilDirection = (transform.position - collision.transform.position).normalized;

            if (Attacking)
            {
                if (Beats(hand, enemyHand))
                {
                    rb.velocity = Vector2.zero;
                    comboTimer = 0f;
                    killStreak++;
                    gameController.score += killStreak * 74;
                    FindObjectOfType<ComboSliderUI>().StartCombo();
                    Destroy(collision.gameObject);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    rb.velocity = recoilDirection * 40f;
                    killStreak = 0;
                }

                Attacking = false;
                isMoving = false;
            }
            else
            {
                rb.velocity = Vector2.zero;
                killStreak = 0;
            }
        }

        else if (collision.gameObject.CompareTag("Limits"))
        {
            Debug.Log("Muerto");
        }
    }

    bool Beats(string playerHand, string enemyHand)
    {
        return (playerHand == "Rock" && enemyHand == "Scissors") ||
               (playerHand == "Paper" && enemyHand == "Rock") ||
               (playerHand == "Scissors" && enemyHand == "Paper");
    }

    void UpdateVisualByHand()
    {
        switch (hand)
        {
            case "Rock": spriteRenderer.color = Color.red; break;
            case "Paper": spriteRenderer.color = Color.green; break;
            case "Scissors": spriteRenderer.color = Color.white; break;
        }
    }
}
