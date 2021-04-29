using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Globals;

public class Word : IDetails
{
    public string WordSpelling{ get; set; }
    public Sprite WordClue { get; set; }
    public Point StartPosition { get; set; }
    public Direction WordDirection { get; set; }
    public List<Point> CharPositions { get; set; }
    public bool Isolated { get; set; }
    public string Answer { get; set; }
}
