using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shield : MonoBehaviour

{
    Animator animator;
    CircleCollider2D Collider;
    Fireball fireball;
    public bool isShielding;

    void Start()
    {
        Collider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isShielding)
        {
            isShielding = true;
            Collider.isTrigger = true;
            Debug.Log(Collider.isTrigger);

            animator.SetBool("Shielding", true);

        }
        if (Input.GetKeyUp(KeyCode.K) && isShielding )
        {
            isShielding = false;
            Collider.isTrigger = false;
            Debug.Log(Collider.isTrigger);
            animator.SetBool("Shielding", false);
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    { // regarder l'objet qui entre en collision
        if (collision.gameObject.GetComponent<Fireball>())
        {
            Destroy(fireball); // si boule de feu -> destroy 
        }
    }
}
