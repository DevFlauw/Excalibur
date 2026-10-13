using UnityEngine;

// Fée statique sans point de vie, capturée avec le flacon. Nécessite un Collider2D.
public class Fairy : MonoBehaviour
{
    [SerializeField] private GameObject capture_fx;

    public void Capture()
    {
        // Si le prefab fx de capture existe, spawn une instance
        if (capture_fx)
        {
            Instantiate(capture_fx, transform.position, Quaternion.identity);
        }

        // Retire la fée de la scène
        Destroy(gameObject);
    }
}

