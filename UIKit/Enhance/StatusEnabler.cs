using System;
using Gs2.Unity.UiKit.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Enhance
{
    public partial class StatusEnabler : MonoBehaviour
    {
        private void OnChanged() {
            switch (this._context.Status) {

                case Enhance.UIStatus.SelectWeapon:
                    this.target.SetActive(this.selectWeapon);
                    break;
                case Enhance.UIStatus.Enhance:
                    this.target.SetActive(this.enhance);
                    break;
                case Enhance.UIStatus.Unleash:
                    this.target.SetActive(this.unleash);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public partial class StatusEnabler
    {
        private Enhance _context;
        
        public void Awake() {
            this._context = GetComponent<Enhance>() ?? GetComponentInParent<Enhance>(true);
            if (this._context == null) {
                Debug.LogWarning($"{gameObject.GetFullPath()}: Couldn't find the Enhance.");
            }
        }
        
        private UnityAction _onChanged;

        public void OnEnable()
        {
            this._onChanged = () =>
            {
                OnChanged();
            };
            this._context.onChangeStatus.AddListener(this._onChanged);
            OnChanged();
        }

        public void OnDisable()
        {
            if (this._onChanged != null) {
                this._context.onChangeStatus.RemoveListener(this._onChanged);
                this._onChanged = null;
            }
        }
    }
    
    public partial class StatusEnabler
    {
        public bool selectWeapon;
        public bool enhance;
        public bool unleash;

        public GameObject target;
    }
}
