using UnityEngine;
using UnityEngine.InputSystem;

public class MapSizeChange : MonoBehaviour
{
    [Header("Objects to toggle on/off")]
    public GameObject[] objectsFullMap;
    public GameObject[] objectsMiniMap;

    [Header("Objects with sound that need to be turned on/off")]
    public GameObject[] soundObjects;

    [Header("Input system")]
    public PlayerInput playerInput;
    public string uiNavigationActionMap = "UI Navigation";
    public string playerActionMap = "Player";

    private bool isExpanded = false;

    void Start()
    {
        ToggleObjects(isExpanded);
    }

    public void ToggleMap(InputAction.CallbackContext context)
    {
        if (gameObject.activeInHierarchy)
            if (context.performed)
                if (isExpanded)
                    CollapseMinimap();
                else
                    ExpandMinimap();
    }

    void ExpandMinimap()
    {
        isExpanded = true;
        ToggleObjects(isExpanded);
        //Time.timeScale = 0f; // Pause the game
    }

    void CollapseMinimap()
    {
        isExpanded = false;
        ToggleObjects(isExpanded);
        //Time.timeScale = 1f; // Resume the game
    }


    private void ToggleObjects(bool isExpanded)
    {
        foreach (GameObject gameObject in objectsMiniMap)
            gameObject.SetActive(!isExpanded);

        foreach (GameObject gameObject in objectsFullMap)
            gameObject.SetActive(isExpanded);

        foreach (GameObject gameObject in soundObjects)
            foreach (AudioSource audioSource in gameObject.GetComponents<AudioSource>())
                audioSource.enabled = !isExpanded;

        if (isExpanded)
            playerInput.SwitchCurrentActionMap(uiNavigationActionMap);
        else
            playerInput.SwitchCurrentActionMap(playerActionMap);

        playerInput.neverAutoSwitchControlSchemes = isExpanded;

        Cursor.visible = !isExpanded;

    }

}
