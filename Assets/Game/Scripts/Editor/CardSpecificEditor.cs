using UnityEditor;
using UnityEngine;

public class CardSpecificEditor : IEntityEditor
{
    public string EntityName => card?.Name;

    private readonly ResourceAssetPickerControl<Object> spritePickerControl;
    private Card card;
    private Vector2 descriptionScrollPosition;
    private Sprite sprite;

    private readonly EditorWindow window;

    public CardSpecificEditor(EditorWindow window)
    {
        this.window = window;

        spritePickerControl = new ResourceAssetPickerControl<Object>(window, "Sprite");
        ClearValues();
    }

    public void Draw()
    {
        if (card == null) return;
        card.Name = EditorGUILayout.TextField("Name", card.Name).Trim();

        spritePickerControl.OnGUI();
        sprite = (Sprite)EditorGUILayout.ObjectField(new GUIContent("test"), sprite, typeof(Sprite), true);

        EditorGUILayout.Space();
        card.SpriteName = EditorGUILayout.TextField("Sprite Name", card.SpriteName);
        card.BackgroundColour = EditorGUILayout.ColorField("Background Colour", card.BackgroundColour);

        EditorGUILayout.LabelField("Description");
        descriptionScrollPosition = EditorGUILayout.BeginScrollView(descriptionScrollPosition, GUILayout.Height(75));
        card.Description = EditorGUILayout.TextArea(card.Description, GUILayout.ExpandHeight(true)).Trim();
        EditorGUILayout.EndScrollView();

        card.AttackPoints = EditorGUILayout.IntField("Attack Points", card.AttackPoints);
        card.HealthCost = EditorGUILayout.IntField("Health Cost", card.HealthCost);
        card.IsImmediate = EditorGUILayout.Toggle("Is Immediate", card.IsImmediate);
    }

    public void ClearValues()
    {
        card = new Card();
        spritePickerControl.ClearValue();
    }

    public void Save(string filePath)
    {
        CardUtility.SaveToFile(filePath, card);
    }

    public string Load(string filePath)
    {
        ClearValues();

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = EditorUtility.OpenFilePanel("Open card asset file", Card.AssetFilePath, "card");
        }

        CardUtility.LoadFromFile(filePath, out card);
        return filePath;
    }
}