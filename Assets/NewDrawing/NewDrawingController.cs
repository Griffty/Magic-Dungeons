using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewDrawingController : MonoBehaviour
{
    public Vector2Int inputResolution;
    public Vector2Int outputResolution;
    public Vector2 referencedRes;
    public float sizeScale;
    public int brushSize = 3;
    
    public UnityEngine.UI.Image canvas;
    private Texture2D currentTexture;
    private Color[] _clearBg;
    private Sprite _sprite;
    
    [SerializeField] private CanvasScaler canvasScaler;

    private NewNetworkConfidenceDisplay _confidenceDisplay;
    private void Start()
    {
        _confidenceDisplay = FindObjectOfType<NewNetworkConfidenceDisplay>();
        
        referencedRes = canvasScaler.referenceResolution;
        _clearBg = new Color[inputResolution.x*inputResolution.y];
        for (int i = 0; i < _clearBg.Length; i++)
        {
            _clearBg[i] = new Color(0,0,0, 0.2f);
        }
        if (inputResolution.x != inputResolution.y)
        {
            Debug.Log("Image is not rectangular");
        }
        sizeScale = inputResolution.x / canvas.rectTransform.sizeDelta.x;

        currentTexture = new Texture2D(inputResolution.x, inputResolution.y, TextureFormat.RGBA32, false);
        currentTexture.SetPixels(_clearBg);
        currentTexture.Apply();
        _sprite = Sprite.Create(currentTexture, new Rect(Vector2.zero, new Vector2(inputResolution.x, inputResolution.y)), new Vector2(0.5f, 0.5f));
        canvas.sprite = _sprite;
    }
    private Vector2 _lastMousePos = Vector2.zero;
    private void Update()
    {
        if (!canvas.gameObject.activeInHierarchy)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Clear();
            return;
        }
        
        Vector2 mousePos = Input.mousePosition;
        Vector2 anchoredPosition = canvas.rectTransform.anchoredPosition;
        Vector2 posOnCanvas = mousePos - anchoredPosition;
        if (posOnCanvas.x > canvas.rectTransform.sizeDelta.x - brushSize / 2 ||
            posOnCanvas.y > canvas.rectTransform.sizeDelta.y - brushSize / 2 ||
            posOnCanvas.x < brushSize / 2 ||
            posOnCanvas.y < brushSize / 2) 
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _lastMousePos = posOnCanvas;
        }
        if (Input.GetMouseButton(0))
        {
            DrawPoint(posOnCanvas);
            _lastMousePos = posOnCanvas;
        }
        if (Input.GetMouseButton(1))
        {
            ErasePoint(posOnCanvas);
            _lastMousePos = posOnCanvas;
        }
    }

    private void FixedUpdate()
    {
        if (currentTexture != null)
        {
            _confidenceDisplay.MakePrediction(currentTexture);
        }
    }

    public void Clear()
    {
        currentTexture.SetPixels(_clearBg);
        currentTexture.Apply();
    }
    private void DrawPoint(Vector2 posOnCanvas)
    {
        _lastMousePos *= sizeScale;
        posOnCanvas *= sizeScale;
        BresenhamLineDrawing.DrawLine(currentTexture, (int)_lastMousePos.x, (int)_lastMousePos.y, (int)posOnCanvas.x, (int)posOnCanvas.y, brushSize,
            Color.white);
        currentTexture.Apply();
    }
    private void ErasePoint(Vector2 posOnCanvas)
    {
        _lastMousePos *= sizeScale;
        posOnCanvas *= sizeScale;
        BresenhamLineDrawing.DrawLine(currentTexture, (int)_lastMousePos.x, (int)_lastMousePos.y, (int)posOnCanvas.x, (int)posOnCanvas.y, brushSize,
            Color.clear);
        currentTexture.Apply();
    }
}
public static class BresenhamLineDrawing{
    public  static void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, int bs, Color color)
    {
        int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;
        for(;;) {
            
            for (int x = 0; x < bs; x++)
            {
                for (int y = 0; y < bs; y++)
                {
                    texture.SetPixel(x0 + x - (bs/2), y0 + y - (bs/2), color);
                }
                
            }
            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; x0 += sx; }
            if (e2 < dy) { err += dx; y0 += sy; }
        }
    }
}
