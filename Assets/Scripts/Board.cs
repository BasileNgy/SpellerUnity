using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject mcellPrefab;

    public Cell[,] mAllCells = new Cell[10, 10];

    public Cell[,] Create()
    {
        for(int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                //Create the cell
                GameObject newCell = Instantiate(mcellPrefab, transform);
                newCell.name = "Cell (" + x + ":" + y + ")";
                newCell.GetComponent<Cell>().centerPoint = new Vector2(x + 0.5f, y + 0.5f);
                newCell.GetComponent<Cell>().cornerLowLeft = new Vector2(x, y);

                //Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(x + 0.5f , y + 0.5f);

                //Setup
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);

                //Debug.Log("Case "+ x + ":"+ y +" initialisée");
            }
        }

        return mAllCells;
    }

}
