using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public CollisionBehavior collisionBehavior;

    public CollisionBehavior CollisionBehavior
    {
        get
        {
            return collisionBehavior;
        }
        set
        {
            collisionBehavior = value;
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
