using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public delegate void ResourceAssetPickedEventHandler(object sender, ObjectEventArgs args);
public class ObjectEventArgs : EventArgs
{
    public Object Asset { get; }
    public ObjectEventArgs(Object asset)
    {
        Asset = asset;
    }
}

public class ResourceAssetPickerEditorWindow : EditorWindow
{
    private enum ScrollDirection
    {
        Up,
        Down
    }

    public event ResourceAssetPickedEventHandler AssetPicked;
    private void OnAssetPicked(Object obj) => AssetPicked?.Invoke(this, new ObjectEventArgs(obj));

    private const float doubleClickTimeWindow = 0.5f;

    private GUIStyle regularLabelGuiStyle;
    private GUIStyle selectedLabelGuiStyle;
    private Vector2 scrollPosition;

    private List<Object> objects;

    private int selectedIndex;

    private string searchString;
    private double lastDoubleClickTime;
    private double clicks;

    private float availableHeight;
    private float searchBarHeight;
    private int renderedObjectsCount;
    private int informationPanelHeight;

    private string lastSearchString;
    private int selectionRange;

    private bool HasValidSelection => selectedIndex > 0 && selectedIndex < objects.Count;
    private GUIStyle previewBackgroundGuiStyle;

    private void OnEnable()
    {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        texture.SetPixel(0, 0, new Color(62 / 255f, 125 / 255f, 231 / 255f));
        texture.Apply();

        regularLabelGuiStyle = new GUIStyle { fontSize = EditorStyles.label.fontSize, normal = new GUIStyleState
        {
            textColor = Color.black
        } };

        selectedLabelGuiStyle = new GUIStyle { fontSize = EditorStyles.label.fontSize, normal = new GUIStyleState
        {
            textColor = Color.white, background = texture
        } };

        minSize = new Vector2(225, 440);
        position = new Rect(Screen.width / 2f, Screen.height / 2f, 225, 440);

        previewBackgroundGuiStyle = new GUIStyle(GUI.skin.FindStyle("ObjectFieldThumb"))
        {
            margin = new RectOffset(5, 5, 5, 5)
        };
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        lastSearchString = searchString;
        searchString = EditorGUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"))?.ToLower();
        
        searchBarHeight = GUILayoutUtility.GetLastRect().height;
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        {
            // Remove focus if cleared
            searchString = "";
            GUI.FocusControl(null);
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        RenderObjects();
        RenderObjectInformation();

        HandleKeyInput();
    }

    private void HandleKeyInput()
    {
        if (Event.current == null || !Event.current.isKey) return;

        switch (Event.current.type)
        {
            case EventType.keyDown:
                GUI.FocusControl($"Button{selectedIndex}");
                switch (Event.current.keyCode)
                {
                    case KeyCode.UpArrow:
                        if (selectedIndex <= 0)
                        {
                            selectedIndex = 0;
                            scrollPosition.y = 0;
                            break;
                        }

                        selectedIndex--;
                        ScrollIfNeeded(ScrollDirection.Up);
                        break;
                    case KeyCode.DownArrow:
                        if (selectedIndex >= selectionRange)
                        {
                            selectedIndex = selectionRange;
                            scrollPosition.y = availableHeight / renderedObjectsCount * selectedIndex;
                            break;
                        }

                        selectedIndex++;
                        ScrollIfNeeded(ScrollDirection.Down);

                        break;
                    case KeyCode.Return:
                        Send(objects[selectedIndex]);
                        break;
                }

                break;
        }

        Repaint();
    }

    private void ScrollIfNeeded(ScrollDirection direction)
    {
        float heightPerElement = availableHeight / renderedObjectsCount;

        switch (direction)
        {
            case ScrollDirection.Up:
                if (selectedIndex <= objects.Count - 1 - renderedObjectsCount)
                {
                    scrollPosition.y -= heightPerElement;
                }
                break;
            case ScrollDirection.Down:
                if (selectedIndex >= renderedObjectsCount)
                {
                    scrollPosition.y += heightPerElement;
                }

                break;
        }
    }

    private void RenderObjectInformation()
    {
        if (!HasValidSelection) return;  

        EditorGUIExtensions.Splitter(1f, EditorStyles.miniButton.padding, EditorStyles.miniButton.margin);
        EditorGUILayout.BeginHorizontal();

        Texture2D previewTexture = AssetPreview.GetAssetPreview(objects[selectedIndex]) ?? 
            AssetPreview.GetMiniTypeThumbnail(objects[selectedIndex].GetType());

        GUILayoutOption[] options =
        {
            GUILayout.Width(64), GUILayout.Height(64)
        };

        Rect rect = GUILayoutUtility.GetRect(new GUIContent(previewTexture), previewBackgroundGuiStyle, options);
        GUI.Box(rect, GUIContent.none, previewBackgroundGuiStyle);

        const float percent = 0.95f;

        // Scaling the rect to the preview image size factoring the border percent.
        rect.width *= percent;
        rect.height *= percent;

        // The amount difference between the full box size and our preview image size.
        // We use this to centre the preview image
        float delta = rect.width / percent - rect.width;
        rect.x += delta / 2;
        rect.y += delta / 2;

        GUI.DrawTexture(rect, previewTexture, ScaleMode.StretchToFill);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField(objects[selectedIndex].name);
        EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(objects[selectedIndex]));

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void RenderObjects()
    {
        renderedObjectsCount = 0;
        availableHeight = 0;

        informationPanelHeight = HasValidSelection ? -100 : 0;
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, 
            GUIStyle.none, GUILayout.Width(position.width), GUILayout.Height(position.height + informationPanelHeight));

        EditorGUILayout.BeginVertical();

        List<Object> displayObjects = string.IsNullOrEmpty(searchString) ? objects : 
            objects.Where(obj => obj == null || obj.name.ToLower().StartsWith(searchString)).ToList();

        if (lastSearchString != searchString)
        {
            Object obj = objects[selectedIndex];
            if (displayObjects.Contains(obj))
            {
                selectedIndex = displayObjects.IndexOf(obj);
            }
        }

        selectionRange = displayObjects.Count - 1;

        foreach (Object obj in displayObjects)
        {
            RenderObject(obj);
        }
     
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private void RenderObject(Object value)
    {
        if (value == null)
        {
            RenderObject("None", null);
        }
        else
        {
            RenderObject(value.name, value);
        }
    }

    private void RenderObject(string text, Object value)
    {
        GUIStyle active = regularLabelGuiStyle;
        int index = objects.IndexOf(value);

        if (selectedIndex == index)
        {
            active = selectedLabelGuiStyle;
        }

        Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(text), active);
        if (availableHeight <= position.height - (Mathf.Abs(informationPanelHeight) + searchBarHeight))
        {
            availableHeight += buttonRect.height;
            renderedObjectsCount++;
        }

        GUI.SetNextControlName($"ObjectButton{index}");
        if (!GUI.Button(buttonRect, text, active)) return;
        if (selectedIndex != index & clicks > 0)
        {
            clicks = 0;
        }

        clicks++;
        selectedIndex = objects.IndexOf(value);
        Repaint();

        double delta = EditorApplication.timeSinceStartup - lastDoubleClickTime;
        if (delta < doubleClickTimeWindow && clicks >= 2)
        {
            clicks = 0;
            Send(value);
        }

        lastDoubleClickTime = EditorApplication.timeSinceStartup;
    }

