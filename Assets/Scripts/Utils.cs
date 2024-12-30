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
   
}
