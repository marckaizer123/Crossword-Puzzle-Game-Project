using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using Globals;

public class Word : IDetails
{


    public string WordSpelling{ get; set; }
    public Sprite WordClue { get; set; }
    public Point WordPosition { get; set; }
    public Direction WordDirection { get; set; }
    public long AttemptsCount { get; set; }
    public bool FailedMaxAttempts { get; set; }
    public bool Isolated { get; set; }
    public int OutputSequence { get; set; }
}
