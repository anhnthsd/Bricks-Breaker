using System;
using System.Collections.Generic;
using Game.Script.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UIGamePlay : View
    {
        [SerializeField] private Button btnBallReturn;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private TextMeshProUGUI txtBestScore;

        [SerializeField] private GameObject objGamePlay;
        [SerializeField] private GameObject uiModeClassic;
        [SerializeField] private GameObject uiModeTower;


        [SerializeField] private List<Image> lsStar;
        [SerializeField] private Sprite imgLightStar;
        [SerializeField] private Sprite imgDarktStar;
        [SerializeField] private GameObject fxStar;
        [SerializeField] private Image imgStarProgress;

        private void OnEnable()
        {
            txtScore.text = "Score: 0";
            GameController.ins.OnPlayGame += SetTopMode;
            GameController.ins.EventUpdateScore += UpdateScore;
            GameController.ins.SetVisibleBtnBallReturn += UpdateBtnBallReturn;
            objGamePlay.SetActive(true);
            GameController.ins.OnRestart();
        }

        private void OnDisable()
        {
            GameController.ins.OnPlayGame -= SetTopMode;
            GameController.ins.EventUpdateScore -= UpdateScore;
            GameController.ins.SetVisibleBtnBallReturn -= UpdateBtnBallReturn;
            objGamePlay.SetActive(false);
        }

        public override void Initialize()
        {
            btnBallReturn.onClick.AddListener((() => GameController.ins.BallReturn()));
        }

        public override void Show()
        {
            base.Show();
            Debug.Log(GameController.ins.currentMode);
        }

        private void SetTopMode(GameMode mode)
        {
            uiModeClassic.SetActive(mode == GameMode.Classic);
            uiModeTower.SetActive(mode == GameMode.Tower);
        }

        private void UpdateStar(int star)
        {
            for (int i = 0; i < lsStar.Count; i++)
            {
                if (i >= star)
                {
                    lsStar[i].sprite = imgDarktStar;
                    continue;
                }

                var fx = Instantiate(fxStar);
                fx.transform.position = lsStar[i].transform.position;
                Destroy(fx, 1);
                lsStar[i].sprite = imgLightStar;
            }
        }

        private void UpdateScore(int score, int star)
        {
            txtScore.text = "Score: " + score;
            UpdateStar(star);
            float percent = (float)score / LevelTowerModel.Ins.levelInfos[GameController.ins.levelPlay].scoreStar;
            imgStarProgress.fillAmount = percent;
        }

        private void UpdateBtnBallReturn(bool isActive)
        {
            btnBallReturn.gameObject.SetActive(isActive);
        }
    }
}