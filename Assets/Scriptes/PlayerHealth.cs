using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Vie du joueur (3 cœurs) et flacon de fée
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int max_health = 3;

    private int current_health;
    private bool has_fairy;

    public UnityEvent<int> OnHealthChanged;   // Envoie le nombre de cœurs restants
    public UnityEvent<bool> OnBottleChanged;  // Envoie true si le flacon contient une fée

    public int Health { get { return current_health; } }
    public bool HasFairy { get { return has_fairy; } }

    void Start()
    {
        // Le joueur commence avec tous ses points de vie et un flacon vide
        current_health = max_health;
        has_fairy = false;

        // Met à jour l'interface au démarrage
        OnHealthChanged?.Invoke(current_health);
        OnBottleChanged?.Invoke(has_fairy);
    }

    public void TakeDamage(int amount)
    {
        // Ignore les dégâts si le joueur est déjà mort
        if (current_health <= 0) return;

        current_health -= amount;

        if (current_health <= 0)
        {
            if (has_fairy)
            {
                // La fée du flacon est consommée et rend tous les points de vie
                has_fairy = false;
                current_health = max_health;
                OnBottleChanged?.Invoke(has_fairy);
            }
            else
            {
                // Plus de points de vie et pas de fée : le joueur meurt
                current_health = 0;
                OnHealthChanged?.Invoke(current_health);
                Die();
                return;
            }
        }

        OnHealthChanged?.Invoke(current_health);
    }

    // Met une fée dans le flacon. Retourne false si le flacon est déjà plein
    public bool CaptureFairy()
    {
        if (has_fairy) return false;

        has_fairy = true;
        OnBottleChanged?.Invoke(has_fairy);
        return true;
    }

    private void Die()
    {
        // Recommence la scène
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
