using UnityEngine;
using System.Collections;
using System.Text;

public class MyGUILayoutUtility {

    public static string FitTextToStyle(string text, GUIStyle style, float width)
    {
        string returnText = string.Copy(text);
        float textWidth = style.CalcSize(new GUIContent(returnText)).x;
        int trimAmount = 1;
        while (textWidth > width && trimAmount < text.Length)
        {
            returnText = GetClippedText(text, trimAmount);
            trimAmount++;
            textWidth = style.CalcSize(new GUIContent(returnText)).x;
        }
        //Debug.Log(string.Format("Text: {0}, Width: {1}, Size: {2}", text, width, textWidth));
        return returnText;
    }

    static string GetClippedText(string text, int amount)
    {
        amount = Mathf.Clamp(amount, 0, text.Length - 1);
        return text.Substring(0, text.Length - amount) + "...";
    }

}
