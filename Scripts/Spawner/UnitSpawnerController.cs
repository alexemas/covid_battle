using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerController : MonoBehaviour
{
    [SerializeField] private UnitSpawnerModel _model;

    void Start()
    {
        _model.SetEnemySpawnerList(GameObject.FindGameObjectsWithTag(TagEnum.EnemySpawner.ToString()));
        _model.SetBonusSpawnerList(GameObject.FindGameObjectsWithTag(TagEnum.BonusSpawner.ToString()));

        StartCoroutine(_model.SpawnRandomUnits());
    }

    public void DestroyUnits()
    {
        GameObject[] unitList = GameObject.FindGameObjectsWithTag(TagEnum.Enemy.ToString());

        for (int i = 0; i < unitList.Length; i++)
        {
            Destroy(unitList[i]);
        }
    }
}
