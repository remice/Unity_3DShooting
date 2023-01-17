using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    enum CurMode { PointChange = 0, PrefabChange = 1 }

    [Header("MainAttackPoint")]
    public GameObject mainAttackPoint;
    [Header("Attack Points")]
    public GameObject[] attackPoints;
    [Header("Prefabs")]
    public GameObject[] attackPrefabs;
    [Header("Managers")]
    public GameObject g_attackPointUIManager;
    public GameObject g_attackPrefabUIManager;
    public GameObject g_camManager;

    [Header("Debugger"), SerializeField]
    private GameObject curAttackPoint;
    [SerializeField]
    private GameObject curAttackPrefab;

    private CamControl s_cam;
    private float attackDelay = 1;
    private float delayRatio = 1;
    private float curDelay;
    private float spawnDelay;
    private int curPointIndex = 0;
    private int curPrefabIndex = 0;
    private GameObject[] instants = new GameObject[50];
    private UIManager s_attackPointUIManager, s_attackPrefabUIManager;
    private CurMode state = CurMode.PointChange;
    private string curBulletName;

    void Start()
    {
        s_cam = g_camManager.GetComponent<CamControl>();
        s_attackPointUIManager = g_attackPointUIManager.GetComponent<UIManager>();
        s_attackPrefabUIManager = g_attackPrefabUIManager.GetComponent<UIManager>();
        ChangePoint(attackPoints[curPointIndex]);
        ChangePrefab(attackPrefabs[curPrefabIndex]);
        ChangeUIWithState();
    }

    void Update()
    {
        curDelay += Time.deltaTime;
        ChangeState();
        if (curDelay < 0) return;
        if(Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(ExAttack());
        }
    }

    private void ChangeState()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            state = CurMode.PointChange;
            ChangeUIWithState();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            state = CurMode.PrefabChange;
            ChangeUIWithState();
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            switch (state)
            {
                case CurMode.PointChange:
                    curPointIndex += 1;
                    if (curPointIndex >= attackPoints.Length) curPointIndex = 0;
                    ChangePoint(attackPoints[curPointIndex]);
                    break;
                case CurMode.PrefabChange:
                    curPrefabIndex += 1;
                    if (curPrefabIndex >= attackPrefabs.Length) curPrefabIndex = 0;
                    ChangePrefab(attackPrefabs[curPrefabIndex]);
                    break;
                default:
                    Debug.Log("Chanage State Error");
                    break;
            }
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            switch (state)
            {
                case CurMode.PointChange:
                    curPointIndex -= 1;
                    if (curPointIndex < 0) curPointIndex = attackPoints.Length - 1;
                    ChangePoint(attackPoints[curPointIndex]);
                    break;
                case CurMode.PrefabChange:
                    curPrefabIndex -= 1;
                    if (curPrefabIndex < 0) curPrefabIndex = attackPrefabs.Length - 1;
                    ChangePrefab(attackPrefabs[curPrefabIndex]);
                    break;
                default:
                    Debug.Log("Chanage State Error");
                    break;
            }
        }
    }

    private void ChangeUIWithState()
    {
        switch (state)
        {
            case CurMode.PointChange:
                s_attackPointUIManager.EnableImage(curPointIndex);
                s_attackPrefabUIManager.SelectImage(curPrefabIndex);
                break;
            case CurMode.PrefabChange:
                s_attackPrefabUIManager.EnableImage(curPrefabIndex);
                s_attackPointUIManager.SelectImage(curPointIndex);
                break;
            default:
                Debug.Log("ChangeUIWithState Error");
                break;
        }
    }

    private void ChangePoint(GameObject target)
    {
        curAttackPoint = target;
        AttackShapes attackShapes = curAttackPoint.GetComponent<AttackShapes>();
        if (attackShapes == null) return;
        attackDelay = attackShapes.delay;
        s_attackPointUIManager.EnableImage(curPointIndex);
    }

    private void ChangePrefab(GameObject target)
    {
        curAttackPrefab = target;
        Bullet bullet = curAttackPrefab.GetComponent<Bullet>();
        if (bullet == null) return;
        delayRatio = bullet.delayRatio;
        spawnDelay = bullet.spawnDelay;
        curBulletName = bullet.prefabName;
        s_attackPrefabUIManager.EnableImage(curPrefabIndex);
    }

    private IEnumerator ExAttack()
    {
        curDelay = -attackDelay * delayRatio;

        Vector3 dir = (s_cam.RayToTarget() - transform.position).normalized;

        GameObject attackPoint = Instantiate(curAttackPoint);
        attackPoint.transform.position = mainAttackPoint.transform.position;
        attackPoint.transform.rotation = Quaternion.LookRotation(dir);

        Transform[] childTransform = attackPoint.GetComponentsInChildren<Transform>();

        for (var i = 1; i < childTransform.Length; i++)
        {
            instants[i] = ObjectPooler.instant.GetObject(curBulletName);
            instants[i].transform.position = childTransform[i].transform.position;
            instants[i].transform.rotation = childTransform[i].transform.rotation;

            Bullet bullet = instants[i].GetComponent<Bullet>();
            bullet.canMove = false;

            yield return new WaitForSeconds(spawnDelay);
        }
        for (var i = 1; i < childTransform.Length; i++)
        {
            if(instants[i] != null)
            {
                Bullet bullet = instants[i].GetComponent<Bullet>();
                bullet.canMove = true;
            }
        }
        Destroy(attackPoint);
        yield return null;
    }
}
