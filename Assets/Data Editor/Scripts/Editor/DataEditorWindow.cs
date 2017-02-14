using UnityEngine;
using System.Collections;
using UnityEditor;

namespace GameDataManager
{
    public class DataEditorWindow : EditorWindow
    {
        private GUISkin skin;
        private string guiSkinPath = "GUI Skins/gskin";
        private GUIStyle tableCellStyle;

        private GUISkin Skin
        {
            get
            {
                if (skin == null)
                {
                    skin = Resources.Load<GUISkin>(guiSkinPath);
                }
                return skin;
            }
        }

        [MenuItem("Window/Data Editor/Open")]
        static void Open()
        {
            DataEditorWindow window = GetWindow<DataEditorWindow>("Data Editor");
        }

        [MenuItem("Window/Data Editor/Close")]
        static void DoClose()
        {
            DataEditorWindow window = GetWindow<DataEditorWindow>("Data Editor");
            window.Close();
        }

        [MenuItem("Window/Data Editor/Test")]
        static void Test()
        {

        }

        private void OnEnable()
        {
            Debug.Log("DataEditorWindow OnEnable");
        }

        private void OnDisable()
        {
            AssetDatabase.SaveAssets();
            Debug.Log("Saved assets");
        }

        GUITable table;
        Vector2 scrollPos;
        Rect scrollContentRect;
        float scrollbar = 0.5f;
        private void OnGUI()
        {
            GUI.skin = Skin;
            tableCellStyle = new GUIStyle("TableCell");
            Rect windowRect = new Rect(new Vector2(position.width / 4, position.height / 4), new Vector2(position.width / 2, position.height / 2));
            GUI.BeginGroup(windowRect, (GUIStyle)"TableRow");
            //TableHeader();
            if (table == null)
            {
                table = GetTable();
            }
            MyGUILayout.Header(table.header);
            Repaint();
            Rect scrollRect = new Rect(windowRect);
            scrollRect.y = 0;
            scrollRect.width = 18f;
            scrollRect.x = windowRect.width - 18f;
            scrollContentRect = new Rect(Vector2.zero, windowRect.size);
            scrollContentRect.height = table.rows.Length * 22;

            Rect scrollViewRect = new Rect(Vector2.zero, windowRect.size);
            scrollViewRect.y += 18;
            scrollViewRect.height -= 18;
            float contentHeight = scrollContentRect.height - 33;
            float pctVisible = scrollViewRect.height / (contentHeight);

            scrollContentRect.y -= scrollbar + 20;
            GUI.BeginGroup(scrollViewRect,(GUIStyle)"TEST");
            GUI.BeginGroup(scrollContentRect);
            table.ShowTable();
            GUI.EndGroup();
            GUI.EndGroup();

            if (contentHeight > scrollViewRect.height)
            {
                scrollbar = GUI.VerticalScrollbar(scrollRect, scrollbar, pctVisible * contentHeight, 0f, contentHeight);
                //Debug.Log(scrollbar);
            }
            else
            {
                scrollbar = 0f;
            }

            
            //GUI.EndScrollView();
            GUI.EndGroup();
        }

        private GUIHeader header;

        void TableHeader()
        {
            GUIStyle headerStyle = new GUIStyle("TableHeader");
            if (header == null)
            {
                header = new GUIHeader(new string[] { "Name", "ID", "Unit Type" }, headerStyle, new float[] { 128f, 64f, 96f }, new System.Action<int> (ColumnClicked), this);
            }
            MyGUILayout.Header(header);
            Repaint();
        }

        void ColumnClicked(int index)
        {
            Debug.Log("Clicked column " + index);
            table.SortByColumn(index);
        }

        GUITable GetTable()
        {
            GUI.skin = Skin;
            GUITable table = new GUITable();
            table.header = new GUIHeader(new string[] { "Name", "ID", "Unit Type" }, "TableHeader", new float[] { 128f, 64f, 96f }, new System.Action<int>(ColumnClicked), this);
            table.rows = new GUIRow[]
            {
                new GUIRow(false, new string[] { "HHouse", "house8", "Industry" }, "eight", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "IHouse", "house9", "Industry" }, "nine", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "JHouse", "house10", "Industry" }, "ten", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "AHouse", "house1", "Housing" }, "one", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "BHouse", "house2", "Housing" }, "two", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "EHouse", "house5", "Services" }, "five", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "FHouse", "house6", "Services" }, "six", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "CHouse", "house3", "Housing" }, "three", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "DHouse", "house4", "Services" }, "four", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "GHouse", "house7", "Services" }, "seven", "TableCell", "TableRow", table.header.options),
                new GUIRow(false, new string[] { "KHouse", "house11", "Industry" }, "eleven", "TableCell", "TableRow", table.header.options)
            };
            return table;
        }
    }
}
