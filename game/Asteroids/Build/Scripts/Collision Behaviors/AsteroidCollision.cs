using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCollision : CollisionBehavior
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Collide(GameObject obj)
    {
        if (obj.GetComponent<Bullet>() != null)
        {
            gameObject.GetComponent<Asteroid>().Break();
        }
    }
}
