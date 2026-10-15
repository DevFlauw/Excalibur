using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public GameObject GameSceneButton;

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Scene1");
    }
}
