using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UI_InspectController : MonoBehaviour
{
    [SerializeField] Button openInspect;
    [SerializeField] Button closeInspect;
    [SerializeField] Camera inspectCamera;
    [Header("Тексты интерфейса")]
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text itemDescriptionText;
    [Header("UI Панели (RectTransform)")]
    [SerializeField] RectTransform canvasRect;
    [SerializeField] RectTransform inventoryMenu;
    [SerializeField] RectTransform inspectMenu;

    [Header("Настройки анимации")]
    [SerializeField] float animationDuration = 0.5f;
    [SerializeField] Ease animationEase = Ease.OutBack;

    [Header("Логика 3D Осмотра")]
    [SerializeField] UI_InspectDragArea dragArea;
    [SerializeField] Transform inspectSpawnPoint;

    float centerPosX = 0f;
    float leftHiddenPosX;
    float rightHiddenPosX;
    Dictionary<GameObject, GameObject> inspectionPool = new Dictionary<GameObject, GameObject>();
    GameObject spawnedInspectObject;

    void Awake() => Bootstrapper.InspectController = this;
    void Start()
    {
        openInspect.onClick.AddListener(OpenInspect);
        closeInspect.onClick.AddListener(CloseInspect);
        float width = canvasRect.rect.width;
        leftHiddenPosX = -width;
        rightHiddenPosX = width;
        inventoryMenu.anchoredPosition = new Vector2(centerPosX, 0);
        inspectMenu.anchoredPosition = new Vector2(rightHiddenPosX, 0);
        if (inspectCamera != null) inspectCamera.gameObject.SetActive(false);
        UpdateInspectButtonVisibility();
    }

    public void OpenInspect()
    {
        if (Bootstrapper.RadialInventory == null) return;

        ItemSettings itemToInspect = Bootstrapper.RadialInventory.GetCurrentFrontItem();
        if (itemToInspect == null || itemToInspect.ItemData.Prefab == null) return;

        if (itemNameText != null) itemNameText.text = itemToInspect.ItemData.ItemName;
            
        if (itemDescriptionText != null) itemDescriptionText.text = itemToInspect.ItemData.ItemDescription;

        inventoryMenu.DOAnchorPosX(leftHiddenPosX, animationDuration).SetEase(animationEase).SetUpdate(true);
        inspectMenu.DOAnchorPosX(centerPosX, animationDuration).SetEase(animationEase).SetUpdate(true);

        if (inspectCamera != null) inspectCamera.gameObject.SetActive(true);

        GameObject prefab = itemToInspect.ItemData.Prefab;

        if (inspectionPool.TryGetValue(prefab, out GameObject pooledObject))
        {
            spawnedInspectObject = pooledObject;
            spawnedInspectObject.SetActive(true);
            spawnedInspectObject.transform.localRotation = Quaternion.identity; 
        }
        else
        {
            spawnedInspectObject = Instantiate(prefab, inspectSpawnPoint);
            spawnedInspectObject.transform.localPosition = Vector3.zero;
            spawnedInspectObject.transform.localScale = Vector3.one * itemToInspect.ItemData.InspectScaleMultiplier;
            
            if (spawnedInspectObject.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;
            if (spawnedInspectObject.TryGetComponent(out Collider col)) col.enabled = false;

            int inspectLayer = LayerMask.NameToLayer("InspectItem");
            if (inspectLayer != -1) SetLayerRecursively(spawnedInspectObject, inspectLayer);

            inspectionPool.Add(prefab, spawnedInspectObject);
        }

        dragArea.objectToRotate = spawnedInspectObject.transform;
    }

    public void CloseInspect()
    {
        inventoryMenu.DOAnchorPosX(centerPosX, animationDuration).SetEase(animationEase).SetUpdate(true);
        inspectMenu.DOAnchorPosX(rightHiddenPosX, animationDuration).SetEase(animationEase).SetUpdate(true);

        if (spawnedInspectObject != null)
        {
            spawnedInspectObject.SetActive(false); 
            dragArea.objectToRotate = null;
            spawnedInspectObject = null;
        }
        if (inspectCamera != null) inspectCamera.gameObject.SetActive(false);
    }
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        
        obj.layer = newLayer;
        foreach (Transform child in obj.transform) SetLayerRecursively(child.gameObject, newLayer);
    }

    public void UpdateInspectButtonVisibility()
    {
        if (Bootstrapper.RadialInventory == null || openInspect == null) return;
        ItemSettings currentItem = Bootstrapper.RadialInventory.GetCurrentFrontItem();
        openInspect.gameObject.SetActive(currentItem != null);
    }

    void OnDestroy()
    {
        if (Bootstrapper.InspectController == this) Bootstrapper.InspectController = null;
    }
}