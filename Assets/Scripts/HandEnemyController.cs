using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandEnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite RockHand;
    public Sprite ScissorsHand;
    public Sprite PaperHand;

    private Enemy enemy;
    public string enemyHand;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = FindFirstObjectByType<Enemy>();
        if (enemy != null)
        {
            enemyHand = enemy.enemyHand;
            SetSpriteByHand(enemyHand);
            if (enemy.GetComponent<SpriteRenderer>().flipX == true)
            {

                spriteRenderer.flipX = true;
                transform.position = new Vector3(transform.position.x - 3.50f, transform.position.y, transform.position.z);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

        if (enemy == null)
        {
            Destroy(gameObject); 
            return;
        }
        
 
    }


    void SetSpriteByHand(string enemyHand)
    {
        switch (enemyHand)
        {
            case "Rock":
                spriteRenderer.sprite = RockHand;
                break;
            case "Paper":
                spriteRenderer.sprite = PaperHand;
                break;
            case "Scissors":
                spriteRenderer.flipY = !spriteRenderer.flipY;
                spriteRenderer.sprite = ScissorsHand;
                break;
        }
    }
}
