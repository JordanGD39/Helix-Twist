using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip[] breakingSounds;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private AudioClip pointSound;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayLandingSound()
    {
        audioSource.PlayOneShot(landSound);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }

    public void PlayPointSound()
    {
        audioSource.PlayOneShot(pointSound);
    }

    public void PlayBreakingSound(int i)
    {
        audioSource.PlayOneShot(breakingSounds[i]);
    }
}
