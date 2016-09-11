using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {


    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), transform.parent.GetComponent<Collider2D>());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            Combat hitCombat = other.gameObject.GetComponent<Combat>();
            if (hitCombat != null)
            {
                hitCombat.TakeDamage(10);
                Destroy(gameObject);
            }
        }
    }
}
