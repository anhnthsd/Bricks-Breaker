using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Script.UI
{
    public class PopupManager : MonoBehaviour
    {
        public static PopupManager ins;

        [SerializeField] private View[] popViews;
        private View _currentPopup;
        private readonly Stack<View> _history = new Stack<View>();

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            for (int i = 0; i < popViews.Length; i++)
            {
                popViews[i].Initialize();
                popViews[i].Hide();
            }
        }

        public static T GetView<T>() where T : View
        {
            for (int i = 0; i < ins.popViews.Length; i++)
            {
                if (ins.popViews[i] is T tView)
                {
                    return tView;
                }
            }

            return null;
        }

        public static T Show<T>(bool remember = true) where T : View
        {
            for (int i = 0; i < ins.popViews.Length; i++)
            {
                if (ins.popViews[i] is T tView)
                {
                    if (ins._currentPopup != null && ins._currentPopup != ins.popViews[i])
                    {
                        if (remember)
                        {
                            ins._history.Push(ins._currentPopup);
                        }

                        ins._currentPopup.Hide();
                    }

                    ins.popViews[i].Show();
                    ins._currentPopup = ins.popViews[i];
                    return tView;
                }
            }

            return null;
        }

        public static void Show(View view, bool remenber = true)
        {
            if (ins._currentPopup != null)
            {
                if (remenber)
                {
                    ins._history.Push(ins._currentPopup);
                }

                ins._currentPopup.Hide();
            }

            view.Show();

            ins._currentPopup = view;
        }

        public static void ShowLast()
        {
            if (ins._history.Count != 0)
            {
                Show(ins._history.Pop(), false);
            }
        }
    }
}