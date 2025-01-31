using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Lottery
{
    public class ScaleEffect : MonoBehaviour
    {
        public float startScale;
        public float targetScale;
        public float effectSeconds;

        public UnityEvent onComplete = new UnityEvent();
        
        private float _elapsedSeconds;

        public void Start() {
            IEnumerator Impl() {
                while (Math.Abs(this.effectSeconds - this._elapsedSeconds) > 0.01f) {
                    yield return null;
                }
                this.onComplete.Invoke();
            }

            StartCoroutine(Impl());
        }

        public void OnEnable() {
            this._elapsedSeconds = 0f;
        }

        public void Update() {
            this._elapsedSeconds += Time.deltaTime;
            if (this._elapsedSeconds >= this.effectSeconds) {
                this._elapsedSeconds = this.effectSeconds;
            }
            var scale = (this._elapsedSeconds / this.effectSeconds) * (this.targetScale - this.startScale) + this.startScale;
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