    private void Send(Object value)
    {
        OnAssetPicked(value);
        Close();
    }

    public void Select(Object selected)
    {
        selectedIndex = objects.IndexOf(selected);
    }

    public void GetFiles(string folderPath, Type objectType)
    {
        objects = new List<Object>();
        Object[] assets = Resources.LoadAll(folderPath, objectType);

        objects.Add(null);
        foreach (Object asset in assets)
        {
            objects.Add(asset);
        }
    }
}

public delegate void ResourceAssetPickedEventHandler<T>(object sender, ResourceAssetPickedEventArgs<T> args);
public class ResourceAssetPickedEventArgs<T> : EventArgs
{
    public T Asset { get; }
    public ResourceAssetPickedEventArgs(T asset)
    {
        Asset = asset;
    }
}

public static class ResourceAssetPickerEditorWindow<T> where T : Object
{
    public static void Popup(Object value, ResourceAssetPickedEventHandler<T> resourceAssetPickedEventHandler, string folderPath = "")
    {
        ResourceAssetPickerEditorWindow window = ScriptableObject.CreateInstance<ResourceAssetPickerEditorWindow>();
        window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 225, 430);
        window.titleContent = new GUIContent($"Select {typeof(T).Name}");
        window.ShowAuxWindow();
        window.GetFiles(folderPath, typeof(T));
        window.Select(value);

        window.AssetPicked += (sender, args) =>
        {
            resourceAssetPickedEventHandler?.Invoke(window, new ResourceAssetPickedEventArgs<T>((T)args.Asset));
        };
    }
}