using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private GameObject[] otherUIToDisable;
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Slider loadingSlider; 

    void Start ()
    {
        loadingScreenCanvas.SetActive(false);
        loadingSlider.interactable = false;

    }

    public void LoadingScreen(string sceneToLoad)
    {
        DataPersistenceManager.instance.SaveGame();

        Time.timeScale = 1;

        foreach (GameObject canvas in otherUIToDisable)
            canvas.SetActive(false);

        loadingScreenCanvas.SetActive(true);

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }
    
    IEnumerator LoadSceneAsync (string sceneToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01( loadOperation.progress / 0.9f );
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
