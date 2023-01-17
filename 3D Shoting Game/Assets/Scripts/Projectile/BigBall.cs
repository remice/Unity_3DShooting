using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBall : MonoBehaviour
{
    public float sizeUpRatioPerSeconds;
    public GameObject particle;
    public GameObject ballParticle;
    public GameObject tail;

    Bullet bullet;
    TrailRenderer trail;
    Vector3 saveScale;
    Vector3 saveParticleScale;

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
        trail = tail.GetComponent<TrailRenderer>();
        saveScale = transform.localScale;
        if (ballParticle != null)
            saveParticleScale = ballParticle.transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = particle.transform.localScale = saveScale;
        if (ballParticle != null)
            ballParticle.transform.localScale = saveParticleScale;
        trail.startWidth = saveScale.x * 0.3f;
        trail.time = saveScale.x * 0.3f;
    }

    private void Update()
    {
        if(bullet.canMove)
        {
            transform.localScale += new Vector3(1, 1, 1) * sizeUpRatioPerSeconds * Time.deltaTime;
            particle.transform.localScale = transform.localScale;
            if (ballParticle != null)
                ballParticle.transform.localScale += saveParticleScale * sizeUpRatioPerSeconds * Time.deltaTime;
            trail.startWidth = transform.localScale.x * 0.3f;
            trail.time = transform.localScale.x * 0.3f;
        }
    }
}
