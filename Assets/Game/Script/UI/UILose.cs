using Game.Script.Model;
using TMPro;
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

        [SerializeField] private Text txtLevel;
        [SerializeField] private Text txtBestScore;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private TextMeshProUGUI txtDiamond;

        public override void Initialize()
        {
            btnClose.onClick.AddListener((Close));
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>()));
            btnRetry.onClick.AddListener((PlayAgain));
        }

        public override void Show()
        {
            base.Show();
            SetUIMode(GameController.ins.currentGameMode);
            UpdateView();
        }

        private void PlayAgain()
        {
            Hide();
            GameController.ins.Retry();
        }

        public void SetTowerView(int level)
        {
            txtLevel.text = "Bậc - " + level;
        }

        public void SetClassicView(int score)
        {
            txtScore.text = score.ToString();
        }

        private void SetUIMode(GameMode mode)
        {
            UIModeClassic.SetActive(mode == GameMode.Classic);
            UIModeTower.SetActive(mode == GameMode.Tower);
        }

        private void UpdateView()
        {
            txtBestScore.text = UserModel.Ins.userData.bestScoreClassic.ToString();
            txtDiamond.text = UserModel.Ins.userData.diamond.ToString();
        }

        private void Close()
        {
            Hide();
            PopupManager.ShowLast();
            UIManager.Show<UIHome>(false);
        }
    }
}