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

    [Header("Animation de l'épée")]
    [SerializeField] private Animator sword_animator; // Animator qui contient les 4 triggers de coup d'épée

    [Header("Bouclier")]
    [SerializeField] private float shield_speed_multiplier = 0f; // 0 = immobile pendant que le bouclier est levé (comme dans Minish Cap)
    [SerializeField] private float block_angle = 60f; // Demi-angle devant le joueur où le bouclier bloque (60 = cône de 120 degrés)
    [SerializeField] private GameObject shield_visual; // Objet Shield affiché pendant le blocage (optionnel)

    [Header("Flacon")]
    [SerializeField] private float fairy_capture_radius = 1.2f;

    public UnityEvent OnSwordSwing; // À brancher sur le son de l'épée ou d'autres effets

    private PlayerController player_control;
    private PlayerHealth player_health;

    private bool is_shielding;
    private float sword_timer;

    public bool IsShielding { get { return is_shielding; } }

    void Awake()
    {
        player_control = GetComponent<PlayerController>();
        player_health = GetComponent<PlayerHealth>();

        // Le bouclier visuel est caché tant que le joueur ne le lève pas
        if (shield_visual != null)
        {
            shield_visual.SetActive(false);
        }
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
        if (!ctx.performed || sword_timer > 0 || Time.timeScale == 0f || is_shielding) return;

        sword_timer = sword_cooldown;
        OnSwordSwing?.Invoke();

        // Joue l'animation de l'épée selon la direction du joueur
        PlaySwordAnimation();

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

    // Input du flacon (U) : capture la fée la plus proche
    public void Bottle(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || Time.timeScale == 0f) return;

        // Le flacon est déjà plein
        if (player_health.HasFairy)
        {
            Debug.Log("[Bottle] flacon déjà plein : rien à faire");
            return;
        }

        // Cherche les fées par leur script, sans dépendre d'un collider
        Fairy[] fairies = FindObjectsByType<Fairy>(FindObjectsSortMode.None);
        Debug.Log("[Bottle] " + fairies.Length + " fée(s) avec le script Fairy dans la scène");

        // Garde la plus proche, dans le rayon de capture
        Fairy closest = null;
        float closest_distance = fairy_capture_radius;

        foreach (Fairy fairy in fairies)
        {
            float distance = Vector2.Distance(transform.position, fairy.transform.position);
            Debug.Log("[Bottle] fée '" + fairy.name + "' à " + distance.ToString("F1") + " unités");
            if (distance <= closest_distance)
            {
                closest = fairy;
                closest_distance = distance;
            }
        }

        if (closest == null)
        {
            Debug.Log("[Bottle] aucune fée à moins de " + fairy_capture_radius + " unités (" + fairies.Length + " fée(s) avec le script Fairy dans la scène)");
            return;
        }

        // Met la fée dans le flacon puis la retire de la scène
        if (player_health.CaptureFairy())
        {
            closest.Capture();
        }
    }
#endregion

    // Déclenche le trigger d'animation correspondant à la direction du joueur
    private void PlaySwordAnimation()
    {
        if (sword_animator == null) return;

        Vector2 face = player_control.FacingDir;
        string trigger;

        if (face.x > 0) trigger = "SwingRight";
        else if (face.x < 0) trigger = "SwingLeft";
        else if (face.y > 0) trigger = "SwingUp";
        else trigger = "SwingDown";

        sword_animator.SetTrigger(trigger);
    }

    // Retourne true si une attaque venant de cette position est bloquée par le bouclier.
    // Le bouclier ne protège que devant le joueur, dans la direction où il regarde.
    public bool BlocksFrom(Vector2 source_position)
    {
        if (!is_shielding) return false;

        Vector2 to_source = source_position - (Vector2)transform.position;
        return Vector2.Angle(player_control.FacingDir, to_source) <= block_angle;
    }

    private void SetShield(bool value)
    {
        is_shielding = value;

        // Affiche ou cache le bouclier
        if (shield_visual != null)
        {
            shield_visual.SetActive(value);
        }

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
