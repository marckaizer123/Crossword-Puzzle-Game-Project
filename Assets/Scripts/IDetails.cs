using System.Collections;
using System.Collections.Generic;
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
        long AttemptsCount { get; set; }
        bool FailedMaxAttempts { get; set; }
        bool Isolated { get; set; }
        int OutputSequence { get; set; }                         // For output only.
    }
}
