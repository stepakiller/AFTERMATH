using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [HideInInspector] public string id;
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
    public float inspectScaleMultiplier = 1f;
}
