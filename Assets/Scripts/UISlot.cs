using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISlot : Selectable
{
    [Header("Item Slot")]
    public Image icon;
    public Text value;
    public Text equip;

    public void FillInventorySlot(InventoryItem item)
    {
        icon.sprite = item.sprite;
        if (item.type == CollectableType.PistolAmmo || item.type == CollectableType.ShotgunAmmo) value.text = item.ammount.ToString();
        else value.text = string.Empty;
    }

    public void EquipItem()
    {
        equip.text = "Equipped";
    }
}
