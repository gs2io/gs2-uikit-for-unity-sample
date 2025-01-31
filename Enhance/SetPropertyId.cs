using System;
using System.Linq;
using Gs2.Unity.Gs2Exchange.Model;
using Gs2.Unity.UiKit.Gs2Exchange;
using Gs2.Unity.UiKit.Gs2Inventory.Fetcher;
using UnityEngine;

namespace Scenes.Samples.UIKit.Enhance
{
    [RequireComponent(typeof(Gs2ExchangeExchangeExchangeAction))]
    public class SetPropertyId : MonoBehaviour
    {
        public void OnEnable() {
            var fetcher = GetComponent<Gs2InventoryOwnItemSetFetcher>() ?? GetComponentInParent<Gs2InventoryOwnItemSetFetcher>();
            var action = GetComponent<Gs2ExchangeExchangeExchangeAction>();
            var config = action.Config.Where(v => v.Key != "propertyId").ToList();
            config.Add(new EzConfig {
                Key = "propertyId",
                Value = fetcher.ItemSet[0].ItemSetId,
            });
            action.Config = config;
        }
    }
}
