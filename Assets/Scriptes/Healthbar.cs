using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    Enemy enemy;
    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
    }


    public void UpdateLifeBar(float health,float healthMax)
    {
        slider.maxValue = healthMax;
        slider.value = health;
    }
}
