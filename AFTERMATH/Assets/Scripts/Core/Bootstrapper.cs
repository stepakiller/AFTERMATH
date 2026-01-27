using UnityEngine;

public static class Bootstrapper
{
    private static Inventory _inventory;
    
    public static Inventory Inventory 
    {
        get
        {
            if (_inventory == null)
                _inventory = Object.FindFirstObjectByType<Inventory>();
            return _inventory;
        }
        set => _inventory = value;
    }
    
    public static void Reset() => _inventory = null;
}
