using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float maxHP;
    [SerializeField]
    private float curHP;

    private void Awake()
    {
        curHP = maxHP;
    }

    public void ChangeHP(float amount)
    {
        curHP += amount;
        Debug.Log(curHP);
        if (curHP <= 0)
        {
            Debug.Log("Dead");
        }
    }
}
