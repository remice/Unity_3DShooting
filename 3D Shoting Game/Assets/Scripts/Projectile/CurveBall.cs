using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveBall : MonoBehaviour
{
    public Vector3 target;
    public float slerpTime = 0.01f;

    [SerializeField]
    float followTime = 5f;

    float curTime = 0f;

    private void Start()
    {
        slerpTime *= 60;
    }

    void OnEnable()
    {
        curTime = 0f;
    }

    private void Update()
    {
        RotateToTarget();
    }

    private void RotateToTarget()
    {
        if (target == null) return;
        if (curTime >= followTime) return;
        curTime += Time.deltaTime;

        var targetRot = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, slerpTime * Time.deltaTime);
    }
}
