using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class ImageCreate : MonoBehaviour
{
    [SerializeField] public int size;
    [Range(1, 5)] [SerializeField] private int brashSize;
    [Range(0, 1)] [SerializeField] private double smoothing;
    [Range(0, 1)] [SerializeField] private int brashShape;
    [Range(0, 1)] [SerializeField] private double distortionPercent;
    [Range(0, 1)] [SerializeField] private double cDistortionPercent;
    [SerializeField] private bool addFiguresFromCode;
    [SerializeField] private List<int> drawTargets;
    [SerializeField] protected internal List<Figure> figures;
    public const string Folder = "C:\\Users\\Griffty\\Desktop\\Neural-Network-Experiments-main\\Assets\\Data\\Mages_Legend\\Data Original";
    
    public byte[] MakeImage(Figure figure)
    {
        double startTime = Time.realtimeSinceStartup;
        byte[] image = new byte[size*size];
        
        Figure f = ApplyDistortion(figure, distortionPercent, cDistortionPercent);
        
        Painter.Draw(image, f.lines, brashSize, smoothing, brashShape, size);
        Painter.Draw(image, f.polygons, brashSize, smoothing, brashShape, size);
        Painter.Draw(image, f.ellipses, brashSize, smoothing, brashShape, size);
        
        double endTime = Time.realtimeSinceStartup;
        double dif = endTime - startTime;
        Debug.Log(dif+" ");
        
        return image;
    }
    
    public void MakeImage(bool save)
    {
        double startTime = Time.realtimeSinceStartup;
        byte[] image = new byte[size*size];
        if (addFiguresFromCode)
        {
            MakeFigures(image);
        }
        
        List<Figure> newFigures = new List<Figure>();
        foreach (var figure in figures)
        {
            newFigures.Add(ApplyDistortion(figure, distortionPercent, cDistortionPercent));
        }
        
        Draw(image, newFigures);

        if (save)
        {
            Save(image);
        }
        // string docPath =
        //     Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //
        // string s = "";
        // using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))
        // {
        //     for (int x = 0; x < size; x++)
        //     {
        //         for (int y = 0; y < size; y++)
        //         {
        //             if (image[x + y * size] == 0)
        //             {
        //                 s += "000   ";
        //             }
        //             else
        //             {
        //                 s += image[x + y * size] + "   "; 
        //             }
        //             
        //         }
        //         outputFile.WriteLine(s);
        //         s = "";
        //     }
        // }
        //
        // Debug.Log(docPath);
        double endTime = Time.realtimeSinceStartup;
        double dif = endTime - startTime;
        Debug.Log(dif+" ");
    }
    


    private Figure ApplyDistortion(Figure f, double dPercentage, double cPercentage)
    {
        Figure figure = new Figure(f);
        for (int i = 0; i < figure.lines.Count; i++)
        {
            int r = 0;
            while (r == 0)
            {
                r = Random.Range(-1, 2);
            }
            Debug.Log(r);
            double ampx = Random.Range((float)(1 - dPercentage), (float)(dPercentage + 1));
            double ampy = Random.Range((float)(1 - dPercentage), (float)(dPercentage + 1));
            double ampc = Random.Range((float)(1 - cPercentage / 10), (float)(1 + cPercentage / 10)) * r;
            Debug.Log(ampc);
            Line line1 = figure.lines[i];
            
            if (figure.lines.Count == 1)
            {
                line1.x1 = (int)(line1.x1 * ampx);
                line1.y1 = (int)(line1.y1 * ampy);
                line1.curve = (line1.curve * ampc / 10);

                ampx = Random.Range((float)(1 - dPercentage), (float)(dPercentage + 1));
                ampy = Random.Range((float)(1 - dPercentage), (float)(dPercentage + 1));
                line1.x2 = (int)(line1.x2 * ampx);
                line1.y2 = (int)(line1.y2 * ampy);
                break;
            }
            
            if (i == figure.lines.Count - 1)
            {
                line1.x2 = (int)(line1.x2 * ampx);
                line1.y2 = (int)(line1.y2 * ampy);
                line1.curve = (line1.curve * ampc / 10);
                continue;
            }
            
            if (i == 0)
            {
                line1.x1 = (int)(line1.x1 * ampx);
                line1.y1 = (int)(line1.y1 * ampy);
            }
            Line line2 = figure.lines[i+1];
            
            line1.curve = (line1.curve * ampc / 10);
            
            ampx = Random.Range((float)(1 - dPercentage), (float)(dPercentage + 1));
            ampy = Random.Range((float)(1 - dPercentage), (float)(dPercentage + 1));
            line1.x2 = (int)(line1.x2 * ampx);
            line1.y2 = (int)(line1.y2 * ampy);
            line2.x1 = (int)(line2.x1 * ampx);
            line2.y1 = (int)(line2.y1 * ampy);
        }

        return figure;
    }

    private void MakeFigures(byte[] image)
    {
        Figure f1 = new Figure(3);
        f1.lines.Add(new Line(10, 50, 50, 50, 0));
        f1.lines.Add(new Line(10, 10, 40, 40, 0));
        

        // Place to make a figures
        
        //
        
        Painter.Draw(image, f1.lines, brashSize, smoothing, brashShape, size);
        Painter.Draw(image, f1.polygons, brashSize, smoothing, brashShape, size);
        Painter.Draw(image, f1.ellipses, brashSize, smoothing, brashShape, size);
    }

    private void Draw(byte[] image, List<Figure> figures)
    {
        for (int i = 0; i < figures.Count; i++)
        {
            if (drawTargets.Contains(i))
            {
                Figure f = figures[i];
                Painter.Draw(image, f.lines, brashSize, smoothing, brashShape, size);
                Painter.Draw(image, f.polygons, brashSize, smoothing, brashShape, size);
                Painter.Draw(image, f.ellipses, brashSize, smoothing, brashShape, size);
            }
        }
    }

    private void Save(byte[] image)
    {
        File.WriteAllBytes(Folder + "\\mageImage.bytes", image);
        byte[] label = new byte[1];
        for (int i = 0; i < figures.Count; i++)
        {
            if (drawTargets.Contains(i))
            {
                label[0] = (byte)figures[i].label;
            }
        }
        File.WriteAllBytes(Folder + "\\mageLabel.bytes", label);
    }

    public void SaveSettings()
    {
        DirectoryInfo d = new DirectoryInfo(Folder);
        FileInfo[] files = d.GetFiles("*.txt");
        int i = 0;
        foreach (var file in files)
        {
            if (file.Name.Contains("figure"))
            {
                i++;
            }
        }

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Folder, $"figure{i}.txt")))
        {
            for (int j = 0; j < figures.Count; j++)
            {
                outputFile.WriteLine("figure " + j);
                outputFile.WriteLine("label " + figures[j].label);
                for (int k = 0; k < figures[j].lines.Count; k++)
                {
                    outputFile.WriteLine("+line " + k);
                    outputFile.WriteLine("|| x1: " + figures[j].lines[k].x1);
                    outputFile.WriteLine("|| y1: " + figures[j].lines[k].y1);
                    outputFile.WriteLine("|| x2: " + figures[j].lines[k].x2);
                    outputFile.WriteLine("|| y2: " + figures[j].lines[k].y2);
                    outputFile.WriteLine("---");
                }

                
                for (int k = 0; k < figures[j].ellipses.Count; k++)
                {
                    outputFile.WriteLine("|ellipse " + k);
                    outputFile.WriteLine("|| x0: " + figures[j].ellipses[k].x0);
                    outputFile.WriteLine("|| y0: " + figures[j].ellipses[k].y0);
                    outputFile.WriteLine("|| rx: " + figures[j].ellipses[k].rx);
                    outputFile.WriteLine("|| ry: " + figures[j].ellipses[k].ry);
                    outputFile.WriteLine("---");
                }

                
                for (int k = 0; k < figures[j].polygons.Count; k++)
                {
                    outputFile.WriteLine("|polygon " + k);
                    for (int l = 0; l < figures[j].polygons[k].edges.Count; l++)
                    {
                        outputFile.WriteLine("||edge " + l);
                        outputFile.WriteLine("=== x1: " + figures[j].polygons[k].edges[l].x1);
                        outputFile.WriteLine("=== y1: " + figures[j].polygons[k].edges[l].y1);
                        outputFile.WriteLine("=== x2: " + figures[j].polygons[k].edges[l].x2);
                        outputFile.WriteLine("=== y2: " + figures[j].polygons[k].edges[l].y2);
                    }
                    outputFile.WriteLine("---");
                }
                
                outputFile.WriteLine(" ");
                outputFile.WriteLine(" ");
                outputFile.WriteLine(" ");
            }
        }
    }
}



