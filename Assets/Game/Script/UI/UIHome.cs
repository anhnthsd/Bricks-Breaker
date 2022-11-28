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

        public override void Initialize()
        {
            btnQuest.onClick.AddListener((() => PopupManager.Show<UIQuest>()));
            btnModeClassic.onClick.AddListener(() => PlayGame(GameMode.Classic));
            btnModeTower.onClick.AddListener(() => PlayGame(GameMode.Tower));
            SetScroll();
        }
        

        private void PlayGame(GameMode gameMode, int level = 0)
        {
            GameController.ins.PlayGame(gameMode, level);
            UIManager.Show<UIGamePlay>(); 
        }

        private void SetScroll()
        {
            for (int i = 0; i < 20; i++)
            {
                var newItem = Instantiate(lvlItemPrefab, contentScroll);
                var script = newItem.GetComponent<LvlItem>();
                script.SetLevel
                    (i + 1, i >= 17, Random.Range(0, 4), (i1) => PlayGame(GameMode.Tower, i1));
            }
        }
    }
}

public enum GameMode
{
    Classic,
    Tower,
}