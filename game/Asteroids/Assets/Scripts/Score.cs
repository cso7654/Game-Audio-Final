using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Score : MonoBehaviour
{
    int points;
    
    public int scoreMilestone;
    public StudioEventEmitter scoreMilestoneFMOD;

    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            int prevPoints = points;
            points = value;
            if (Mathf.Floor(prevPoints / scoreMilestone) < Mathf.Floor(points / scoreMilestone))
            {
                //Score milestone reached, play sound
                scoreMilestoneFMOD.Play();
            }
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

        GUI.Label(new Rect(10, 10, Screen.width, Screen.height / 20 * 1.5f), "SCORE: " + points);
    }
}
