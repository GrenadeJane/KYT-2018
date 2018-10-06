using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmObjectCop : SwarmObject
{


    #region Properties

    bool _goesToUniqueTarget;
    public bool GoesToUniqueTarget
    {
       get { return _goesToUniqueTarget;  }     
       set
        {
            bool previousState = _goesToUniqueTarget;
            _goesToUniqueTarget = value;
            if (previousState != value)
                DrivenBySwarmMovement = !value;
            }
    }
    public FestBeeSwarm SwarmTarget;

    #endregion

    public void AssignNewTarget(Vector3 position )
    {
        RelativeTargetPosition = position;
        GoesToUniqueTarget = true;
    }
}
