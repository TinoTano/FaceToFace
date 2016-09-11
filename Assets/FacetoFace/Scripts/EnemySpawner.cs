using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;
    public int numEnemies;

    public override void OnStartServer()
    {
        for (int i = 0; i < numEnemies; i++)
        {
            Vector3 position = new Vector3(Random.Range(-8f, 8f), Random.Range(5f, 0), 0);
            Quaternion rotation = Quaternion.Euler(Vector3.zero);
            GameObject enemy = (GameObject)Instantiate(enemyPrefab,position,rotation);
            NetworkServer.Spawn(enemy);
        }
    }
}
