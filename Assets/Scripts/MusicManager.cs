﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        EventManager.AddListener(Events.LevelStarted, OnLevelStart);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(Events.LevelStarted, OnLevelStart);
    }

    private void OnLevelStart()
    {
        audioSource.time = 71f;
    }
}