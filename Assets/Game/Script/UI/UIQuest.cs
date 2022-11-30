using System;
using System.Collections.Generic;
using Game.Script.Data;
using Game.Script.Model;
using TMPro;
using UnityEngine;

namespace Game.Script.UI
{
    public class UIQuest : View
    {
        public GameObject itemDailyPrefab;
        public Transform dailyContentScroll;
        public GameObject dailyScroll;

        public GameObject itemMaintPrefab;
        public GameObject mainScroll;
        public Transform mainContentScroll;

        private List<MainMissionItem> lsMain;
        private List<DailyButtonItem> lsDaily;

        [SerializeField] private TextMeshProUGUI txtDiamond;

        public override void Initialize()
        {
            SetDailyScroll();
            SetMainScroll();
        }

        private void OnEnable()
        {
            GameManager.UpdateDiamond += UpdateView;
        }

        private void OnDisable()
        {
            GameManager.UpdateDiamond -= UpdateView;
        }

        public override void Show()
        {
            gameObject.SetActive(true);
            UpdateView();
            ChooseDailyQ();
        }

        public void SetDailyScroll()
        {
            var list = DailyMissionModel.Ins.missionInfos;
            lsDaily = new List<DailyButtonItem>();
            for (int i = 0; i < list.Count; i++)
            {
                var mission = Instantiate(itemDailyPrefab, dailyContentScroll);
                var script = mission.GetComponent<DailyButtonItem>();
                var i1 = i;
                script.UpdateMission(list[i], (() => ClaimDailyMission(list[i1].type)));
                lsDaily.Add(script);
            }
        }

        public void UpdateDailyScroll()
        {
            var list = DailyMissionModel.Ins.missionInfos;
            for (int i = 0; i < list.Count; i++)
            {
                int i1 = i;
                lsDaily[i].UpdateMission(list[i], (() => ClaimDailyMission(list[i1].type)));
            }
        }

        public void ClaimDailyMission(TypeMissionDaily typeMissionDaily)
        {
            GameManager.ClaimQDaily(typeMissionDaily);
        }

        public void SetMainScroll()
        {
            var list = MainMissionModel.Ins.missionInfos;
            lsMain = new List<MainMissionItem>();
            for (int i = 0; i < list.Count; i++)
            {
                var mission = Instantiate(itemMaintPrefab, mainContentScroll);
                var script = mission.GetComponent<MainMissionItem>();
                var i1 = i;
                script.UpdateMission(list[i], (() => ClaimMainMission(list[i1].type)));
                lsMain.Add(script);
            }
        }

        public void UpdateMainScroll()
        {
            var list = MainMissionModel.Ins.missionInfos;
            for (int i = 0; i < list.Count; i++)
            {
                int i1 = i;
                lsMain[i].UpdateMission(list[i], (() => ClaimMainMission(list[i1].type)));
            }
        }

        public void ClaimMainMission(TypeMissionMain typeMissionMain)
        {
            GameManager.ClaimQMain(typeMissionMain);
        }

        private void UpdateView()
        {
            txtDiamond.text = UserModel.Ins.userData.diamond.ToString();
            UpdateMainScroll();
            UpdateDailyScroll();
        }

        public void ChooseDailyQ()
        {
            dailyScroll.SetActive(true);
            mainScroll.SetActive(false);
        }

        public void ChooseMainQ()
        {
            dailyScroll.SetActive(false);
            mainScroll.SetActive(true);
        }

        public void Close()
        {
            Hide();
            PopupManager.ShowLast();
        }
    }
}