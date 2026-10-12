using UnityEngine;
using UnityEngine.InputSystem;

// Déplacement du joueur en vue du dessus (ZQSD)
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody2D body2D;
    private Vector2 move_input;
    private Vector2 facing_dir = Vector2.down; // Direction vers laquelle regarde le joueur (4 directions)
    private float speed_multiplier = 1f;

    public Vector2 FacingDir { get { return facing_dir; } }

    void Awake()
    {
        // Récupère le rigidbody 2D
        body2D = GetComponent<Rigidbody2D>();

        // Vue du dessus : pas de gravité et pas de rotation
        body2D.gravityScale = 0f;
        body2D.freezeRotation = true;
    }
    void FixedUpdate()
    {
        // Applique la vitesse de déplacement (réduite quand le bouclier est levé)
        body2D.linearVelocity = move_input * speed * speed_multiplier;
    }

    // Change le multiplicateur de vitesse (utilisé par le bouclier)
    public void SetSpeedMultiplier(float value)
    {
        speed_multiplier = value;
    }

    // Input de direction (ZQSD)
    public void Move(InputAction.CallbackContext ctx)
    {
        // Limite la magnitude à 1 pour que les diagonales ne soient pas plus rapides
        move_input = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1f);

        if (move_input.sqrMagnitude > 0.01f)
        {
            // Mémorise la direction principale (horizontale ou verticale) pour l'épée
            if (Mathf.Abs(move_input.x) >= Mathf.Abs(move_input.y))
            {
                facing_dir = new Vector2(Mathf.Sign(move_input.x), 0f);
            }
            else
            {
                facing_dir = new Vector2(0f, Mathf.Sign(move_input.y));
            }
        }
    }
}

