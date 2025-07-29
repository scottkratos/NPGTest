using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private UISlot originalParent;
    private RectTransform rt;
    private Image image;
    [HideInInspector] public int dragIndex;

    private void Start()
    {
        originalParent = transform.GetComponentInParent<UISlot>();
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (originalParent.currentItem.type == CollectableType.None) return;
        transform.SetParent(originalParent.transform.parent.parent);
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (originalParent.currentItem.type == CollectableType.None) return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent.transform);
        rt.anchoredPosition = Vector2.zero;
        image.raycastTarget = true;
    }
}
