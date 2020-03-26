using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private Color dressColor;
    
    private bool _isPlaying;
    private bool _isKicking;

    private SoundEffects _soundEffects;

    private SpriteRenderer[] _spriteRenderers;

    public string Name => name;
    public Color DressColor => dressColor;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _soundEffects = GetComponent<SoundEffects>();
    }

    void Start()
    {
        EventManager.AddListener(Events.SwitchPlayer, OnSwitchPlayer);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        EventManager.RemoveListener(Events.SwitchPlayer, OnSwitchPlayer);
    }
    
    public bool IsPlaying
    {
        get => _isPlaying;

        set
        {
            _isPlaying = value;
            
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.color = _isPlaying ? Color.white : Color.grey;
                spriteRenderer.sortingLayerName = _isPlaying ? "PlayerUp" : "PlayerDown";
            }
        }
    }

    public bool IsKicking
    {
        get => _isKicking;
        set
        {
            if (value)
            {
                EventManager.TriggerEvent(Events.UpdateUI);
                EventManager.TriggerEvent(Events.SwitchPlayer);
                
                StartCoroutine(StopKicking());
                
                _soundEffects.PlayOnKick();
            }

            _isKicking = value;
        }
    }

    IEnumerator StopKicking()
    {
        yield return new WaitForSeconds(Constants.SleepAfterKick);

        IsKicking = false;
    }

    void OnSwitchPlayer()
    {
        IsPlaying = !IsPlaying;
    }
}
