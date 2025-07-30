using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isPaused;
    public InventoryItem[] boxItems;
    public List<string> usedUUIDs = new List<string>();
    [HideInInspector] public string lastLevel;
    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public InventoryItem[] playerInventory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        lastLevel = SceneManager.GetActiveScene().name;
        if (playerInventory.Length > 0 && PlayerController.instance != null) PlayerController.instance.inventory = playerInventory;
    }

    public void OpenInventory()
    {
        if (isPaused) UIManager.instance.CloseInventory();
        else UIManager.instance.OpenInventory();

        isPaused = !isPaused;
    }

    public void OpenBox()
    {
        if (isPaused) UIManager.instance.CloseInventory();
        else UIManager.instance.OpenBox();

        isPaused = !isPaused;
    }
}
