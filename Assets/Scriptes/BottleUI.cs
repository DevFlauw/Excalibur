using UnityEngine;
using UnityEngine.UI;

// Affiche l'état du flacon : avec une fée dedans ou vide
public class BottleUI : MonoBehaviour
{
    [Header("Option A : changer le sprite du flacon")]
    [SerializeField] private Image bottle_image;
    [SerializeField] private Sprite empty_sprite; // Flacon vide
    [SerializeField] private Sprite full_sprite;  // Flacon avec la fée

    [Header("Option B : afficher / cacher la fée dans le flacon")]
    [SerializeField] private GameObject fairy_icon; // Image de la fée, enfant du flacon

    // Le flacon est toujours vide au lancement du jeu
    void Awake()
    {
        SetBottle(false);
    }

    // À brancher sur PlayerHealth.OnBottleChanged
    public void SetBottle(bool has_fairy)
    {
        // Option A : échange le sprite si les deux sprites sont assignés
        if (bottle_image != null && empty_sprite != null && full_sprite != null)
        {
            bottle_image.sprite = has_fairy ? full_sprite : empty_sprite;
        }

        // Option B : montre la fée uniquement quand le flacon est plein
        if (fairy_icon != null)
        {
            fairy_icon.SetActive(has_fairy);
        }
    }
}
