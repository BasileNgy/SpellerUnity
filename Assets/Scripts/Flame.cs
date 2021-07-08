using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : BoardElement
{
    private int rebond;
    private int nbrMaxRebond = 2;

    private void Start()
    {
        offSetx = 0.5f;
        offSety = 0.5f;
        rebond = 0;
        movesMax = 2;
        nameSpell = ItemName.FIREBALL;
    }

    private void Update()
    {
        //test si la position est atteinte et deplace le sprite
        if (transform.position != objectivePosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectivePosition, 4 * Time.deltaTime);
            reached = false;
        }
        else
        {
            if(isMoving)
            { 
                movesDone++;
                if (movesDone == movesMax)
                {
                    reached = true;
                    isMoving = false;
                }
                else
                {
                    objectivePosition = objectivesPositionsList[movesDone];
                }
                //position = objectivePosition;
            }
        }
    }

    /*
     * Si la flamme a atteint le bord du terrain, elle retourne true et incrémente la valeur du rebond
     */
    public override bool JobDone()
    {
        if (boardPosition.x == 0 || boardPosition.y == 0 || boardPosition.x == 9 || boardPosition.y == 9 )
        {
            rebond ++;
        }
        if (rebond > nbrMaxRebond)
            return true;

        return false;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            gameManager.AppliesDamage(this, collision.gameObject.GetComponent<PlayerController>());
        }
    }
}