public static class Painter
    {
        private static void Plot(byte[] canvas, int x, int y, double c, int brashSize, double smoothing, int brashShape, int canvasSize)
        {
            if (x < 0 || x > canvasSize || y < 0 || y > canvasSize || c < 0)
            {
                return;
            }
            
            c = Math.Pow(c, 0.1);
            c *= 255;
            
                if (brashSize == 1)
            {
                canvas[y * canvasSize + x] = (byte)c;
                return;
            }
            for (int by = -brashSize+1; by < brashSize; by++)
            {
                for (int bx = -brashSize+1; bx < brashSize; bx++)
                {
                    if (bx + x > 0 || bx + x < canvasSize || by + y > 0 || by + y < canvasSize)
                    {
                        byte color = (byte)(c - smoothing * Dist(x, y, bx + x, by + y) / Dist(x, y, brashSize - 1 + x, brashSize - 1 + y) * c);

                        if (canvas[(by + y) * canvasSize + (bx + x)] > color)
                        {
                            continue;
                        }
                        if (brashShape == 1)
                        {
                            if (Dist(x, y, x+ bx, y + by) < brashSize)
                            {
                                canvas[(by + y) * canvasSize + (bx + x)] = color;
                            }   
                            continue;
                        }
                        canvas[(by + y) * canvasSize + (bx + x)] = color;
                        
                    }
                }   
            }
        }

        static float Dist(int x0, int y0, int x1, int y1)
        {
            return Mathf.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1));
        }

        static int Ipart(double x) { return (int)x;}

        static int Round(double x) {return Ipart(x+0.5);}
    
        static double Fpart(double x) {
            if(x<0) return (1-(x-Math.Floor(x)));
            return (x-Math.Floor(x));
        }
    
        static double Rfpart(double x) {
            return 1-Fpart(x);
        }

        public static void Draw(byte[] canvas, List<Polygon> polygons, int brashSize, double smoothing, int brashShape, int canvasSize)
        {
            foreach (var polygon in polygons)
            {
                Draw(canvas, polygon.edges, brashSize, smoothing, brashShape, canvasSize);
            }
        }
        
        public static void Draw(byte[] canvas, List<Line> lines, int brashSize, double smoothing, int brashShape, int canvasSize)
        {
            foreach (var line in lines)
            {
                Draw(canvas, line, brashSize, smoothing, brashShape, canvasSize);
            }
        }

        public static void Draw(byte[] canvas, Line line, int brashSize, double smoothing, int brashShape, int canvasSize)
        {
            double x0 = line.x1, y0 = line.y1, x1 = line.x2, y1 = line.y2;
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            double temp;
            if (steep)
            {
                temp = x0;
                x0 = y0;
                y0 = temp;
                temp = x1;
                x1 = y1;
                y1 = temp;
            }

            if (x0 > x1)
            {
                temp = x0;
                x0 = x1;
                x1 = temp;
                temp = y0;
                y0 = y1;
                y1 = temp;
            }

            double dx = x1 - x0;
            double dy = y1 - y0;
            double gradient = dy / dx;

            double xEnd = Round(x0);
            double yEnd = y0 + gradient * (xEnd - x0);
            double xGap = Rfpart(x0 + 0.5);
            double xPixel1 = xEnd;
            double yPixel1 = Ipart(yEnd);

            if (steep)
            {
                Plot(canvas, (int)yPixel1, (int)xPixel1, Rfpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)yPixel1 + 1, (int)xPixel1, Fpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
            }
            else
            {
                Plot(canvas, (int)xPixel1, (int)yPixel1, Rfpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)xPixel1, (int)yPixel1 + 1, Fpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
            }

            double intery = yEnd + gradient;

            xEnd = Round(x1);
            yEnd = y1 + gradient * (xEnd - x1);
            xGap = Fpart(x1 + 0.5);
            double xPixel2 = xEnd;
            double yPixel2 = Ipart(yEnd);
            
            if (steep)
            {
                Plot(canvas, (int)yPixel2, (int)xPixel2, Rfpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)yPixel2 + 1, (int)xPixel2, Fpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
            }
            else
            {
                Plot(canvas, (int)xPixel2, (int)yPixel2, Rfpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)xPixel2, (int)yPixel2 + 1, Fpart(yEnd) * xGap, brashSize, smoothing, brashShape, canvasSize);
            }
            
            double c = line.curve;
            int midPointD = (int)((xPixel2 + xPixel1)/2);
            int yShift = (int)((xPixel2 - midPointD) * c * 2.55);
            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    double ipart = Ipart(intery);
                    double y = ipart + (x - midPointD) * (x - midPointD) * -1 * c / (canvasSize / (double)10) + yShift; // (x in global) + (x in local from center)^2 * -1 (to make it face opposite way) * c (height modifier) / 10 (to make it less sensitive);
                    Plot(canvas, (int)y, x, Rfpart(intery), brashSize, smoothing, brashShape, canvasSize);
                    Plot(canvas, (int)y + 1, x, Fpart(intery), brashSize, smoothing, brashShape, canvasSize);
                    intery += gradient;
                }
            }
            else
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    double ipart = Ipart(intery);
                    double y = ipart + (x - midPointD) * (x - midPointD) * -1 * c / 10 + yShift;
                    Plot(canvas, x, (int)y, Rfpart(intery), brashSize, smoothing, brashShape, canvasSize);
                    Plot(canvas, x, (int)y + 1, Fpart(intery), brashSize, smoothing, brashShape, canvasSize);
                    intery += gradient;
                }
            }
        }
        public static void Draw(byte[] canvas, List<Ellipse> circles, int brashSize, double smoothing, int brashShape, int canvasSize)
        {
            foreach (var circle in circles)
            {
                DrawEllipse(canvas, circle.rx, circle.ry, circle.x0, circle.y0, brashSize, smoothing, brashShape, canvasSize);
            }
        }
        static void DrawEllipse(byte[] canvas, double rx, double ry, double xc, double yc, int brashSize, double smoothing, int brashShape, int canvasSize)
        {
            double dx, dy, d1, d2, x, y;
            int c = 1;
            x = 0;
            y = ry;
            
            d1 = (ry * ry) - (rx * rx * ry) + (0.25f * rx * rx);
            dx = 2 * ry * ry * x;
            dy = 2 * rx * rx * y;
            
            while (dx < dy)
            {

                
                Plot(canvas, (int)(x + xc), (int)(y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)(-x + xc), (int)(y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)(x + xc), (int)(-y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)(-x + xc), (int)(-y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                
                if (d1 < 0)
                {
                    x++;
                    dx = dx + (2 * ry * ry);
                    d1 = d1 + dx + (ry * ry);
                }
                else
                {
                    x++;
                    y--;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d1 = d1 + dx - dy + (ry * ry);
                }
            }
            
            d2 = ((ry * ry) * ((x + 0.5f) * (x + 0.5f)))
                + ((rx * rx) * ((y - 1) * (y - 1)))
                - (rx * rx * ry * ry);
            
            while (y >= 0)
            {
                
                Plot(canvas, (int)(x + xc), (int)(y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)(-x + xc), (int)(y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)(x + xc), (int)(-y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                Plot(canvas, (int)(-x + xc), (int)(-y+yc), c, brashSize, smoothing, brashShape, canvasSize);
                
                if (d2 > 0)
                {
                    y--;
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + (rx * rx) - dy;
                }
                else
                {
                    y--;
                    x++;
                    dx = dx + (2 * ry * ry);
                    dy = dy - (2 * rx * rx);
                    d2 = d2 + dx - dy + (rx * rx);
                }
            }
        }
    }
