using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UIGamePlay : View
    {
        [SerializeField] private Button btnBallReturn;
        [SerializeField] private TextMeshProUGUI txtScore;

        private BaseModePlay currentMode;

        public GameObject UIModeClassic;
        public GameObject UIModeTower;

        public override void Initialize()
        {
            btnBallReturn.onClick.AddListener((() => BallController.ins.BallReturn()));
        }

        public override void Show()
        {
            base.Show();
            currentMode = GameController.ins.currentMode;
            txtScore.text = "Score: " + currentMode.Score;
            Debug.Log(GameController.ins.currentMode);
        }

        private void SetTopMode(GameMode mode)
        {
            UIModeClassic.SetActive(mode == GameMode.Classic);
            UIModeTower.SetActive(mode == GameMode.Tower);
        }
    }
}