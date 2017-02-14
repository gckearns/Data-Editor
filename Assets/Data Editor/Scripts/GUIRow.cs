using UnityEngine;
using System.Collections;

public class GUIRow
{
    private GUIStyle currentStyle;

    public GUIRow(bool value, string[] texts, string id, GUIStyle cellStyle, GUIStyle rowStyle, GUILayoutOptions[] cellOptions, params GUILayoutOption[] rowOptions)
    {
        this.isOn = value;
        this.controlName = id;
        this.rowStyle = rowStyle;
        this.rowOptions = rowOptions;
        cells = new GUICell[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            cells[i] = new GUICell(texts[i], cellStyle, id + "_" + i, cellOptions[i]);
        }
    }

    private GUIStyle rowStyle { get; set; }

    public GUICell[] cells { get; set; }
    public GUILayoutOption[] rowOptions { get; set; }
    public string controlName { get; set; }
    public bool isOn { get; set; }

    public bool hasFocus
    {
        get
        {
            return GUI.GetNameOfFocusedControl().Contains(controlName);
        }
    }

    public GUIStyle Style
    {
        get
        {
            if (currentStyle == null)
            {
                currentStyle = new GUIStyle(rowStyle);
            }
            currentStyle.normal = isOn ? (hasFocus ? rowStyle.onFocused : rowStyle.onNormal) : (hasFocus ? rowStyle.focused : rowStyle.normal);
            return currentStyle;
        }
    }
}