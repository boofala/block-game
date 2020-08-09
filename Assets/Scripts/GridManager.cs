using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject planePrefab;

    private int numRows = GameSettings.NUM_ROWS;
    private int numColumns = GameSettings.NUM_COLUMNS;
    private float xSize = 1f, zSize = 1f;
    private GameObject[,] tiles;
    private float xOffset, zOffset;

    void Awake()
    {
        xOffset = - (numRows - 1) / 2f;
        zOffset = (numColumns - 1) / 2f;
        tiles = new GameObject[numRows, numColumns];
        for (int i = 0; i < numColumns * numRows; i++)
        {
            int rowIdx = i / numColumns;
            int columnIdx = i % numColumns;
            GameObject tile = Instantiate(planePrefab, new Vector3(xOffset + (xSize * columnIdx), 0f, zOffset + (-zSize * rowIdx)), Quaternion.identity);
            tiles[rowIdx, columnIdx] = tile;
        }
    }

    public void SetColor(int row, int column, Color color)
    {
        GameObject tile = this.tiles[row, column];
        var renderer = tile.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
}
    
    public Vector3 GetPosition(int row, int column)
    {
        return this.tiles[row, column].transform.position;
    }
}
