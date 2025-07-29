using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isPaused;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void PauseGame()
    {
        if (isPaused) UIManager.instance.CloseInventory();
        else UIManager.instance.OpenInventory();

        isPaused = !isPaused;
    }
}
