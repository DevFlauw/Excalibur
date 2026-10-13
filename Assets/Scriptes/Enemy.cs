using UnityEngine;

// Ennemi statique avec 2 points de vie qui tire des boules de feu vers le joueur
public class Enemy : MonoBehaviour
{
    [SerializeField] private int max_health = 2;
    [SerializeField] private float fire_interval = 2f;
    [SerializeField] private float detection_range = 8f; // Distance à partir de laquelle l'ennemi tire
    [SerializeField] private Fireball fireball_prefab;
    [SerializeField] private GameObject death_fx;

    private int current_health;
    private float fire_timer;
    private Transform target;

    void Start()
    {
        current_health = max_health;
        fire_timer = fire_interval;

        // Trouve le joueur dans la scène
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player != null)
        {
            target = player.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        // L'ennemi ne tire que si le joueur est assez proche
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
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;

        Fireball fireball = Instantiate(fireball_prefab, transform.position, Quaternion.identity);
        fireball.Init(dir);
    }

    // Appelée par l'épée du joueur
    public void TakeDamage(int amount)
    {
        current_health -= amount;

        if (current_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Si le prefab fx de mort existe, spawn une instance
        if (death_fx)
        {
            Instantiate(death_fx, transform.position, Quaternion.identity);
        }

        // Supprime l'ennemi
        Destroy(gameObject);
    }

    // Affiche la portée de détection dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detection_range);
    }
}
