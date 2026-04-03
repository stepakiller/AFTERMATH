using UnityEngine;
using UnityEngine.UI;
public class ExitButtonLogic : MonoBehaviour
{
    [SerializeField] Button exitButton;
    void Start() => exitButton.onClick.AddListener(CloseGame);
    void CloseGame() => Application.Quit();
}
