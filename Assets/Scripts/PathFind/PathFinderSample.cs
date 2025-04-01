using System.Collections.Generic;
using UnityEngine;

public class PathFinderSample : MonoBehaviour
{
    [SerializeField] PathFinder m_PathFinder = new PathFinder();

    [SerializeField] Transform m_Target; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray; 
        Gizmos.DrawWireSphere(transform.position, m_PathFinder.m_PathFinderSize);
        //Gizmos.color = Color.yellow; 
        //Gizmos.DrawRay(transform.position, Vector3.forward);
        //Gizmos.DrawRay(transform.position, -Vector3.forward);
        //Gizmos.DrawRay(transform.position, -Vector3.right);
        //Gizmos.DrawRay(transform.position, Vector3.right);
        //Gizmos.DrawRay(transform.position,(-Vector3.right + Vector3.forward).normalized);
        //Gizmos.DrawRay(transform.position,(Vector3.right + Vector3.forward).normalized);
        //Gizmos.DrawRay(transform.position,(-Vector3.right + -Vector3.forward).normalized);
        //Gizmos.DrawRay(transform.position,(Vector3.right + -Vector3.forward).normalized);

        

        List<PathFinder.FPathFinderNode> closeNodes = new List<PathFinder.FPathFinderNode>();
        List<PathFinder.FPathFinderNode> farNodes = new List<PathFinder.FPathFinderNode>();
        Gizmos.color = Color.blue;

        m_PathFinder.FindingBestInBetweenPositionGizmos(transform.position, m_Target.position, out closeNodes,out farNodes);
        foreach (var item in closeNodes)
        {
            Gizmos.DrawWireSphere(item.Position, 0.1f); 
        }
        Gizmos.color = Color.red;
        foreach (var item in farNodes)
        {
            Gizmos.DrawSphere(item.Position, 0.1f); 
        }
        //if (closeNodes.Count <= 0) return; 
        //Vector3 bestPos =  m_PathFinder.FindBestLineInNodes(closeNodes);
        if(m_PathFinder.GetDetourPos(transform.position, m_Target.position, out Vector3 bestPos))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, bestPos);
            Gizmos.DrawLine(m_Target.position, bestPos);
        }



    }
}
