using Coherence.Toolkit;
using PlayerControls;
using UnityEngine;

public static class Utils
{
   
    //shuffle any List<> 
    public static void Shuffle<T>(this System.Collections.Generic.List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static T RandomInList<T>(this System.Collections.Generic.List<T> list)
    {
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }


    public static bool IsInAngle(this UnityEngine.Transform centerTransform,  float degAngle,Vector3 targetPos)
    {
        float dot = Vector3.Dot(centerTransform.forward.normalized, (targetPos - centerTransform.position).normalized);


        float dotInDeg = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (dotInDeg <= degAngle)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    public static bool GetSimulatorSync(out CoherenceSync sync)
    {
        //MainSimulator simulator = FindFirstObjectByType<MainSimulator>(FindObjectsInactive.Exclude);
        MainSimulator simulator = UnityEngine.Object.FindFirstObjectByType<MainSimulator>(UnityEngine.FindObjectsInactive.Exclude);

        if (simulator == null)
        {
            sync = null; 
            Debug.Log("simulator not found");
            return false;
        }

        sync = simulator.GetComponent<CoherenceSync>();

        return true;
    }

    public static bool GetSimulator(out MainSimulator simulator)
    {
        
        simulator = UnityEngine.Object.FindFirstObjectByType<MainSimulator>(UnityEngine.FindObjectsInactive.Exclude);

        if (simulator == null)
        {
            //Debug.Log("simulator not found");
            return false;
        }

        return true; 
    }

    public static bool GetSimulatorLocal(out MainSimulator simulator)
    {

        simulator = ConnectionsHandler.Instance.Main_Simulator; 

        if (simulator == null)
        {
            Debug.Log("simulator not found");
            return false;
        }

        return true;
    }


    public static Vector3 VectorTowardsTransform(this UnityEngine.Transform from, Transform to)
    {
        return (to.position - from.position).normalized;
    }
    public static Vector3 VectorTowardsTransform(this UnityEngine.Transform from, Transform to, bool isFlat = true)
    {
        Vector3 direction = (to.position - from.position).normalized;
        if (isFlat) direction.y = 0;
        return direction;
    }

    public static Vector3 VectorTowardsPosition(this UnityEngine.Transform from, Vector3 to)
    {
        return (to - from.position).normalized;
    }

    public static Vector3 VectorTowardsPosition(this UnityEngine.Transform from, Vector3 to, bool isFlat = true)
    {
        Vector3 direction = (to - from.position).normalized;
        if (isFlat) direction.y = 0;
        return direction;
    }

    public static Collider FindClosestCollider(Vector3 pos, Collider[] colliders)
    {
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(pos, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        return closestCollider;
    }

    public static Collider FindClosestCollider(Vector3 pos, Collider[] colliders,Transform MasterMask)
    {
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if(collider.transform == MasterMask)
            {
                continue;
            }
            if (collider.TryGetComponent<TinyPlayer>(out TinyPlayer player))
            {
                //Debug.Log("tinyplayer found");
                if (player.m_IntPlayerState != 0)
                {
                    //Debug.Log("tinyplayer not in play ");
                    continue;
                }
            }
            float distance = Vector3.Distance(pos, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        
        return closestCollider;
    }

    public static Collider FindClosestCollider(Vector3 pos, Collider[] colliders,int gameIDMask)
    {
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent<Entity>(out Entity entity))
            {
                if(entity.GameID == gameIDMask)
                {
                    continue;
                }
            }
            if (collider.TryGetComponent<TinyPlayer>(out TinyPlayer player))
            {
                if (player.m_IntPlayerState != 0)
                {
                    continue;
                }
            }
            float distance = Vector3.Distance(pos, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = collider;
            }
        }

        return closestCollider;
    }

    public static Vector3 RotateAround(Vector3 pos, Vector3 targetPos, Vector3 axis, float angle)
    {
        Vector3 dir = pos - targetPos;
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        Vector3 newDir = rot * dir;
        Vector3 myPos = targetPos + newDir;

        return myPos;
    }







}
