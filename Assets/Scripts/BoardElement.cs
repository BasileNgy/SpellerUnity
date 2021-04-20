using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Spell
{
    ARROW, FIREBALL, STONEWALL
}

public enum Direction
{
    UP, RIGHT, DOWN, LEFT, NW, NE, SW, SE
}

public class BoardElement : MonoBehaviour
{
    //real position transform
    public Vector3 position;
    //position on the chessboard
    public Vector3 boardPosition;
    //goal position to move to
    public Vector3 objectivePosition;
    //position reached
    public bool reached;
    public Board board;

    public int deplacement;

    //Orientation du sprite et son équivalence en Vecteur
    public Direction orientation;
    public Vector2Int orientationAxes;

    //permet de centrer le sprite de l'item au centre de la case
    protected float offSetx;
    protected float offSety;

    public GameManager gameManager;

    void Update()
    {
        
    }

    /*
     * Récupération de la position à atteindre
     */
    public Vector3 MoveCharacter(Direction dir)
    {
        orientation = dir;
        Vector3 objectif = GetObjectivePosition();
        objectivePosition = objectif + new Vector3(offSetx, offSety, 0);

        return objectif;
    }

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
     * Calcule le vecteur de direction 
     
    public Vector3 GetVectorMove()
    {
        Vector3 objectif = boardPosition;
        orientationAxes = Vector2Int.zero;

        switch (orientation)
        {
            case Direction.UP:
                if (position.y < 9)
                {
                    objectif = new Vector3(boardPosition.x, boardPosition.y + 1, 0);
                    orientationAxes.y = 1;
                }
                else
                    Debug.Log("Bord du terrain");
                break;

            case Direction.RIGHT:
                if (position.x < 9)
                {
                    objectif = new Vector3(boardPosition.x + 1, boardPosition.y, 0);
                    orientationAxes.x = 1;
                }
                else
                    Debug.Log("Bord du terrain");
                break;

            case Direction.DOWN:
                if (position.y > 1)
                {
                    objectif = new Vector3(boardPosition.x, boardPosition.y - 1, 0);
                    orientationAxes.y = -1;
                }
                else
                    Debug.Log("Bord du terrain");
                break;

            case Direction.LEFT:
                if (position.x > 1)
                {
                    objectif = new Vector3(boardPosition.x - 1, boardPosition.y, 0);
                    orientationAxes.x = -1;
                }
                else
                    Debug.Log("Bord du terrain");
                break;

            default:
                objectif = boardPosition;
                Debug.LogError("Erreur Direction déplacement");
                break;
        }
        boardPosition = objectif;
        return objectif;
    }*/

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
