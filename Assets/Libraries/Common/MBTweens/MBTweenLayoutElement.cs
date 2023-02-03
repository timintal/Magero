using UnityEngine;
using UnityEngine.UI;

namespace MBTweens
{
    public class MBTweenLayoutElement : MBTweenBase
    {
        #region Fields

        [SerializeField] Vector2 startFlexible;
        [SerializeField] Vector2 endFlexible;
        
        [SerializeField] Vector2 startPrefrred;
        [SerializeField] Vector2 endPrefrred;
        
        [SerializeField] Vector2 startMin;
        [SerializeField] Vector2 endMin;

        [SerializeField] private LayoutElement target;
        
        #endregion

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

            if (target == null)
            {
                target = GetComponent<LayoutElement>();
            }

        }

        #endregion


        #region Public Methods

        public static MBTweenLayoutElement ChangeLayoutTo(LayoutElement target, Vector2 flex, Vector2 preffered, Vector2 min, float duration)
        {
            MBTweenLayoutElement tween = target.GetComponent<MBTweenLayoutElement>();
            if (tween == null)
            {
                tween = target.gameObject.AddComponent<MBTweenLayoutElement>();
            }

            tween.startFlexible = new Vector2(target.flexibleWidth, target.flexibleHeight);
            tween.endFlexible = flex;
            tween.startPrefrred = new Vector2(target.preferredWidth, target.preferredHeight);
            tween.endPrefrred = preffered;
            tween.startMin = new Vector2(target.minWidth, target.minHeight);
            tween.endMin = min;
            tween.SetEndState(0, duration);

            return tween;
        }

        #endregion


        #region Private Methods

        

        protected override void UpdateTweenWithFactor(float factor)
        {
            if (startFlexible != endFlexible)
            {
                target.flexibleWidth = Mathf.Lerp(startFlexible.x, endFlexible.x, factor);
                target.flexibleHeight = Mathf.Lerp(startFlexible.y, endFlexible.y, factor);
            }

            if (startPrefrred != endPrefrred)
            {
                target.preferredWidth = Mathf.Lerp(startPrefrred.x, endPrefrred.x, factor);
                target.preferredHeight = Mathf.Lerp(startPrefrred.y, endPrefrred.y, factor);
            }

            if (startMin != endMin)
            {
                target.minWidth = Mathf.Lerp(startMin.x, endMin.x, factor);
                target.minHeight = Mathf.Lerp(startMin.y, endMin.y, factor);
            }

        }

        #endregion


    }
}