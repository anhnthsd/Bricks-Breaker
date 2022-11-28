using System;

namespace Game.Script.UI
{
    public class MainMissionItem : ButtonMission<MainMissionInfo>
    {
        public override void UpdateMission(MainMissionInfo info, Action onClaimClick)
        {
            txtName.text = info.name;
            txtDiamond.text = info.amountDiamond.ToString();
            txtProgress.text = $"{info.amountProgress}/{info.requirement}";
            imgProgress.fillAmount = (float)info.amountProgress / info.requirement;
            imgDiamond.interactable = info.isComplete && info.canClaim;
            OnClaimClick = onClaimClick;
        }
    }
}