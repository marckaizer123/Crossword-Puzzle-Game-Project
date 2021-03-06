using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Globals;

/// <summary>
/// Function for the grid that the crossword would be placed on.
/// </summary>
public class GridManager : Singleton<GridManager>
{
    [SerializeField]
    private GameObject crosswordPanel;


    [SerializeField]
    private Transform grid;

    [SerializeField]
    private GameObject[] tilePrefabs;

    public float TileSize //Gets the width of the tile.
    {
        get
        {
            return (tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x * tilePrefabs[0].transform.localScale.x);
        }
    }

    /// <summary>
    /// A dictionary that contains all the tiles in the game.
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void PlaceTile(int tileIndex, int x, int y, Vector3 gridStart)
    {

        //Creates a new tile and makes a reference to that tile using the newTile variable.
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Sets the position of the tile.
        newTile.Setup(new Point(x, y), new Vector3(gridStart.x + (TileSize * x), gridStart.y - (TileSize * y), 0), grid);
    }

    private void CreateGrid()
    {
        Tiles = new Dictionary<Point, TileScript>();

        //Calculate the grid's size using World Units

        float worldGridLength = GameManager.Instance.GridCellCount * TileSize;

        //Convert to Screen Units

        float screenGridLength = Camera.main.WorldToScreenPoint(new Vector3(worldGridLength, worldGridLength)).x - Camera.main.WorldToScreenPoint(new Vector3(0, 0)).x;
        float screenPanelYPosition = Camera.main.WorldToScreenPoint(crosswordPanel.transform.position).y;

        //Calculates the grid's start point.

        Vector3 gridStart = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width - screenGridLength) / 2, screenPanelYPosition));

        for (int y = 0; y < GameManager.Instance.GridCellCount; y++) //The y positions of the tiles
        {
            for (int x = 0; x < GameManager.Instance.GridCellCount; x++) // The x positions of the tiles
            {
                //Places the tile in the world
                PlaceTile(0,
                          x,
                          y,
                          gridStart);
            }
        }

        CreateNodes(); //Adds the tiles, their gridPosition, and their worldPositions into a list.
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




    // Update is called once per frame
    void Update()
    {
        
    }
}
