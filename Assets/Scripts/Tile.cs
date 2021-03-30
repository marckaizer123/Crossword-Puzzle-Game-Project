using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Point GridPosition { get; set; }

    public Text TileLetter;

    public Vector2 WorldPosition
    {
        get
        {
            return GetComponent<Image>().sprite.bounds.center;
        }
    }

    

    public void Setup(Transform parent, Point gridPos)
    {
        this.transform.SetParent(parent, false);
        this.GridPosition = gridPos;
        Grid.Instance.Tiles.Add(gridPos, this);
    }
}
