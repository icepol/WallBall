using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private SoundEffects _soundEffects;
    private Collider2D _collider;
    
    private void Awake()
    {
        _soundEffects = GetComponent<SoundEffects>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Ball>())
        {
            EventManager.TriggerEvent(Events.BallOnGrass);
            
            _soundEffects.PlayOnScream();
            _collider.enabled = false;
        }
    }
}
