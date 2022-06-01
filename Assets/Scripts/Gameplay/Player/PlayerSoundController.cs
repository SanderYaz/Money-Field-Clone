using System.Collections;
using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip currentClip;
    [SerializeField] private AudioSource source;
    private PlayerController controller;
    
    public void Initialize(PlayerController playerController)
    {
     
        controller = playerController;

        
        
        source.clip = currentClip;
        source.Play();

    }

    private void Update()
    {
        if (!source || !source.clip || !currentClip) return;

        var dir = Vector3.forward * -controller.Movement.FloatingJoystick.Vertical + Vector3.right * -controller.Movement.FloatingJoystick.Horizontal;

        var pitch = dir.magnitude <= 0.3f ? 1 : (dir.magnitude * 2f) * 1.5f;
        source.pitch = Mathf.Lerp(source.pitch, pitch, Time.deltaTime *2);
    }

}
