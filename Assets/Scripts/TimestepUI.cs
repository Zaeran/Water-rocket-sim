using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimestepUI : MonoBehaviour
{
    public LineRenderer r;

    public void RenderTimesteps(List<Timestep> steps)
    {
        r.positionCount = steps.Count + 1;
        r.SetPosition(0, Vector3.zero);
        for(int i = 0; i < steps.Count; i++)
        {
            r.SetPosition(i + 1, new Vector3(steps[i].Time * 5, steps[i].Speed / 10, 0));
        }
    }

}
