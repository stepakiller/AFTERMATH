using UnityEngine;

public static class Bootstrapper
{
    static Inventory _inventory;
    static Transform _playerTransform;
    static HotbarManager _hotbarManager;

    public static Inventory Inventory 
    {
        get
        {
            if (_inventory == null)
            {
                _inventory = Object.FindFirstObjectByType<Inventory>();
                if (_inventory == null) Debug.LogError("На сцене не найден скрипт Inventory!");
            }
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
                else Debug.LogError("На сцене не найден объект с тегом Player!");
            }
            return _playerTransform;
        }
        set => _playerTransform = value;
    }

    public static HotbarManager HotbarManager
    {
        get
        {
            if (_hotbarManager == null)
            {
                _hotbarManager = Object.FindFirstObjectByType<HotbarManager>();
                if (_hotbarManager == null) Debug.LogError("На сцене не найден скрипт HotbarManager!");
            }
            return _hotbarManager;
        }
        set => _hotbarManager = value;
    }

    public static void Reset() 
    {
        _inventory = null;
        _playerTransform = null;
        _hotbarManager = null;
    }
}