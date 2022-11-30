using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Script.Model
{
    public class UserModel
    {
        public static UserModel _ins;

        public static UserModel Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new UserModel();
                }

                return _ins;
            }
        }

        public UserData userData;

        public void Init()
        {
            if (PlayerPrefs.HasKey("UserInfo"))
            {
                Load();
            }
            else
            {
                userData = new UserData()
                {
                    levelTower = 1
                };
                Save();
            }
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(userData);
            PlayerPrefs.SetString("UserInfo", json);
        }

        public void Load()
        {
            var str = PlayerPrefs.GetString("UserInfo");
            userData = JsonConvert.DeserializeObject<UserData>(str);
        }

        public void Victory(int level)
        {
            if (level == userData.levelTower)
            {
                userData.levelTower++;
            }

            Save();
        }

        public void ClaimDiamond(int diamond)
        {
            userData.diamond += diamond;
            Save();
        }
    }
}

[Serializable]
public class UserData
{
    public int diamond;
    public int bestScoreClassic;
    public int levelTower;
}