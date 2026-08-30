using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(BuildingSpawner))]
public class BuildingSpawnerEditor : Editor
{
    private static bool placing = false;

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (!placing) return;

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Handles.DrawWireDisc(hit.point, hit.normal, 1f);

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                PlaceBuildingAtPoint(hit.point);
                e.Use();
            }
        }

        SceneView.RepaintAll();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (placing)
        {
            if (GUILayout.Button("Stop Placing (Shortcut: Ctrl+Shift+M)")) 
            {
                TogglePlacing();
            }
        }
        else
        {
            if (GUILayout.Button("Start Placing (Shortcut: Ctrl+Sift+M)"))
            {
                TogglePlacing();
            }
        }
    }

    private void TogglePlacing()
    {
        placing = !placing;
        SceneView.RepaintAll();
    }

    [MenuItem("Tools/Toggle Building Placer %#m")] // Ctrl + Shift + l
    private static void TogglePlacingShortcut()
    {
        placing = !placing;
        SceneView.RepaintAll();
    }

    private void PlaceBuildingAtPoint(Vector3 position)
    {
        BuildingSpawner spawner = (BuildingSpawner)Selection.activeGameObject?.GetComponent<BuildingSpawner>();
        if (spawner == null)
        {
            Debug.LogWarning("Select a GameObject with a BuildingSpawner script.");
            return;
        }

        if (spawner.buildingPrefabs == null || spawner.buildingPrefabs.Length == 0)
        {
            Debug.LogWarning("No building prefabs assigned.");
            return;
        }

        GameObject prefab = spawner.buildingPrefabs[Random.Range(0, spawner.buildingPrefabs.Length)];
        GameObject newBuilding = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        if (newBuilding != null)
        {
            newBuilding.transform.position = position;
            Undo.RegisterCreatedObjectUndo(newBuilding, "Spawn Random Building");
            Selection.activeGameObject = newBuilding;
            EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);
        }
    }
}
