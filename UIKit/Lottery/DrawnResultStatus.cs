using System;
using System.Linq;
using Gs2.Unity.Gs2Lottery.ScriptableObject;
using Gs2.Unity.UiKit.Gs2Lottery.Context;
using Gs2.Unity.UiKit.Gs2Lottery.Fetcher;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Lottery
{
    [RequireComponent(typeof(Gs2LotteryOwnDrawnPrizeContext))]
    [RequireComponent(typeof(Gs2LotteryOwnDrawnPrizeListFetcher))]
    public class DrawnResultStatus : MonoBehaviour
    {
        private Gs2LotteryOwnDrawnPrizeListFetcher _fetcher;
        private Gs2LotteryOwnDrawnPrizeContext _context;
        
        public int index;
        public GameObject character;
        public GameObject rarity;

        public UnityEvent onComplete = new UnityEvent();

        public void Awake() {
            this._fetcher = GetComponent<Gs2LotteryOwnDrawnPrizeListFetcher>();
            this._context = GetComponent<Gs2LotteryOwnDrawnPrizeContext>();
        }

        public void OnEnable() {
            this.index = 0;
            this._context.DrawnPrize = OwnDrawnPrize.New(
                this._fetcher.Context.LotteryModel.Namespace,
                this.index
            );
        }

        public void Next() {
            if (this.index < this._fetcher.DrawnPrizes.Count-1) {
                this._context.DrawnPrize.index = ++this.index;
                this.character.SetActive(false);
                this.character.SetActive(true);
                this.rarity.SetActive(false);
                this.rarity.SetActive(true);
            }
            else {
                this.onComplete.Invoke();
            }
        }
    }
}
