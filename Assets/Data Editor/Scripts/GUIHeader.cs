using UnityEngine;
using System.Collections;

public class GUIHeader
{
    private Rect[] headerRects;
    public bool dragging { get; set; }

    public GUILayoutOptions[] options { get; set; }
    public string[] texts { get; set; }
    public System.Action<int> action { get; set; }
    public GUIStyle style { get; set; }

    public GUIHeader(string[] texts, GUIStyle style, float[] widths, System.Action<int> buttonAction)
    {
        this.texts = texts;
        headerRects = new Rect[widths.Length];
        isResizing = new bool[widths.Length];
        options = new GUILayoutOptions[widths.Length];
        action = buttonAction;
        this.style = style;
        for (int i = 0; i < widths.Length; i++)
        {
            headerRects[i].width = widths[i];
            options[i] = new GUILayoutOptions(GUILayout.Width(widths[i]));
        }
    }

    public bool[] isResizing { get; set; }

    public Rect[] rects
    {
        get
        {
            return headerRects;
        }
        set
        {
            headerRects = value;
        }
    }



    public bool GetColumnResizing(int index)
    {
        for (int i = 0; i < isResizing.Length; i++)
        {
            if (i < index && isResizing[i])
            {
                isResizing[i] = false;
            }
        }
        return (isResizing[index] && dragging);
    }
}
