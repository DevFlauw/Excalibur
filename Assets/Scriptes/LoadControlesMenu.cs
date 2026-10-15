using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlesMENU : MonoBehaviour
{
    public GameObject ControlesButton;

    public void LoadControlesMenu()
    {
        SceneManager.LoadSceneAsync("MenuControles");
    }
}

