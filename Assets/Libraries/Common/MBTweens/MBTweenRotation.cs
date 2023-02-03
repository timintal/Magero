using UnityEngine;

namespace MBTweens
{
    public class MBTweenRotation : MBTweenBase
    {

        #region Fields

        [SerializeField] Vector3 startRotation;
        [SerializeField] Vector3 endRotation;

        [SerializeField] Transform target;

        #endregion


        #region Properties

        public Vector3 StartRotation
        {
            get => startRotation;
            set => startRotation = value;
        }

        public Vector3 EndRotation
        {
            get => endRotation;
            set => endRotation = value;
        }

        public Transform Target
        {
            get => target;
            set => target = value;
        }

        #endregion


        #region Unity Lifecycle

        protected override void Awake()
        {
            if (target == null)
            {
                target = transform;
            }

            base.Awake();
        }

        #endregion


        #region Public Methods

        public static MBTweenRotation RotateTo(Transform target, Vector3 rotation, float duration)
        {
            MBTweenRotation tween = target.GetComponent<MBTweenRotation>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<MBTweenRotation>();
            }

            tween.startRotation = target.localRotation.eulerAngles;
            tween.endRotation = rotation;
            tween.SetEndState(0, duration);

            return tween;
        }

        #endregion


        #region Private Methods

        protected override void UpdateTweenWithFactor(float factor)
        {
            target.localRotation =
                Quaternion.LerpUnclamped(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), factor);
        }

        #endregion



        #region Event Handlers

        #endregion
    }
}