using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Header("해당 프리팹")]
    public PoolItems[] prefabs;
    [Header("참조용 오브젝트")]
    public GameObject mainCharacter;

    public static ObjectPooler instant;

    private void Awake()
    {
        instant = this;
        InitialPools();
    }

    private void InitialPools()
    {
        for(var i = 0; i < prefabs.Length; i++)
        {
            prefabs[i].pool = new GameObject[prefabs[i].maxCount];
            for(var j = 0; j < prefabs[i].maxCount; j++)
            {
                prefabs[i].pool[j] = Instantiate(prefabs[i].prefab);
                prefabs[i].pool[j].SetActive(false);
            }
            prefabs[i].maxCount = -1;
        }
    }

    public GameObject GetObject(string name)
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].name == name)
            {
                if (++prefabs[i].maxCount >= prefabs[i].pool.Length) prefabs[i].maxCount = 0;
                CurveBall curveBall = prefabs[i].pool[prefabs[i].maxCount].GetComponent<CurveBall>();
                if (curveBall != null) curveBall.target = mainCharacter.transform.position;
                prefabs[i].pool[prefabs[i].maxCount].SetActive(false);
                prefabs[i].pool[prefabs[i].maxCount].SetActive(true);
                return prefabs[i].pool[prefabs[i].maxCount];
            }
        }
        Debug.Log("Error" + name);
        return null;
    }


    [System.Serializable]
    public struct PoolItems
    {
        public string name;
        public GameObject prefab;
        public int maxCount;
        public GameObject[] pool;
    }
}
