using System.IO;
using UnityEditor;
using UnityEngine;

public class EntityCreatorEditorWindow : EditorWindow
{
    private bool isEditing;
    private bool isEnemy;
    private string loadedFilePath;

    private EnemySpecificEditor enemySpecificEditor;
    private CardSpecificEditor cardSpecificEditor;
    private IEntityEditor activeEditor;

    [MenuItem("Window/Entity Creator")]
    public static void Init()
    {
        EntityCreatorEditorWindow window = (EntityCreatorEditorWindow)GetWindow(typeof(EntityCreatorEditorWindow));
        window.titleContent = new GUIContent("Entity Creator");
        window.Focus();
    }

    [MenuItem("Assets/Open in Entity Editor")]
    public static void OpenInEditor()
    {
        string filePath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string extension = Path.GetExtension(filePath);

        Init();
        EntityCreatorEditorWindow window = (EntityCreatorEditorWindow)GetWindow(typeof(EntityCreatorEditorWindow));
        switch (extension)
        {
            case ".enemy":
                window.LoadEnemy(filePath);
                break;
            case ".card":
                window.LoadCard(filePath);
                break;
        }
    }

    [MenuItem("Assets/Open in Entity Editor", true)]
    public static bool OpenInEditorValidation()
    {
        string extension = Path.GetExtension(AssetDatabase.GetAssetPath(Selection.activeObject));
        return extension == ".enemy" || extension == ".card";
    }

    private void OnEnable()
    {
        enemySpecificEditor = new EnemySpecificEditor();
        cardSpecificEditor = new CardSpecificEditor(this);
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(titleContent.text, EditorStyles.boldLabel);
        Rect openButtonRect = GUILayoutUtility.GetRect(new GUIContent("Open"), EditorStyles.miniButton, GUILayout.Width(50));
        if (GUI.Button(openButtonRect, new GUIContent("Open"), EditorStyles.miniButton))
        {
            GenericMenu toolsMenu = new GenericMenu();

            toolsMenu.AddItem(new GUIContent("Card"), false, () => LoadCard());
            toolsMenu.AddItem(new GUIContent("Enemy"), false, () => LoadEnemy());
            toolsMenu.DropDown(openButtonRect);

            EditorGUIExtensions.RemoveFocus();
            GUIUtility.ExitGUI();
        }

        if (string.IsNullOrEmpty(activeEditor?.EntityName))
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
                string extension = isEnemy ? "enemy" : "card";
                string savePath = isEnemy ? Enemy.AssetFilePath : Card.AssetFilePath;
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                filePath = EditorUtility.SaveFilePanel($"Save {extension} asset file", savePath, activeEditor?.EntityName, extension);
                loadedFilePath = filePath;
            }

            EditorGUIExtensions.RemoveFocus();
            activeEditor?.Save(filePath);
        }

        GUI.enabled = true;

        Rect newButtonRect = GUILayoutUtility.GetRect(new GUIContent("New"), EditorStyles.miniButton, GUILayout.Width(50));
        if (GUI.Button(newButtonRect, new GUIContent("New"), EditorStyles.miniButton))
        {
            GenericMenu toolsMenu = new GenericMenu();

            toolsMenu.AddItem(new GUIContent("Card"), false, NewCard);
            toolsMenu.AddItem(new GUIContent("Enemy"), false, NewEnemy);
            toolsMenu.DropDown(newButtonRect);

            EditorGUIExtensions.RemoveFocus();
            GUIUtility.ExitGUI();
        }

        if (!isEditing)
        {
            GUI.enabled = false;
        }

        if (GUILayout.Button(new GUIContent("Close"), EditorStyles.miniButton, GUILayout.Width(50)))
        {
            EditorGUIExtensions.RemoveFocus();
            ClearValues(false);
        }

        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (!isEditing)
        {
            EditorGUILayout.HelpBox("To begin editing an entity, open or create a new entity.", MessageType.Info);
            return;
        }

        activeEditor?.Draw();

        EditorGUIExtensions.Splitter();
        EditorGUILayout.Space();
    }

    private void ClearValues(bool edit = true)
    {
        cardSpecificEditor.ClearValues();
        enemySpecificEditor.ClearValues();

        loadedFilePath = string.Empty;
        isEnemy = false;
        isEditing = edit;
    }

    private void NewCard()
    {
        ClearValues();
        isEnemy = false;

        activeEditor = cardSpecificEditor;
    }

    private void NewEnemy()
    {
        ClearValues();
        isEnemy = true;

        activeEditor = enemySpecificEditor;
    }

    private void LoadCard(string filePath = null)
    {
        ClearValues();
        activeEditor = cardSpecificEditor;
        isEnemy = false;

        loadedFilePath = cardSpecificEditor.Load(filePath);
    }

    private void LoadEnemy(string filePath = null)
    {
        ClearValues();
        activeEditor = enemySpecificEditor;
        isEnemy = true;

        loadedFilePath = enemySpecificEditor.Load(filePath);
    }
}