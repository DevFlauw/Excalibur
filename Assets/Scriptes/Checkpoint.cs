using UnityEngine;



public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject activate_fx;
    [SerializeField] private Color inactive_color = Color.gray;   // Couleur avant activation
    [SerializeField] private Color active_color = Color.yellow;   // Couleur une fois activé

    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
        bool is_current = PlayerHealth.HasCheckpoint && PlayerHealth.CheckpointPosition == (Vector2)transform.position;
        SetActiveVisual(is_current);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player == null) return;

        // point de reapparition 
        if (PlayerHealth.HasCheckpoint && PlayerHealth.CheckpointPosition == (Vector2)transform.position) return;

        PlayerHealth.SetCheckpoint(transform.position);
        SetActiveVisual(true);
        Debug.Log("Checkpoint activé : " + transform.position);

        // Si le prefab fx d'activation existe, spawn une instance
        if (activate_fx)
        {
            Instantiate(activate_fx, transform.position, Quaternion.identity);
        }
    }

    // montre si le checkpoint est active
    private void SetActiveVisual(bool is_active)
    {
        if (sprite != null)
        {
            sprite.color = is_active ? active_color : inactive_color;
        }
    }
}
