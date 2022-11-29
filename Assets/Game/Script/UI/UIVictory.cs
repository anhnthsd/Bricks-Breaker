using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UIVictory : View
    {
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnQuest;
        [SerializeField] private Button btnContinue;
        
        public override void Initialize()
        {
            btnClose.onClick.AddListener((() => Close()));
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>()));
            btnContinue.onClick.AddListener((() => MoveToNextLevel()));
        }

        private void MoveToNextLevel()
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