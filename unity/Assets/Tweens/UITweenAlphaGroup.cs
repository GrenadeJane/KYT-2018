using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace ActiveMe
{
    namespace Tweens
    {
        [RequireComponent(typeof(CanvasGroup))]
        public class UITweenAlphaGroup : UITweener
        {
            [SerializeField]
            public float src = 0;
            [SerializeField]
            public float dst = 1;

            private CanvasGroup canvasGroup;

            private void Start()
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            public override void ResetAtBeginning()
            {
                if (canvasGroup == null)
                    canvasGroup = GetComponent<CanvasGroup>();
                base.ResetAtBeginning();
                canvasGroup.alpha = src;
            }

            public override void ResetAtTheEnd()
            {
                if (canvasGroup == null)
                    canvasGroup = GetComponent<CanvasGroup>();
                base.ResetAtTheEnd();
                canvasGroup.alpha = dst;
            }

            public override void Animate()
            {
                base.Animate();

                canvasGroup.alpha = Mathf.Lerp(src, dst, curve.Evaluate(factor));
            }

            private void Update()
            {
                // Update factor over time
                FactorUpdate();

                //Do Tween
                Animate();

                // Check if factor is "< 0" or "> 1"
                CheckEndTween();
            }

        }
    }
}
