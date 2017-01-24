using UnityEngine;
using System.Collections;

public class GUILayoutOptions {

    public GUILayoutOptions(params GUILayoutOption[] options)
    {
        this.options = options;
    }

    public GUILayoutOption[] options { get; set; }
}
