using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject header;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject itemPreview;
    [SerializeField] private GameObject boxPanel;
    [SerializeField] private GameObject inventoryGrid;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private GameObject saveText;

    [Header("Inventory References")]
    private List<UISlot> inventoryUISlots = new List<UISlot>();
    [SerializeField] private TMPro.TextMeshProUGUI itemName;
    [SerializeField] private TMPro.TextMeshProUGUI itemDesc;
    [SerializeField] private InventoryItemGUI[] gui;

    [Header("Box References")]
    private List<UISlot> boxUISlots = new List<UISlot>();

    public static UIManager instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance.StartCoroutine(instance.Fade(false));
            Destroy(gameObject);
        }

        StartCoroutine(Fade(false));
        for (int i = 0; i < GameManager.instance.boxItems.Length; i++)
        {
            GameObject slot = Instantiate(slotPrefab, boxPanel.transform.GetChild(0));
            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 180);
            boxUISlots.Add(slot.GetComponent<UISlot>());
        }

        for (int i = 0; i < PlayerController.instance.inventory.Length; i++)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryGrid.transform);
            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 180);
            slot.GetComponent<UISlot>().isPlayerInventory = true;
            inventoryUISlots.Add(slot.GetComponent<UISlot>());
        }

        PopulateInventory();
        PopulateBox();
    }

    private void Update()
    {
        //Hack to make the text work instead of checking for damage / heal
        if (PlayerController.instance.health >= 75) healthText.text = "Fine";
        else if (PlayerController.instance.health < 75 && PlayerController.instance.health > 25) healthText.text = "Caution";
        else if (PlayerController.instance.health <= 25) healthText.text = "Danger";
    }

    //BlackScreen management
    private IEnumerator Fade(bool fadeIn)
    {
        blackScreen.SetActive(true);
        float fadeTimer = .5f;
        float localTimer = 0;
        Image image = blackScreen.GetComponent<Image>();

        while (localTimer < fadeTimer)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeIn ? Mathf.Lerp(0, 1, localTimer / fadeTimer) : Mathf.Lerp(1, 0, localTimer / fadeTimer));
            localTimer += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, fadeIn ? 1 : 0);
        if (!fadeIn) blackScreen.SetActive(false);
    }

    public void StartLoading()
    {
        StartCoroutine(Fade(true));
    }

    //Inventory Management
    public void OpenInventory()
    {
        PopulateInventory();
        inGame.SetActive(false);
        header.SetActive(true);
        itemPreview.SetActive(true);
        boxPanel.SetActive(false);
        inventory.SetActive(true);
    }

    public void PopulateInventory()
    {
        for (int i = 0; i < inventoryUISlots.Count; i++)
        {
            inventoryUISlots[i].FillInventorySlot(PlayerController.instance.inventory[i], gui[(int)PlayerController.instance.inventory[i].type]);
        }
    }

    public void ShowSaveGame()
    {
        CancelInvoke("HideSaveGame");
        saveText.SetActive(true);
        Invoke("HideSaveGame", 2);
    }

    private void HideSaveGame()
    {
        saveText.SetActive(false);
    }

    public void ShowItemInformation(InventoryItem item)
    {
        itemName.text = item.itemName;
        itemDesc.text = item.itemDesc;
        ItemProxy.instance.ChangeMesh(gui[(int)item.type].mesh, gui[(int)item.type].material);
    }

    public void HideItemOptions()
    {
        for (int i = 0; i < inventoryUISlots.Count; i++)
        {
            inventoryUISlots[i].ShowItemOptions(false);
        }
    }

    public void ChangeInventorySlotContents(int slot1, int slot2)
    {
        InventoryItem item1 = new InventoryItem();
        InventoryItem item2 = new InventoryItem();
        item1.itemName = PlayerController.instance.inventory[slot1].itemName;
        item1.itemDesc = PlayerController.instance.inventory[slot1].itemDesc;
        item1.type = PlayerController.instance.inventory[slot1].type;
        item1.ammount = PlayerController.instance.inventory[slot1].ammount;
        item2.itemName = PlayerController.instance.inventory[slot2].itemName;
        item2.itemDesc = PlayerController.instance.inventory[slot2].itemDesc;
        item2.type = PlayerController.instance.inventory[slot2].type;
        item2.ammount = PlayerController.instance.inventory[slot2].ammount;

        PlayerController.instance.inventory[slot1] = item2;
        PlayerController.instance.inventory[slot2] = item1;
        PopulateInventory();
        if (item1.type == CollectableType.Weapon || item2.type == CollectableType.Weapon) PlayerController.instance.ForceUnequip();
    }

    public void CloseInventory()
    {
        inGame.SetActive(true);
        header.SetActive(false);
        inventory.SetActive(false);
    }

    //Box Management
    public void OpenBox()
    {
        PopulateInventory();
        inGame.SetActive(false);
        header.SetActive(true);
        itemPreview.SetActive(false);
        boxPanel.SetActive(true);
        inventory.SetActive(true);
    }

    public void PopulateBox()
    {
        for (int i = 0; i < boxUISlots.Count; i++)
        {
            boxUISlots[i].FillInventorySlot(GameManager.instance.boxItems[i], gui[(int)GameManager.instance.boxItems[i].type]);
        }
    }

    public void ChangeBoxSlotContents(int slot1, int slot2)
    {
        InventoryItem item1 = new InventoryItem();
        InventoryItem item2 = new InventoryItem();
        item1.itemName = GameManager.instance.boxItems[slot1].itemName;
        item1.itemDesc = GameManager.instance.boxItems[slot1].itemDesc;
        item1.type = GameManager.instance.boxItems[slot1].type;
        item1.ammount = GameManager.instance.boxItems[slot1].ammount;
        item2.itemName = GameManager.instance.boxItems[slot2].itemName;
        item2.itemDesc = GameManager.instance.boxItems[slot2].itemDesc;
        item2.type = GameManager.instance.boxItems[slot2].type;
        item2.ammount = GameManager.instance.boxItems[slot2].ammount;

        GameManager.instance.boxItems[slot1] = item2;
        GameManager.instance.boxItems[slot2] = item1;
        PopulateBox();
        if (item1.type == CollectableType.Weapon || item2.type == CollectableType.Weapon) PlayerController.instance.ForceUnequip();
    }

    public void ChangeBoxAndInventoryContents(int slot1, int slot2, bool isAInventory)
    {
        InventoryItem item1 = new InventoryItem();
        InventoryItem item2 = new InventoryItem();
        item1.itemName = isAInventory ? PlayerController.instance.inventory[slot1].itemName : GameManager.instance.boxItems[slot1].itemName;
        item1.itemDesc = isAInventory ? PlayerController.instance.inventory[slot1].itemDesc : GameManager.instance.boxItems[slot1].itemDesc;
        item1.type = isAInventory ? PlayerController.instance.inventory[slot1].type : GameManager.instance.boxItems[slot1].type;
        item1.ammount = isAInventory ? PlayerController.instance.inventory[slot1].ammount : GameManager.instance.boxItems[slot1].ammount;
        item2.itemName = isAInventory ? GameManager.instance.boxItems[slot2].itemName : PlayerController.instance.inventory[slot2].itemName;
        item2.itemDesc = isAInventory ? GameManager.instance.boxItems[slot2].itemDesc : PlayerController.instance.inventory[slot2].itemDesc;
        item2.type = isAInventory ? GameManager.instance.boxItems[slot2].type : PlayerController.instance.inventory[slot2].type;
        item2.ammount = isAInventory ? GameManager.instance.boxItems[slot2].ammount : PlayerController.instance.inventory[slot2].ammount;

        if (isAInventory)
        {
            PlayerController.instance.inventory[slot1] = item2;
            GameManager.instance.boxItems[slot2] = item1;
        }
        else
        {
            GameManager.instance.boxItems[slot1] = item2;
            PlayerController.instance.inventory[slot2] = item1;
        }
        
        PopulateInventory();
        PopulateBox();
        if (item1.type == CollectableType.Weapon || item2.type == CollectableType.Weapon) PlayerController.instance.ForceUnequip();
    }
}
