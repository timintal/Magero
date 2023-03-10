using UnityEngine;

namespace MBTweens
{
    public class MBTweenRotationEulers : MBTweenBase
    {

        #region Fields

        [SerializeField] Vector3 startRotation;
        [SerializeField] Vector3 endRotation;

        [SerializeField] Transform target;

        #endregion


        #region Properties

        public Vector3 StartRotation
        {
            get { return startRotation; }
            set { startRotation = value; }
        }

        public Vector3 EndRotation
        {
            get { return endRotation; }
            set { endRotation = value; }
        }

        public Transform Target => target;

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

        public static MBTweenRotationEulers RotateTo(Transform target, Vector3 rotation, float duration)
        {
            MBTweenRotationEulers tween = target.GetComponent<MBTweenRotationEulers>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<MBTweenRotationEulers>();
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
            target.localRotation = Quaternion.Euler(Vector3.LerpUnclamped(startRotation, endRotation, factor));
        }

        #endregion



        #region Event Handlers

        #endregion
    }
}