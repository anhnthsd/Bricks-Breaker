using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private Text txtLevel;

        public override void Initialize()
        {
            btnClose.onClick.AddListener((Close));
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>()));
            btnContinue.onClick.AddListener((MoveToNextLevel));
        }

        public void UpdateView(int star)
        {
            txtLevel.text = "Bậc - " + GameController.ins.levelPlay;
            SetStar(star);
            GameManager.GetStarTower(star);
        }

        private void SetStar(int star)
        {
            for (int i = 0; i < star; i++)
            {
                StartCoroutine(ShowStar(i, 0.3f * i + 0.5f));
            }
        }

        IEnumerator ShowStar(int i, float time)
        {
            yield return new WaitForSeconds(time);
            var fx = Instantiate(fxStar);
            fx.transform.position = lsStar[i].transform.position;
            Destroy(fx, 1);
            lsStar[i].sprite = imgLightStar;
        }

        private void MoveToNextLevel()
        {
            Hide();
            GameController.ins.OnRestart();
            GameController.ins.PlayGame(GameMode.Tower, GameController.ins.levelPlay + 1);
        }

        private void Close()
        {
            Hide();
            PopupManager.ShowLast();
            UIManager.Show<UIHome>(false);
        }
    }
}