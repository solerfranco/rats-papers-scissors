using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController: MonoBehaviour
{
    public Transform player;         // Referencia al jugador
    public float radius = 2f;        // Radio de la órbita

    public Sprite RockHand;
    public Sprite ScissorsHand;
    public Sprite PaperHand;

    private SpriteRenderer spriteRenderer;
    private PlayerMovementSlap playerObj;
    private string playerhand;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerObj = FindFirstObjectByType<PlayerMovementSlap>();
        
    }

    void Update()
    {
        if (playerObj != null)
        {
            playerhand = playerObj.hand;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            spriteRenderer.flipY = PlayerMovementSlap.facing;
        }
        if (!playerObj.isMoving)
        {
            SetSpriteByHand(playerhand);

            //Psición del mouse en mundo
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 direction = (mouseWorldPos - player.position).normalized;

            transform.position = player.position + direction * radius;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }


    void SetSpriteByHand(string hand)
    {
        switch (hand)
        {
            case "Rock":
                spriteRenderer.sprite = RockHand;
                break;
            case "Paper":
                spriteRenderer.sprite = PaperHand;
                break;
            case "Scissors":
                spriteRenderer.sprite = ScissorsHand;
                break;
        }
    }
}