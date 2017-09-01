using UnityEditor;
using UnityEngine;

public class CardContentEditor : IContentEditor
{
    public string ContentName => card?.Name;

    private readonly ResourceAssetPickerControl<Sprite> spritePickerControl;
    private Card card;
    private Vector2 descriptionScrollPosition;

    private readonly EditorWindow window;

    public CardContentEditor(EditorWindow window)
    {
        this.window = window;

        spritePickerControl = new ResourceAssetPickerControl<Sprite>(window, "Sprite");
        ClearValues();
    }

    public void Draw()
    {
        if (card == null) return;
        card.Name = EditorGUILayout.TextField("Name", card.Name).Trim();
        card.SpritePath = spritePickerControl.OnGUI().FilePath;

        EditorGUILayout.Space();
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
        ContentUtility.SaveToFile(filePath, card);
    }

    public string Load(string filePath)
    {
        ClearValues();

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = EditorUtility.OpenFilePanel("Open card asset file", Card.AssetFilePath, "card");
        }

        ContentUtility.LoadFromFile(filePath, out card);
        spritePickerControl.SetValue(card.SpritePath);

        return filePath;
    }
}