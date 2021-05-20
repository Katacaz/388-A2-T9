using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeScreen : MonoBehaviour
{
    public ParticleSystem[] effects;

    public AudioSource audioSource;
    public AudioClip smokeSND;

    public float smokeDuration = 5f;
    private float timer;
    public bool smokeActivated;

    public float smokeRadius = 6f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (smokeActivated)
        {
            if (timer < smokeDuration)
            {
                timer += Time.deltaTime;
            } else
            {
                smokeActivated = false;
            }
        } else
        {
            StopSmoke();
        }
    }

    public void StartSmoke(float duration)
    {
        if (smokeSND != null)
        {
            audioSource.clip = smokeSND;
            audioSource.Play();
        }
        timer = 0;
        smokeDuration = duration;
        smokeActivated = true;
        SetParticleSystemsTo(true);
    }
    public void StopSmoke()
    {
        audioSource.Stop();
        smokeActivated = false;
        timer = 0;
        SetParticleSystemsTo(false);
    }
    private void SetParticleSystemsTo(bool state)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (state)
            {
                effects[i].Play();
            } else
            {
                effects[i].Stop();
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, smokeRadius);
    }
}
