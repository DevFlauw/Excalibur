using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/*
using UnityEngine;

// Affiche les cœurs de vie du joueur : un GameObject par cœur
public class HeartsUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts; // Glisse ici les 3 cœurs, dans l'ordre

    // À brancher sur PlayerHealth.OnHealthChanged
    public void SetHealth(int value)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            // Un cœur est affiché tant que son index est inférieur à la vie restante
            hearts[i].SetActive(i < value);
        }
    }
}
*/
