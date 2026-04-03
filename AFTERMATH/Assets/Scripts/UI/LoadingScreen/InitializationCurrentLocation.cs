using System;
using UnityEngine;

public class InitializationCurrentLocation : MonoBehaviour
{
    [SerializeField] GameObject[] locations;
    int currentLocation;
    void Awake()
    {
        currentLocation = PlayerPrefs.GetInt("currentLocation", 0);
        for (int i = 0; i < locations.Length; i++) locations[i].SetActive(i == currentLocation);
    }
}