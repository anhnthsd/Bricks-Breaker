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
        public bool _isLock;
        private int level;

        public Action<int> OpenLevel;

        public void SetLevel(int lvl, bool isLock = true, int sumStar = 0, Action<int> onClick = null)
        {
            level = lvl;
            txtLvl.text = lvl.ToString();
            imgLock.SetActive(isLock);
            star.SetActive(!isLock);
            SetLock(isLock);
            for (var i = 0; i < sumStar; i++)
            {
                lsStar[i].sprite = whiteStar;
            }

            OpenLevel = onClick;
        }

        private void SetLock(bool isLock)
        {
            _isLock = isLock;
            gameObject.GetComponent<Button>().interactable = !isLock;
        }

        public void Select()
        {
            OpenLevel?.Invoke(level);
        }
    }
}