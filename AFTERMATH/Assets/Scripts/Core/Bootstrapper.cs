using UnityEngine;

public static class Bootstrapper
{
    public static Inventory Inventory { get; set; }
    public static Transform PlayerTransform { get; set; }
    public static HotbarManager HotbarManager { get; set; }
    public static UI_RadialInventory RadialInventory { get; set; }
    public static UI_InspectController InspectController { get; set; }

    public static void Reset() 
    {
        Inventory = null;
        PlayerTransform = null;
        HotbarManager = null;
        RadialInventory = null;
        InspectController = null;
    }
}