using UnityEngine;
using SunnySword.Combat;

public class Projectile : MonoBehaviour
{
    private float damage;
    private float speed;
    private GameObject owner;
    private LayerMask targetLayer; 

    public void Setup(float damage, float speed, GameObject owner, LayerMask targetLayer)
    {
        this.damage = damage;
        this.speed = speed;
        this.owner = owner;
        this.targetLayer = targetLayer; 

        Collider2D projCol = GetComponent<Collider2D>();
        if (projCol != null)
        {
            Physics2D.IgnoreCollision(projCol, owner.GetComponent<Collider2D>());
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) Physics2D.IgnoreCollision(projCol, player.GetComponent<Collider2D>());
        }

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner || collision.CompareTag("Player")) return;

        Debug.Log($"A flecha tocou em: {collision.name} na layer {LayerMask.LayerToName(collision.gameObject.layer)}");

        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            if (collision.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(damage);
                Debug.Log("Dano aplicado! Destruindo flecha.");
                Destroy(gameObject); 
                return; 
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}