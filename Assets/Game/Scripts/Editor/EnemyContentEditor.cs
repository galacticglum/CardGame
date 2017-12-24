using UnityEditor;
using UnityEngine;

public class EnemyContentEditor : IContentEditor
{
    public string ContentName => enemy?.Name;

    private readonly ResourceAssetPickerControl<Sprite> spritePickerControl;
    private Enemy enemy;
    private Vector2 descriptionScrollPosition;

    public EnemyContentEditor(EditorWindow window)
    {
        spritePickerControl = new ResourceAssetPickerControl<Sprite>(window, "Sprite");
        ClearValues();
    }

    public void Draw()
    {
        if (enemy == null) return;

        enemy.Name = EditorGUILayout.TextField("Name", enemy.Name).Trim();
        enemy.SpritePath = spritePickerControl.OnGUI().FilePath;

        EditorGUILayout.Space();
        enemy.BackgroundColour = EditorGUILayout.ColorField("Background Colour", enemy.BackgroundColour);

        EditorGUILayout.LabelField("Description");
        descriptionScrollPosition = EditorGUILayout.BeginScrollView(descriptionScrollPosition, GUILayout.Height(75));
        enemy.Description = EditorGUILayout.TextArea(enemy.Description, GUILayout.ExpandHeight(true)).Trim();
        EditorGUILayout.EndScrollView();

        enemy.AttackPoints = EditorGUILayout.IntField("Attack Points", enemy.AttackPoints);
        enemy.HealthPoints = EditorGUILayout.IntField("Health", enemy.HealthPoints);
    }

    public void ClearValues()
    {
        enemy = new Enemy();
        spritePickerControl.ClearValue();
    }

    public void Save(string filePath)
    {
        ContentUtility.SaveToFile(filePath, enemy);
    }

    public string Load(string filePath)
    {
        ClearValues();

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = EditorUtility.OpenFilePanel("Open Card asset file", Card.AssetFilePath, "Card");
        }

        ContentUtility.LoadFromFile(filePath, out enemy);
        spritePickerControl?.SetValue(enemy?.SpritePath);

        return filePath;
    }
}
