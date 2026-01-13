using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnPositionX;
    [SerializeField] private float _spawnPositionZ;

    private List<Cube> _cubes;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        _cubes ??= new List<Cube>();
        
        while (true)
        {
            yield return wait;
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        GetNewOrDisabled(transform, _cubes, _cubePrefab).transform.localPosition = new Vector3(
            Random.Range(-_spawnPositionX, _spawnPositionX), 0, Random.Range(-_spawnPositionZ, _spawnPositionZ));
    }
    
    private static T GetNewOrDisabled<T>(Transform position, List<T> listObjects, T prefab) where T: MonoBehaviour
    {
        foreach (var aButton in listObjects.Where(aButton =>aButton && aButton.gameObject.activeSelf == false))
        {
            aButton.gameObject.SetActive(true);
            aButton.transform.SetParent(position);
            return aButton;
        }

        if (position is null || prefab is null)
        {
            Debug.LogWarning("Break null error");
            return null;
        }

        if (prefab.IsDestroyed())
        {
            Debug.LogWarning("Break destroyed null error");
            return null;
        }

        T button = Instantiate(prefab, position);
        button.gameObject.SetActive(true);
        listObjects.Add(button); 
        return button;
    }
}