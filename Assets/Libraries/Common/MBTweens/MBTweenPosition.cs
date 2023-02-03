using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace MBTweens
{
    public class MBTweenPosition : MBTweenBase
    {
        [SerializeField] Vector3 startPosition;
        [SerializeField] Vector3 endPosition;
        [SerializeField] Transform target;
        RectTransform anchorTarget;

        private bool useAnchorTarget;
        
        public Vector3 StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        public Vector3 EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        public Transform Target
        {
            get { return target; }
            set { target = value; }
        }

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = transform;
            }

            CheckAnchorTaget(); 
        }

        private void OnValidate()
        {
            CheckAnchorTaget();
        }
        
        protected override float GetFactor(int startFactor)
        {
            if (smooth)
            {
                Vector3 current = GetPosition();
                var factor = Vector3.Distance(startPosition, current) / Vector3.Distance(endPosition, startPosition);
                return factor;
            }
            
            return startFactor;
        }

        private void CheckAnchorTaget()
        {
            anchorTarget = target as RectTransform;
            useAnchorTarget = anchorTarget != null;
        }
        
        protected Vector3 GetPosition()
        {
            if (useAnchorTarget)
            {
                return anchorTarget.anchoredPosition;
            }
            else 
            {
                return target.localPosition;
            }
        }

        protected override void UpdateTweenWithFactor(float factor)
        {
            if (useAnchorTarget)
            {
                anchorTarget.anchoredPosition = startPosition + (endPosition - startPosition) * factor;
            }
            else 
            {
                target.localPosition = startPosition + (endPosition - startPosition) * factor;
            }

        }

        public static MBTweenPosition MoveTo(Transform obj, Vector3 targetPos, float duration, float delay = 0f)
        {
            MBTweenPosition tween = obj.GetComponent<MBTweenPosition>();
            if (tween == null)
            {
                tween = obj.gameObject.AddComponent<MBTweenPosition>();
            }

            tween.OnBeginStateSet = new UnityEvent();
            tween.OnEndStateSet = new UnityEvent();
            tween.EndPosition = targetPos;
            tween.StartPosition = obj.transform.localPosition;

            tween.target = obj;
            tween.SetEndState(delay, duration);

            return tween;
        }

        public static MBTweenPosition MoveToUI(RectTransform tr, Vector3 targetPos, float duration, float delay = 0f)
        {
            MBTweenPosition tween = tr.GetComponent<MBTweenPosition>();
            if (tween == null)
            {
                tween = tr.gameObject.AddComponent<MBTweenPosition>();
            }

            tween.OnBeginStateSet = new UnityEvent();
            tween.OnEndStateSet = new UnityEvent();
            tween.EndPosition = targetPos;
            tween.StartPosition = tr.anchoredPosition;

            tween.anchorTarget = tr;
            tween.useAnchorTarget = true;
            tween.SetEndState(delay, duration);

            return tween;
        }
    }
}