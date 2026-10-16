using UnityEngine;
using UnityEngine.UI;


public class BottleUI : MonoBehaviour
{
    [Header("Option A : changer le sprite du flacon")]
    [SerializeField] private Image bottle_image;
    [SerializeField] private Sprite empty_sprite; // Flacon vide
    [SerializeField] private Sprite full_sprite;  // Flacon avec la fée

    [Header("Option B : afficher / cacher la fée dans le flacon")]
    [SerializeField] private GameObject fairy_icon; // Image de la fée, enfant du flacon

    
    public void SetBottle(bool has_fairy)
    {
       
        if (bottle_image != null && empty_sprite != null && full_sprite != null)
        {
            bottle_image.sprite = has_fairy ? full_sprite : empty_sprite;
        }

        
        if (fairy_icon != null)
        {
            fairy_icon.SetActive(has_fairy);
        }
    }
}
