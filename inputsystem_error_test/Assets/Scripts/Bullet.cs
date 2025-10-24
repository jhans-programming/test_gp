using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifetime = 10f;
    private float timeCreated;

    private void Awake()
    {
        timeCreated = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
        if (Time.time - timeCreated >= lifetime)
        {
            // In case the bullet didn't hit anything, destroy it after some time
            // Keep scene objects clean
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Damage player. Ex: other.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
