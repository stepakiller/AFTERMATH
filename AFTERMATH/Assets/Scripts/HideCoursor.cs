using UnityEngine;

public class HideCoursor : MonoBehaviour
{
    [SerializeField] bool playOnAwake;
    void Awake()
    {
        if(playOnAwake) HideAndLockCoursor();
    }
    public void HideAndLockCoursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
