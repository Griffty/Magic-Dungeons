using System;
using System.Collections.Generic;

[Serializable]
public class Figure
{
    public int label;
    public List<Line> lines = new();
    public List<Ellipse> ellipses = new();
    public List<Polygon> polygons = new();

    public Figure(int label, List<Line> lines, List<Ellipse> ellipses, List<Polygon> polygons)
    {
        this.label = label;
        if (lines != null) this.lines = lines;
        if (ellipses != null) this.ellipses = ellipses;
        if (polygons != null) this.polygons = polygons;
    }
    public Figure(int label)
    {
        this.label = label;
    }

    public Figure(Figure figure)
    {
        this.label = figure.label;
        this.lines = new List<Line>(figure.lines);
        this.ellipses = new List<Ellipse>(figure.ellipses);
        this.polygons = new List<Polygon>(figure.polygons);
        for (int i = 0; i < figure.lines.Count; i++)
        {
            figure.lines[i] = new Line(figure.lines[i]);
        }
        for (int i = 0; i < figure.polygons.Count; i++)
        {
            figure.polygons[i] = new Polygon(figure.polygons[i]);
        }
        
        for (int i = 0; i < figure.ellipses.Count; i++)
        {
            figure.ellipses[i] = new Ellipse(figure.ellipses[i]);
        }
    }
}
