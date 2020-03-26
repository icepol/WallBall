using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Ball>())
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
}
