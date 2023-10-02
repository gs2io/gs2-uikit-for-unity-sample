using System;
using Gs2.Unity.Gs2Dictionary.Model;
using Gs2.Unity.UiKit.Core;
using Gs2.Unity.UiKit.Gs2Dictionary.Fetcher;
using Gs2.Unity.UiKit.Gs2Formation.Fetcher;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Formation
{
    public class FormationSlotSelectEffector : MonoBehaviour
    {
        private SelectedFormationSlot _selectedFormationSlot;
        private Gs2FormationOwnSlotFetcher _fetcher;

        public void Awake() {
            this._selectedFormationSlot = GetComponent<SelectedFormationSlot>() ?? GetComponentInParent<SelectedFormationSlot>();
            if (this._selectedFormationSlot == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the SelectedFormationSlot.");
                enabled = false;
            }
            this._fetcher = GetComponent<Gs2FormationOwnSlotFetcher>() ?? GetComponentInParent<Gs2FormationOwnSlotFetcher>();
            if (this._fetcher == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the Gs2FormationOwnSlotFetcher.");
                enabled = false;
            }
        }

        private UnityAction<EzEntry> _onChangeWeapon;
        private UnityAction<EzEntry> _onChangeHead;
        private UnityAction<EzEntry> _onChangeArmor;

        public void OnEnable() {
            this._onChangeWeapon = item =>
            {
                if (this._fetcher.Context.Slot.SlotName != "Weapon") return;
                var slot = this._fetcher.Slot;
                slot.PropertyId = item.EntryId;
                this._fetcher.SetTemporarySlot(slot);
            };
            this._selectedFormationSlot.onChangeWeapon.AddListener(this._onChangeWeapon);
            
            this._onChangeHead = item =>
            {
                if (this._fetcher.Context.Slot.SlotName != "Head") return;
                var slot = this._fetcher.Slot;
                slot.PropertyId = item.EntryId;
                this._fetcher.SetTemporarySlot(slot);
            };
            this._selectedFormationSlot.onChangeHead.AddListener(this._onChangeHead);
            
            this._onChangeArmor = item =>
            {
                if (this._fetcher.Context.Slot.SlotName != "Armor") return;
                var slot = this._fetcher.Slot;
                slot.PropertyId = item.EntryId;
                this._fetcher.SetTemporarySlot(slot);
            };
            this._selectedFormationSlot.onChangeArmor.AddListener(this._onChangeArmor);
        }

        public void OnDisable() {
            if (this._onChangeWeapon != null) {
                this._selectedFormationSlot.onChangeWeapon.RemoveListener(this._onChangeWeapon);
                this._onChangeWeapon = null;
            }
            if (this._onChangeHead != null) {
                this._selectedFormationSlot.onChangeHead.RemoveListener(this._onChangeHead);
                this._onChangeHead = null;
            }
            if (this._onChangeArmor != null) {
                this._selectedFormationSlot.onChangeArmor.RemoveListener(this._onChangeArmor);
                this._onChangeArmor = null;
            }
        }
    }
}
