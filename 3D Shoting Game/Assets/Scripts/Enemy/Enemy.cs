using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public GameObject target;
    private float curHP;
    private bool isDead;

    private void Start()
    {
        curHP = maxHP;
        isDead = false;
    }

    private void Update()
    {
        transform.LookAt(target.transform);
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void ChangeHP(float amount)
    {
        if (isDead) return;
        curHP += amount;
        if (curHP > maxHP) curHP = maxHP;
        if (curHP < 0)
        {
            isDead = true;
            DestroySelf();
        }
    }
}
