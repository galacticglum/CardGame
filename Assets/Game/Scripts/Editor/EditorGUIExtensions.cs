using UnityEditor;
using UnityEngine;

// http://answers.unity3d.com/questions/216584/horizontal-line.html
public static class EditorGUIExtensions
{
    private const string FocusOutUid = "__FOCUS_OUT_CONTROL__";

    private static readonly GUIStyle splitter;
    private static readonly Color splitterColor = EditorGUIUtility.isProSkin ? new Color(0.157f, 0.157f, 0.157f) : new Color(0.5f, 0.5f, 0.5f);

    static EditorGUIExtensions()
    {
        splitter = new GUIStyle
        {
            normal = { background = EditorGUIUtility.whiteTexture },
            stretchWidth = true,
            margin = new RectOffset(0, 0, 7, 7)
        };
    }

    public static void Splitter(Color rgb, float thickness = 1)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitter, GUILayout.Height(thickness));

        if (Event.current.type != EventType.Repaint) return;
        Color restoreColor = GUI.color;
        GUI.color = rgb;
        splitter.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    public static void Splitter(float thickness, GUIStyle splitterStyle)
    {
        Rect position = GUILayoutUtility.GetRect(GUIContent.none, splitterStyle, GUILayout.Height(thickness));

        if (Event.current.type != EventType.Repaint) return;
        Color restoreColor = GUI.color;
        GUI.color = splitterColor;
        splitterStyle.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    public static void Splitter(float thickness, RectOffset margin, RectOffset padding)
    {
        GUIStyle style = new GUIStyle(splitter);
        splitter.margin = margin;
        splitter.padding = padding;

        Rect position = GUILayoutUtility.GetRect(GUIContent.none, style, GUILayout.Height(thickness));

        if (Event.current.type != EventType.Repaint) return;
        Color restoreColor = GUI.color;
        GUI.color = splitterColor;
        style.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    public static void Splitter(float thickness = 1)
    {
        Splitter(thickness, splitter);
    }

    public static void Splitter(Rect position)
    {
        if (Event.current.type != EventType.Repaint) return;
        Color restoreColor = GUI.color;
        GUI.color = splitterColor;
        splitter.Draw(position, false, false, false, false);
        GUI.color = restoreColor;
    }

    public static void RemoveFocus()
    {
        GUI.SetNextControlName(FocusOutUid);
        GUI.TextField(new Rect(-100, -100, 1, 1), "");
        GUI.FocusControl(FocusOutUid);
    }
}

