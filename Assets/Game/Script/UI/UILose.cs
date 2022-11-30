using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UILose : View
    {
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnQuest;
        [SerializeField] private Button btnRetry;
        
        [SerializeField] private GameObject UIModeClassic;
        [SerializeField] private GameObject UIModeTower;
        public override void Initialize()
        {
            btnClose.onClick.AddListener((Close));
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>(false)));
            btnRetry.onClick.AddListener((PlayAgain));
        }

        private void PlayAgain()
        {
            GameController.ins.Retry();
            Hide();
        }

        private void Close()
        {
            Hide();
            PopupManager.ShowLast();
            UIManager.Show<UIHome>(false);
        }
        
        private void SetUIMode(GameMode mode)
        {
            Debug.Log("SetUIMode:" + mode);
            UIModeClassic.SetActive(mode == GameMode.Classic);
            UIModeTower.SetActive(mode == GameMode.Tower);
        }
    }
}