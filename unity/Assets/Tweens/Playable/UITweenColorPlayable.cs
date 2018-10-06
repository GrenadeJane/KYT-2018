using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace ActiveMe
{
    namespace Tweens
    {
        public class UITweenColorPlayable : UITweenerPlayable
        {
            public ExposedReference<MaskableGraphic> subject;
            public Color src = Color.white;
            public Color dst = Color.white;

            private MaskableGraphic _subject;

            public override void OnGraphStart(Playable playable)
            {
                _subject = subject.Resolve(playable.GetGraph().GetResolver());
            }

            public override void ProcessFrame(Playable playable, FrameData info, object playerData)
            {
                if (_subject == null || playable.GetTime() <= 0)
                    return;

                float f = (float)(playable.GetTime() / playable.GetDuration()) * info.weight;
                _subject.color = Color.Lerp(src, dst, curve.Evaluate(f));
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