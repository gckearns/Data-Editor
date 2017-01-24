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

        private void OnGUI()
        {
            GUI.skin = Skin;
            tableCellStyle = new GUIStyle("TableCell");
            TableHeader();
            if (table == null)
            {
                table = GetTable();
            }
            table.ShowTable();
        }

        private GUIHeader header;

        void TableHeader()
        {
            GUIStyle headerStyle = new GUIStyle("TableHeader");
            if (header == null)
            {
                header = new GUIHeader(new string[] { "Name", "ID", "Unit Type" }, headerStyle, new float[] { 128f, 64f, 96f }, new System.Action<int> (ColumnClicked));
            }
            MyGUILayout.Header(header);
            Repaint();
        }

        void ColumnClicked(int index)
        {
            Debug.Log("Clicked column " + index);
        }

        GUITable GetTable()
        {
            GUI.skin = Skin;
            tableCellStyle = new GUIStyle("TableCell");
            GUITable table = new GUITable();
            table.rows = new GUIRow[]
            {
                new GUIRow(false, new string[] { "House", "house", "Building" }, "one", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "two", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "three", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "four", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "five", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "six", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "seven", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "eight", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "nine", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "ten", tableCellStyle, header.options),
                new GUIRow(false, new string[] { "House", "house", "Building" }, "eleven", tableCellStyle, header.options)
            };
            return table;
        }
    }
}
