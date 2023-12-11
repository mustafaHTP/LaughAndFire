using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor { get; private set; }

    [SerializeField] private Color[] _colors;
    [SerializeField] private SpriteRenderer _fillSR;

    public void SetDefaultColor(Color color)
    {
        DefaultColor = color;
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        _fillSR.color = color;
    }

    public void SetRandomColor()
    {
        int randomColorIndex = Random.Range(0, _colors.Length);
        DefaultColor = _colors[randomColorIndex];
        _fillSR.color = DefaultColor;
    }
}
