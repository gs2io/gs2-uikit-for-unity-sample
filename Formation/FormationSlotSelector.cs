using System;
using Gs2.Unity.UiKit.Core;
using Gs2.Unity.UiKit.Gs2Dictionary.Fetcher;
using Gs2.Unity.UiKit.Gs2Formation.Fetcher;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Formation
{
    public class FormationSlotSelector : MonoBehaviour
    {
        private SelectedFormationSlot _selectedFormationSlot;
        private Gs2DictionaryOwnEntryFetcher _fetcher;

        public void Awake() {
            this._selectedFormationSlot = GetComponent<SelectedFormationSlot>() ?? GetComponentInParent<SelectedFormationSlot>();
            if (this._selectedFormationSlot == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the SelectedFormationSlot.");
                enabled = false;
            }
            this._fetcher = GetComponent<Gs2DictionaryOwnEntryFetcher>() ?? GetComponentInParent<Gs2DictionaryOwnEntryFetcher>();
            if (this._fetcher == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the Gs2DictionaryOwnEntryFetcher.");
                enabled = false;
            }
        }

        public void OnSelect() {
            if (!this._fetcher?.Fetched ?? false) return;
            this._selectedFormationSlot.OnSelect(this._fetcher.Entry);
        }
    }
}
