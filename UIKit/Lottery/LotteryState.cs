using System;
using System.Collections;
using Gs2.Unity.Gs2Lottery.ScriptableObject;
using Gs2.Unity.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Lottery
{
    public class LotteryState : MonoBehaviour
    {
        public GameObject menu;
        public GameObject drawEffect;
        public GameObject drawResult;
        public GameObject drawSummary;

        public Namespace Namespace;
        
        public enum State
        {
            Menu,
            DrawEffect,
            DrawResult,
            DrawSummary,
        }

        public State state;

        public void Awake() {
            this.state = State.Menu;
            Update();
        }

        public void Update() {
            switch (this.state) {
                case State.Menu:
                    this.menu.SetActive(true);
                    this.drawEffect.SetActive(false);
                    this.drawResult.SetActive(false);
                    this.drawSummary.SetActive(false);
                    break;
                case State.DrawEffect:
                    this.menu.SetActive(false);
                    this.drawEffect.SetActive(true);
                    this.drawResult.SetActive(false);
                    this.drawSummary.SetActive(false);
                    break;
                case State.DrawResult:
                    this.menu.SetActive(false);
                    this.drawEffect.SetActive(false);
                    this.drawResult.SetActive(true);
                    this.drawSummary.SetActive(false);
                    break;
                case State.DrawSummary:
                    this.menu.SetActive(false);
                    this.drawEffect.SetActive(false);
                    this.drawResult.SetActive(false);
                    this.drawSummary.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Next() {
            switch (this.state) {
                case State.Menu:
                    Gs2ClientHolder.Instance.Gs2.Lottery.ClearDrawnResult(this.Namespace);
                    this.state = State.DrawEffect;
                    break;
                case State.DrawEffect:
                    this.state = State.DrawResult;
                    break;
                case State.DrawResult:
                    this.state = State.DrawSummary;
                    break;
                case State.DrawSummary:
                    this.state = State.Menu;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
