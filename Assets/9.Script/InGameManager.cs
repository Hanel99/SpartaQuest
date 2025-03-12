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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
