using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; set; }

    public Vector2 WorldPosition
    {
        get
        {
            //return GetComponent<SpriteRenderer>().bounds.center;
            return GetComponent<Image>().sprite.bounds.center;
        }
    }

    public Text TileLetter;

    

    public bool IsEmpty { get; set; }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {
        this.transform.SetParent(parent, false);

        IsEmpty = true; // sets the tile to be empty.
        
        this.GridPosition = gridPos;
        this.transform.position = worldPos;

       


        GridManager.Instance.Tiles.Add(gridPos, this);
    }

    public void Setup2(Transform parent, Point gridPos, char letter)
    {
        this.transform.SetParent(parent, false);
        this.GridPosition = gridPos;
        this.TileLetter.text = letter.ToString();
    }

    private void Awake()
    {       
        
    }
}
