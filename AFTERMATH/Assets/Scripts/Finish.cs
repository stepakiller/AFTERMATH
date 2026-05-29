using UnityEngine;
using UnityEngine.SceneManagement;
public class Finish : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") SceneManager.LoadScene(0);
    }
}
