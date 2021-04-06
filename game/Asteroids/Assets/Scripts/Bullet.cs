using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 position;
    public Vector3 direction;
    public Vector3 velocity;

    public float speed = 0.25f;

    public GameObject collisionManager;

    bool destroy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        position += velocity;

        transform.position = position;

        float zAngle = Mathf.Atan2(direction.y, direction.x);
        zAngle *= Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zAngle);

        //Clearing
        float edgeX = Camera.main.GetComponent<PlayField>().width / 2;
        float edgeY = Camera.main.GetComponent<PlayField>().height / 2;
        float deltaX = Camera.main.GetComponent<PlayField>().center.x;
        float deltaY = Camera.main.GetComponent<PlayField>().center.y;

        if (position.x > edgeX + 1 - deltaX ||
            position.x < -(edgeX + 1) - deltaX ||
            position.y > edgeY + 1 - deltaY ||
            position.y < -(edgeY + 1) - deltaY)
        {
            destroy = true;
        }

        if (destroy)
        {
            collisionManager.GetComponent<CollisionDetection>().objects.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        destroy = true;
    }
}
