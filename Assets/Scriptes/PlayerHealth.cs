/*using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Vie du joueur (3 cœurs)
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int max_health = 3;

    private int current_health;
    private bool has_fairy;

    // Dernier checkpoint atteint 
    private static bool has_checkpoint;
    private static Vector2 checkpoint_position;

    public UnityEvent<int> OnHealthChanged;   // nombre de cœurs restants
    public UnityEvent<bool> OnBottleChanged;  // le flacon contient une fée

    public int Health { get { return current_health; } }
    public bool HasFairy { get { return has_fairy; } }
    public static bool HasCheckpoint { get { return has_checkpoint; } }
    public static Vector2 CheckpointPosition { get { return checkpoint_position; } }
    private HeartsUI hudCanvas;

    // Remet le checkpoint à zéro 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCheckpoint()
    {
        has_checkpoint = false;
    }

    // Appelée par un Checkpoint
    public static void SetCheckpoint(Vector2 position)
    {
        has_checkpoint = true;
        checkpoint_position = position;
    }

    void Start()
    {
        hudCanvas = GameObject.FindGameObjectWithTag("HUD").GetComponent<HeartsUI>();
        current_health = max_health;
        has_fairy = false;

        
        if (has_checkpoint)
        {
            transform.position = checkpoint_position;
        }

        
        OnHealthChanged?.Invoke(current_health);
        OnBottleChanged?.Invoke(has_fairy);
    }

    public void TakeDamage(int amount)
    {
        
        if (current_health <= 0) return;

        current_health -= amount;

        if (current_health <= 0)
        {
            if (has_fairy)
            {
                
                has_fairy = false;
                current_health = max_health;
                OnBottleChanged?.Invoke(has_fairy);
            }
            else
            {
                
                current_health = 0;
                OnHealthChanged?.Invoke(current_health);
                Die();
                return;
            }
        }

        hudCanvas.SetHealth(current_health);
        OnHealthChanged?.Invoke(current_health);
    }

    // fée dans le flacon. Retourne false si le flacon est déjà plein
    public bool CaptureFairy()
    {
        if (has_fairy) return false;

        has_fairy = true;
        OnBottleChanged?.Invoke(has_fairy);
        return true;
    }

    private void Die()
    {
        // Recommence la scène 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
*/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Vie du joueur (3 cœurs)
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int max_health = 3;

    private int current_health;
    private bool has_fairy;

    // Dernier checkpoint atteint 
    private static bool has_checkpoint;
    private static Vector2 checkpoint_position;

    public UnityEvent<int> OnHealthChanged;   // nombre de cœurs restants
    public UnityEvent<bool> OnBottleChanged;  // le flacon contient une fée

    public int Health { get { return current_health; } }
    public bool HasFairy { get { return has_fairy; } }
    public static bool HasCheckpoint { get { return has_checkpoint; } }
    public static Vector2 CheckpointPosition { get { return checkpoint_position; } }

    public HeartsUI Hearts;

    // Remet le checkpoint à zéro 
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCheckpoint()
    {
        has_checkpoint = false;
    }

    // Appelée par un Checkpoint
    public static void SetCheckpoint(Vector2 position)
    {
        has_checkpoint = true;
        checkpoint_position = position;
    }

    void Start()
    {
        
        current_health = max_health;
        has_fairy = false;
        Hearts.HasFairy(has_fairy);

        if (has_checkpoint)
        {
            transform.position = checkpoint_position;
        }

        
        OnHealthChanged?.Invoke(current_health);
        OnBottleChanged?.Invoke(has_fairy);
    }

    public void TakeDamage(int amount)
    {
        
        if (current_health <= 0) return;

        current_health -= amount;

        if (current_health <= 0)
        {
            if (has_fairy)
            {
                
                has_fairy = false;
                current_health = max_health;
                OnBottleChanged?.Invoke(has_fairy);
                Hearts.HasFairy(has_fairy);
            }
            else
            {
                
                current_health = 0;
                OnHealthChanged?.Invoke(current_health);
                Die();
                return;
            }
        }
        Hearts.SetHealth(current_health);
        OnHealthChanged?.Invoke(current_health);
    }

    // fée dans le flacon. Retourne false si le flacon est déjà plein
    public bool CaptureFairy()
    {
        if (has_fairy) return false;
        has_fairy = true;
        Hearts.HasFairy(has_fairy);
        OnBottleChanged?.Invoke(has_fairy);
        return true;
    }

    private void Die()
    {
        // Recommence la scène 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
