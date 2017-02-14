using UnityEngine;
using System.Collections;
using UnityEditor;

public class MyGUILayout {

    public static bool Cell(bool value, GUICell cell)
    {
        Rect rect = GUILayoutUtility.GetRect(new GUIContent(cell.text), cell.toggleStyle, cell.options.options);
        GUI.SetNextControlName(cell.controlName);
        rect.width = rect.width + cell.toggleStyle.margin.right;
        cell.isOn = EditorGUI.Toggle(rect, value, cell.toggleStyle);
        //rect.x = rect.x + cell.toggleStyle.padding.left;
        //rect.width = rect.width - cell.toggleStyle.padding.right;
        string fittedText = MyGUILayoutUtility.FitTextToStyle(cell.text, cell.toggleStyle, rect.width);
        EditorGUI.LabelField(rect, fittedText, cell.TextStyle);
        return cell.isOn;
    }

    public static void Row(GUIRow row)
    {
        EditorGUILayout.BeginHorizontal(row.Style);
        for (int i = 0; i < row.cells.Length; i++)
        {
            row.isOn = Cell(row.isOn, row.cells[i]);
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
        //header.window.Repaint();
        return rect;
    }

    static Rect DrawDivider(Rect rect)
    {
        Rect dividerRect = new Rect(rect);
        dividerRect.x = rect.xMax + 7;
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
            if (width < -7)
            {
                width = -7f;
            }
        }
        return width;
    }
}
