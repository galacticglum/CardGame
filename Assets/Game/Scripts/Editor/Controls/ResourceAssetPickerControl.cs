using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceAssetPickerControl<T> where T : Object
{
    /// <summary>
    /// The control label, the text before the picker.
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// The size of the preview box.
    /// </summary>
    public Vector2 PreviewSize { get; }

    /// <summary>
    /// The none label text, this text is rendered when the asset is null.
    /// </summary>
    public string NoneLabelText { get; set; } = "None";

    /// <summary>
    /// The percentage of the main box which should be used as a border. 
    /// <remarks>This is from 0 to 1.</remarks>
    /// </summary>
    public float BorderPercent { get; set; } = 0.05f;

    /// <summary>
    /// The padding percent of the preview image. This is the inverse of the boder percentage (1 - BorderPercent).
    /// </summary>
    public float Percent => 1 - BorderPercent;

    private readonly EditorWindow window;
    private ResourceAsset<T> asset;

    public ResourceAssetPickerControl(EditorWindow window, string label) : this(window, label, new Vector2(128, 128))
    {
    }

    public ResourceAssetPickerControl(EditorWindow window, string label, Vector2 previewSize)
    {
        PreviewSize = previewSize;
        Label = label;
        this.window = window;

        asset = new ResourceAsset<T>();
    }

    /// <summary>
    /// Render the control.
    /// </summary>
    /// <returns>The asset picked by the user (if picked at all.</returns>
    /// <remarks>The return asset can be null, make sure you check before using it.</remarks>
    public ResourceAsset<T> OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        string assetText = asset.Object == null ? NoneLabelText : AssetDatabase.GetAssetPath(asset.Object);
        EditorGUILayout.LabelField(new GUIContent(Label), new GUIContent(assetText), EditorStyles.textField);
        if (GUILayout.Button("Select", EditorStyles.miniButton, GUILayout.Width(50)))
        {
            ResourceAssetPickerEditorWindow<T>.Popup(asset.Object, (sender, args) =>
            {
                asset.Object = args.Asset;
            });
        }

        EditorGUILayout.EndHorizontal();
        GUIStyle style = new GUIStyle(GUI.skin.FindStyle("ObjectFieldThumb"))
        {
            margin = new RectOffset((int)window.position.width - (128 + EditorStyles.miniButton.margin.left), 0, 0, 0)
        };

        Texture2D previewTexture = AssetPreview.GetAssetPreview(asset.Object) ??
                                   AssetPreview.GetMiniTypeThumbnail(asset.Object?.GetType());

        Rect rect = GUILayoutUtility.GetRect(new GUIContent(previewTexture), style, GUILayout.Width(PreviewSize.x), GUILayout.Height(PreviewSize.y));
        GUI.Box(rect, GUIContent.none, style);

        // Scaling the rect to the preview image size factoring the border percent.
        rect.width *= Percent;
        rect.height *= Percent;

        // The amount difference between the full box size and our preview image size.
        // We use this to centre the preview image
        float delta = rect.width / Percent - rect.width;
        rect.x += delta / 2;
        rect.y += delta / 2;

        if (previewTexture != null)
        {
            GUI.DrawTexture(rect, previewTexture, ScaleMode.StretchToFill);

            // Align the button the to the bottom right corner of the full box rect.
            Rect buttonRect = new Rect(rect.x - delta / 2, rect.y - delta / 2, rect.width * 0.5f, rect.height * 0.15f);
            buttonRect.x += rect.width / Percent - buttonRect.width;
            buttonRect.y += rect.height / Percent - buttonRect.height + 1;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.GetStyle("OL TitleLeft"));
            if (GUI.Button(buttonRect, "Open", buttonStyle))
            {
                AssetDatabase.OpenAsset(asset.Object);
            }
        }
        else
        {
            string typeText = $"({typeof(T).Name})";

            Vector2 noneTextSize = EditorStyles.largeLabel.CalcSize(new GUIContent(NoneLabelText));
            Vector2 typeTextSize = EditorStyles.largeLabel.CalcSize(new GUIContent(typeText));

            // Centre the none label and move it up by the height of the type label.
            float width = rect.x + rect.width / 2 - noneTextSize.x / 2;
            float height = rect.y + rect.height / 2 - noneTextSize.y / 2 - typeTextSize.y;
            Rect noneLabelRect = new Rect(width, height, noneTextSize.x, noneTextSize.y);

            // Centre the type text label to the absolute centre of the box rect.
            width = rect.x + rect.width / 2 - typeTextSize.x / 2;
            height = rect.y + rect.height / 2 - typeTextSize.y / 2;
            Rect typeLabelRect = new Rect(width, height, typeTextSize.x, typeTextSize.y);

            // Draw the labels.
            GUI.Label(noneLabelRect, NoneLabelText, EditorStyles.largeLabel);
            GUI.Label(typeLabelRect, typeText, EditorStyles.largeLabel);
        }

        return asset;
    }

    public void ClearValue()
    {
        asset.Object = null;
    }

    public void SetValue(string path)
    {
        T obj = Resources.Load<T>(path);
        if (obj == null) return;
        asset.Object = obj;
    }
}