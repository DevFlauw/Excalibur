using UnityEngine;

// Ennemi qui tire des boules de feu, avec une barre de vie
public class Enemy : MonoBehaviour
{
    [SerializeField] private int max_health = 2;
    [SerializeField] private float fire_interval = 2f;
    [SerializeField] private float detection_range = 8f; // Distance à partir de laquelle l'ennemi tire
    [SerializeField] private GameObject fireball_prefab;
    [SerializeField] private Transform fire_point; // Point d'où part la boule de feu (ex : AttachPoint). Si vide, elle part du centre de l'ennemi
    [SerializeField] private GameObject death_fx;

    [Header("Barre de vie")]
    [SerializeField] private Healthbar healthbar; // Si vide, le script la cherche tout seul dans les enfants de l'ennemi

    private int current_health;
    private float fire_timer;
    private Transform target;

    void Start()
    {
        current_health = max_health;
        fire_timer = fire_interval;

        // Si la barre n'est pas assignée dans l'Inspector, on la cherche dans les enfants
        if (healthbar == null)
        {
            healthbar = GetComponentInChildren<Healthbar>();
        }

        if (healthbar == null)
        {
            Debug.LogWarning("Enemy : aucune Healthbar trouvée sur '" + name + "'. Ajoute une barre de vie en enfant de l'ennemi ou assigne-la dans l'Inspector.", this);
        }

        UpdateHealthbar();

        // le joueur dans la scène
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        if (Vector2.Distance(transform.position, target.position) > detection_range) return;

        fire_timer -= Time.deltaTime;

        if (fire_timer <= 0f)
        {
            fire_timer = fire_interval;
            Shoot();
        }
    }

    // Tire une boule de feu en direction du joueur
    private void Shoot()
    {
        // Message si le prefab n'est pas assigné
        if (fireball_prefab == null)
        {
            Debug.LogWarning("Enemy : Fireball Prefab est vide ou détruit. Assigne le prefab du Project.", this);
            return;
        }

        // La boule part du fire_point s'il existe, sinon du centre de l'ennemi
        Vector2 origin = fire_point != null ? (Vector2)fire_point.position : (Vector2)transform.position;

        GameObject fireball = Instantiate(fireball_prefab, origin, Quaternion.identity);
        Vector2 dir = ((Vector2)target.position - origin).normalized;
        fireball.GetComponent<Fireball>().Init(dir);
    }

    // l'épée du joueur
    public void TakeDamage(int amount)
    {
        current_health -= amount;
        if (current_health < 0) current_health = 0;

        UpdateHealthbar();

        if (current_health <= 0)
        {
            Die();
        }
    }

    // Met à jour la barre de vie (ne fait rien si elle n'existe pas)
    private void UpdateHealthbar()
    {
        if (healthbar != null)
        {
            healthbar.UpdateLifeBar(current_health, max_health);
        }
    }

    private void Die()
    {
        // le prefab 
        if (death_fx)
        {
            Instantiate(death_fx, transform.position, Quaternion.identity);
        }

        // Supprime l'ennemi
        Destroy(gameObject);
    }

    // Affiche la portée
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detection_range);
    }
}
