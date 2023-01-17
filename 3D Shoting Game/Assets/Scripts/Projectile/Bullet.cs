using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("공통")]
    public string prefabName;
    public bool isEnemyBullet;
    public float damage;
    public float speed;
    public float destroyTime;
    public bool canMove;
    [Header("플레이어 관련")]
    public float spawnDelay;
    public float delayRatio;

    private bool isAttack;


    private void OnEnable()
    {
        canMove = true;
        isAttack = false;
        StopAllCoroutines();
        StartCoroutine(DestroySelf(destroyTime));
    }

    private void Update()
    {
        if (canMove)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttack) return;
        if (!isEnemyBullet) // 플레이어 탄환
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy == null) return;
            enemy.ChangeHP(-damage);
            isAttack = true;
            StartCoroutine(DestroySelf(0));
        }
        else // 적 탄환
        {
            PlayerInfo player = other.GetComponent<PlayerInfo>();
            if (player == null) return;
            player.ChangeHP(-damage);
            isAttack = true;
            StartCoroutine(DestroySelf(0));
        }
    }

    public IEnumerator DestroySelf(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        this.gameObject.SetActive(false);
        yield return null;
    }
}
