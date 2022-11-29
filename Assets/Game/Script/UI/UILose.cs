using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UILose : View
    {
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnQuest;
        [SerializeField] private Button btnRetry;
        
        public override void Initialize()
        {
            btnClose.onClick.AddListener((() => Close()));
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>()));
            btnRetry.onClick.AddListener((() => PlayAgain()));
        }

        private void PlayAgain()
        {
            
        }

        private void Close()
        {
            Hide();
            PopupManager.ShowLast();
            UIManager.Show<UIHome>();
        }
    }
}