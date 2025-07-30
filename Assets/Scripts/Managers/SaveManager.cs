using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private GameObject loadBtn;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        if (loadBtn != null)
        {
            PlayerData data = SaveSystem.LoadPlayer();
            loadBtn.SetActive(data != null);
        }
    }

    public void SaveCurrentProgress()
    {
        SaveSystem.SavePlayer();
    }

    public void LoadProgress()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        GameManager.instance.playerInventory = data.inventoryItems;
        GameManager.instance.playerWeaponEquipped = data.isWeaponEquipped;
        GameManager.instance.playerHealth = data.playerHealth;
        GameManager.instance.boxItems = data.boxItems;
        GameManager.instance.usedUUIDs = data.usedUUIDs;
        GameManager.instance.lastLevel = data.lastLevel;
        GameManager.instance.lastPosition = new Vector3(data.lastPositionX, data.lastPositionY, data.lastPositionZ);
        GameManager.instance.LoadLevel(data.lastLevel);
    }
}
