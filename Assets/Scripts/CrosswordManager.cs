using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordManager : Singleton<CrosswordManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;

    [SerializeField]
    private Transform map;



    /// <summary>
    /// A dictionary that contains all the tiles in the game.
    /// </summary>
    public Dictionary<Point, TileScript> Tiles { get; set; }

    public float TileSize //Gets the width of the tile.
    {
        get
        {
            return (tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x * tilePrefabs[0].transform.localScale.x);
        }
    }

    [SerializeField]
    private int gridRows;


    private void PlaceTile(string tileType, int x, int y, Vector3 gridStart)
    {
        //Changes tileType into an integer so that it can be used as an index when we create a new tile.
        int tileIndex = int.Parse(tileType);

        //Creates a new tile and makes a reference to that tile using the newTile variable.
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        //Sets the position of the tile.
        newTile.Setup(new Point(x, y), new Vector3(gridStart.x + (TileSize * x), gridStart.y - (TileSize * y), 0), map);
    }

    private void CreateGrid()
    {
        Tiles = new Dictionary<Point, TileScript>();

        //Calculate the grid's size using World Units

        float worldGridLength = gridRows * TileSize;

        //Convert to Screen Units

        float ScreenGridLength = Camera.main.WorldToScreenPoint(new Vector3(worldGridLength, worldGridLength)).x - Camera.main.WorldToScreenPoint(new Vector3(0,0)).x; 

        //Calculates the grid's start point.

        Vector3 gridStart = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width - ScreenGridLength)/2, Screen.height));

        for (int y = 0; y < gridRows; y++) //The y positions of the tiles
        {
            for (int x = 0; x < gridRows; x++) // The x positions of the tiles
            {
                //Places the tile in the world
                PlaceTile("0",
                          x,
                          y,
                          gridStart);
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
