using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointLister : MonoBehaviour
{
    [SerializeField]
    private WapointManager wm;
    private Transform tf;//stores the transform of the waypoint
    // Start is called before the first frame update
    void Start()
    {
        if (!tf)
        {
            tf = GetComponent<Transform>(); //sets the transform of the waypoint
        }
        if (wm)
        {
            wm.waypoints.Add(tf);//adds the transform of the waypoint to the list of waypoints
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
