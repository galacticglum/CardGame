using Assets.Game.Scripts.Editor;
using UnityEditor;
using UnityEngine;

public class CardCreatorEditorWindow : EditorWindow
{
    private string cardName;
    private string cardSpriteName;
    private string description;
    private int attackPoints;
    private int immediateActionPoints;

    private Vector2 descriptionScrollPosition;
    private string loadedFilePath;

    private string previousCardName;

    [MenuItem("Window/Card Creator")]
    public static void Init()
    {
        CardCreatorEditorWindow window = (CardCreatorEditorWindow)GetWindow(typeof(CardCreatorEditorWindow));
        window.titleContent = new GUIContent("Card Creator");
    }

    private void OnEnable()
    {
        cardName = string.Empty;
        cardSpriteName = string.Empty;
        description = string.Empty;
        attackPoints = 0;
        immediateActionPoints = 0;
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Card Creator", EditorStyles.boldLabel);

        GUI.SetNextControlName("LoadCardButton");
        if (GUILayout.Button("Open", EditorStyles.miniButton, GUILayout.Width(50)))
        {
            GUI.FocusControl("LoadCardButton");
            LoadCard();
        }

        if (string.IsNullOrEmpty(cardName))
        {
            GUI.enabled = false;
        }

        GUI.SetNextControlName("SaveButton");
        if (GUILayout.Button("Save", EditorStyles.miniButton, GUILayout.Width(50)))
        {
            GUI.FocusControl("SaveButton");

            string filePath = loadedFilePath;
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = EditorUtility.SaveFilePanel("Save card asset file", Card.AssetFilePath, cardName, "json");
            }

            CardUtility.SaveToFile(filePath, cardName, cardSpriteName, description, attackPoints, immediateActionPoints);
        }

        GUI.enabled = true;

        GUI.SetNextControlName("ClearButton");
        if (GUILayout.Button("New", EditorStyles.miniButton, GUILayout.Width(50)))
        {
            GUI.FocusControl("ClearButton");
            ClearValues();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUI.SetNextControlName("NameTextField");
        previousCardName = cardName;
        cardName = EditorGUILayout.TextField("Name", cardName).Trim();

        if (GUI.GetNameOfFocusedControl() == "NameTextField" && previousCardName != cardName)
        {
            cardSpriteName = cardName;
        }

        cardSpriteName = EditorGUILayout.TextField("Sprite Name", cardSpriteName);

        EditorGUILayout.LabelField("Description");
        descriptionScrollPosition = EditorGUILayout.BeginScrollView(descriptionScrollPosition, GUILayout.Height(75));
        description = EditorGUILayout.TextArea(description, GUILayout.ExpandHeight(true)).Trim();
        EditorGUILayout.EndScrollView();

        attackPoints = EditorGUILayout.IntField("Attack Points", attackPoints);
        immediateActionPoints = EditorGUILayout.IntField("Immediate Action Points", immediateActionPoints);

        EditorGUIExtensions.Splitter();
        EditorGUILayout.Space();
    }

    private void ClearValues()
    {
        cardName = string.Empty;
        cardSpriteName = string.Empty;
        description = string.Empty;
        attackPoints = 0;
        immediateActionPoints = 0;

        loadedFilePath = string.Empty;
    }

    private void LoadCard()
    {
        string filePath = EditorUtility.OpenFilePanel("Open card asset file", Card.AssetFilePath, "json");
        loadedFilePath = filePath;
        CardUtility.LoadFromFile(filePath, out cardName, out cardSpriteName, out description, out attackPoints, out immediateActionPoints);
    }
}
