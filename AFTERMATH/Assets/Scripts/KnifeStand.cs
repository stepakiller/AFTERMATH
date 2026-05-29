using UnityEngine;
using UnityEngine.Events;

public class KnifeStand : MonoBehaviour, Interactable
{
    [SerializeField] GameObject[] _standKnifeModels;
    [SerializeField] ItemData _requiredKnifeData; 
    [SerializeField] GameObject[] disableObjects;
    [SerializeField] GameObject[] enableObjects;

    int _placedKnivesCount = 0;

    public void Interact()
    {
        if (_placedKnivesCount >= _standKnifeModels.Length) return;
        ItemSettings currentItem = Bootstrapper.Inventory.CurrentItem;
        if (currentItem != null && currentItem.ItemData == _requiredKnifeData) PlaceKnife(currentItem);
    }

    void PlaceKnife(ItemSettings knifeItem)
    {
        _standKnifeModels[_placedKnivesCount].SetActive(true);
        _placedKnivesCount++;
        Bootstrapper.HotbarManager.RemoveCurrentItem();
        Bootstrapper.Inventory.HideObjectInPocket();
        Destroy(knifeItem.gameObject);
        if (_placedKnivesCount >= _standKnifeModels.Length)
        {
            Debug.Log("<color=green>Все ножи на месте! Запускаем событие.</color>");
            for (int i = 0; i < disableObjects.Length; i++) disableObjects[i].SetActive(false);
            for (int i = 0; i < enableObjects.Length; i++) enableObjects[i].SetActive(true);
        }
    }
}
