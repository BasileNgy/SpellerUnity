using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BoardElement
{
    public float speed;
    private Animator animator;
    [HideInInspector]
    public int health;

    public int moveCounter;
    public int maxMove = 2;

    public string playerName;

    [HideInInspector]
    public int tagg;

    void Start()
    {
        animator = GetComponent<Animator>();
        position = transform.position;
        objectivePosition = transform.position;

        moveCounter = 0;

        health = 3;

        offSetx = 0.5f;
        offSety = 0.8f;
    }

    void Update()
    {
        //test si la position est atteinte et déplace le sprite
        if (transform.position != objectivePosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectivePosition, speed * Time.deltaTime);
            reached = false;
        }
        else
        {
            position = objectivePosition;
            reached = true;
        }

        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove() 
    {
        if (transform.position != objectivePosition)
        {
            animator.SetFloat("moveX", orientationAxes.x);
            animator.SetFloat("moveY", orientationAxes.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    public void TakeDamages()
    {
        health--;
        Debug.Log("Ouch touché, mtn ma vie actuelle est : " + health);
    }
}
