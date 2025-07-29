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

    [Header("Inventory References")]
    [SerializeField] private UISlot[] uiSlots;
    [SerializeField] private TMPro.TextMeshProUGUI itemName;
    [SerializeField] private TMPro.TextMeshProUGUI itemDesc;

    public static UIManager instance;

    private void Start()
    {
        instance = this;
        StartCoroutine(Fade(false));
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
        blackScreen.SetActive(false);
    }

    //Inventory Management
    public void OpenInventory()
    {
        PopulateInventory();
        inGame.SetActive(false);
        header.SetActive(true);
        inventory.SetActive(true);
    }

    private void PopulateInventory()
    {
        uiSlots[0].Select();
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].FillInventorySlot(PlayerController.instance.inventory[i]);
        }
    }

    public void ShowItemInformation(InventoryItem item)
    {
        itemName.text = item.itemName;
        itemDesc.text = item.itemDesc;
        //Mudar mesh do proxy
    }

    public void CloseInventory()
    {
        inGame.SetActive(true);
        header.SetActive(false);
        inventory.SetActive(false);
    }
}
