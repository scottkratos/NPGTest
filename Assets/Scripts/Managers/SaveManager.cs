using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SaveCurrentProgress()
    {
        SaveSystem.SavePlayer();
    }

    public void LoadProgress()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        PlayerController.instance.inventory = data.inventoryItems;
        GameManager.instance.boxItems = data.boxItems;
        GameManager.instance.usedUUIDs = data.usedUUIDs;
        GameManager.instance.lastLevel = data.lastLevel;
        GameManager.instance.lastPosition = data.lastPosition;
    }
}
