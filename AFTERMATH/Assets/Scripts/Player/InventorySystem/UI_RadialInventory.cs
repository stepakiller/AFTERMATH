using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class UI_RadialInventory : MonoBehaviour
{
    private struct SlotReference
    {
        public GameObject Root;
        public RectTransform Rect;
        public UI_DraggableItem DragItem;
        public CanvasGroup Group;
    }

    [Header("Настройки круга")]
    public float radius = 200f;
    public GameObject radialSlotPrefab;
    public Transform centerPoint;
    
    [Header("Настройки карусели")]
    public float rotationDuration = 0.25f;
    public Ease rotationEase = Ease.InOutQuad;

    private List<SlotReference> spawnedSlots = new List<SlotReference>();
    private float currentAngleOffset = 0f;
    private float targetAngleOffset = 0f;
    private int currentFrontIndex = 0; 

    void Start() => Bootstrapper.HotbarManager.OnInventoryChanged += UpdateRadialUI;
    
    void OnDestroy()
    {
        if (Bootstrapper.HotbarManager != null) 
            Bootstrapper.HotbarManager.OnInventoryChanged -= UpdateRadialUI;
    }

    public void RotateLeft()
    {
        if (spawnedSlots.Count <= 1) return;
        targetAngleOffset += 360f / spawnedSlots.Count;
        currentFrontIndex = (currentFrontIndex - 1 + spawnedSlots.Count) % spawnedSlots.Count;
        
        AnimateRotation();
        UpdateInteractability();
    }

    public void RotateRight()
    {
        if (spawnedSlots.Count <= 1) return;
        targetAngleOffset -= 360f / spawnedSlots.Count;
        currentFrontIndex = (currentFrontIndex + 1) % spawnedSlots.Count;
        
        AnimateRotation();
        UpdateInteractability();
    }

    private void AnimateRotation()
    {
        DOTween.Kill(this);
        DOTween.To(() => currentAngleOffset, x => {
            currentAngleOffset = x;
            RefreshPositions();
        }, targetAngleOffset, rotationDuration)
        .SetEase(rotationEase)
        .SetUpdate(true)
        .SetId(this);
    }

    public void UpdateRadialUI()
    {
        Bootstrapper.HotbarManager.radialItems.RemoveAll(item => item == null);
        
        foreach (var slot in spawnedSlots) Destroy(slot.Root);
        spawnedSlots.Clear();

        List<ItemSettings> items = Bootstrapper.HotbarManager.radialItems;
        if (items.Count == 0) return;

        currentFrontIndex = 0;
        currentAngleOffset = 0f;
        targetAngleOffset = 0f;

        for (int i = 0; i < items.Count; i++)
        {
            GameObject obj = Instantiate(radialSlotPrefab, centerPoint);
            
            SlotReference reference = new SlotReference
            {
                Root = obj,
                Rect = obj.GetComponent<RectTransform>(),
                DragItem = obj.GetComponentInChildren<UI_DraggableItem>(),
                Group = obj.GetComponentInChildren<CanvasGroup>()
            };

            UI_Slot slotLogic = obj.GetComponent<UI_Slot>();
            if (slotLogic != null) { slotLogic.slotIndex = i; slotLogic.containerType = ItemContainer.Radial; }

            if (reference.DragItem != null)
            {
                reference.DragItem.containerType = ItemContainer.Radial;
                reference.DragItem.Setup(items[i].ItemData, i);
            }

            spawnedSlots.Add(reference);
        }

        RefreshPositions();
        UpdateInteractability();
    }

    private void RefreshPositions()
    {
        int count = spawnedSlots.Count;
        if (count == 0) return;

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = (-90f + (i * angleStep) + currentAngleOffset) * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
            
            // Используем закэшированный RectTransform
            spawnedSlots[i].Rect.localPosition = pos;
            spawnedSlots[i].Root.transform.rotation = centerPoint.parent.rotation;
        }
        List<SlotReference> sorted = new List<SlotReference>(spawnedSlots);
        sorted.Sort((a, b) => b.Rect.localPosition.y.CompareTo(a.Rect.localPosition.y));
        for (int i = 0; i < sorted.Count; i++) sorted[i].Root.transform.SetSiblingIndex(i);
    }

    private void UpdateInteractability()
    {
        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            var slot = spawnedSlots[i];
            if (slot.Group == null) continue;

            bool isFront = (i == currentFrontIndex);
            slot.Group.blocksRaycasts = isFront;
            slot.Group.alpha = isFront ? 1f : 0.4f; 
            slot.Root.transform.localScale = isFront ? Vector3.one : Vector3.one * 0.8f;
        }
    }

    public ItemSettings GetCurrentFrontItem()
    {
        var radialItems = Bootstrapper.HotbarManager.radialItems;
        if (radialItems.Count == 0) return null;
        return radialItems[currentFrontIndex];
    }
}