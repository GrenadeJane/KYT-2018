using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using ActiveMe.Tweens;

public class TweenerMixerBehaviour : PlayableBehaviour
{
    float blendedFactor = 0;
    float defaultFactor = 0;

    UITweener m_TrackBinding;
    bool m_FirstFrameHappened;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as UITweener;

        if (m_TrackBinding == null)
            return;

        if (!m_FirstFrameHappened)
        {
            m_FirstFrameHappened = true;
        }

        int inputCount = playable.GetInputCount ();

        float totalWeight = 0f;
        int currentInputs = 0;

        blendedFactor = 0;
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<TweenerBehaviour> inputPlayable = (ScriptPlayable<TweenerBehaviour>)playable.GetInput(i);
            TweenerBehaviour input = inputPlayable.GetBehaviour ();

            float normalisedTime = (float)(inputPlayable.GetTime() * input.inverseDuration);
            //Debug.Log(inputWeight);
            blendedFactor += normalisedTime;// * inputWeight;

            totalWeight += inputWeight;

            if (!Mathf.Approximately (inputWeight, 0f))
                currentInputs++;
        }

        blendedFactor += defaultFactor * (1f - totalWeight);

        m_TrackBinding.ProgressNormalized = blendedFactor;
        m_TrackBinding.Animate();
    }

    public override void OnGraphStop (Playable playable)
    {
        m_FirstFrameHappened = false;

        if (m_TrackBinding == null)
            return;

    }
}
