using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isPaused;
    public InventoryItem[] boxItems;
    public List<string> collectedItems = new List<string>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
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
