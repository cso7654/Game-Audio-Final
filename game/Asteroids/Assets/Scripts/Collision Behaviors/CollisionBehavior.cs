using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Collide(GameObject obj);
}
