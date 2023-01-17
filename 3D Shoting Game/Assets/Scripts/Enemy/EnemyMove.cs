using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMove : MonoBehaviour
{
    public enum EnemyState { MoveMode, AttackMode}

    public float halfLength;
    public float maxSpeed;
    public float reachMaxSpeedSecond;
    public GameObject mainCharacter;
    [Serializable]
    public struct Pattern
    {
        public GameObject points;
        public GameObject prefabs;
    }
    [Serializable]
    public struct PatternInfo
    {
        public int[] patternIndex;
        public float[] delay;
        public Vector3[] rotInRepeat;
        public int[] repeatCount;
        public float[] repeatDelay;
    }
    [Header("Pattern")]
    public Pattern[] patterns;
    public PatternInfo[] patternInfos;

    private Vector3 firPos;
    private Vector3 target;
    private Vector3 movDir;
    private bool upSpeed = true;
    private float speed = 0;
    private EnemyState curState;
    private int curPattern = 0;
    private Vector3 curRot = new Vector3(0, 0, 0);
    private Vector3 saveRot;
    private int count = 0;

    private void Start()
    {
        firPos = target = transform.position;
        SetTarget();
    }

    private void SetTarget()
    {
        Vector3 nexPos = new Vector3(UnityEngine.Random.Range(firPos.x - halfLength, firPos.x + halfLength),
            UnityEngine.Random.Range(firPos.y - halfLength, firPos.y + halfLength),
            UnityEngine.Random.Range(firPos.z - halfLength, firPos.z + halfLength));
        if(Vector3.Distance(target, nexPos) < maxSpeed * 2)
        {
            SetTarget();
            return;
        }
        target = nexPos;
        curState = EnemyState.MoveMode;
    }

    private void Update()
    {
        ChangeSpeed();
        MoveToTarget();
    }

    private void ChangeSpeed()
    {
        if (curState != EnemyState.MoveMode) return;
        if(upSpeed && speed < maxSpeed)
        {
            speed += maxSpeed * Time.deltaTime / reachMaxSpeedSecond;
        }
        if(!upSpeed)
        {
            speed -= maxSpeed * Time.deltaTime / reachMaxSpeedSecond;
        }
        if(speed <= 0)
        {
            speed = 0;
            upSpeed = true;
            SetTarget();
            curState = EnemyState.AttackMode;
            AttackToPlayer();
        }
    }

    private void MoveToTarget()
    {
        movDir = new Vector3(target.x - transform.position.x, target.y - transform.position.y, target.z - transform.position.z).normalized;
        transform.position += movDir * speed * Time.deltaTime;
        if(Vector3.Distance(transform.position, target) < maxSpeed / 2)
        {
            upSpeed = false;
        }
    }

    private void AttackToPlayer()
    {
        if (curState != EnemyState.AttackMode) return;
        StartCoroutine(ExAttack());
    }

    private IEnumerator ExAttack()
    {
        for(var i = 0; i < patternInfos[curPattern].patternIndex.Length; i++)
        {
            yield return new WaitForSeconds(patternInfos[curPattern].delay[i]);
            curRot = transform.eulerAngles;
            saveRot = new Vector3(0, 0, 0);
            var index = patternInfos[curPattern].patternIndex[i];
            GameObject instObject = Instantiate(patterns[index].prefabs);
            Bullet instBullet = instObject.GetComponent<Bullet>();
            string prefabName = instBullet.prefabName;
            Destroy(instBullet.gameObject);
            for (var k = 0; k < patternInfos[curPattern].repeatCount[i]; k++)
            {
                yield return new WaitForSeconds(patternInfos[curPattern].repeatDelay[i]);
                GameObject parents = Instantiate(patterns[index].points);
                parents.transform.position = transform.position;
                parents.transform.rotation = Quaternion.Euler(curRot.x, curRot.y, curRot.z);
                Transform[] childTransform = parents.GetComponentsInChildren<Transform>();
                for (var j = 1; j < childTransform.Length; j++)
                {
                    GameObject clone = ObjectPooler.instant.GetObject(prefabName);
                    clone.transform.position = childTransform[j].transform.position;
                    Vector3 dir = (clone.transform.position - parents.transform.position).normalized;
                    clone.transform.rotation = Quaternion.LookRotation(dir);
                    count += 1;
                    Debug.Log(count);
                }
                saveRot += patternInfos[curPattern].rotInRepeat[i];
                curRot = transform.eulerAngles + saveRot;
                Destroy(parents);
            }
        }
        curPattern += 1;
        if (curPattern == patternInfos.Length) curPattern = 0;
        curState = EnemyState.MoveMode;
        yield return null;
    }
}
