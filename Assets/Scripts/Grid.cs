using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Globals;

/// <summary>
/// Function for the grid that the crossword would be placed on.
/// </summary>
public class Grid : Singleton<Grid>
{
    [SerializeField]
    private GameObject crosswordPanel;

    [SerializeField]
    private GameObject[] tilePrefabs;

    /// <summary>
    /// A dictionary that contains all the tiles and their respective grid positions in the game.
    /// </summary>
    public Dictionary<Point, Tile> Tiles { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        Tiles = new Dictionary<Point, Tile>();

        for (int x = 0; x < Crossword.Instance.wordMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < Crossword.Instance.wordMatrix.GetLength(1); y++)
            {
                if (Crossword.Instance.wordMatrix[x, y].Equals('\0'))
                {
                    PlaceTile(1, x, y, Crossword.Instance.wordMatrix[x, y]);
                }
                else
                {
                    PlaceTile(0, x, y, Crossword.Instance.wordMatrix[x, y]);
                }            
            }
        }

        Quiz.Instance.HighlightTiles(0);
        Quiz.Instance.FocusOnWord(0);
        Quiz.Instance.ChangeAnswerText(0);
    }

    private void PlaceTile(int tileIndex, int x, int y, char letter)
    {
        Tile newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<Tile>();

        //Sets the parameters of the tile.
        newTile.Setup(crosswordPanel.transform, new Point(x, y), letter);
    }
}
