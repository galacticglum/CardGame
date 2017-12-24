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

    /// <summary>
    /// Creates tabs from buttons, with their bottom edge removed by the magic of Haxx
    /// </summary>
    /// <remarks> 
    /// The line will be misplaced if other elements is drawn before this
    /// </remarks> 
    /// <returns>Selected tab</returns>
    public static int Tabs(string[] options, int selected)
    {
        const float activeColour = 0.9f;
        const float StartSpace = 10;

        GUILayout.Space(StartSpace);

        Color oldBackgroundColour = GUI.backgroundColor;
        Color highlightCol = new Color(activeColour, activeColour, activeColour);
        Color transparentColour = new Color(0, 0, 0, 0);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            padding = {bottom = 8},
            margin = {bottom = 0},
            border = {bottom = 0}
        };

        GUILayout.BeginHorizontal();
        {  
            for (int i = 0; i < options.Length; ++i)
            {
                GUI.backgroundColor = i == selected ? highlightCol : transparentColour;
                if (GUILayout.Button(options[i], buttonStyle))
                {
                    selected = i; 
                }
            }
        }

        GUILayout.EndHorizontal();

        GUI.backgroundColor = oldBackgroundColour;
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, highlightCol);
        texture.Apply();

        Splitter(1f, new RectOffset(0, 0, 0, 7), new RectOffset(0, 0, 0, 0));

        return selected;
    }

    public static int Toolbar(int selected, params string[] options)
    {
        EditorGUILayout.BeginHorizontal();

        for (int i = 0; i < options.Length; i++)
        {
            if (GUILayout.Toggle(i == selected, options[i], EditorStyles.toolbarButton) != (i == selected))
            {
                selected = i;
            }
        }

        EditorGUILayout.EndHorizontal();

        return selected;
    }
}

