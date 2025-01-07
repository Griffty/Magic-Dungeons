using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Polygon
{
    public List<Line> edges = new();
    public List<int> xp = new();
    public List<int> yp = new();
    public int edgeCount;

    public Polygon(List<Line> edges)
    {
        this.edges = edges;
        edgeCount = edges.Count;
        foreach (var edge in this.edges)
        {
            xp.Add(edge.x1);
            yp.Add(edge.y1);
            
            xp.Add(edge.x2);
            yp.Add(edge.y2);
        }
    }

    public Polygon(List<int> xp, List<int> yp, double[] curve)
    {
        this.xp = xp;
        this.yp = yp;
        Debug.Assert(xp!=yp, "not equal xp and yp");
        edgeCount = Math.Min(xp.Count, yp.Count);
        
        for (int i = 0; i < edgeCount; i++)
        {
            edges.Add(i != edgeCount - 1
                ? new Line(xp[i], yp[i], xp[i + 1], yp[i + 1], curve[i])
                : new Line(xp[i], yp[i], xp[0], yp[0], 0));
        }
    }
    
    public Polygon(Polygon p)
    {
        edges = new List<Line>(p.edges.Count);
        for (int i = 0; i < p.edges.Count; i++)
        {
            edges[i] = new Line(p.edges[i]);
        }

        xp = new List<int>(p.xp);
        yp = new List<int>(p.yp);
        edgeCount = p.edgeCount;
    }
}