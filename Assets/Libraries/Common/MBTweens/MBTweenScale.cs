using UnityEngine;
using UnityEngine.Events;

namespace MBTweens
{
    public class MBTweenScale : MBTweenBase
    {
        #region Fields

        [SerializeField] public Vector3 startScale;
        [SerializeField] public Vector3 endScale;

        [SerializeField] private Transform target;

        #endregion


        #region Properties

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        
        
        protected virtual Vector3 Scale
        {
            get => target.localScale;
            set => target.localScale = value;
        }
        #endregion


        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = transform;
            }
        }

        #endregion


        #region Public Methods

        public static MBTweenScale ScaleTo(Transform target, Vector3 scale, float duration)
        {
            MBTweenScale tween = target.GetComponent<MBTweenScale>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<MBTweenScale>();
            }

            tween.OnBeginStateSet = new UnityEvent();
            tween.OnEndStateSet = new UnityEvent();

            tween.startScale = target.localScale;
            tween.endScale = scale;
            tween.target = target;

            tween.SetEndState(0, duration);

            return tween;
        }

        public static MBTweenScale ScaleTo(Transform target, Vector3 scale, float duration, float delay)
        {
            MBTweenScale tween = target.GetComponent<MBTweenScale>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<MBTweenScale>();
            }

            tween.OnBeginStateSet = new UnityEvent();
            tween.OnEndStateSet = new UnityEvent();

            tween.startScale = target.localScale;
            tween.endScale = scale;
            tween.delay = delay;
            tween.target = target;

            tween.SetEndState(delay, duration);

            return tween;
        }

        #endregion


        #region Private Methods
                
        protected override float GetFactor(int startFactor)
        {
            if (smooth)
            {
                Vector3 current = target.localScale;
                var factor = Vector3.Distance(startScale, current) / Vector3.Distance(endScale, startScale);
                return factor;
            }
            
            return startFactor;
        }

        protected override void UpdateTweenWithFactor(float factor)
        {
            Scale = startScale + (endScale - startScale) * factor;
        }

        #endregion


    }
}
