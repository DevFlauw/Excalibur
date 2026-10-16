using UnityEngine;

// Boule de feu tirée par l'ennemi. Nécessite un Rigidbody2D et un Collider2D en "Is Trigger".
[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask obstacle_layer; // Murs et obstacles qui arrêtent la boule de feu
    [SerializeField] private GameObject impact_fx;

    void Start()
    {
        // Détruit la boule de feu après un certain temps si elle n'a rien touché
        Destroy(gameObject, lifetime);
    }

    // Donne la direction de la boule de feu
    public void Init(Vector2 direction)
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.linearVelocity = direction.normalized * speed;

        // Tourne le sprite dans la direction du tir
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            PlayerCombat combat = player.GetComponent<PlayerCombat>();

            if (combat != null && combat.IsShielding)
            {
                // Le joueur a son bouclier levé : le projectile est bloqué, aucun dégât
            }
            else
            {
                // Sinon le joueur perd un point de vie
                player.TakeDamage(damage);
            }

            Impact();
            return;
        }

        // Touche un mur ou un obstacle
        if (((1 << collision.gameObject.layer) & obstacle_layer) != 0)
        {
            Impact();
        }
    }
    private void Impact()
    {
        // Si le prefab fx d'impact existe, spawn une instance
        if (impact_fx)
        {
            Instantiate(impact_fx, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
