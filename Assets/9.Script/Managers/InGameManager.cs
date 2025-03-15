using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{

    public ResourceData resourceData;
    public static InGameManager instance;




    public Transform spawnPoint;
    public Transform zombieParent;
    public List<Zombie> zombies = new List<Zombie>();

    float spawnTime = 0.9f;
    float tempTime = 0f;

    int zombieCount = 0;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }






    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > spawnTime)
        {
            tempTime = 0f;
            SpawnZombie();
        }

    }




    public void SpawnZombie()
    {
        // 풀링은 여유되면...
        // GameObject zombie = resourceData.zombiePrefab.Spawn(spawnPoint.position, Quaternion.identity);

        if (zombies.Count > 50)
            return;


        GameObject zombie = Instantiate(resourceData.zombiePrefab, spawnPoint.position, Quaternion.identity, zombieParent);
        zombie.name = $"Zombie{zombieCount}";
        zombies.Add(zombie.GetComponent<Zombie>());
        zombieCount++;
    }
}
