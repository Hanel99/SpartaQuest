using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{

    public ResourceData resourceData;
    public static InGameManager instance;




    public Transform spawnPoint;
    public List<Zombie> zombies = new List<Zombie>();



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

        for (int i = 0; i < 10; i++)
        {
            SpawnZombie();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void SpawnZombie()
    {
        // GameObject zombie = resourceData.zombiePrefab.Spawn(spawnPoint.position, Quaternion.identity);

        GameObject zombie = Instantiate(resourceData.zombiePrefab, spawnPoint.position, Quaternion.identity);
        zombies.Add(zombie.GetComponent<Zombie>());
    }
}
