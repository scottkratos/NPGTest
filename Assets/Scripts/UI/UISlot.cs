using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UISlot : Selectable, IPointerClickHandler, IDropHandler
{
    [Header("References")]
    [SerializeField] private GameObject slotOption;

    [Header("Item Slot")]
    public Image icon;
    public TMPro.TextMeshProUGUI value;
    public TMPro.TextMeshProUGUI equip;
    private int slotIndex;
    [HideInInspector] public InventoryItem currentItem;
    public bool isPlayerInventory;

    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).gameObject == gameObject)
            {
                slotIndex = i;
                break;
            }
        }
        GetComponentInChildren<UIDraggable>().dragIndex = slotIndex;
    }

    public void FillInventorySlot(InventoryItem item)
    {
        currentItem = item;
        icon.sprite = item.sprite;
        if (item.type == CollectableType.PistolAmmo) value.text = item.ammount.ToString();
        else value.text = string.Empty;
    }

    public void UseItem()
    {
        switch (currentItem.type)
        {
            case CollectableType.Heal:
                PlayerController.instance.Heal();
                PlayerController.instance.inventory[slotIndex] = new InventoryItem();
                UIManager.instance.HideItemOptions();
                UIManager.instance.PopulateInventory();
                break;
            case CollectableType.KeyItem:
                //Check key item usage
                break;
            case CollectableType.PistolAmmo:
                //Send error message
                break;
            case CollectableType.Weapon:
                PlayerController.instance.EquipWeapon();
                UIManager.instance.HideItemOptions();
                //Show equipped feedback
                break;
        }
    }

    public void ShowItemOptions(bool show)
    {
        slotOption.SetActive(show);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.instance.HideItemOptions();
        if (!isPlayerInventory) return;
        if (currentItem.type != CollectableType.None) ShowItemOptions(!slotOption.activeSelf);
    }

    public void OnDrop(PointerEventData eventData)
    {
        UIDraggable drag = eventData.pointerDrag.GetComponent<UIDraggable>();
        if (drag == null) return;

        if (drag.isInventory == isPlayerInventory)
        {
            if (drag.dragIndex == slotIndex) return;
            if (isPlayerInventory) UIManager.instance.ChangeInventorySlotContents(drag.dragIndex, slotIndex);
            else UIManager.instance.ChangeBoxSlotContents(drag.dragIndex, slotIndex);
        }
        else UIManager.instance.ChangeBoxAndInventoryContents(drag.dragIndex, slotIndex, drag.isInventory);

        UIManager.instance.ShowItemInformation(currentItem);
        //Hide equip feedback;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        UIManager.instance.ShowItemInformation(currentItem);
    }
}