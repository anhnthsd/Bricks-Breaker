using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public abstract class ButtonMission<T> : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI txtName;
        [SerializeField] protected TextMeshProUGUI txtDiamond;
        [SerializeField] protected Button imgDiamond;
        [SerializeField] protected Image imgProgress;
        [SerializeField] protected TextMeshProUGUI txtProgress;
    
        protected Action OnClaimClick;

        private void Start()
        {
            imgDiamond.onClick.AddListener(ClaimClick);
        }

        public abstract void UpdateMission(T info, Action onClaimClick);

        private void ClaimClick()
        {
            OnClaimClick?.Invoke();
        }
    }
}