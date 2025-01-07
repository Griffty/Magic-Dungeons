using System;
using UnityEngine;

[Serializable]
public class Ellipse
{
    public int x0;
    public int y0;
    public int rx;
    public int ry;

    public Ellipse(int x0, int y0, int rx, int ry)
    {
        this.x0 = x0;
        this.y0 = y0;
        this.rx = rx;
        this.ry = ry;
    }
    
    public Ellipse(int x0, int y0, int r)
    {
        this.x0 = x0;
        this.y0 = y0;
        rx = r;
        ry = r;
    }

    public Ellipse(Vector2Int center, int rx, int ry)
    {
        x0 = center.x;
        y0 = center.y;
        this.rx = rx;
        this.ry = ry;
    }

    public Ellipse(Ellipse ellipse)
    {
        x0 = ellipse.x0;
        y0 = ellipse.y0;
        rx = ellipse.rx;
        ry = ellipse.ry;
    }
}