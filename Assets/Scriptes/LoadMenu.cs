using UnityEngine;
using UnityEngine.SceneManagement;

public class MENU : MonoBehaviour
{
    public GameObject MENUButton;

    public void LoadMENU()
    {
        SceneManager.LoadSceneAsync("MENU");
    }
}
