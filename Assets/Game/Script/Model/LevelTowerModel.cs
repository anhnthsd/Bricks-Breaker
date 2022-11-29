using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Script.Model
{
    public class LevelTowerModel
    {
        public static LevelTowerModel _ins;

        public static LevelTowerModel Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new LevelTowerModel();
                }

                return _ins;
            }
        }

        public List<LevelInfo> levelInfos;

        public void Init()
        {
            if (PlayerPrefs.HasKey("LevelTower"))
            {
                Load();
            }
            else
            {
                levelInfos = new List<LevelInfo>();
                for (int i = 0; i < 60; i++)
                {
                    levelInfos.Add(new LevelInfo(i + 1, 0));
                }

                Save();
            }
        }

        public void Victory(int level, int star)
        {
            levelInfos[level - 1].star = star;
            Save();
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(levelInfos);
            PlayerPrefs.SetString("LevelTower", json);
        }

        private void Load()
        {
            var data = PlayerPrefs.GetString("LevelTower");
            levelInfos = JsonConvert.DeserializeObject<List<LevelInfo>>(data);
        }
    }
}

public class LevelInfo
{
    public int level;
    public int star;

    public LevelInfo(int level, int star)
    {
        this.level = level;
        this.star = star;
    }
}