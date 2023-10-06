using System;
using Gs2.Unity.UiKit.Core;
using Gs2.Unity.UiKit.Gs2Inventory.Context;
using UnityEngine;

namespace Scenes.Samples.UIKit.Enhance
{
    public class Weapon : MonoBehaviour
    {
        private Enhance _enhance;
        private Gs2InventoryOwnItemSetContext _context;

        public void Awake() {
            this._enhance = GetComponent<Enhance>() ?? GetComponentInParent<Enhance>(true);
            if (this._enhance == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the Enhance.");
            }
            this._context = GetComponent<Gs2InventoryOwnItemSetContext>() ?? GetComponentInParent<Gs2InventoryOwnItemSetContext>(true);
            if (this._context == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the Gs2InventoryOwnItemSetContext.");
            }
        }

        public void OnClick() {
            this._enhance.OnSelectWeapon(this._context.ItemSet);
        }
    }
}
