using System.Collections.Generic;
using Game.Script.Data;
using UnityEngine;

namespace Game.Script
{public class BrickPooler : MonoBehaviour
    {
        public static BrickPooler BrickInstance;
        public List<BaseBrick> pooledObjects;
        public int amountToPool;

        public bool expand = true;

        void Awake()
        {
            BrickInstance = this;
        }

        void Start()
        {
            pooledObjects = new List<BaseBrick>();
            for (int i = 0; i < amountToPool; i++)
            {
                var prefab = Resources.Load<DataBrick>("DataBrick").brickInfo
                    .Find(s => s.type == TypeOfBrick.Normal)
                    .prefab;
                BaseBrick brickNew = Instantiate(prefab);
                brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.Normal);
                brickNew.gameObject.SetActive(false);
                pooledObjects.Add(brickNew);
            }
        }

        public BaseBrick GetObject(TypeOfBrick brickType)
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].gameObject.activeInHierarchy && brickType == pooledObjects[i].typeOfBrick)
                {
                    return pooledObjects[i];
                }
            }

            if (expand)
            {
                var prefab = Resources.Load<DataBrick>("DataBrick").brickInfo
                    .Find(s => s.type == brickType)
                    .prefab;
                BaseBrick brickNew = Instantiate(prefab);
                brickNew.GetComponent<BaseBrick>().SetSprite(brickType);
                brickNew.gameObject.SetActive(false);
                pooledObjects.Add(brickNew);
                return brickNew;
            }
            else
            {
                return null;
            }
        }

        public void UpdatePosition()
        {
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (pooledObjects[i].gameObject.activeInHierarchy)
                {
                    pooledObjects[i].transform.position = new Vector3(pooledObjects[i].transform.position.x,
                        pooledObjects[i].transform.position.y - 0.5f, 0);
                }
            }
        }
    }
}