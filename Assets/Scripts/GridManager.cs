using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float xOffset, zOffset;
    public int numRows, numColumns;
    public float xSize, zSize;
    public GameObject planePrefab;
    private GameObject[,] tiles;

    void Awake()
    {
        tiles = new GameObject[numRows, numColumns];
        for (int i = 0; i < numColumns * numRows; i++)
        {
            int rowIdx = i / numColumns;
            int columnIdx = i % numColumns;
            GameObject tile = Instantiate(planePrefab, new Vector3(xOffset + (xSize * columnIdx), 0f, zOffset + (-zSize * rowIdx)), Quaternion.identity);
            tiles[rowIdx, columnIdx] = tile;
        }
    }

    public Color GetColor(int row, int column)
    {
        GameObject tile = this.tiles[row, column];
        return tile.GetComponent<Renderer>().material.GetColor("_Color");
    }

    public void SetColor(int row, int column, Color color)
    {
        GameObject tile = this.tiles[row, column];
        tile.GetComponent<Renderer>().material.SetColor("_Color", color);
    }
    
    public Vector3 GetPosition(int row, int column)
    {
        return this.tiles[row, column].transform.position;
    }


}
