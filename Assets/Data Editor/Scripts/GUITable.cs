using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class GUITable {

    private int shiftStartIndex = -1;
    private int clickedRowIndex = -1;
    private float repeatStartDelay = 0.750f;
    private float repeatInterval = 0.075f;
    private long lastTime = DateTimeOffset.Now.Ticks;
    private bool downPressed = false;
    private bool downDelayed = false;
    private bool upPressed = false;
    private bool upDelayed = false;
    private int sortedBy = -1;
    private bool isReverseSorted = false;

    public GUIHeader header { get; set; }
    public GUIRow[] rows { get; set; }
    public int focusedRow { get; set; }
    public bool isFocused { get; set; }
    public int ShiftStartIndex
    {
        get
        {
            return shiftStartIndex;
        }
        set
        {
            shiftStartIndex = value;
        }
    }
    public int ClickedRowIndex
    {
        get
        {
            return clickedRowIndex;
        }
        set
        {
            clickedRowIndex = value;
        }
    }
    public bool RowClicked
    {
        get
        {
            return (ClickedRowIndex != -1) ? true : false;
        }
    }
    public Event cEvent
    {
        get
        {
            return Event.current;
        }
    }
    public float DeltaTime
    {
        get
        {
            float deltaTicks = DateTimeOffset.Now.Ticks - lastTime;
            lastTime = DateTimeOffset.Now.Ticks;
            return (deltaTicks / 10000000);
        }
    }

    public void ShowTable()
    {
        //MyGUILayout.Header(header);
        //header.window.Repaint();
        isFocused = false;
        ClickedRowIndex = -1;
        for (int i = 0; i < rows.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            MyGUILayout.Row(rows[i]);
            if (EditorGUI.EndChangeCheck())
            {
                ClickedRowIndex = i;
            }
            if (rows[i].hasFocus)
            {
                focusedRow = i;

                isFocused = true;
            }
        }

        ProcessShift(cEvent.shift);
        SetKeysUp(cEvent.keyCode);
        if (isFocused && !cEvent.alt && cEvent.isKey)
        {
            ProcessKey(cEvent.keyCode);
        }
        else if (RowClicked)
        {
            ProcessClick();
        }
    }

    void ProcessShift(bool shiftHeld)
    {
        if (shiftHeld && ShiftStartIndex == -1)
        {
            ShiftStartIndex = focusedRow;
        }
        else if (!shiftHeld && ((isFocused && !cEvent.alt && ((cEvent.keyCode == KeyCode.DownArrow) || (cEvent.keyCode == KeyCode.UpArrow))) || (RowClicked)))
        {
            ShiftStartIndex = -1;
        }
    }

    void SetKeysUp(KeyCode key)
    {
        if (cEvent.type == EventType.KeyUp && ((key == KeyCode.DownArrow) || (key == KeyCode.UpArrow)))
        {
            downDelayed = false;
            downPressed = false;
            upDelayed = false;
            upPressed = false;
            repeatStartDelay = 0.750f;
            repeatInterval = 0.075f;
        }
    }

    void ProcessKey(KeyCode key)
    {
        if (key == KeyCode.DownArrow && focusedRow + 1 < rows.Length)
        {
            if (GetDownPress())
            {
                ArrowDown();
            }
        }
        else if (key == KeyCode.UpArrow && focusedRow - 1 >= rows.GetLowerBound(0))
        {
            if (GetUpPress())
            {
                ArrowUp();
            }
        }
    }

    bool GetDownPress()
    {
        if (!downPressed && cEvent.type == EventType.KeyDown)
        {
            downPressed = true;
            return true;
        }
        else if (downPressed && !downDelayed)
        {
            repeatStartDelay -= DeltaTime;
            if (repeatStartDelay <= 0)
            {
                downDelayed = true;
                repeatStartDelay = 0.750f;
                return true;
            }
        }
        else if (downDelayed)
        {
            repeatInterval -= DeltaTime;
            if (repeatInterval <= 0)
            {
                repeatInterval = 0.075f;
                return true;
            }
        }
        return false;
    }

    bool GetUpPress()
    {
        if (!upPressed && cEvent.type == EventType.KeyDown)
        {
            upPressed = true;
            return true;
        }
        else if (upPressed && !upDelayed)
        {
            repeatStartDelay -= DeltaTime;
            if (repeatStartDelay <= 0)
            {
                upDelayed = true;
                repeatStartDelay = 0.750f;
                return true;
            }
        }
        else if (upDelayed)
        {
            repeatInterval -= DeltaTime;
            if (repeatInterval <= 0)
            {
                repeatInterval = 0.075f;
                return true;
            }
        }
        return false;
    }

    void ArrowDown()
    {
        focusedRow++;
        Focus();
        if (cEvent.shift)
        {
            ShiftSelectRange(focusedRow);
        }
        else if (!cEvent.control)
        {
            DeselectAllExcept(focusedRow);
        }
    }

    void ArrowUp()
    {
        focusedRow--;
        Focus();
        if (cEvent.shift)
        {
            ShiftSelectRange(focusedRow);
        }
        else if (!cEvent.control)
        {
            DeselectAllExcept(focusedRow);
        }
    }

    void ShiftSelectRange(int rowEndIndex)
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].isOn = (((rowEndIndex >= shiftStartIndex) && (i >= shiftStartIndex) && (i <= rowEndIndex)) 
                || ((rowEndIndex < shiftStartIndex) && (i <= shiftStartIndex) && (i >= rowEndIndex))) ? true : false;
        }
    }


    void ProcessClick()
    {
        if (ClickedRowIndex != -1 && Event.current.shift)
        {
            ShiftSelectRange(ClickedRowIndex);
        }
        else if (ClickedRowIndex != -1 && !Event.current.control && !Event.current.shift)
        {
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i].isOn = (ClickedRowIndex == i) ? rows[i].isOn : false;
            }
            isFocused = true;
        }
    }

    void Focus()
    {
        GUIRow row = rows[focusedRow];
        GUI.FocusControl(row.controlName + "_" + 0);
    }

    void DeselectAll()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].isOn = false;
        }
    }

    void DeselectAllExcept(int index)
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].isOn = (i == index) ? true : false;
        }
    }

    public void SortByColumn(int index)
    {
        if (index != sortedBy || (index == sortedBy && isReverseSorted))
        {
            Array.Sort(rows, delegate (GUIRow x, GUIRow y)
            {
                int result = x.cells[index].text.CompareTo(y.cells[index].text);
                if (result == 0 && index != 0)
                {
                    result = x.cells[0].text.CompareTo(y.cells[0].text);
                }
                if (result == 0 && index != 1)
                {
                    result = x.cells[1].text.CompareTo(y.cells[1].text);
                }
                return result;
            });
            isReverseSorted = false;
            Debug.Log("Sorting by column index " + index);
        }
        else if (index == sortedBy && !isReverseSorted)
        {
            Array.Reverse(rows);
            isReverseSorted = true;
            Debug.Log("Reversing sort of column index " + index);
        }
        sortedBy = index;
    }
}
