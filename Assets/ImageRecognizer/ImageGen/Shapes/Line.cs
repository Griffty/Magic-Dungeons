using System;
using UnityEngine;

[Serializable]
public class Line
{
    public int x1;
    public int y1;
    public int x2;
    public int y2;
    [Range(0,1)]public double curve;

    public Line(int x1, int y1, int x2, int y2, double curve)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.curve = curve;
    }

    public Line(Line line)
    {
        x1 = line.x1;
        y1 = line.y1;
        x2 = line.x2;
        y2 = line.y2;
        curve = line.curve;
    }
}