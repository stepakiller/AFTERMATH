using UnityEngine;

public class ItemSettings : MonoBehaviour
{
    public ItemData ItemData;
    public Rigidbody Rb;
    public Collider Col;
    public Vector3 HoldPositionOffset;
    public Vector3 HoldRotationOffset;
    public float HandScale = 1f;

    Equippable _cachedEquippable;
    bool _isEquippableSearched = false;

    public Equippable EquippableComponent 
    {
        get 
        { 
            if (!_isEquippableSearched) 
            {
                _cachedEquippable = GetComponentInChildren<Equippable>(true);
                _isEquippableSearched = true;
            }
            return _cachedEquippable; 
        }
    }
}