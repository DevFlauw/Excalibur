
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Épée (J), bouclier (K) et flacon (U) du joueur
public class PlayerCombat : MonoBehaviour
{
    [Header("Épée")]
    [SerializeField] private int sword_damage = 1;
    [SerializeField] private float sword_cooldown = 0.35f;
    [SerializeField] private Vector2 sword_size = new Vector2(1f, 1.2f); // x = portée devant le joueur, y = largeur
    [SerializeField] private float sword_distance = 0.8f; // Distance entre le joueur et le centre de la zone

    [Header("Bouclier")]
    [SerializeField] private float shield_speed_multiplier = 0.5f; // Vitesse divisée par 2

    [Header("Flacon")]
    [SerializeField] private float fairy_capture_radius = 1.2f;

    public UnityEvent OnSwordSwing; // À brancher sur l'animation ou le son de l'épée

    private PlayerController player_control;
    private PlayerHealth player_health;

    private bool is_shielding;
    private float sword_timer;

    public bool IsShielding { get { return is_shielding; } }

    void Awake()
    {
        player_control = GetComponent<PlayerController>();
        player_health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        // Réduit le cooldown de l'épée
        if (sword_timer > 0)
        {
            sword_timer -= Time.deltaTime;
        }
    }

#region Inputs
    // Input de l'épée (J)
    public void Sword(InputAction.CallbackContext ctx)
    {
        // Ignore si le bouton n'est pas pressé, si le cooldown est actif ou si le jeu est en pause
        if (!ctx.performed || sword_timer > 0 || Time.timeScale == 0f) return;

        sword_timer = sword_cooldown;
        OnSwordSwing?.Invoke();

        // Calcule la zone d'attaque devant le joueur
        Vector2 dir = player_control.FacingDir;
        Vector2 center = (Vector2)transform.position + dir * sword_distance;

        // Si le joueur regarde en haut ou en bas, la zone est tournée de 90 degrés
        Vector2 size = dir.x != 0 ? sword_size : new Vector2(sword_size.y, sword_size.x);

        // Récupère tous les colliders dans la zone
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);

        // Évite de toucher deux fois le même ennemi s'il a plusieurs colliders
        HashSet<Enemy> hit_enemies = new HashSet<Enemy>();

        foreach (Collider2D hit in hits)
        {
            // Ennemi : 1 point de dégât
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null && hit_enemies.Add(enemy))
            {
                enemy.TakeDamage(sword_damage);
            }

            // Herbe : elle est coupée
            Grass grass = hit.GetComponent<Grass>();
            if (grass != null)
            {
                grass.Cut();
            }
        }
    }

    // Input du bouclier (K) : maintenir pour garder le bouclier levé
    public void Shield(InputAction.CallbackContext ctx)
    {
        if (ctx.started || ctx.performed)
        {
            SetShield(true);
        }
        else if (ctx.canceled)
        {
            SetShield(false);
        }
    }

    // Input du flacon (U) : capture une fée proche
    public void Bottle(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || Time.timeScale == 0f) return;

        // Le flacon est déjà plein
        if (player_health.HasFairy) return;

        // Cherche une fée autour du joueur
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, fairy_capture_radius);

        foreach (Collider2D hit in hits)
        {
            Fairy fairy = hit.GetComponent<Fairy>();
            if (fairy != null)
            {
                // Met la fée dans le flacon puis la retire de la scène
                if (player_health.CaptureFairy())
                {
                    fairy.Capture();
                }
                break;
            }
        }
    }
#endregion

    private void SetShield(bool value)
    {
        is_shielding = value;

        // Le joueur se déplace deux fois moins vite avec le bouclier levé
        player_control.SetSpeedMultiplier(value ? shield_speed_multiplier : 1f);
    }

    void OnDisable()
    {
        // Baisse le bouclier si le script est désactivé
        if (is_shielding)
        {
            SetShield(false);
        }
    }

    // Affiche la zone de l'épée et le rayon de capture dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, fairy_capture_radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * sword_distance, new Vector3(sword_size.y, sword_size.x, 0f));
    }
}

