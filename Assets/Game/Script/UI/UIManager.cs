using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Script.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager ins;

        [SerializeField] private View startView;
        [SerializeField] private View[] _views;

        private View _currentView;
        private readonly Stack<View> _history = new Stack<View>();

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            for (int i = 0; i < _views.Length; i++)
            {
                _views[i].Initialize();
                _views[i].Hide();
            }

            if (startView!=null)
            {
                Show(startView,true);
            }
        }

        public static T GetView<T>() where T : View
        {
            for (int i = 0; i < ins._views.Length; i++)
            {
                if (ins._views[i] is T tView)
                {
                    return tView;
                }
            }

            return null;
        }

        public static void Show<T>(bool remember = true) where T : View
        {
            for (int i = 0; i < ins._views.Length; i++)
            {
                if (ins._views[i] is T)
                {
                    if (ins._currentView != null)
                    {
                        if (remember)
                        {
                            ins._history.Push(ins._currentView);
                        }

                        ins._currentView.Hide();
                    }

                    ins._views[i].Show();
                    ins._currentView = ins._views[i];
                }
            }
        }

        public static void Show(View view, bool remenber = true)
        {
            if (ins._currentView != null)
            {
                if (remenber)
                {
                    ins._history.Push(ins._currentView);
                }
                ins._currentView.Hide();
            }
            view.Show();

            ins._currentView = view;
        }

        public static void ShowLast()
        {
            if (ins._history.Count!=0)
            {
                Show(ins._history.Pop(),false);
            }
        }
    }
}