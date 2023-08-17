using System;
using System.Collections;
using Gs2.Unity.Gs2Lottery.ScriptableObject;
using Gs2.Unity.UiKit.Gs2Lottery.Context;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scenes.Samples.UIKit.Lottery
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(FadeEffect))]
    public class FadeActivator : MonoBehaviour
    {
        private Gs2LotteryOwnDrawnPrizeContext _context;
        private FadeEffect _fadeEffect;
        private DateTime _startAt;

        public void Awake() {
            this._context = GetComponent<Gs2LotteryOwnDrawnPrizeContext>() ?? GetComponentInParent<Gs2LotteryOwnDrawnPrizeContext>(true);
            this._fadeEffect = GetComponent<FadeEffect>();
        }

        public void OnEnable() {
            this._startAt = DateTime.Now;
            this._fadeEffect.enabled = false;

            var image = GetComponent<Image>();
            var tmp = image.color;
            tmp.a = 0f;
            image.color = tmp;
        }

        public void Update() {
            if (DateTime.Now - this._startAt > TimeSpan.FromSeconds(this._context.DrawnPrize.Index * 0.3f)) {
                this._fadeEffect.enabled = true;
            }
        }
    }
}
