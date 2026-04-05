using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_InspectController : MonoBehaviour
{
    [SerializeField] Button openInspect;
    [SerializeField] Button closeInspect;
    [SerializeField] Camera inspectCamera;
    [Header("UI Панели (RectTransform)")]
    [SerializeField] RectTransform canvasRect;
    [SerializeField] RectTransform inventoryMenu;
    [SerializeField] RectTransform inspectMenu;

    [Header("Настройки анимации")]
    [SerializeField] float animationDuration = 0.5f;
    [SerializeField] Ease animationEase = Ease.OutBack;

    [Header("Логика 3D Осмотра")]
    [SerializeField] UI_RadialInventory radialInventory;
    [SerializeField] UI_InspectDragArea dragArea;
    [SerializeField] Transform inspectSpawnPoint;

    private float centerPosX = 0f;
    private float leftHiddenPosX;
    private float rightHiddenPosX;
    private Dictionary<GameObject, GameObject> inspectionPool = new Dictionary<GameObject, GameObject>();
    private GameObject spawnedInspectObject;

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
    }

    public void OpenInspect()
    {
        ItemSettings itemToInspect = radialInventory.GetCurrentFrontItem();
        if (itemToInspect == null || itemToInspect.ItemData.prefab == null) return;

        inventoryMenu.DOAnchorPosX(leftHiddenPosX, animationDuration).SetEase(animationEase).SetUpdate(true);
        inspectMenu.DOAnchorPosX(centerPosX, animationDuration).SetEase(animationEase).SetUpdate(true);

        if (inspectCamera != null) inspectCamera.gameObject.SetActive(true);

        GameObject prefab = itemToInspect.ItemData.prefab;

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
            spawnedInspectObject.transform.localScale = Vector3.one * itemToInspect.ItemData.inspectScaleMultiplier;
            
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
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}