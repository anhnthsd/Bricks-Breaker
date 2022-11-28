using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UIResult : View
    {
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnQuest;
        
        public override void Initialize()
        {
            btnClose.onClick.AddListener((() => Close()));
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>()));
        }
        
        
        
        private void Close()
        {
            Hide();
        }
    }
}