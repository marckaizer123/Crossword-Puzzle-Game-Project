using UnityEngine;
using Globals;

namespace Interfaces
{
   interface IDetails
    {
        string WordSpelling { get; set; }
        Sprite WordClue { get; set; }
        Point StartPosition { get; set; }
        Direction WordDirection { get; set; }
        bool Isolated { get; set; }
        string Answer { get; set; }
    }
}
