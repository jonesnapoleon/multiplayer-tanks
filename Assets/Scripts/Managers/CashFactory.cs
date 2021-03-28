using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CashFactory : NetworkBehaviour
{
    // Factory operates on server side
    public float spawnTime = 2f;
    public float xSpawnRange = 10f;
    public float zSpawnRange = 10f;

    private ObjectPooler objectPooler;

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        objectPooler = ObjectPooler.Instance;
        InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
    }

    public void Spawn()
    {
        // Randomize spawn location
        Vector3 position = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), 0, Random.Range(-zSpawnRange, zSpawnRange));
        GameObject spawnedObj = objectPooler.SpawnFromPool("Cash", position, Quaternion.identity);
        NetworkServer.Spawn(spawnedObj);
    }
}
