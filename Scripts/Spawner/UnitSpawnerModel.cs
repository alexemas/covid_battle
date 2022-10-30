using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class UnitSpawnerModel
{
    [SerializeField]
    private float _chanceSpawnBomb = 10;
    [SerializeField]
    private float _chanceSpawnSpecial = 10;
    [SerializeField]
    private float _minWait = 0.3f;
    [SerializeField]
    private float _maxWait = 1;
    [SerializeField]
    private float _minForce = 10;
    [SerializeField]
    private float _maxForce = 20;
    [SerializeField]
    private GameObject bombPrefab;
    [SerializeField] 
    private GameObject[] _unitPrefabList;
    [SerializeField] 
    private GameObject[] _bonusUnitPrefabs;

    private GameObject[] _enemySpawnerList;
    private GameObject[] _bonusSpawnerList;

    public void SetEnemySpawnerList(GameObject[] spawnerList)
    {
        _enemySpawnerList = spawnerList;
    }

    public void SetBonusSpawnerList(GameObject[] spawnerList)
    {
        _bonusSpawnerList = spawnerList;
    }

    public IEnumerator SpawnRandomUnits()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minWait, _maxWait));

            float rnd = UnityEngine.Random.Range(0, 100);
            Transform rndPosition = _enemySpawnerList[UnityEngine.Random.Range(0, _enemySpawnerList.Length)].transform;
            GameObject rndUnit =
                rnd < _chanceSpawnBomb ?
                bombPrefab :
                _unitPrefabList[UnityEngine.Random.Range(0, _unitPrefabList.Length)]
            ;
            GameObject units = MonoBehaviour.Instantiate(
                rndUnit,
                rndPosition.transform.position,
                rndPosition.transform.rotation
            );

            float specialRnd = UnityEngine.Random.Range(0, 100);

            SpawnSpecial();

            units.GetComponent<Rigidbody2D>().AddForce(
                rndPosition.transform.up * UnityEngine.Random.Range(_minForce, _maxForce),
                ForceMode2D.Impulse
            );

            MonoBehaviour.Destroy(units, 5f);
        }
    }

    private void SpawnSpecial()
    {
        float specialRnd = UnityEngine.Random.Range(0, 100);

        if (specialRnd < _chanceSpawnSpecial)
        {
            GameObject bonusUnit = _bonusUnitPrefabs[UnityEngine.Random.Range(0, _bonusUnitPrefabs.Length)];
            Transform spesialRndPosition = _bonusSpawnerList[UnityEngine.Random.Range(0, _bonusSpawnerList.Length)].transform;

            GameObject units = MonoBehaviour.Instantiate(
                bonusUnit,
                spesialRndPosition.transform.position,
                spesialRndPosition.transform.rotation
            );

            units.GetComponent<Rigidbody2D>().AddForce(
                spesialRndPosition.transform.up * UnityEngine.Random.Range(_minForce, _maxForce), ForceMode2D.Impulse);

            MonoBehaviour.Destroy(units, 5f);
        }
    }
}
