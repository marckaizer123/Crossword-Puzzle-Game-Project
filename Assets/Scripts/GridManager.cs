using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Globals;

/// <summary>
/// Function for the grid that the crossword would be placed on.
/// </summary>
public class GridManager : Singleton<GridManager>
{
    [SerializeField]
    private GameObject crosswordPanel;

    [SerializeField]
    private GameObject[] tilePrefabs;

    /// <summary>
    /// A dictionary that contains all the tiles in the game.
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }

    public float TileSize //Gets the width of the tile.
    {
        get
        {
            return (tilePrefabs[0].GetComponent<Image>().sprite.bounds.size.x * tilePrefabs[0].transform.localScale.x);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {

        for (int x = 0; x < CrosswordManager.Instance.wordMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < CrosswordManager.Instance.wordMatrix.GetLength(1); y++)
            {
                if (CrosswordManager.Instance.wordMatrix[x, y].Equals('\0'))
                {
                    PlaceTile(1, x, y, CrosswordManager.Instance.wordMatrix[x, y]);
                }
                else
                {
                    PlaceTile(0, x, y, CrosswordManager.Instance.wordMatrix[x, y]);
                }

               
            }
        }
    }

    private void PlaceTile(int tileIndex, int x, int y, char letter)
    {
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Sets the parameters of the tile.
        newTile.Setup2(crosswordPanel.transform, new Point(x, y), letter);
    }

    

    private void CreateNodes()
    {
        Dictionary<Point, Node> nodes = new Dictionary<Point, Node>();

        //Add nodes to node dictionary
        foreach (TileScript tile in GridManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }
}
