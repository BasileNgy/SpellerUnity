using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public static Cell[,] cellsBoard;
    public GameObject playerObject1;
    public GameObject playerObject2;

    public Arrow arrow;
    public Flame flame;
    private PlayerController player1;
    private PlayerController player2;
    private PlayerController playerInTurn;
    private List<BoardElement> listItem;

    public Spell spellSelected;
    /*Spell ideas :
     * 3 boules de feu en diagonale
     * 3 bloc de terre sur une ligne autour du personnage
     * bourrasque pour déplacer l'adversaire
     * balle qui peut rebondir sur 3 murs à grande vitesse (4 cases par tour)
     */

    public UIManager uiManager;

    private void Awake()
    {
        cellsBoard = mBoard.Create();
    }

    void Start()
    {      
        player1 = playerObject1.GetComponent<PlayerController>();
        player2 = playerObject2.GetComponent<PlayerController>();
        listItem = new List<BoardElement>();

        playerInTurn = player1;
        SetupPlayers();

        uiManager.DisableAllControllers();
        uiManager.DisableWInPanel();

        BeginTurn();
    }

    private void SetupPlayers()
    {
        player1.SetPositionCharacter(new Vector3(1, 1, 0));
        player2.SetPositionCharacter(new Vector3(8, 8, 0));

        player1.tagg = 1;
        player2.tagg = 2;

        player1.health = 3;
        player2.health = 3;

        uiManager.SetLifeBars(player1);
        uiManager.SetLifeBars(player2);
    }

    void Update()
    {
        if (player1.health == 0)
        {
            uiManager.EnableWInPanel(player2);
            uiManager.DisableAllControllers();
        }     
        else if (player2.health == 0)
        {
            uiManager.EnableWInPanel(player1);
            uiManager.DisableAllControllers();
        }
            
    }

    public void PlayerDeplacement(int i)
    {
        if (playerInTurn.moveCounter < playerInTurn.maxMove)
        {
            switch (i)
            {
                case 0:
                    playerInTurn.MoveCharacter(Direction.UP);
                    break;
                case 1:
                    playerInTurn.MoveCharacter(Direction.RIGHT);
                    break;
                case 2:
                    playerInTurn.MoveCharacter(Direction.DOWN);
                    break;
                case 3:
                    playerInTurn.MoveCharacter(Direction.LEFT);
                    break;
            }

            playerInTurn.moveCounter++;

            if (playerInTurn.moveCounter == 2)
            {
                uiManager.DisableMoveArrows();
                uiManager.EnableSpellButtons();
            }
                
        }
        else
        {
            Debug.Log("Nombre de déplacement maximum atteint");
        }
            
    }

    public void BeginTurn()
    {
        Debug.Log("Début du tour");
        uiManager.SetNameTurn(playerInTurn);
        uiManager.EnableMoveArrows();
        playerInTurn.moveCounter = 0;
    }

    public void EndTour()
    {
        MoveItems();

        uiManager.DisableAttackArrows();
        uiManager.DisableSpellButtons();
        uiManager.DisableMoveArrows();

        nextPlayer();

        BeginTurn();
    }

    private void MoveItems()
    {
        List<BoardElement> itemToRemove = new List<BoardElement>();

        foreach (BoardElement item in listItem)
        {
            for (int i = 0; i < item.deplacement; i++)
            {
                if (item.JobDone())
                {
                    itemToRemove.Add(item);
                    Destroy(item.gameObject);
                }
                else
                {
                    item.MoveCharacter(item.orientation);
                }
            }
        }
        foreach (BoardElement item in itemToRemove)
            listItem.Remove(item);
    }


    private void nextPlayer()
    {
        if (playerInTurn == player1)
            playerInTurn = player2;
        else
            playerInTurn = player1;
    }

    //Spawn 3 Arrows on a specific side of the player
    public void SpawnArrows(Direction dir)
    {
        float x1 = playerInTurn.boardPosition.x;
        float x2 = playerInTurn.boardPosition.x;
        float x3 = playerInTurn.boardPosition.x;

        float y1 = playerInTurn.boardPosition.y;
        float y2 = playerInTurn.boardPosition.y;
        float y3 = playerInTurn.boardPosition.y;
        float rotation = 0;

        switch (dir)
        {
            case Direction.UP:
                y1 += 1;

                x2 -= 1;
                y2 += 1;

                x3 += 1;
                y3 += 1;
                rotation = 90;
                break;

            case Direction.RIGHT:
                x1 += 1;

                x2 += 1;
                y2 += 1;

                x3 += 1;
                y3 -= 1;
                rotation = 0;
                break;

            case Direction.DOWN:
                y1 -= 1;

                x2 += 1;
                y2 -= 1;

                x3 -= 1;
                y3 -= 1;
                rotation = 270;
                break;

            case Direction.LEFT:
                x1 -= 1;

                x2 -= 1;
                y2 += 1;

                x3 -= 1;
                y3 -= 1;
                rotation = 180;
                break;
        }
        Arrow arrowClone1 = Instantiate(arrow, new Vector3(x1+0.5f, y1+0.5f, 0), new Quaternion(0,0,0,0));

        arrowClone1.transform.rotation = Quaternion.Euler(0, 0, rotation);
        arrowClone1.boardPosition = new Vector3(x1, y1, 0);
        arrowClone1.objectivePosition = arrowClone1.transform.position;
        arrowClone1.orientation = dir;
        arrowClone1.gameManager = this;
        listItem.Add(arrowClone1);

        if(x2 >= 0 && x2 <= 9 && y2 >= 0 && y2 <= 9)
        {
            Arrow arrowClone2 = Instantiate(arrow, new Vector3(x2 + 0.5f, y2 + 0.5f, 0), new Quaternion(0, 0, 0, 0));

            arrowClone2.transform.rotation = Quaternion.Euler(0, 0, rotation);
            arrowClone2.boardPosition = new Vector3(x2, y2, 0);
            arrowClone2.objectivePosition = arrowClone2.transform.position;
            arrowClone2.orientation = dir;
            arrowClone2.gameManager = this;
            listItem.Add(arrowClone2);
        }

        if (x3 >= 0 && x3 <= 9 && y3 >= 0 && y3 <= 9)
        {
            Arrow arrowClone3 = Instantiate(arrow, new Vector3(x3 + 0.5f, y3 + 0.5f, 0), new Quaternion(0, 0, 0, 0));

            arrowClone3.transform.rotation = Quaternion.Euler(0, 0, rotation);
            arrowClone3.boardPosition = new Vector3(x3, y3, 0);
            arrowClone3.objectivePosition = arrowClone3.transform.position;
            arrowClone3.orientation = dir;
            arrowClone3.gameManager = this;
            listItem.Add(arrowClone3);
        }
    }

    /* Spawn 1 Flame in diagonal a specific side of the player
     * On utilise les boutons de directions classiques qu'on tourne à 45°
     * donc on Haut = Nord West, Droite = Nord Est etc...
     */
    public void SpawnFlame(Direction dir)
    {
        float x = playerInTurn.boardPosition.x;
        float y = playerInTurn.boardPosition.y;
        float rotation = 0;
        Direction orientation = Direction.NW;
        switch (dir)
        {
            case Direction.UP:
                x -= 1;
                y += 1;
                orientation = Direction.NW;
                break;

            case Direction.RIGHT:
                x += 1;
                y += 1;
                orientation = Direction.NE;
                break;

            case Direction.DOWN:
                x += 1;
                y -= 1;
                orientation = Direction.SE;
                break;

            case Direction.LEFT:
                x -= 1;
                y -= 1;
                orientation = Direction.SW;
                break;
        }
        Flame flameClone = Instantiate(flame, new Vector3(x + 0.5f, y + 0.5f, 0), new Quaternion(0, 0, 0, 0));

        flameClone.transform.rotation = Quaternion.Euler(0, 0, rotation);
        flameClone.boardPosition = new Vector3(x, y, 0);
        flameClone.objectivePosition = flameClone.transform.position;
        flameClone.orientation = orientation;
        flameClone.gameManager = this;
        listItem.Add(flameClone);
    }

    //Choisis un sort à lancer
    public void SelectSpell(int spellID)
    {
        switch(spellID)
        {
            case 0 : 
                spellSelected = Spell.ARROW;
                break;
            case 1:
                spellSelected = Spell.FIREBALL;
                break;

            default :
                Debug.Log("Error Spell ID");
                break;
        }
        uiManager.DisableSpellButtons();
        if(spellSelected == Spell.FIREBALL)
            uiManager.EnableAttackDiagonalArrows(playerInTurn);
        else
            uiManager.EnableAttackArrows(playerInTurn);
    }

    //Choisi l'orientation du sort et le lancer
    public void LaunchSpell(int OrientationID)
    {
        switch(spellSelected)
        {
            case Spell.ARROW :
                SpawnArrows((Direction) OrientationID);
                break;
            case Spell.FIREBALL:
                SpawnFlame((Direction)OrientationID);
                break;
            default:
                Debug.Log("Error Spell or Orientation");
                break;
        }
        uiManager.DisableAttackArrows();
    }

    public void AppliesDamage(BoardElement item, PlayerController player)
    {
        listItem.Remove(item);
        Destroy(item.gameObject);
        player.TakeDamages();
        uiManager.SetLifeBars(player);
    }
}
