using UnityEditor;
using UnityEngine;

public class EnemyContentEditor : IContentEditor
{
    public string ContentName => entityName;

    private string entityName;
    private string cardSpriteName;
    private string description;
    private int attackPoints;
    private int healthPoints;

    private Vector2 descriptionScrollPosition;
    private string previousEntityName;

    public EnemyContentEditor()
    {
        ClearValues();
    }

    public void Draw()
    {
        GUI.SetNextControlName("NameTextFieldEnemy");
        previousEntityName = entityName;
        entityName = EditorGUILayout.TextField("Name", entityName).Trim();

        if (GUI.GetNameOfFocusedControl() == "NameTextField" && previousEntityName != entityName)
        {
            cardSpriteName = entityName;
        }

        cardSpriteName = EditorGUILayout.TextField("Sprite Name", cardSpriteName);

        EditorGUILayout.LabelField("Description");
        descriptionScrollPosition = EditorGUILayout.BeginScrollView(descriptionScrollPosition, GUILayout.Height(75));
        description = EditorGUILayout.TextArea(description, GUILayout.ExpandHeight(true)).Trim();
        EditorGUILayout.EndScrollView();

        attackPoints = EditorGUILayout.IntField("Attack Points", attackPoints);
        healthPoints = EditorGUILayout.IntField("Health Points", healthPoints);     
    }

    public void ClearValues()
    {
        entityName = string.Empty;
        cardSpriteName = string.Empty;
        description = string.Empty;
        attackPoints = 0;
        healthPoints = 0;
    }

    public void Save(string filePath)
    {
        EnemyUtility.SaveToFile(filePath, entityName, cardSpriteName, description, attackPoints, healthPoints);
    }

    public string Load(string filePath)
    {
        ClearValues();

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = EditorUtility.OpenFilePanel("Open enemy asset file", Enemy.AssetFilePath, "enemy");
        }

        EnemyUtility.LoadFromFile(filePath, out entityName, out cardSpriteName, out description, out attackPoints, out healthPoints);
        return filePath;
    }
}
