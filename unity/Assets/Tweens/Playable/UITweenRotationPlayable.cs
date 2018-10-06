using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace ActiveMe
{
    namespace Tweens
    {
        public class UITweenRotationPlayable : UITweenerPlayable
        {
            public ExposedReference<RectTransform> subject;
            public bool isLocal = true;
            public bool useQuaternion = true;
            public Vector3 src;
            public Vector3 dst;

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
                if (useQuaternion)
                {
                    if (isLocal)
                        _subject.localRotation = Quaternion.Lerp(Quaternion.Euler(src), Quaternion.Euler(dst), curve.Evaluate(f));
                    else
                        _subject.rotation = Quaternion.Lerp(Quaternion.Euler(src), Quaternion.Euler(dst), curve.Evaluate(f));
                }
                else
                {
                    if (isLocal)
                        _subject.localEulerAngles = Vector3.Lerp(src, dst, curve.Evaluate(f));
                    else
                        _subject.eulerAngles = Vector3.Lerp(src, dst, curve.Evaluate(f));
                }
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