using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float acceleration = 0.2f; 
    public float maxSpeed = 8f;
    public float stopDuration = 10f;
    public float drag = 5f;
    public string enemyHand;
    public string Rats;
    public Sprite[] RatsSprites;
    public SpriteMeshType sprites;

    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private string[] TicTacToe = { "Rock", "Paper", "Scissors" };
    private string[] RatsType = { "Basic", "Fast", "Special" };
    private bool isBeingPushed = false;
    private bool canMove = true;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Target").transform;
        enemyHand = TicTacToe[Random.Range(0, TicTacToe.Length)];
        Rats = TicTacToe[Random.Range(0, TicTacToe.Length)];
        SetRatSprite(enemyHand);
        if (target.position.x > transform.position.x){
            spriteRenderer.flipX = true;
        }

    }

    void FixedUpdate()
    {
        if (isBeingPushed)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * drag);
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
                isBeingPushed = false;
            }
        }
        else if (target != null && canMove)
        {
            // Aumenta la velocidad progresivamente hasta el límite
            speed = Mathf.Min(speed + acceleration * Time.fixedDeltaTime, maxSpeed);

            Vector2 direction = (target.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canMove = false;
            isBeingPushed = true;

            StartCoroutine(RecoverMovement());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator RecoverMovement()
    {
        yield return new WaitForSeconds(stopDuration);
        canMove = true;
    }

    void SetRatSprite(string Rat)
    {
        switch (Rat)
        {
            case "Scissors":
                spriteRenderer.sprite = RatsSprites[0];
                break;
            case "Papper":
                spriteRenderer.sprite = RatsSprites[1];
                break;
            case "Rock":
                spriteRenderer.sprite = RatsSprites[2];
                break;
        }
    }
}
