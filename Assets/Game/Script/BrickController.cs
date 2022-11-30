using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Script
{
    public class BrickController : MonoBehaviour
    {
        public BallController ballController;

        public Transform parentBrick;
        public BaseBrick[,] lsBrick;

        public GameObject fxDamage;
        public GameObject fxDamageVer;
        public GameObject fxBurstHor;
        public GameObject fxBurstVer;
        public GameObject fxBurstSur;

        private int _currentRow;

        public static event Action OnEndTurn;
        private int[,] _lsMap;
        private int[,] _lsNumber;

        private const int MAX_ROW = 8;
        private const float BRICK = 0.8f;


        public void CreateBrickWithMap(int[,] lsMap, int[,] lsNumber, int rows)
        {
            _lsMap = lsMap;
            _lsNumber = lsNumber;
            _currentRow = rows;

            y = parentBrick.position.y;
            lsBrick = new BaseBrick[lsMap.GetLongLength(0), lsMap.GetLongLength(1)];
            for (int j = lsBrick.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = lsBrick.GetLength(0) - 1; i > lsBrick.GetLength(0) - 1 - _currentRow; i--)
                {
                    var brickType = (TypeOfBrick)lsMap[i, j];
                    var number = lsNumber[i, j];
                    CreateNewBrick(i, j, brickType, number);
                }
            }
        }

        public void AddNewMap(int[,] lsMap, int[,] lsNumber)
        {
            _lsMap = lsMap;
            _lsNumber = lsNumber;
            Debug.Log("AddNewMap");
        }

        public void CreateNewBrick(int i, int j, TypeOfBrick brickType, int number)
        {
            if (brickType == TypeOfBrick.Empty) return;
            // var prefab = Resources.Load<DataBrick>("DataBrick").brickInfo
            //     .Find(s => s.type == brickType)
            //     .prefab;
            // BaseBrick brickNew = Instantiate(prefab);
            // brickNew.GetComponent<BaseBrick>().SetSprite(brickType);
            BaseBrick brickNew = BrickPooler.BrickInstance.GetObject(brickType);

            if (brickNew)
            {
                brickNew.transform.SetParent(parentBrick);
                brickNew.SetPosition(new Vector2(
                    -2.45f + 0.8f * j,
                    (0.8f * lsBrick.GetLength(0)) + 1.8f - 0.8f * (_currentRow - 1) - 0.8f * i));
                brickNew.Active(true);
                brickNew.OnSpawn(number);
                brickNew.typeOfBrick = brickType;
                brickNew.i = i;
                brickNew.j = j;
                lsBrick[i, j] = brickNew;
            }
        }

        public void DelBrick(int i, int j)
        {
            if (lsBrick[i, j].CanDieOnBottom())
            {
                GameController.ins.IncreaseScore();
            }

            lsBrick[i, j] = null;
        }


        private float y = 0f;

        public void AfterTurn()
        {
            y -= 0.8f;
            var newPosition = new Vector3(parentBrick.position.x, y, 0);
            parentBrick.transform.DOMove(newPosition, 0.2f);

            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                for (int j = 0; j < lsBrick.GetLength(1); j++)
                {
                    if (lsBrick[i, j])
                    {
                        if ((lsBrick.GetLength(0) - 1 - _currentRow + MAX_ROW - 1) == i)
                        {
                            if (lsBrick[i, j].CanDieOnBottom())
                            {
                                GameController.ins.EndGame();
                            }
                            else
                            {
                                if (lsBrick[i, j].typeOfBrick == TypeOfBrick.AddBall)
                                {
                                    var itemS = lsBrick[i, j].GetComponent<ItemAddBall>();
                                    ballController.CreateNewBall(lsBrick[i, j].transform.position, itemS.sumBall);
                                    itemS.DestroyBrick();
                                }
                                else
                                {
                                    lsBrick[i, j].GetComponent<BaseBrick>().DestroyBrick();
                                }
                            }
                        }
                    }
                }
            }

            AddMap();
            OnEndTurn?.Invoke();
        }

        private void AddMap()
        {
            if (_currentRow > _lsMap.GetLength(0) - 1) GameController.ins.EndMap();
            if (_currentRow > _lsMap.GetLength(0) - 1) return;

            for (int j = 0; j < lsBrick.GetLength(1); j++)
            {
                // if (lsBrick[lsBrick.GetLength(0) - 1 - _currentRow, j])
                // {
                //     lsBrick[lsBrick.GetLength(0) - 1 - _currentRow, j].Active(true);
                // }

                var brickType = (TypeOfBrick)_lsMap[lsBrick.GetLength(0) - 1 - _currentRow, j];
                var number = _lsNumber[lsBrick.GetLength(0) - 1 - _currentRow, j];
                CreateNewBrick(lsBrick.GetLength(0) - 1 - _currentRow, j, brickType, number);
            }

            _currentRow++;
        }

        public bool IsSpecialTurn()
        {
            if (_lsMap.GetLength(0) - _currentRow <= 3) return false;
            for (int i = lsBrick.GetLength(0) - 1; i > _currentRow + 3; i--)
            {
                for (int j = 0; j < lsBrick.GetLength(1); j++)
                {
                    var item = lsBrick[i, j];
                    if (!item) continue;
                    if (item.CanDieOnBottom())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool IsClearMap()
        {
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                for (int j = 0; j < lsBrick.GetLength(1); j++)
                {
                    var item = lsBrick[i, j];
                    if (!item) continue;
                    if (item) return false;
                }
            }

            return true;
        }


        private void OnDamageHor(int hor)
        {
            for (int j = 0; j < lsBrick.GetLength(1); j++)
            {
                if (!lsBrick[hor, j]) continue;
                if (lsBrick[hor, j].CanDieOnBottom() && lsBrick[hor, j].gameObject.activeInHierarchy)
                {
                    lsBrick[hor, j].TakeDamage();
                }
            }
        }

        private void OnDamageVer(int ver)
        {
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                if (!lsBrick[i, ver]) continue;
                if (lsBrick[i, ver].CanDieOnBottom() && lsBrick[i, ver].gameObject.activeInHierarchy)
                {
                    lsBrick[i, ver].TakeDamage();
                }
            }
        }

        private void OnDamageBoth(int hor, int ver)
        {
            OnDamageHor(hor);
            OnDamageVer(ver);
        }

        public void OnDamage(Vector2 position, TypeOfBrick type, int hor, int ver)
        {
            switch (type)
            {
                case TypeOfBrick.DamageHorizontal:
                    OnDamageHor(hor);
                    ShowFxHor(position);
                    break;
                case TypeOfBrick.DamageVertical:
                    OnDamageVer(ver);
                    ShowFxVer(position);
                    break;
                case TypeOfBrick.DamageBoth:
                    OnDamageBoth(hor, ver);
                    ShowFxHor(position);
                    ShowFxVer(position);
                    break;
            }
        }

        private void ShowFxVer(Vector2 position)
        {
            var fxVer = Instantiate(fxDamageVer);
            fxVer.transform.position = position;
            Destroy(fxVer, 0.3f);
        }

        private void ShowFxHor(Vector2 position)
        {
            var fxHor = Instantiate(fxDamage);
            fxHor.transform.position = position;
            Destroy(fxHor, 0.3f);
        }

        private void ShowFxBurstHor(Vector2 position)
        {
            var fxHor = Instantiate(fxBurstHor);
            fxHor.transform.position = position;
            Destroy(fxHor, 0.3f);
        }

        private void ShowFxBurstVer(Vector2 position)
        {
            var fxVer = Instantiate(fxBurstVer);
            fxVer.transform.position = position;
            Destroy(fxVer, 0.3f);
        }

        private void ShowFxBurstSur(Vector2 position)
        {
            var fxSur = Instantiate(fxBurstSur);
            fxSur.transform.position = position;
            Destroy(fxSur, 0.5f);
        }

        private void OnBurstHor(int hor)
        {
            for (int j = 0; j < lsBrick.GetLength(1); j++)
            {
                if (!lsBrick[hor, j]) continue;
                if (lsBrick[hor, j].CanDieOnBottom())
                {
                    lsBrick[hor, j].DestroyBrick();
                }
            }
        }

        private void OnBurstVer(int ver)
        {
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                if (!lsBrick[i, ver]) continue;
                if (lsBrick[i, ver].CanDieOnBottom())
                {
                    lsBrick[i, ver].DestroyBrick();
                }
            }
        }

        private void OnBurstBoth(int hor, int ver)
        {
            OnBurstHor(hor);
            OnBurstVer(ver);
        }

        private void OnBurstSurround(int hor, int ver)
        {
            for (int i = hor - 1; i <= hor + 1 && i < lsBrick.GetLength(0) && i >= 0; i++)
            {
                for (int j = ver - 1; j <= ver + 1 && j < lsBrick.GetLength(1) && j >= 0; j++)
                {
                    if (lsBrick[i, j])
                    {
                        lsBrick[i, j].DestroyBrick();
                    }
                }
            }
        }

        public void OnBurst(Vector2 position, int hor, int ver, TypeOfBrick type)
        {
            switch (type)
            {
                case TypeOfBrick.DeleteHorizontal:
                    OnBurstHor(hor);
                    ShowFxBurstHor(position);
                    break;
                case TypeOfBrick.DeleteVertical:
                    OnBurstVer(ver);
                    ShowFxBurstVer(position);
                    break;
                case TypeOfBrick.DeleteBoth:
                    OnBurstBoth(hor, ver);
                    ShowFxBurstHor(position);
                    ShowFxBurstVer(position);
                    break;
                case TypeOfBrick.DeleteSurround:
                    OnBurstSurround(hor, ver);
                    ShowFxBurstSur(position);
                    break;
            }
        }

        public void OnRestart()
        {
            if (lsBrick != null)
            {
                for (int i = 0; i < lsBrick.GetLength(0); i++)
                {
                    for (int j = 0; j < lsBrick.GetLength(1); j++)
                    {
                        if (lsBrick[i, j])
                        {
                            lsBrick[i, j].Remove();
                        }
                    }
                }
            }
        }
    }
}