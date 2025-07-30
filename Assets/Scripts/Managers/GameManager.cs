using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPaused;
    public InventoryItem[] boxItems;
    public List<string> usedUUIDs = new List<string>();
    [HideInInspector] public string lastLevel;
    [HideInInspector] public Vector3 lastPosition;
    [HideInInspector] public InventoryItem[] playerInventory;
    [HideInInspector] public string doorUUID;
    private string levelToLoad;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    private int audioIndex;
    private bool isAlternating;
    [HideInInspector] public int playerHealth;
    [HideInInspector] public bool playerWeaponEquipped;
    private bool needToSetPosition;

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
    }

    public void OpenInventory()
    {
        if (isPaused) UIManager.instance.CloseInventory();
        else UIManager.instance.OpenInventory();

        isPaused = !isPaused;
        if (isPaused) PlayerController.instance.StopMovement();
    }

    public void OpenBox()
    {
        if (isPaused) UIManager.instance.CloseInventory();
        else UIManager.instance.OpenBox();

        isPaused = !isPaused;
    }

    public void LoadLevel(string level, string _doorUUID)
    {
        doorUUID = _doorUUID;
        levelToLoad = level;
        playerHealth = PlayerController.instance.health;
        playerInventory = PlayerController.instance.inventory;
        playerWeaponEquipped = PlayerController.instance.isWeaponEquipped;
        UIManager.instance.StartLoading();
        Invoke("StartLoading", 1);
    }

    public void LoadLevel(string level)
    {
        levelToLoad = level;
        needToSetPosition = true;
        UIManager.instance.StartLoading();
        Invoke("StartLoading", 1);
    }

    private void StartLoading()
    {
        lastLevel = levelToLoad;
        SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single).completed += SetPlayerStuff;
    }

    private void SetPlayerStuff(AsyncOperation asyncOp)
    {
        //Hack to save player stuff
        PlayerController.instance.health = instance.playerHealth;
        PlayerController.instance.inventory = instance.playerInventory;
        PlayerController.instance.isWeaponEquipped = instance.playerWeaponEquipped;
        if (needToSetPosition)
        {
            PlayerController.instance.gameObject.transform.position = lastPosition;
            needToSetPosition = false;
        }
    }

    private void Update()
    {
        //Hack solution to audio problem
        if (!audioSource.isPlaying)
        {
            if (isAlternating)
            {
                audioIndex++;
                if (audioIndex >= audioClips.Length) audioIndex = 0;
                audioSource.clip = audioClips[audioIndex];
                audioSource.Play();
            }
            else
            {
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    isAlternating = true;
                    audioSource.clip = audioClips[audioIndex];
                    audioSource.Play();
                }
            }
        }
    }
}
