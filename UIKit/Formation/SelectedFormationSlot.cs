using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gs2.Gs2Dictionary.Model;
using Gs2.Unity.Gs2Dictionary.Model;
using Gs2.Unity.Gs2Formation.Model;
using Gs2.Unity.Gs2Key.ScriptableObject;
using Gs2.Unity.UiKit.Core;
using Gs2.Unity.UiKit.Gs2Formation;
using Gs2.Unity.UiKit.Gs2Formation.Context;
using Gs2.Unity.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Scenes.Samples.UIKit.Formation
{
    public partial class SelectedFormationSlot : MonoBehaviour
    {
        public enum Slot
        {
            Weapon,
            Head,
            Armor,
        }

        public Slot selected;

        public void OnSelectWeapon() {
            this.selected = Slot.Weapon;
            this.updateContext.Invoke();
        }

        public void OnSelectHead() {
            this.selected = Slot.Head;
            this.updateContext.Invoke();
        }

        public void OnSelectArmor() {
            this.selected = Slot.Armor;
            this.updateContext.Invoke();
        }

        public UnityEvent updateContext = new UnityEvent();
    }

    public partial class SelectedFormationSlot
    {
        public UnityEvent<EzEntry> onChangeWeapon = new UnityEvent<EzEntry>();
        public UnityEvent<EzEntry> onChangeHead = new UnityEvent<EzEntry>();
        public UnityEvent<EzEntry> onChangeArmor = new UnityEvent<EzEntry>();

        private List<EzSlotWithSignature> _slots = new List<EzSlotWithSignature>();
        public Gs2FormationFormSetFormAction action;

        public void OnSelect(EzEntry entry) {
            var slotName = "";
            switch (this.selected) {
                case Slot.Weapon:
                    this.onChangeWeapon.Invoke(entry);
                    slotName = "Weapon";
                    break;
                case Slot.Head:
                    this.onChangeHead.Invoke(entry);
                    slotName = "Head";
                    break;
                case Slot.Armor:
                    this.onChangeArmor.Invoke(entry);
                    slotName = "Armor";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var slot = this._slots.FirstOrDefault(v => v.Name == slotName);
            if (slot == null) {
                slot = new EzSlotWithSignature {
                    Name = slotName,
                    PropertyType = "gs2_dictionary",
                    Body = entry.EntryId,
                };
                this._slots.Add(slot);
            }
            else {
                slot.Body = entry.EntryId;
                slot.Signature = null;
            }
        }

    }
    
    public partial class SelectedFormationSlot
    {
        public UnityEvent<List<EzSlotWithSignature>> onCompletePrepareCommit = new UnityEvent<List<EzSlotWithSignature>>();
        public ErrorEvent onError = new ErrorEvent();
        
        public void PrepareCommit() {
            IEnumerator Impl() {
                foreach (var slot in this._slots) {
                    if (slot.Signature != null) {
                        continue;
                    }
                    var future = Gs2ClientHolder.Instance.Gs2.Dictionary.Namespace(
                        Entry.GetNamespaceNameFromGrn(slot.Body)
                    ).Me(
                        Gs2GameSessionHolder.Instance.GameSession
                    ).Entry(
                        Entry.GetEntryNameFromGrn(slot.Body)
                    ).GetEntryWithSignature(
                        this.action.Key.Grn
                    );
                    yield return future;
                    if (future.Error != null) {
                        this.onError.Invoke(future.Error, null);
                        yield break;
                    }
                    slot.Body = future.Result.Body;
                    slot.Signature = future.Result.Signature;
                }
                this.onCompletePrepareCommit.Invoke(this._slots);
            }

            StartCoroutine(Impl());
        }
    }
}
