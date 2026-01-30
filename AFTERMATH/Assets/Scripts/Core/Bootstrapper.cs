using UnityEngine;

public static class Bootstrapper
{
    static Inventory _inventory;
    static Transform _playerTransform;
    
    public static Inventory Inventory 
    {
        get
        {
            if (_inventory == null) _inventory = Object.FindFirstObjectByType<Inventory>();
            else Debug.LogError("На сцене не найден скрипт Inventory");
            return _inventory;
        }
        set => _inventory = value;
    }
    public static Transform PlayerTransform
    {
        get
        {
            if (_playerTransform == null)
            {
                var playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) _playerTransform = playerObj.transform;
                else Debug.LogError("На сцене не найден Player");
            }
            return _playerTransform;
        }
        set => _playerTransform = value;
    }
    
    public static void Reset() 
    {
        _inventory = null;
        _playerTransform = null;
    }
}
