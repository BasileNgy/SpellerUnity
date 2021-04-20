using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIType
{

}

public class UIManager : MonoBehaviour
{
    public CanvasController moveDirections;
    public CanvasController spellDirections;
    public CanvasController spells;
    public CanvasController turnButtons;
    public CanvasController winPanel;
    public Text winnerName;
    public GameObject spellDirectionUP;
    public GameObject spellDirectionRIGHT;
    public GameObject spellDirectionDOWN;
    public GameObject spellDirectionLEFT;
    public HealthBar redLife;
    public HealthBar greenLife;
    public Text turnName;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void DisableAllControllers()
    {
        DisableAttackArrows();
        DisableMoveArrows();
        DisableSpellButtons();
    }

    public void SetNameTurn(PlayerController p)
    {
        turnName.text = "Tour de "+p.playerName;
    }
        
    public void DisableMoveArrows()
    {
        moveDirections.gameObject.SetActive(false);
    }

    public void EnableMoveArrows()
    {
        moveDirections.gameObject.SetActive(true);
    }

    /* Désactive les flèches d'attaques et reset la rotation
     */
    public void DisableAttackArrows()
    {
        EnableDirectionsAttack();
        spellDirections.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
        spellDirections.gameObject.SetActive(false);
        Debug.Log("Camouflage" + spellDirections.transform.rotation);
    }

    /*réactive toutes les flèches de directions avant le tour du prochain joueur
     */
    private void EnableDirectionsAttack()
    {
        spellDirectionUP.SetActive(true);
        spellDirectionRIGHT.SetActive(true);
        spellDirectionDOWN.SetActive(true);
        spellDirectionLEFT.SetActive(true);
    }

    public void EnableAttackArrows(PlayerController p)
    {
        DisableDirections(p);
        spellDirections.gameObject.SetActive(true);
    }

    public void EnableAttackDiagonalArrows(PlayerController p)
    {
        spellDirections.transform.Rotate(Vector3.forward * 45);
        DisableDiagonalDirections(p);
        spellDirections.gameObject.SetActive(true);
        Debug.Log("Rotation"+spellDirections.transform.rotation);
    }

    public void EnableSpellButtons()
    {
        spells.gameObject.SetActive(true);
    }

    public void DisableSpellButtons()
    {
        spells.gameObject.SetActive(false);
    }

    public void SetLifeBars(PlayerController player)
    {
        if (player.tagg == 1)
            redLife.setFilledHearts(player.health);
        else if (player.tagg == 2) 
            greenLife.setFilledHearts(player.health);
    }

    /*
     * Désactive les directions de tir impossibles quand le joueur est contre un mur
     */
    private void DisableDirections(PlayerController p)
    {
        if(p.boardPosition.x == 0)
        {
            spellDirectionLEFT.SetActive(false);
        }
        if (p.boardPosition.x == 9)
        {
            spellDirectionRIGHT.SetActive(false);
        }
        if (p.boardPosition.y == 0)
        {
            spellDirectionDOWN.SetActive(false);
        }
        if (p.boardPosition.y == 9)
        {
            spellDirectionUP.SetActive(false);
        }
    }

    /*
     * Désactive les directions de tir diagonal impossibles quand le joueur est contre un mur
     */
    private void DisableDiagonalDirections(PlayerController p)
    {
        if (p.boardPosition.x == 0)
        {
            spellDirectionUP.SetActive(false);
            spellDirectionLEFT.SetActive(false);
        }
        if (p.boardPosition.x == 9)
        {
            spellDirectionRIGHT.SetActive(false);
            spellDirectionDOWN.SetActive(false);
        }
        if (p.boardPosition.y == 0)
        {
            spellDirectionDOWN.SetActive(false);
            spellDirectionLEFT.SetActive(false);
        }
        if (p.boardPosition.y == 9)
        {
            spellDirectionUP.SetActive(false);
            spellDirectionRIGHT.SetActive(false);
        }
    }

    public void DisableWInPanel()
    {
        winPanel.gameObject.SetActive(false);
    }

    public void EnableWInPanel(PlayerController player)
    {
        if (player.tagg == 1)
            winnerName.text = "Red Wins !";
        else if (player.tagg == 2)
            winnerName.text = "Green Wins !";
        winPanel.gameObject.SetActive(true);
    }
}
