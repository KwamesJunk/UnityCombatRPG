using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        float waypointRadius = 0.3f;

        for (int i = 0; i < transform.childCount; i++)
        {
            int j = (i + 1) % transform.childCount;

            Gizmos.DrawSphere(getWaypointPosition(i), waypointRadius);
            Gizmos.DrawLine(getWaypointPosition(i), getWaypointPosition(j));
        }
    }

    public Vector3 getWaypointPosition(int i)
    {
        return transform.GetChild(i%transform.childCount).transform.position;
    }
}
