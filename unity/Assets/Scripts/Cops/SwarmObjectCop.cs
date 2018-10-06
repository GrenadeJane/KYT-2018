﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmObjectCop : SwarmObject
{


    #region Properties

    public bool GoesToUniqueTarget;
    public FestBeeSwarm SwarmTarget;

    #endregion

    public void AssignNewTarget(Vector3 position )
    {
        RelativeTargetPosition = position;
        GoesToUniqueTarget = true;
    }
}