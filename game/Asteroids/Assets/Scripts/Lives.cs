using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    int lifeCount = 3;

    public int LifeCount
    {
        get
        {
            return lifeCount;
        }
        set
        {
            lifeCount = value;
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

    private void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.height / 20;

        GUI.Label(new Rect(10, 20 + Screen.height / 20 * 1.5f, Screen.width, Screen.height / 20 * 1.5f), "LIVES: " + lifeCount);
    }
}
