using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUICell {

    private GUIStyle textStyle;

    public GUICell(string text, GUIStyle style, string id, GUILayoutOptions options)
    {
        this.text = text;
        this.toggleStyle = style;
        this.textStyle = new GUIStyle(style);
        this.controlName = id;
        this.options = options;
        this.rect = new Rect();
        this.isOn = false;
        this.hasFocus = false;
    }

    public Rect rect { get; set; }
    public string text { get; set; }
    public GUIStyle toggleStyle { get; set; }
    public string controlName { get; set; }
    public bool hasFocus { get; set; }
    public bool isOn { get; set; }
    public GUILayoutOptions options { get; set; }

    public GUIStyle TextStyle
    {
        get
        {
            if (textStyle == null)
            {
                textStyle = new GUIStyle(toggleStyle);
            }
            textStyle.normal = isOn ? (hasFocus ? toggleStyle.onFocused : toggleStyle.onNormal) : (hasFocus ? toggleStyle.focused : toggleStyle.normal);
            return textStyle;
        }
    }
}
