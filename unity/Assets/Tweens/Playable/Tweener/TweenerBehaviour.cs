using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class TweenerBehaviour : PlayableBehaviour
{
    public float inverseDuration;

    public override void OnGraphStart(Playable playable)
    {
        double duration = playable.GetDuration();
        if (Mathf.Approximately((float)duration, 0f))
            throw new UnityException("A TransformTween cannot have a duration of zero.");

        inverseDuration = 1f / (float)duration;
    }
}
