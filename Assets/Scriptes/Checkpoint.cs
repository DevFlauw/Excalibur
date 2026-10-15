using UnityEngine;

// Point de sauvegarde.

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject activate_fx;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();
        if (player == null) return;

        
        if (PlayerHealth.HasCheckpoint && PlayerHealth.CheckpointPosition == (Vector2)transform.position) return;

        PlayerHealth.SetCheckpoint(transform.position);

        // Si le prefab fx d'activation existe, spawn une instance
        if (activate_fx)
        {
            Instantiate(activate_fx, transform.position, Quaternion.identity);
        }
    }
}
