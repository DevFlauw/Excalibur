using UnityEngine;

// Herbe coupée par l'épée du joueur. Nécessite un Collider2D.
public class Grass : MonoBehaviour
{
    [SerializeField] private GameObject cut_fx;

    public void Cut()
    {
        // Si le prefab fx de coupe existe, spawn une instance
        if (cut_fx)
        {
            Instantiate(cut_fx, transform.position, Quaternion.identity);
        }

        // Supprime l'herbe
        Destroy(gameObject);
    }
}
