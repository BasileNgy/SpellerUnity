using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName
{
    ARROW, FIREBALL, STONEWALL, PLAYER
}

public enum Direction
{
    UP, RIGHT, DOWN, LEFT, NW, NE, SW, SE
}

public class BoardElement : MonoBehaviour
{
    //real position transform
    public Vector3 position;
    //position on the chessboard (nombre entier naturels positifs)
    public Vector3 boardPosition;
    //goal position to move to
    public Vector3 objectivePosition;
    //list of postions to moveTo
    public List<Vector3> objectivesPositionsList; 
    //flag position reached
    public bool reached;
    public Board board;

    //Nombre maximal de déplacement autorisés en une seule itération
    protected int movesMax;
    //Nombre de déplacement effectués
    protected int movesDone;
    protected bool isMoving;

    public ItemName nameSpell;

    //Orientation du sprite et son équivalence en Vecteur
    public Direction orientation;
    public Vector2Int orientationAxes;

    //permet de centrer le sprite de l'item au centre de la case
    protected float offSetx;
    protected float offSety;

    public GameManager gameManager;

    private void Start()
    {
        objectivesPositionsList = new List<Vector3>();
        objectivePosition = transform.position;
        isMoving = false;
    }

    /*
     * Récupération de la position à atteindre
     */
    public void MoveItem(Direction dir)
    {
        orientation = dir;
        objectivesPositionsList.Clear();
        for (int i = 0; i < movesMax; i++)
        {
            Vector3 objectif = GetObjectivePosition();
            Vector3 oneOfTheObjectivePosition = objectif + new Vector3(offSetx, offSety, 0);
            objectivesPositionsList.Add(oneOfTheObjectivePosition);
            objectivePosition = objectivesPositionsList[0];
        }
        isMoving = true;
        movesDone = 0;
    }

    /*
     * Calcul de la positon objectif de l'item
     */
    private Vector3 GetObjectivePosition()
    {
        int x = (int) boardPosition.x;
        int y = (int) boardPosition.y;

        switch (orientation)
        {
            case Direction.UP:
                if (y < 9)
                {
                    y += 1;
                    orientationAxes.x = 0;
                    orientationAxes.y = 1;
                }
                break;

            case Direction.RIGHT:
                if (x < 9)
                {
                    x += 1;
                    orientationAxes.x = 1;
                    orientationAxes.y = 0;
                }
                break;

            case Direction.DOWN:
                if (y > 0)
                {
                    y -= 1;
                    orientationAxes.x = 0;
                    orientationAxes.y = -1;
                }
                break;

            case Direction.LEFT:
                if (x > 0)
                {
                    x -= 1;
                    orientationAxes.x = -1;
                    orientationAxes.y = 0;
                }
                break;

            case Direction.NW:
                if(x == 0 && y == 9)
                {
                    x += 1;
                    y -= 1;
                    orientation = Direction.SE;
                }
                else if(x == 0)
                {
                    x += 1;
                    y += 1;
                    orientation = Direction.NE;
                }
                else if(y == 9)
                {
                    x -= 1;
                    y -= 1;
                    orientation = Direction.SW;
                }
                else
                {
                    x -= 1;
                    y += 1;
                }
                break;

            case Direction.NE:
                if (x == 9 && y == 9)
                {
                    x -= 1;
                    y -= 1;
                    orientation = Direction.SW;
                }
                else if (x == 9)
                {
                    x -= 1;
                    y += 1;
                    orientation = Direction.NW;
                }
                else if (y == 9)
                {
                    x += 1;
                    y -= 1;
                    orientation = Direction.SE;
                }
                else
                {
                    x += 1;
                    y += 1;
                }
                break;

            case Direction.SW:
                if (x == 0 && y == 0)
                {
                    x += 1;
                    y += 1;
                    orientation = Direction.NE;
                }
                else if (x == 0)
                {
                    x += 1;
                    y -= 1;
                    orientation = Direction.SE;
                }
                else if (y == 0)
                {
                    x -= 1;
                    y += 1;
                    orientation = Direction.NW;
                }
                else
                {
                    x -= 1;
                    y -= 1;
                }
                break;

            case Direction.SE:
                if (x == 9 && y == 0)
                {
                    x -= 1;
                    y += 1;
                    orientation = Direction.NW;
                }
                else if (x == 9)
                {
                    x -= 1;
                    y -= 1;
                    orientation = Direction.SW;
                }
                else if (y == 0)
                {
                    x += 1;
                    y += 1;
                    orientation = Direction.NE;
                }
                else
                {
                    x += 1;
                    y -= 1;
                }
                break;
            

            default:
                Debug.LogError("Erreur Direction déplacement");
                break;
        }
        boardPosition.x = x;
        boardPosition.y = y;

        return GameManager.cellsBoard[x, y].cornerLowLeft;
    }
    
    /*
     * Set le joueur à une case précise
     */
    public void SetPositionCharacter(Vector3 pos)
    {
        boardPosition = pos;
        transform.position = new Vector3(pos.x + offSetx, pos.y + offSety, 0);
        objectivePosition = transform.position;
    }

    /*
     * Set un item à une case précise
     */
    public void SetPosition(Vector3 pos)
    {
        transform.position = new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0);
        objectivePosition = transform.position;
    }

    public virtual bool JobDone()
    {
        return false;
    }
}
