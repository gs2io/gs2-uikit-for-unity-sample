using System;
using Gs2.Unity.UiKit.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Formation
{
    public class SelectedFormationSlotEnabler : MonoBehaviour
    {
        public SelectedFormationSlot.Slot slot;
        public GameObject target;

        private void OnUpdateContext() {
            this.target.SetActive(this._context.selected == this.slot);
        }
        
        private SelectedFormationSlot _context;

        public void Awake() {
            this._context = GetComponent<SelectedFormationSlot>() ?? GetComponentInParent<SelectedFormationSlot>();
            if (this._context == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the SelectedFormationSlot.");
                enabled = false;
            }
        }
        
        private UnityAction _onFetched;

        public void OnEnable()
        {
            this._onFetched = () =>
            {
                OnUpdateContext();
            };
            this._context.updateContext.AddListener(this._onFetched);
            OnUpdateContext();
        }

        public void OnDisable()
        {
            if (this._onFetched != null) {
                this._context.updateContext.RemoveListener(this._onFetched);
                this._onFetched = null;
            }
        }
    }
}
