using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBaseCop : SwarmBase<SwarmObjectCop>
{
    public bool IsMovedByUser = false;

    public SwarmObjectCop GetRandomnlyObject()
    {
        List<SwarmObjectCop> objects = new List<SwarmObjectCop>(SwarmObjects);
        while(objects.Count > 0)
        {
            SwarmObjectCop obj = objects[Random.Range(0, objects.Count - 1)];
            // :: if the cop is already busy
            if (obj.GoesToUniqueTarget)
                objects.Remove(obj);
            else
                return obj;
        }
        return null;
    }


    protected override void UpdateSwarmObjects()
    {
        // Updating the swarm objects
        foreach ( SwarmObjectCop swarmObject in SwarmObjects)
        {
            if (swarmObject.ReachedTargetPosition && !(swarmObject.GoesToUniqueTarget))
            {
                swarmObject.RelativeTargetPosition = Random.insideUnitSphere * m_SwarmRadius;
            }

            swarmObject.UpdatePosition();
        }
    }

    protected override void Update()
    {
        if (IsMovedByUser)
            UpdateSwarmObjects();
        else
            base.Update();
    }
}
