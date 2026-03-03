using UnityEngine;

public class ItemSettings : MonoBehaviour
{
    [field: SerializeField] public ItemData ItemData { get; private set; }
    [field: SerializeField] public Vector3 HoldPositionOffset { get; private set; }
    [field: SerializeField] public Vector3 HoldRotationOffset { get; private set; }
    [field: SerializeField] public float HandScale { get; private set; }
}
