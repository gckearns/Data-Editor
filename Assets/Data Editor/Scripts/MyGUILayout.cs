using UnityEngine;
using System.Collections;
using UnityEditor;

public class MyGUILayout {

    public static bool ToggleRow(GUIRow row)
    {
        GUIStyle rowStyle = new GUIStyle("TableRow");
        
        rowStyle.normal = row.value ? rowStyle.onNormal : rowStyle.normal;
        if (GUI.GetNameOfFocusedControl().Contains("row" + row.id))
        {
            rowStyle.normal = row.value ? rowStyle.onFocused: rowStyle.focused;
        }
        EditorGUILayout.BeginHorizontal(rowStyle);
        for (int i = 0; i < row.texts.Length; i++)
        {
            GUI.SetNextControlName(("row" + row.id + i));
            EditorGUI.BeginChangeCheck();
            row.value = GUILayout.Toggle(row.value, row.texts[i], row.cellStyle, row.options[i].options);
            if (EditorGUI.EndChangeCheck() && row.value)
            {
                GUI.FocusControl(("row" + row.id + i));
                //row.isFocused = true;
            }
        }
        EditorGUILayout.EndHorizontal();
        return row.value;
    }

    public static void ShowRow(GUIRow row)
    {
        GUIStyle rowStyle = new GUIStyle("TableRow");

        rowStyle.normal = row.value ? rowStyle.onNormal : rowStyle.normal;
        if (GUI.GetNameOfFocusedControl().Contains("row" + row.id))
        {
            rowStyle.normal = row.value ? rowStyle.onFocused : rowStyle.focused;
        }
        EditorGUILayout.BeginHorizontal(rowStyle);
        for (int i = 0; i < row.texts.Length; i++)
        {
            GUI.SetNextControlName(("row" + row.id + i));
            EditorGUI.BeginChangeCheck();
            row.value = GUILayout.Toggle(row.value, row.texts[i], row.cellStyle, row.options[i].options);
            if (EditorGUI.EndChangeCheck())
            {
                GUI.FocusControl(("row" + row.id + i));
                //row.isFocused = true;
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public static Rect Header(GUIHeader header)
    {
        if (!header.dragging && Event.current.type == EventType.MouseDrag)
        {
            header.dragging = true;
        }
        if (Event.current.type == EventType.MouseUp)
        {
            header.dragging = false;
            for (int i = 0; i < header.isResizing.Length; i++)
            {
                header.isResizing[i] = false;
            }
        }
        Rect rect = EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < header.texts.Length; i++)
        {
            if (EllipsisButton(header.texts[i], new GUIStyle("TableHeader"), header.rects[i].width, header.options[i].options))
            {
                header.action(i);
            }
            if (Event.current.type == EventType.Repaint)
            {
                header.rects[i] = GUILayoutUtility.GetLastRect();
            }
            Rect mouseRect = DrawDivider(header.rects[i]);
            Rect resizeRect = DrawResizeCursor(mouseRect);
            if (!header.isResizing[i] && Event.current.type == EventType.MouseDown && resizeRect.Contains(Event.current.mousePosition))
            {
                header.isResizing[i] = true;
            }
            if (header.isResizing[i])
            {
                header.rects[i].width = MoveDivider(header.GetColumnResizing(i), header.rects[i], header.rects[i].width);
                header.options[i].options = new GUILayoutOption[] { GUILayout.Width(header.rects[i].width) };
            }
        }
        EditorGUILayout.EndHorizontal();
        return rect;
    }

    static Rect DrawDivider(Rect rect)
    {
        Rect dividerRect = new Rect(rect);
        dividerRect.x = rect.xMax + 8;
        dividerRect.width = 1f;
        EditorGUI.DrawRect(dividerRect, Color.black);
        return dividerRect;
    }

    static Rect DrawResizeCursor(Rect rect)
    {
        Rect resizeRect = new Rect(rect);
        resizeRect.xMin = resizeRect.xMin - 8;
        resizeRect.xMax = resizeRect.xMax + 8;
        EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.SplitResizeLeftRight);
        return resizeRect;

    }

    static bool EllipsisButton(string text, GUIStyle style, float width, params GUILayoutOption[] options)
    {
        string fittedText = MyGUILayoutUtility.FitTextToStyle(text, style, width);
        return GUILayout.Button(fittedText, style, options);
    }

    static float MoveDivider(bool isDragging, Rect buttonRect, float width)
    {
        //if (Event.current.isMouse && isDragging)
        if (Event.current.type == EventType.MouseDrag)
        { 
            width = Event.current.mousePosition.x - buttonRect.x - 8;
            if (width < -15)
            {
                width = -15f;
            }
        }
        return width;
    }
}
