using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadRetry : MonoBehaviour
{
    private String  reloadScene;
    [SerializeField] private GameObject[] otherUIToDisable;
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Slider loadingSlider; 

    void Start ()
    {
        reloadScene = SceneManager.GetActiveScene().name;
        loadingScreenCanvas.SetActive(false);
    }

    public void Button()
    {
        DataPersistenceManager.instance.SaveGame();

        Time.timeScale = 1;

        foreach (GameObject canvas in otherUIToDisable)
            canvas.SetActive(false);

        loadingScreenCanvas.SetActive(true);

        Debug.Log("Reload Button presed!");

        StartCoroutine(LoadSceneAsync(reloadScene));
    }
    
    IEnumerator LoadSceneAsync ( string reloadScene)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(reloadScene);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01( loadOperation.progress / 0.9f );
            loadingSlider.value = progressValue;
            yield return null;
        }
    }

}
