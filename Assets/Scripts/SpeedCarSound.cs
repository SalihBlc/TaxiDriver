using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCarSound : MonoBehaviour
{
    public float maxSpeed;
    public float minSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    private AudioSource carAudio;

    public float minPitch;
    public float maxPitch;
    public float maxVolume;  // Maximum volume of the audio

    void Start()
    {
        carAudio = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EngineSound();
    }

    void EngineSound()
    {
        currentSpeed = carRb.velocity.magnitude;

        // Calculate pitch based on speed
        float pitchFactor = Mathf.Clamp((currentSpeed - minSpeed) / (maxSpeed - minSpeed), 0f, 1f);
        float pitch = Mathf.Lerp(minPitch, maxPitch, pitchFactor);

        // Set pitch and volume
        carAudio.pitch = pitch;
        carAudio.volume = pitchFactor * maxVolume;

        // Play the sound if the car is moving
        if (currentSpeed > 0)
        {
            if (!carAudio.isPlaying)
            {
                carAudio.Play();
            }
        }
        else
        {
            // Stop the sound if the car is not moving
            carAudio.Stop();
        }
    }
}
