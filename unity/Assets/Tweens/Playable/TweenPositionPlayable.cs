using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ActiveMe
{
    namespace Tweens
    {
        public class TweenPositionPlayable : UITweenerPlayable
        {
            public ExposedReference<Transform> subject;

            private Transform _subject;

            public bool isFirstClipInTrack; // J'utilise ça en attendant de trouver l'option pour voir les autres clips d'une meme track sinon je ne peux pas reset à la position du début de la timeline
            public bool isLocal = true;
            public Vector3 src;
            public Vector3 dst;

            public override void OnGraphStart(Playable playable)
            {
                _subject = subject.Resolve(playable.GetGraph().GetResolver());

                if (_subject != null && isFirstClipInTrack)
                {
                    if (isLocal)
                        _subject.localPosition = src;
                    else
                        _subject.position = src;
                }
            }

            public override void ProcessFrame(Playable playable, FrameData info, object playerData)
            {
                if (_subject == null || playable.GetTime() <= 0)
                    return;

                float f = (float)(playable.GetTime() / playable.GetDuration()) * info.weight;

                if (isLocal)
                    _subject.localPosition = Vector3.Lerp(src, dst, curve.Evaluate(f));
                else
                    _subject.position = Vector3.Lerp(src, dst, curve.Evaluate(f));
            }

            public override void OnBehaviourPlay(Playable playable, FrameData info)
            {
                _subject = subject.Resolve(playable.GetGraph().GetResolver());

                if (_subject == null)
                    return;

                if (isFirstClipInTrack)
                {
                    if (isLocal)
                        _subject.localPosition = Vector3.Lerp(src, dst, curve.Evaluate(0f));
                    else
                        _subject.position = Vector3.Lerp(src, dst, curve.Evaluate(0f));
                }
            }
            public override void OnBehaviourPause(Playable playable, FrameData info)
            {
                if (_subject == null)
                    return;
            }

            public override void OnGraphStop(Playable playable)
            {
                base.OnGraphStop(playable);

                _subject = subject.Resolve(playable.GetGraph().GetResolver());

                if (_subject != null && isFirstClipInTrack)
                {
                    if (isLocal)
                        _subject.localPosition = Vector3.Lerp(src, dst, curve.Evaluate(0f));
                    else
                        _subject.position = Vector3.Lerp(src, dst, curve.Evaluate(0f));
                }
            }
        }

    }
}