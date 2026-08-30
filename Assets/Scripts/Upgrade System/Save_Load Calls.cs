using UnityEngine;

public class Save_LoadCalls : MonoBehaviour
{


    public void OnSaveClicked()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void OnLoadClicked()
    {
        DataPersistenceManager.instance.LoadGame();
    }
}
