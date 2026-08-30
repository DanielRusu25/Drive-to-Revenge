using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class BuildingPlacerWindow : EditorWindow
{
    private static bool placing = false;
    private static GameObject[] buildingPrefabs;

    [MenuItem("Tools/Building Placer")]
    public static void ShowWindow()
    {
        GetWindow<BuildingPlacerWindow>("Building Placer");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Building Placer Tool", EditorStyles.boldLabel);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty prop = so.FindProperty("buildingPrefabs");

        EditorGUI.BeginChangeCheck();
        var newPrefabs = EditorGUILayout.ObjectField("Prefab Holder", prefabHolder, typeof(BuildingSpawner), true) as BuildingSpawner;
        if (EditorGUI.EndChangeCheck())
        {
            if (newPrefabs != null)
            {
                prefabHolder = newPrefabs;
                buildingPrefabs = prefabHolder.buildingPrefabs;
            }
        }

        EditorGUI.BeginDisabledGroup(buildingPrefabs == null || buildingPrefabs.Length == 0);

        if (!placing)
        {
            if (GUILayout.Button("Start Placing (Ctrl+Shift+K)"))
            {
                StartPlacing();
            }
        }
        else
        {
            if (GUILayout.Button("Stop Placing (Ctrl+Shift+K)"))
            {
                StopPlacing();
            }
        }

        EditorGUI.EndDisabledGroup();
    }

    private static BuildingSpawner prefabHolder;

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        placing = false;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!placing || buildingPrefabs == null || buildingPrefabs.Length == 0) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(hit.point, hit.normal, 1f);

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                PlaceBuilding(hit.point);
                e.Use();
            }
        }

        SceneView.RepaintAll();
    }

    private static void StartPlacing()
    {
        placing = true;
    }

    private static void StopPlacing()
    {
        placing = false;
    }

    private static void PlaceBuilding(Vector3 position)
    {
        if (buildingPrefabs == null || buildingPrefabs.Length == 0)
        {
            Debug.LogWarning("No prefabs set.");
            return;
        }

        GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
        GameObject newBuilding = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

        if (newBuilding != null)
        {
            newBuilding.transform.position = position;
            Undo.RegisterCreatedObjectUndo(newBuilding, "Spawn Building");
            Selection.activeGameObject = newBuilding;
            EditorSceneManager.MarkSceneDirty(newBuilding.scene);
        }
    }

    // Optional: shortcut to toggle placing
    [MenuItem("Tools/Toggle Building Placing %#k")]
    private static void TogglePlacing()
    {
        placing = !placing;
    }
}
