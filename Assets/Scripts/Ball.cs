using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float maxXVelocity;
    [SerializeField] private float maxYVelocity;

    [SerializeField] private ParticleSystem dust;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody2D;
    private SoundEffects _soundEffects;
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.bodyType = RigidbodyType2D.Static;

        _soundEffects = GetComponent<SoundEffects>();
        
        EventManager.AddListener(Events.LevelStarted, OnLevelStarted);
    }

    private void Update()
    {
        Vector2 velocity = _rigidbody2D.velocity;
        
        _rigidbody2D.velocity = new Vector2(
            Mathf.Clamp(velocity.x, -maxXVelocity, maxXVelocity),
            Mathf.Clamp(velocity.y, -maxYVelocity * 0.5f, maxYVelocity)
            );
    }

    private void OnDestroy()
    {
        EventManager.AddListener(Events.LevelStarted, OnLevelStarted);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Bricks bricks = other.gameObject.GetComponent<Bricks>();
        if (bricks)
        {
            EventManager.TriggerEvent(Events.BallBounce);

            Instantiate(dust, transform.position, Quaternion.identity);
            
            _soundEffects.PlayOnBall();
        }
    }

    public void Kick(float force, Vector2 sourcePosition)
    {
        EventManager.TriggerEvent(Events.BallBounce);
        
        Vector2 kickForce = new Vector2((transform.position.x - sourcePosition.x) * force * 0.01f, force);
        
        _rigidbody2D.AddForce(kickForce, ForceMode2D.Impulse);
    }
    
    void OnLevelStarted()
    {
        transform.parent = null;
        
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
}
