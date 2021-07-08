using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BoardElement
{

    private void Start()
    {
        offSetx = 0.5f;
        offSety = 0.5f;
        movesMax = 1;
        nameSpell = ItemName.ARROW;
    }

    private void Update()
    {
        //test si la position est atteinte et déplace le sprite
        if (transform.position != objectivePosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectivePosition, 4 * Time.deltaTime);
            reached = false;
        }
        else
        {
            isMoving = false;
            reached = true;
        }
    }

    /*
     * Si la flèche a atteint le bord du terrain, elle retourne true
     */
    public override bool JobDone()
    {
        switch(orientation)
        {
            case Direction.UP:
                if (boardPosition.y == 9)
                    return true;
                break;

            case Direction.RIGHT:
                if (boardPosition.x == 9)
                    return true;
                break;

            case Direction.DOWN:
                if (boardPosition.y == 0)
                    return true;
                break;

            case Direction.LEFT:
                if (boardPosition.x == 0)
                    return true;
                break;

            default: 
                return false;
        }
        return false;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            gameManager.AppliesDamage(this, collision.gameObject.GetComponent<PlayerController>());
        }
    }
}
