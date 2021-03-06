using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; set; }

    public Vector2 WorldPosition
    {
        get
        {
            return GetComponent<SpriteRenderer>().bounds.center;
        }
    }

    public bool IsEmpty { get; set; }

    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {

        IsEmpty = true; // sets the tile to be empty.
        this.GridPosition = gridPos;
        this.transform.position = worldPos;
        this.transform.SetParent(parent);
        GridManager.Instance.Tiles.Add(gridPos, this);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
