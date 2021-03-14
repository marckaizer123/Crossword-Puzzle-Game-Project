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


    public void Setup(Transform parent, Point gridPos, char letter)
    {
        this.transform.SetParent(parent, false);
        this.GridPosition = gridPos;
        this.TileLetter.text = letter.ToString();
        GridManager.Instance.Tiles.Add(gridPos, this);
    }

    private void Awake()
    {       
        
    }
}
