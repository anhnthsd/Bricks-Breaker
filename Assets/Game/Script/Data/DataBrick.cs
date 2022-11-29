using System;
using System.Collections.Generic;
using Game.Script;
using UnityEngine;

namespace Game.Script.Data
{
    [CreateAssetMenu(fileName = "DataBrick", menuName = "Data/DataBrick")]
    public class DataBrick : ScriptableObject
    {
        public List<BrickInfo> brickInfo;
    }
}
[Serializable]
public class BrickInfo
{
    public TypeOfBrick type;
    public BaseBrick prefab;
    public List<Sprite> lsSprite;
}