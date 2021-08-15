using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public static Cell[,] cellsBoard;
    public GameObject playerObject1;
    public GameObject playerObject2;
    public ShaderManager shaderManager;

    public Arrow arrow;
    public Flame flame;
    private PlayerController player1;
    private PlayerController player2;
    private PlayerController playerInTurn;
    private List<BoardElement> listItem;

    public ItemName spellSelected;
    /*Spell ideas :
     * 3 bloc de terre sur une ligne autour du personnage
     * bourrasque pour déplacer l'adversaire
     * Miroir pour renvoyer une attaque dans l'autre sens
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

        FirstPlayerGenerator();
        SetupPlayers();

        uiManager.DisableAllControllers();
        uiManager.DisableWInPanel();
        uiManager.DisableCancelButton();
        uiManager.DisableEndTurnButton();

        BeginTurn();
    }

    private void SetupPlayers()
    {
        player1.SetPositionCharacter(new Vector3(1.5f, 1.8f, 0));
        player2.SetPositionCharacter(new Vector3(8.5f, 8.8f, 0));

        player1.tagg = 1;
        player2.tagg = 2;

        player1.health = 3;
        player2.health = 3;

        uiManager.SetLifeBars(player1);
        uiManager.SetLifeBars(player2);
    }

    /*
     * Choix Aléatoire du premier joueur 
     */
    private void FirstPlayerGenerator()
    {
        Random rand = new Random();
        if (rand.Next(2) == 0)
        {
            playerInTurn = player1;
        }
        else
        {
            playerInTurn = player2;
        }
        
    }

    /*
     * Test continu de la condition de victoire : barre de vie de chaque joueur
     */
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

    /*
     * Déplacement du joueur, méthode appelée par les 4 flèches buttons UI
     */
    public void PlayerDeplacement(int i)
    {
        if (playerInTurn.moveCounter < playerInTurn.maxDeplacement)
        {
            switch (i)
            {
                case 0:
                    playerInTurn.MoveItem(Direction.UP);
                    break;
                case 1:
                    playerInTurn.MoveItem(Direction.RIGHT);
                    break;
                case 2:
                    playerInTurn.MoveItem(Direction.DOWN);
                    break;
                case 3:
                    playerInTurn.MoveItem(Direction.LEFT);
                    break;
            }

            playerInTurn.moveCounter++;

            if (playerInTurn.moveCounter == 2)
            {
                EndMoves();
            }
                
        }
        else
        {
            Debug.Log("Nombre de déplacement maximum atteint");
        }
            
    }

    /*
     * Début d'un tour, RAZ du comteur de pas et affichage des UI
     */
    public void BeginTurn()
    {
        playerInTurn.moveCounter = 0;
        uiManager.SetNameTurn(playerInTurn);
        uiManager.EnableMoveArrows();
        uiManager.EnableEndMoveButton();
    }

    /*
     * Fin de tour, déplacement des différents items sur le plateau, activation/désaction des UI nécessaire
     * changement de joueur et début du tour suivvant
     */
    public void EndTour()
    {
        MoveItems();

        uiManager.DisableAttackArrows();
        uiManager.DisableSpellButtons();
        uiManager.DisableMoveArrows();
        uiManager.DisableEndTurnButton();
        uiManager.DisableCancelButton();

        NextPlayer();

        BeginTurn();
    }
    
    /*
     * Désactive et active les UIs nécessaire lors de la fin de la phase de déplacement
     */
    public void EndMoves()
    {
        uiManager.EndMovePhase();
    }

    /*
     * Permet de dépalcer tous les items présent selon leur movesets et tester si leur but est atteint et doivent être détruits
     */
    private void MoveItems()
    {
        List<BoardElement> itemToRemove = new List<BoardElement>();

        foreach (BoardElement item in listItem)
        {
            if (item.JobDone())
            {
                itemToRemove.Add(item);
                Destroy(item.gameObject);
            }
            else
            {
                item.MoveItem(item.orientation);
            }
            
        }
        foreach (BoardElement item in itemToRemove)
            listItem.Remove(item);
    }

    /*
     * Lorsque le tour est terminé, playerINTurn est actualisé avec le joueur suivant
     */
    private void NextPlayer()
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
        float x2 = x1;
        float x3 = x1;

        float y1 = playerInTurn.boardPosition.y;
        float y2 = y1;
        float y3 = y1;
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
                spellSelected = ItemName.ARROW;
                break;
            case 1:
                spellSelected = ItemName.FIREBALL;
                break;

            default :
                Debug.Log("Error Spell ID");
                break;
        }
        uiManager.DisableSpellButtons();
        if(spellSelected == ItemName.FIREBALL)
            uiManager.EnableAttackDiagonalArrows(playerInTurn);
        else
            uiManager.EnableAttackArrows(playerInTurn);
        uiManager.EnableCancelButton();
        uiManager.TurnONCancelButton();
    }

    //Choisi l'orientation du sort et le lancer
    public void LaunchSpell(int OrientationID)
    {
        switch(spellSelected)
        {
            case ItemName.ARROW :
                SpawnArrows((Direction) OrientationID);
                break;
            case ItemName.FIREBALL:
                SpawnFlame((Direction)OrientationID);
                break;
            default:
                Debug.Log("Error Spell or Orientation");
                break;
        }
        uiManager.DisableAttackArrows();
        uiManager.DisableCancelButton();
    }

    /*
     * Permet d'appliquer les différentes conséquences lorsqu'un joueur prends des dégâts
     * destruction de l'objet, update de la health bar, animation shader
     */
    public void AppliesDamage(BoardElement item, PlayerController player)
    {
        listItem.Remove(item);
        Destroy(item.gameObject);
        player.TakeDamages();
        uiManager.SetLifeBars(player);
        shaderManager.StartWaves();
    }
}
