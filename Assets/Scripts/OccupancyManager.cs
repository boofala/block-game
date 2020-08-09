using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupancyManager : MonoBehaviour
{
    private bool[,] occupancyGrid;
    private int numRows = GameSettings.NUM_ROWS;
    private int numColumns = GameSettings.NUM_COLUMNS;


    // Start is called before the first frame update
    void Start()
    {
        this.occupancyGrid = new bool[numRows, numColumns];
    }

    public bool getOccupancy(int row, int column)
    {
        if (isInBounds(row, column))
        {
            return this.occupancyGrid[row, column];
        }
        else return true;
    }

    public void setOccupancy(int row, int column, bool value)
    {
        this.occupancyGrid[row, column] = value;
    }

    private bool isInBounds(int row, int column)
    {
        // check row
        if (row < 0 || row >= numRows)
        {
            return false;
        }
        // check column
        if (column < 0 || column >= numColumns)
        {
            return false;
        }
        return true;
    }

}
