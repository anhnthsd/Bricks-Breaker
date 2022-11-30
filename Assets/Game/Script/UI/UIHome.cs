using System.Collections.Generic;
using Game.Script.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class UIHome : View
    {
        [SerializeField] private Button btnQuest;
        [SerializeField] private Button btnModeClassic;
        [SerializeField] private Button btnModeTower;

        public GameObject lvlItemPrefab;
        public Transform contentScroll;
        private LevelTowerModel _levelTowerModel;

        private List<LvlItem> _lsLvlItems;

        public override void Initialize()
        {
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>(false)));
            btnModeClassic.onClick.AddListener(() => PlayGame(GameMode.Classic));
            btnModeTower.onClick.AddListener(() => PlayGame(GameMode.Tower));
            SetScroll();
        }

        public override void Show()
        {
            base.Show();
            UpdateScroll();
        }

        private void PlayGame(GameMode gameMode, int level = 0)
        {
            UIManager.Show<UIGamePlay>(false);
            GameController.ins.PlayGame(gameMode, level);
        }

        private void SetScroll()
        {
            _levelTowerModel = LevelTowerModel.Ins;
            _lsLvlItems = new List<LvlItem>();
            for (int i = 0; i < _levelTowerModel.levelInfos.Count; i++)
            {
                var newItem = Instantiate(lvlItemPrefab, contentScroll);
                var script = newItem.GetComponent<LvlItem>();
                script.SetLevel
                (_levelTowerModel.levelInfos[i].level, i >= UserModel.Ins.userData.levelTower,
                    _levelTowerModel.levelInfos[i].star, (i1) => PlayGame(GameMode.Tower, i1));
                _lsLvlItems.Add(script);
            }
        }

        private void UpdateScroll()
        {
            var list = _levelTowerModel.levelInfos;
            for (int i = 0; i < list.Count; i++)
            {
                _lsLvlItems[i].SetLevel
                (list[i].level, i >= UserModel.Ins.userData.levelTower, list[i].star,
                    (i1) => PlayGame(GameMode.Tower, i1));
            }
        }
    }
}

public enum GameMode
{
    Classic,
    Tower,
}