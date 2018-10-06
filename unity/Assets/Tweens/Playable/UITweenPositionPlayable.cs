using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace ActiveMe
{
    namespace Tweens
    {
        public class UITweenPositionPlayable : UITweenerPlayable
        {
            [System.Serializable]
            public class LerpVector
            {
                public float src;
                public float dst;
            }

            public ExposedReference<RectTransform> subject;

            public enum Mode
            {
                Auto,
                Position,
            }

            public LerpVector offsetMinX;
            public LerpVector offsetMinY;
            public LerpVector offsetMaxX;
            public LerpVector offsetMaxY;

            public LerpVector posX;
            public LerpVector posY;
            public Mode mode = Mode.Auto;

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
                Vector2 offsetMin = _subject.offsetMin;
                Vector2 offsetMax = _subject.offsetMax;
                Vector2 anchoredPosition = _subject.anchoredPosition;

                float curveFactor = curve.Evaluate(f);

                if (AnchorTool.IsStretchedHorizontally(_subject) && mode == Mode.Auto)
                {
                    offsetMin.x = Mathf.Lerp(offsetMinX.src, offsetMinX.dst, curveFactor);
                    offsetMax.x = -Mathf.Lerp(offsetMaxX.src, offsetMaxX.dst, curveFactor);
                    anchoredPosition.x = offsetMin.x * 0.5f + offsetMax.x * 0.5f;
                }
                else
                    anchoredPosition.x = Mathf.Lerp(posX.src, posX.dst, curveFactor);

                if (AnchorTool.IsStretchedVertically(_subject) && mode == Mode.Auto)
                {
                    offsetMin.y = Mathf.Lerp(offsetMinY.src, offsetMinY.dst, curveFactor);
                    offsetMax.y = -Mathf.Lerp(offsetMaxY.src, offsetMaxY.dst, curveFactor);
                    anchoredPosition.y = offsetMin.y * 0.5f + offsetMax.y * 0.5f;
                }
                else
                    anchoredPosition.y = Mathf.Lerp(posY.src, posY.dst, curveFactor);

                _subject.offsetMin = offsetMin;
                _subject.offsetMax = offsetMax;
                _subject.anchoredPosition = anchoredPosition;
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

            public void SetSrc(Vector2 src)
            {
                posX.src = src.x;
                posY.src = src.y;
            }

            public void SetDst(Vector2 dst)
            {
                posX.dst = dst.x;
                posY.dst = dst.y;
            }

            public float LeftSrc
            {
                get
                {
                    return offsetMinX.src;
                }

                set
                {
                    offsetMinX.src = value;
                }
            }

            public float RightSrc
            {
                get
                {
                    return offsetMaxX.src;
                }

                set
                {
                    offsetMaxX.src = value;
                }
            }

            public float TopSrc
            {
                get
                {
                    return offsetMaxY.src;
                }

                set
                {
                    offsetMaxY.src = value;
                }
            }

            public float BottomSrc
            {
                get
                {
                    return offsetMinY.src;
                }

                set
                {
                    offsetMinY.src = value;
                }
            }

            public float LeftDst
            {
                get
                {
                    return offsetMinX.dst;
                }

                set
                {
                    offsetMinX.dst = value;
                }
            }

            public float RightDst
            {
                get
                {
                    return offsetMaxX.dst;
                }

                set
                {
                    offsetMaxX.dst = value;
                }
            }

            public float TopDst
            {
                get
                {
                    return offsetMaxY.dst;
                }

                set
                {
                    offsetMaxY.dst = value;
                }
            }

            public float BottomDst
            {
                get
                {
                    return offsetMinY.dst;
                }

                set
                {
                    offsetMinY.dst = value;
                }
            }

            public float Left
            {
                get
                {
                    return _subject.offsetMin.x;
                }
            }

            public float Right
            {
                get
                {
                    return -_subject.offsetMax.x;
                }
            }

            public float Top
            {
                get
                {
                    return -_subject.offsetMax.y;
                }
            }

            public float Bottom
            {
                get
                {
                    return _subject.offsetMin.y;
                }
            }

            public void SetLeft(float src, float dst)
            {
                offsetMinX.src = src;
                offsetMinX.dst = dst;
            }

            public void SetRight(float src, float dst)
            {
                offsetMaxX.src = src;
                offsetMaxX.dst = dst;
            }

            public void SetTop(float src, float dst)
            {
                offsetMaxY.src = src;
                offsetMaxY.dst = dst;
            }

            public void SetBottom(float src, float dst)
            {
                offsetMinY.src = src;
                offsetMinY.dst = dst;
            }

            public Vector2 Dst
            {
                get
                {
                    Vector2 offsetMin = _subject.offsetMin;
                    Vector2 offsetMax = _subject.offsetMax;
                    Vector2 anchoredPosition = _subject.anchoredPosition;

                    if (AnchorTool.IsStretchedHorizontally(_subject) && mode == Mode.Auto)
                    {
                        offsetMin.x = offsetMinX.dst;
                        offsetMax.x = -offsetMaxX.dst;
                        anchoredPosition.x = offsetMin.x * 0.5f + offsetMax.x * 0.5f;
                    }
                    else
                        anchoredPosition.x = posX.dst;

                    if (AnchorTool.IsStretchedVertically(_subject) && mode == Mode.Auto)
                    {
                        offsetMin.y = offsetMinY.dst;
                        offsetMax.y = -offsetMaxY.dst;
                        anchoredPosition.y = offsetMin.y * 0.5f + offsetMax.y * 0.5f;
                    }
                    else
                        anchoredPosition.y = posY.dst;

                    return anchoredPosition;
                }
            }

            public Vector2 Src
            {
                get
                {
                    Vector2 offsetMin = _subject.offsetMin;
                    Vector2 offsetMax = _subject.offsetMax;
                    Vector2 anchoredPosition = _subject.anchoredPosition;

                    if (AnchorTool.IsStretchedHorizontally(_subject) && mode == Mode.Auto)
                    {
                        offsetMin.x = offsetMinX.src;
                        offsetMax.x = -offsetMaxX.src;
                        anchoredPosition.x = offsetMin.x * 0.5f + offsetMax.x * 0.5f;
                    }
                    else
                        anchoredPosition.x = posX.src;

                    if (AnchorTool.IsStretchedVertically(_subject) && mode == Mode.Auto)
                    {
                        offsetMin.y = offsetMinY.src;
                        offsetMax.y = -offsetMaxY.src;
                        anchoredPosition.y = offsetMin.y * 0.5f + offsetMax.y * 0.5f;
                    }
                    else
                        anchoredPosition.y = posY.src;

                    return anchoredPosition;
                }
            }
        }
    }
}