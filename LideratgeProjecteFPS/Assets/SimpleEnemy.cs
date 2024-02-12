using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SimpleEnemy : MonoBehaviour, IShootable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool HandleShooted()
    {
        Debug.Log("OUCH!");
        return true;
    }
}
