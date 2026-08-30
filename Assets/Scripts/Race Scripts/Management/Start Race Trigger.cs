using UnityEngine;

public class EnterRaceTrigger : MonoBehaviour
{
    [Header("Money")]
    public int moneyReward;

    [Header("Ui Objects")]
    public string raceName;
    public Texture2D raceImage;
    public string typeText;
    public string objectiveText;
    public string requirementsText;

    [Header("Player References")]
    public Vector3 playerPosition;
    public Quaternion playerRotation;

    [HideInInspector] public RaceManager raceManager;



    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player Body")
        {
            StartCoroutine(raceManager.ShowStartRaceMenu(raceName, raceImage ,typeText, objectiveText, requirementsText));
            raceManager.currentRaceIndex = transform.GetSiblingIndex();
            raceManager.startRaceButton.onClick.AddListener(() => raceManager.OnStartRaceButtonPressed(playerPosition, playerRotation));
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player Body")
        {
            StartCoroutine(raceManager.HideStartRaceMenu());
            raceManager.currentRaceIndex = -1;
            raceManager.startRaceButton.onClick.RemoveListener(() => raceManager.OnStartRaceButtonPressed(playerPosition, playerRotation));
        }
    }

}
