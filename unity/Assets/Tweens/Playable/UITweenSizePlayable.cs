using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace ActiveMe
{
    namespace Tweens
    {
        public class UITweenSizePlayable : UITweenerPlayable
        {
            public ExposedReference<RectTransform> subject;

            [Header("Act as an offset if stretched")]
            public Vector3 src = Vector3.one;
            public Vector3 dst = Vector3.one;

            private RectTransform _subject;

            public override void OnGraphStart(Playable playable)
            {
                _subject = subject.Resolve(playable.GetGraph().GetResolver());
            }

            public override void ProcessFrame(Playable playable, FrameData info, object playerData)
            {
                if (_subject == null || playable.GetTime() <= 0)
                    return;

                float f = (float)(playable.GetTime() / playable.GetDuration()) * info.weight;
                _subject.sizeDelta = Vector3.Lerp(src, dst, curve.Evaluate(f));
            }

            public override void OnBehaviourPlay(Playable playable, FrameData info)
            {
                if (_subject == null)
                    return;
            }
            public override void OnBehaviourPause(Playable playable, FrameData info)
            {
                if (_subject == null)
                    return;
            }
        }
    }
}
