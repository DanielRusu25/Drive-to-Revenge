using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour, IDataPersistence
{
    [Header("Money")]
    public int currentMoney;


    [Header("Start Race Menu")]
    public GameObject startRaceMenu;
    public Button startRaceButton;
    public TMP_Text raceNameText;
    public RawImage raceImage;
    public TMP_Text typeText;
    public TMP_Text objectiveText;
    public TMP_Text requiermentText;
    public int fadeSpeed = 7;

    [Header("End Race Obects")]
    public GameObject endRaceMenu;
    public EndRace endRaceScript;

    [HideInInspector] public int currentRaceIndex;
    private CanvasGroup startRaceMenuPanel;
    private bool shouldFade;

    [Header("Race Objects")]
    public List<GameObject> raceContainers;
    public List<GameObject> enterRaceTriggers;

    [Header("Player")]
    public Transform playerTransform;
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    private void Start()
    {
        startRaceMenuPanel = startRaceMenu.GetComponentInChildren<CanvasGroup>();
        startRaceMenuPanel.alpha = 0f;
        shouldFade = false;

        currentRaceIndex = -1;

        foreach (EnterRaceTrigger trigger in GetComponentsInChildren<EnterRaceTrigger>())
            trigger.raceManager = this;
    }

    private void Update()
    {
        FadeMenu();
    }

    public void EnterRace(int raceIndex)
    {
        for (int i = 0; i < raceContainers.Count; i++)
        {
            enterRaceTriggers[i].SetActive(false);

            if (i == raceIndex)
                raceContainers[i].SetActive(true);
            else
                raceContainers[i].SetActive(false);
        }

        DataPersistenceManager.instance.SaveGame();
    }

    public void ExitRace()
    {
        foreach (GameObject container in raceContainers)
            container.SetActive(false);

        foreach (GameObject trigger in enterRaceTriggers)
            trigger.SetActive(true);

        DataPersistenceManager.instance.SaveGame();
    }

    public IEnumerator ShowStartRaceMenu(string raceName, Texture2D image ,  string info, string objective, string requirements)
    {
        //Handle the menu
        startRaceMenu.SetActive(true);
        shouldFade = true;

        //Handle the UI elements
        raceNameText.text = raceName;
        raceImage.texture = image;
        typeText.text = info;
        objectiveText.text = objective;
        requiermentText.text = requirements;

        yield return new WaitUntil(() => startRaceMenuPanel.alpha >= 1f);
    }

    public IEnumerator HideStartRaceMenu()
    {
        shouldFade = false;
        yield return new WaitUntil(() => startRaceMenuPanel.alpha <= 0f);
        startRaceMenu.SetActive(false);
    }

    public void OnStartRaceButtonPressed(Vector3 position, Quaternion rotation)
    {
        if (currentRaceIndex >= 0)
        {
            Debug.Log($"Entering race with index: {currentRaceIndex}");
            EnterRace(currentRaceIndex);

            Rigidbody playerRigidbody = playerTransform.GetComponent<Rigidbody>();
            playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;

            playerTransform.position = position;
            playerTransform.rotation = rotation;

            playerStartPosition = position;
            playerStartRotation = rotation;

            StartCoroutine(HideStartRaceMenu());
        }
        else
        {
            Debug.LogWarning("No race selected to start!");
        }
    }

    public void OnExitRaceButtonPressed()
    {
        ExitRace();
        ManageMoney(enterRaceTriggers[currentRaceIndex].GetComponent<EnterRaceTrigger>().moneyReward);
        currentRaceIndex = -1;
        endRaceMenu.SetActive(false);
    }

    public void OnRetryButtonPressed()
    {
        if (currentRaceIndex >= 0)
        {
            endRaceMenu.SetActive(false);
            raceContainers[currentRaceIndex].SetActive(false);
            OnStartRaceButtonPressed(playerStartPosition, playerStartRotation);
            raceContainers[currentRaceIndex].SetActive(true);

            ManageMoney(enterRaceTriggers[currentRaceIndex].GetComponent<EnterRaceTrigger>().moneyReward);
        }
    }

    public void FadeMenu()
    {
        if (shouldFade)
        {
            if (startRaceMenuPanel.alpha < 1)
                startRaceMenuPanel.alpha += Time.deltaTime * fadeSpeed;
        }
        else
        {
            if (startRaceMenuPanel.alpha > 0)
                startRaceMenuPanel.alpha -= Time.deltaTime * fadeSpeed;
        }
    }

    public void ManageMoney(int amount)
    {
        if (endRaceScript.raceWon)
            currentMoney += amount;
    }

    public void LoadData(GameData data)
    {
        currentMoney = data.currentMoney;
    }

    public void SaveData(GameData data)
    {
        data.currentMoney = currentMoney;
    }

}
