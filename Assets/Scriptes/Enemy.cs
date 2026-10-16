using UnityEngine;

// Ennemi qui tire des boules de feu 
public class Enemy : MonoBehaviour
{
    [SerializeField] private int max_health = 2;
    [SerializeField] private float fire_interval = 2f;
    [SerializeField] private float detection_range = 8f; // Distance à partir de laquelle l'ennemi tire
    [SerializeField] private GameObject fireball_prefab;
    [SerializeField] private Transform fire_point; // Point d'où part la boule de feu (ex : AttachPoint). Si vide, elle part du centre de l'ennemi
    [SerializeField] private GameObject death_fx;

    private int current_health;
    private float fire_timer;
    private Transform target;
    Healthbar healthbar;

    void Start()
    {
        current_health = max_health;
        fire_timer = fire_interval;
        healthbar = GetComponent<Healthbar>();
        healthbar.UpdateLifeBar(current_health, max_health);
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
        // Message si le prefab no assigné 
        if (fireball_prefab == null)
        {
            Debug.LogWarning("Enemy : Fireball Prefab est vide ou détruit. Assigne le prefab du Project.", this);
            return;
        }
        GameObject fireball = Instantiate(fireball_prefab, transform.position, Quaternion.identity);
        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
        fireball.GetComponent<Fireball>().Init(dir);
    }

    // l'épée du joueur
    public void TakeDamage(int amount)
    {
        current_health -= amount;

        healthbar.UpdateLifeBar(current_health,max_health);
        if (current_health <= 0)
        {
            Die();
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