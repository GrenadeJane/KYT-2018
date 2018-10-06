using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmBaseCop : SwarmBase
{
    public SwarmObjectCop GetRandomnlyObject()
    {
        List<SwarmObject> objects = new List<SwarmObject>(SwarmObjects);
        while(objects.Count > 0)
        {
            SwarmObjectCop obj = objects[Random.Range(0, objects.Count - 1)] as SwarmObjectCop;
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
        foreach (SwarmObject swarmObject in SwarmObjects)
        {
            if (swarmObject.ReachedTargetPosition && !((SwarmObjectCop) swarmObject).GoesToUniqueTarget)
            {
                swarmObject.RelativeTargetPosition = Random.insideUnitSphere * m_SwarmRadius;
            }

            swarmObject.UpdatePosition();
        }
    }
}
