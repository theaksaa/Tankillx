using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    private GameObject TankRed;
    private GameObject TankBlue;

    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        TankRed = GameObject.Find("TankRed");
        TankBlue = GameObject.Find("TankBlue");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
