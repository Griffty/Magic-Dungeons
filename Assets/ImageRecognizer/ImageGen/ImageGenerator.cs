using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class ImageGenerator : MonoBehaviour
{
     [SerializeField] private bool useFiguresFromUI;
     private List<Figure> _all = new();
     private byte[] _images;
     private byte[] _labels;
     [Range(1, 5000)] [SerializeField] private int amount;
     private ImageCreate _imageCreate;
     private void Start()
     {
          _imageCreate = FindObjectOfType<ImageCreate>();
          if (useFiguresFromUI)
          {
               _all = _imageCreate.figures;
          }
          else
          {
               _all = Fill();  
          }
          _images = new byte[_imageCreate.size * _imageCreate.size * amount * _all.Count];
          _labels = new byte[amount * _all.Count];
     }

     private List<Figure> Fill()
     {
          List<Figure> figures = new List<Figure>();

          Figure f1 = new Figure(0);
          f1.lines.Add(new Line(20, 100, 96, 20, 0));
          f1.lines.Add(new Line(32, 20, 108, 100, 0));
          
          figures.Add(f1);
          return figures;
     }

     public void MakeImageSet()
     {
          Start();
          double startTime = Time.realtimeSinceStartup;
          byte[] image;
          for (int i = 0; i < amount; i++)
          {
               for (int a = 0; a < _all.Count; a++)
               {
                    image = _imageCreate.MakeImage(_all[a]);
                    SaveImageToOther(image, _all[a].label, i * _all.Count + a);
               }
          }
          double endTime = Time.realtimeSinceStartup;
          double dif = endTime - startTime;
          Debug.Log("TOTAL TIME: " + dif);
          Save();
     }

     private void Save()
     {
          File.WriteAllBytes(ImageCreate.Folder + "\\mageImages.bytes", _images);
          File.WriteAllBytes(ImageCreate.Folder + "\\mageLabels.bytes", _labels);
     }

     private void SaveImageToOther(byte[] image, int label, int number)
     {
          for (int i = 0; i < _imageCreate.size * _imageCreate.size; i++)
          {
               _images[(_imageCreate.size * _imageCreate.size) * number + i] = image[i];
          }

          _labels[number] = (byte)label;
     }
}       