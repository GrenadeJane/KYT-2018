using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TweenerClip : PlayableAsset, ITimelineClipAsset
{
    public TweenerBehaviour template = new TweenerBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TweenerBehaviour>.Create (graph, template);
        return playable;
    }
}
