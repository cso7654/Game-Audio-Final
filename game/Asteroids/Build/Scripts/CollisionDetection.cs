using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public List<GameObject> objects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {    
        //Loop through each object to check collisions
        for (int i = 0; i < objects.Count; i++)
        {
            GameObject currentObject = objects[i];
            Bounds currentBounds = currentObject.GetComponent<SpriteRenderer>().bounds;

            //Loop through all other objects and check with the current object's position
            for (int j = i + 1; j < objects.Count; j++)
            {
                GameObject comparingObject = objects[j];
                Bounds comparingBounds = comparingObject.GetComponent<SpriteRenderer>().bounds;

                //check collisions
                if (BoundsAreColliding(currentBounds, comparingBounds, true))
                {
                    //Do collision stuff
                    currentObject.GetComponent<CollisionBehavior>().Collide(comparingObject);
                    comparingObject.GetComponent<CollisionBehavior>().Collide(currentObject);
                }
            }
        }
    }

    public bool BoundsAreColliding(Bounds o1, Bounds o2, bool circle)
    {
        if (circle)
        {
            //Use circle method

            //Get radius by averaging the size of the bounds and dividing by 2
            float o1Radius = ((o1.size.x + o1.size.y) / 2f) / 2f;
            float o2Radius = ((o2.size.x + o2.size.y) / 2f) / 2f;

            //Get dist between x and y
            float xDist = o1.center.x - o2.center.x;
            float yDist = o1.center.y - o2.center.y;
            //Get total distance between circles
            float dist = Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));

            //Return whether the distance between the centers is less than the combined radius
            return (dist < o1Radius + o2Radius);
        }
        else
        {
            //Use AABB method

            //Get the upper left corners
            Vector2 o1Corner = new Vector2(o1.center.x - o1.size.x / 2, o1.center.y - o1.size.y);
            Vector2 o2Corner = new Vector2(o2.center.x - o2.size.x / 2, o2.center.y - o2.size.y);

            //compare and return
            return (o1Corner.x < o2Corner.x + o2.size.x &&
                    o1Corner.x + o1.size.x > o2Corner.x &&
                    o1Corner.y < o2Corner.y + o2.size.y &&
                    o1Corner.y + o1.size.y > o2Corner.y);
        }
    }
}
