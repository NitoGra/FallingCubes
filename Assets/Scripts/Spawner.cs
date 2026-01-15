using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnPositionX;
    [SerializeField] private float _spawnPositionZ;

    private Pool _cubes;

    private void Start()
    {
        _cubes = new Pool(_cubePrefab);
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        
        while (enabled)
        {
            yield return wait;
            SpawnCube();
        }
    }

    private void SpawnCube()
    {
        MonoBehaviour cube = _cubes.Get;
        
        Vector3 cubePosition = new Vector3(
            Random.Range(-_spawnPositionX, _spawnPositionX), 0,
            Random.Range(-_spawnPositionZ, _spawnPositionZ));
            
        cube.transform.parent = transform;
        cube.transform.localPosition = cubePosition;
        cube.gameObject.SetActive(true);
    }
}