using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinBall : MonoBehaviour
{
    public float height = 1;
    public float width = 1;

    float saveTime;

    void Start()
    {
        saveTime = Time.time;
    }

    void Update()
    {
        var pos = transform.position;
        float theta = Time.time - saveTime;
        float sin = Mathf.Sin(theta * width);
        pos += transform.up * sin * height;
        transform.position = pos;
    }
}
