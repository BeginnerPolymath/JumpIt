using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DotScript : MonoBehaviour
{
    public float LifeTime = 1f;



    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;

        if(LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
