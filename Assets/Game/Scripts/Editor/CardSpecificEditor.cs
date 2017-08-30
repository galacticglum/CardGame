using UnityEditor;
using UnityEngine;

public class CardSpecificEditor : IEntityEditor
{
    public string EntityName => entityName;

    private string entityName;
    private string cardSpriteName;
    private string description;
    private int attackPoints;
    private int immediateActionPoints;

    private Vector2 descriptionScrollPosition;
    private string previousEntityName;

    public CardSpecificEditor()
    {
        ClearValues();
    }

    public void Draw()
    {
        GUI.SetNextControlName("NameTextFieldCard");
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
        immediateActionPoints = EditorGUILayout.IntField("Immediate Action Points", immediateActionPoints);
    }

    public void ClearValues()
    {
        entityName = string.Empty;
        cardSpriteName = string.Empty;
        description = string.Empty;
        attackPoints = 0;
        immediateActionPoints = 0;
    }

    public void Save(string filePath)
    {
        CardUtility.SaveToFile(filePath, entityName, cardSpriteName, description, attackPoints, immediateActionPoints);
    }

    public string Load(string filePath)
    {
        ClearValues();

        if (string.IsNullOrEmpty(filePath))
        {
            filePath = EditorUtility.OpenFilePanel("Open card asset file", Card.AssetFilePath, "card");
        }

        CardUtility.LoadFromFile(filePath, out entityName, out cardSpriteName, out description, out attackPoints, out immediateActionPoints);
        return filePath;
    }
}