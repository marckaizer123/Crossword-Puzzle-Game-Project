using System.Runtime.CompilerServices;
using UnityEngine;

public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }


    /// <summary>
    /// Check if first point is equal to second point
    /// </summary>
    /// <param name="first">First Point</param>
    /// <param name="second">Second Point</param>
    /// <returns></returns>
    public static bool operator ==(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }

    public static bool operator !=(Point first, Point second)
    {
        return first.X != second.X || first.Y != second.Y;
    }
}