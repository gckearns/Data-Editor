using UnityEngine;
using System.Collections;

public class GUIRow {

    public GUIRow(bool value, string[] texts, string id, GUIStyle style, params GUILayoutOptions[] options)
    {
        this.value = value;
        this.texts = texts;
        this.id = id;
        this.cellStyle = style;
        this.options = options;
    }

    public bool value { get; set; }
    public string[] texts { get; set; }
    public GUIStyle cellStyle { get; set; }
    public GUILayoutOptions[] options { get; set; }
    public string id { get; set; }
    public bool isFocused { get; set; }
}
