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
    private const int NullObjectId = -100;
    private const int NothingSelectedId = -1;

    public event ResourceAssetPickedEventHandler AssetPicked;
    private void OnAssetPicked(Object obj) => AssetPicked?.Invoke(this, new ObjectEventArgs(obj));

    private const float doubleClickTimeWindow = 0.5f;

    private GUIStyle regularLabelGuiStyle;
    private GUIStyle selectedLabelGuiStyle;
    private Vector2 scrollPosition;

    private Dictionary<int, Object> objects;

    private int selectedIndex;
    private int selectedObjectId;

    private string searchString;
    private double lastDoubleClickTime;
    private double clicks;


    private bool HasValidSelection => selectedObjectId != NullObjectId && selectedObjectId != NothingSelectedId;
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
        searchString = EditorGUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
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
    }

    private void Update()
    {
        
    }

    private void RenderObjectInformation()
    {
        if (!HasValidSelection) return;  

        EditorGUIExtensions.Splitter(1f, EditorStyles.miniButton.padding, EditorStyles.miniButton.margin);
        EditorGUILayout.BeginHorizontal();

        Texture2D previewTexture = AssetPreview.GetAssetPreview(objects[selectedObjectId]) ?? 
            AssetPreview.GetMiniTypeThumbnail(objects[selectedObjectId].GetType());

        Rect rect = GUILayoutUtility.GetRect(new GUIContent(previewTexture), previewBackgroundGuiStyle,
            GUILayout.Width(64), GUILayout.Height(64));

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

        EditorGUILayout.LabelField(objects[selectedObjectId].name);
        EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(objects[selectedObjectId]));

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void RenderObjects()
    {
        int heightOffset = HasValidSelection ? -100 : 0;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, 
            GUIStyle.none, GUILayout.Width(position.width), GUILayout.Height(position.height + heightOffset));

        EditorGUILayout.BeginVertical();

        RenderObject("None", null);
        if (string.IsNullOrEmpty(searchString))
        {
            foreach (KeyValuePair<int, Object> pair in objects)
            {
                RenderObject(pair.Value);
            }
        }
        else
        {
            foreach (KeyValuePair<int, Object> obj in
                objects.Where(pair => pair.Value.name.StartsWith(searchString)))
            {
                RenderObject(obj.Value);
            }
        }
     
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private void RenderObject(Object value)
    {
        RenderObject(value.name, value);
    }

    private void RenderObject(string text, Object value)
    {
        GUIStyle active = regularLabelGuiStyle;
        int instanceId = value == null ? NullObjectId : value.GetInstanceID();

        if (selectedObjectId == instanceId)
        {
            active = selectedLabelGuiStyle;
        }

        if (!GUILayout.Button(text, active)) return;
        if (selectedObjectId != instanceId & clicks > 0)
        {
            clicks = 0;
        }

        clicks++;
        selectedObjectId = instanceId;

        double delta = EditorApplication.timeSinceStartup - lastDoubleClickTime;
        if (delta < doubleClickTimeWindow && clicks >= 2)
        {
            clicks = 0;
            OnAssetPicked(value);
            selectedObjectId = NothingSelectedId;
            Close();
        }

        lastDoubleClickTime = EditorApplication.timeSinceStartup;
    }

    public void Select(Object selected)
    {
        selectedObjectId = selected == null ? NullObjectId : selected.GetInstanceID();
    }

    public void GetFiles(string folderPath, Type objectType)
    {
        objects = new Dictionary<int, Object>();
        Object[] assets = Resources.LoadAll(folderPath, objectType);

        foreach (Object asset in assets)
        {
            objects.Add(asset.GetInstanceID(), asset);
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