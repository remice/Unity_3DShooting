using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    [SerializeField]
    private float followRatio = 1f;
    [SerializeField]
    private Transform followPos;

    public GameObject enemy;

    private Vector3 realPos;


    private void Start()
    {
        realPos = followPos.position;
    }

    private void Update()
    {
        RayToTarget();
        transform.LookAt(enemy.transform);
    }

    public Vector3 RayToTarget()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Enemy");
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 100, layerMask, QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }
        return transform.position + transform.forward * 100;
    }

    private void FollowCamPos()
    {
        if (realPos == followPos.position) return;
        var distance = followPos.position - realPos;
        if (distance.magnitude < 0.05f) realPos = followPos.position;
        else
        {
            distance *= followRatio;
            realPos += distance;
        }
        transform.position = realPos;
    }
}
