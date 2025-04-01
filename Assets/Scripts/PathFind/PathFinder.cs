using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathFinder
{
    public LayerMask m_LayerMask; 
    Vector3[] m_PathFinderDirections = new Vector3[]
    {
        Vector3.forward,
        -Vector3.forward,
        -Vector3.right,
        Vector3.right,
        (-Vector3.right + Vector3.forward).normalized,
        (Vector3.right + Vector3.forward).normalized,
        (-Vector3.right + -Vector3.forward).normalized,
        (Vector3.right + -Vector3.forward).normalized
    };

    public struct FPathFinderNode
    {
        public Vector3 Position;
        public float DistanceFromStart;
        public float DistanceToTarget;

        public FPathFinderNode(Vector3 position, float distanceFromStart, float distanceToTarget)
        {
            Position = position;
            DistanceFromStart = distanceFromStart;
            DistanceToTarget = distanceToTarget;
        }


    }

    [SerializeField] public int m_PathFinderSize = 10; 
    [SerializeField] public float m_MaxDistance= 10f; 
    [SerializeField] public float m_Finderheight = 1f; 
    

    public void  FindingBestInBetweenPositionGizmos(Vector3 startPos, Vector3 targetPos,out List<FPathFinderNode> closeNodes, out List<FPathFinderNode> farNodes)
    {
        closeNodes = new List<FPathFinderNode>();
        farNodes = new List<FPathFinderNode>();
        Vector3 height = Vector3.up * m_Finderheight;
        for (int i = 0; i <= m_PathFinderSize; i++)
        {
            for (int j = 0; j <= m_PathFinderSize; j++)
            {
               Vector3 pos =  startPos + new Vector3(i- m_PathFinderSize/2, 0, j-m_PathFinderSize/2); 
                
                if(!Physics.Linecast(pos, targetPos,m_LayerMask) 
                    && !Physics.Linecast(pos, startPos, m_LayerMask) 
                    && Physics.OverlapCapsule(startPos + height, pos + height, 0.6f , m_LayerMask).Length < 1
                    && Physics.OverlapCapsule(targetPos + height, pos + height, 0.6f, m_LayerMask).Length < 1)
                {
                    if ((Vector3.Distance(pos, targetPos) + Vector3.Distance(startPos, pos) > m_MaxDistance))
                    {
                        farNodes.Add(new FPathFinderNode(pos, Vector3.Distance(pos, startPos), Vector3.Distance(pos, targetPos)));
                    }
                    else
                    {
                        closeNodes.Add(new FPathFinderNode(pos, Vector3.Distance(pos, startPos), Vector3.Distance(pos, targetPos)));
                    }

                        
                }
                
                
                //nodes.Add(new FPathFinderNode(pos, Vector3.Distance(pos, startPos), Vector3.Distance(pos, targetPos)));
            }
        }
    }

    public Vector3 FindBestLineInNodes(List<FPathFinderNode> nodes)
    {
        FPathFinderNode node = nodes[0];
        float delta = nodes[0].DistanceToTarget + nodes[0].DistanceFromStart;
        foreach (var item in nodes)
        {
            float newDelta = item.DistanceToTarget + item.DistanceFromStart;
            if (newDelta < delta)
            {
                delta = newDelta;
                node = item;
            }
        }
        return node.Position;

    }

    //optimized 

    public bool GetDetourPos(Vector3 startPos, Vector3 targetPos, out Vector3 detourPos )
    {
        List<FPathFinderNode> nodes = new List<FPathFinderNode>();
        detourPos = Vector3.zero;
        Vector3 height = Vector3.up * m_Finderheight;
        for (int i = 0; i <= m_PathFinderSize; i++)
        {
            for (int j = 0; j <= m_PathFinderSize; j++)
            {
                Vector3 pos = startPos + new Vector3(i - m_PathFinderSize / 2, 0, j - m_PathFinderSize / 2);
                if ((Vector3.Distance(pos, targetPos) + Vector3.Distance(startPos, pos)) > m_MaxDistance) continue;



                if (!Physics.Linecast(pos, targetPos, m_LayerMask)
                    && !Physics.Linecast(pos, startPos, m_LayerMask)
                    && Physics.OverlapCapsule(startPos + height, pos + height, 0.6f, m_LayerMask).Length < 1
                    && Physics.OverlapCapsule(targetPos + height, pos + height, 0.6f, m_LayerMask).Length < 1)
                {
                    nodes.Add(new FPathFinderNode(pos, Vector3.Distance(pos, startPos), Vector3.Distance(pos, targetPos)));
                }

            }
        }
        

        if (nodes.Count <= 0)
        {
            
            return false;
        }

        detourPos =  FindBestLineInNodes(nodes);
        return true; 


    }


}
