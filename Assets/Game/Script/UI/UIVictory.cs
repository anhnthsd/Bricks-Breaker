using System;
using System.Collections.Generic;
using Game.Script.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UIVictory : View
    {
        [SerializeField] private Button btnClose;
        [SerializeField] private Button btnQuest;
        [SerializeField] private Button btnContinue;
        [SerializeField] private List<Image> lsStar;

        [SerializeField] private Sprite imgLightStar;
        [SerializeField] private GameObject fxStar;

        [SerializeField] private TextMeshProUGUI txtLevel;

        public override void Initialize()
        {
            btnClose.onClick.AddListener((Close));
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>()));
            btnContinue.onClick.AddListener((MoveToNextLevel));
        }

        private void OnEnable()
        {
            GameController.ins.EventUpdateScore += UpdateView;
        }

        private void OnDisable()
        {
            GameController.ins.EventUpdateScore -= UpdateView;
        }

        private void UpdateView(int score, int star)
        {
            Debug.Log("Update view Victory");
            txtLevel.text = "Bậc - " + GameController.ins.levelPlay;
            SetStar(star);
        }

        public void SetStar(int star)
        {
            for (int i = 0; i < star; i++)
            {
                var fx = Instantiate(fxStar);
                fx.transform.position = lsStar[i].transform.position;
                Destroy(fx, 1);
                lsStar[i].sprite = imgLightStar;
            }
        }

        private void MoveToNextLevel()
        {
            GameController.ins.OnRestart();
            GameController.ins.PlayGame(GameMode.Tower, GameController.ins.levelPlay + 1);
            Hide();
        }

        private void Close()
        {
            Hide();
            PopupManager.ShowLast();
            UIManager.Show<UIHome>(false);
        }
    }
}