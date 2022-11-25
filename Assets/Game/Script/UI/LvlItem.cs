using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Script.UI
{
    public class LvlItem : MonoBehaviour
    {
        public Text txtLvl;
        [SerializeField] private GameObject star;
        [SerializeField] private Image[] lsStar;
        [SerializeField] private GameObject imgLock;

        public Sprite whiteStar;
        public bool _isLook;
        private int level;

        public Action<int> OpenLevel;
        public void SetLevel(int lvl, bool isLook = true, int sumStar = 0, Action<int> onClick= null)
        {
            level = lvl;
            txtLvl.text = lvl.ToString();
            _isLook = isLook;
            imgLock.SetActive(isLook);
            star.SetActive(!isLook);

            for (var i = 0; i < sumStar; i++)
            {
                lsStar[i].sprite = whiteStar;
            }

            OpenLevel = onClick;
        }

        public void Select()
        {
            OpenLevel?.Invoke(level);
        }
        
    }
}