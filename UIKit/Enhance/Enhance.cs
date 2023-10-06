using System.Collections;
using System.Linq;
using Gs2.Gs2Inventory.Model;
using Gs2.Unity.Gs2Enhance.Model;
using Gs2.Unity.Gs2Inventory.ScriptableObject;
using Gs2.Unity.UiKit.Gs2Enhance;
using Gs2.Unity.UiKit.Gs2Enhance.Fetcher;
using Gs2.Unity.UiKit.Gs2Inventory.Context;
using Gs2.Unity.Util;
using Gs2.Util.LitJson;
using UnityEngine;
using UnityEngine.Events;

namespace Scenes.Samples.UIKit.Enhance
{
    public partial class Enhance : MonoBehaviour
    {
        public enum UIStatus
        {
            SelectWeapon,
            Enhance,
            Unleash,
        }

        private UIStatus _status;

        public UIStatus Status => this._status;

        private OwnItemSet _selectedItemSet;

        public void OnSelectWeapon(
            OwnItemSet itemSet
        ) {
            this.detailContext.ItemSet = itemSet;
            this._selectedItemSet = itemSet;
            this.action.SetTargetItemSetId(itemSet.Grn);
            this._status = UIStatus.Enhance;
            this.onChangeStatus.Invoke();
        }

        public void OnSelectEnhance() {
            this._status = UIStatus.Enhance;
            this.onChangeStatus.Invoke();
        }

        public void OnSelectUnleash() {
            this._status = UIStatus.Unleash;
            this.onChangeStatus.Invoke();
        }

        public void OnClose() {
            this._status = UIStatus.SelectWeapon;
            this.onChangeStatus.Invoke();
        }
    }
    public partial class Enhance
    {
        public Gs2InventoryOwnItemSetContext detailContext;
        public Gs2EnhanceEnhanceEnhanceAction action;
        public UnityEvent onChangeStatus = new UnityEvent();
        public UnityEvent<long> onUpdateEstimateAdditionExperience = new UnityEvent<long>();
        public ErrorEvent onError = new ErrorEvent();
    }
    
    public partial class Enhance
    {
        private long _estimateAdditionExperience;
        public long EstimateAdditionExperience => this._estimateAdditionExperience;
        
        public IEnumerator UpdateEstimateAdditionExperience() {
            var fetcher = this.action.gameObject.GetComponent<Gs2EnhanceRateModelFetcher>() ?? this.action.gameObject.GetComponentInParent<Gs2EnhanceRateModelFetcher>();
            var rateModel = fetcher.RateModel;
            long sumExperienceValue = 0;
            foreach (var material in this.action.Materials) {
                if (material.MaterialItemSetId == null) continue;
                var namespaceName = SimpleItem.GetNamespaceNameFromGrn(material.MaterialItemSetId);
                var inventoryName = SimpleItem.GetInventoryNameFromGrn(material.MaterialItemSetId);
                var itemName = SimpleItem.GetItemNameFromGrn(material.MaterialItemSetId);

                var future = Gs2ClientHolder.Instance.Gs2.Inventory.Namespace(
                    namespaceName
                ).SimpleInventoryModel(
                    inventoryName
                ).SimpleItemModel(
                    itemName
                ).Model();
                yield return future;
                if (future.Error != null) {
                    this.onError.Invoke(future.Error, null);
                }
                long unitExperienceValue = 0;
                try {
                    var v = rateModel.AcquireExperienceHierarchy.Aggregate(
                        JsonMapper.ToObject(future.Result.Metadata),
                        (current, element) => current[element]);
                    long.TryParse(v.ToString(), out unitExperienceValue);
                }
                catch {
                    // ignored
                }
                sumExperienceValue += unitExperienceValue * material.Count;
            }
            this.onUpdateEstimateAdditionExperience.Invoke(this._estimateAdditionExperience = sumExperienceValue);
        }

        public void SetMaterial(string materialItemId, long count) {
            var materials = this.action.Materials.Where(v => v.MaterialItemSetId != materialItemId).ToList();
            if (count > 0) {
                materials.Add(new EzMaterial {
                    MaterialItemSetId = materialItemId,
                    Count = (int)count,
                });
            }
            this.action.Materials = materials;
            StartCoroutine(nameof(UpdateEstimateAdditionExperience));
        }
    }
}
