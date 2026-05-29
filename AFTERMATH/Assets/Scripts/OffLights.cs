using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffLights : MonoBehaviour
{
    [SerializeField] List<GameObject> targetObjects = new List<GameObject>();
    [SerializeField] float delayInSeconds = 2.0f;

    void Start() => StartCoroutine(DisableObjectsRoutine());

    IEnumerator DisableObjectsRoutine()
    {
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
            {
                yield return new WaitForSeconds(delayInSeconds);
                obj.SetActive(false);
            }
        }
    }
}