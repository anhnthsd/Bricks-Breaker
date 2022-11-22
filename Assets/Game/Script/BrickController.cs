using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Script
{
    public class BrickController : MonoBehaviour
    {
        public static BrickController ins;
        public Camera cam;

        public List<BaseBrick> listBricks;
        public Transform parentBrick;
        public TextMeshProUGUI textPrefab;
        public Transform parentText;
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


        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            cam = Camera.main;
        }

        public void CreateBrickWithMap(int[,] lsMap, int[,] lsNumber, int rows)
        {
            _lsMap = lsMap;
            _lsNumber = lsNumber;
            _currentRow = rows;
            lsBrick = new BaseBrick[lsMap.GetLongLength(1), lsMap.GetLongLength(0)];
            if (_currentRow > lsMap.GetLongLength(0) - 1) return;
            for (int j = 0; j < _currentRow; j++)
            {
                AddRowBrick(j);
            }
        }

        public void CreateBrick(int rows)
        {
            _lsMap = new int[10, 7];
            _lsNumber = new int[10, 7];
            _currentRow = rows;
            lsBrick = new BaseBrick[_lsMap.GetLongLength(1), _lsMap.GetLongLength(0)];
            if (_currentRow > _lsMap.GetLongLength(0) - 1) return;
            for (int j = 0; j < _currentRow; j++)
            {
                AddRowBrick(j);
            }
        }

        public void AddRowBrick(int row)
        {
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                int index = row % _lsMap.GetLength(0);
                int number = row < 10 ? Random.Range(1, 6) : Random.Range(7, 20);
                TypeOfBrick brickType = (TypeOfBrick)Random.Range(0, 11);
                CreateNewBrick(i, row, brickType, number);
            }
        }

        public void CreateNewBrick(int i, int j, TypeOfBrick brickType, int number)
        {
            BaseBrick brickNew = null;
            switch (brickType)
            {
                case TypeOfBrick.Normal:
                    brickNew = Instantiate(listBricks[0]);
                    break;
                case TypeOfBrick.Triangle:
                    brickNew = Instantiate(listBricks[1]);
                    brickNew.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case TypeOfBrick.DeleteHorizontal:
                    brickNew = Instantiate(listBricks[4]);
                    brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.DeleteHorizontal);
                    break;
                case TypeOfBrick.DeleteVertical:
                    brickNew = Instantiate(listBricks[4]);

                    brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.DeleteVertical);
                    break;
                case TypeOfBrick.DeleteBoth:
                    brickNew = Instantiate(listBricks[4]);
                    brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.DeleteBoth);
                    break;
                case TypeOfBrick.DeleteSurround:
                    brickNew = Instantiate(listBricks[4]);
                    brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.DeleteSurround);
                    break;
                case TypeOfBrick.AddBall:
                    brickNew = Instantiate(listBricks[2]);
                    break;
                case TypeOfBrick.DamageHorizontal:
                    brickNew = Instantiate(listBricks[3]);
                    brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.DamageHorizontal);
                    break;
                case TypeOfBrick.DamageVertical:
                    brickNew = Instantiate(listBricks[3]);
                    brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.DamageVertical);
                    break;
                case TypeOfBrick.DamageBoth:
                    brickNew = Instantiate(listBricks[3]);
                    brickNew.GetComponent<BaseBrick>().SetSprite(TypeOfBrick.DamageBoth);
                    break;
                case TypeOfBrick.ShootRandom:
                    brickNew = Instantiate(listBricks[5]);
                    break;
            }

            if (brickNew)
            {
                brickNew.transform.SetParent(parentBrick);
                brickNew.SetPosition(new Vector2(-2.45f + 0.8f * i, 2.5f - 0.8f * j));
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

        public void AfterTurn()
        {
            for (int j = lsBrick.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = 0; i < lsBrick.GetLength(0); i++)
                {
                    if (j == 0)
                    {
                        lsBrick[i, j] = null;
                    }
                    else
                    {
                        lsBrick[i, j] = lsBrick[i, j - 1];
                    }

                    if (lsBrick[i, j])
                    {
                        var newPos = new Vector3(-2.45f + 0.8f * i, 2.5f - 0.8f * j, 0);
                        lsBrick[i, j].UpdatePosition(newPos);
                        lsBrick[i, j].i = i;
                        lsBrick[i, j].j = j;
                        if (j == 7)
                        {
                            var item = lsBrick[i, j];
                            if (item.CanDieOnBottom())
                            {
                                GameController.ins.EndGame();
                            }
                            else
                            {
                                if (lsBrick[i, j].typeOfBrick == TypeOfBrick.AddBall)
                                {
                                    var itemS = lsBrick[i, j].GetComponent<ItemAddBall>();
                                    var newBall = Instantiate(BallController.ins.addBall);
                                    newBall.transform.position = newPos;
                                    newBall.transform.DOMove(BallController.ins.ballFirstFall.transform.position, 0.3f)
                                        .OnComplete(() => Destroy(newBall));
                                    BallController.ins.sumAddBall += itemS.sumBall;
                                    itemS.DestroyBrick();
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
            if (_currentRow > _lsMap.GetLongLength(0) - 1) return;

            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                var brickType = (TypeOfBrick)_lsMap[_lsMap.GetLength(0) - 1 - _currentRow, i];
                var number = _lsNumber[_lsMap.GetLength(0) - 1 - _currentRow, i];
                CreateNewBrick(i, 0, brickType, number);
            }

            _currentRow++;
        }

        public bool IsSpecialTurn()
        {
            if (_lsMap.GetLength(0) - _currentRow <= 3) return false;
            for (int j = lsBrick.GetLength(1) - 1; j >= 3; j--)
            {
                for (int i = 0; i < lsBrick.GetLength(0); i++)
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


        private void OnDamageHor(int hor)
        {
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                if (!lsBrick[i, hor]) continue;
                if (lsBrick[i, hor].CanDieOnBottom())
                {
                    lsBrick[i, hor].TakeDamage();
                }
            }
        }

        private void OnDamageVer(int ver)
        {
            for (int i = 0; i < lsBrick.GetLength(1); i++)
            {
                if (!lsBrick[ver, i]) continue;
                if (!lsBrick[ver, i].CanDieOnBottom())
                {
                    lsBrick[ver, i].TakeDamage();
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
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                if (!lsBrick[i, hor]) continue;
                if (lsBrick[i, hor].CanDieOnBottom())
                {
                    lsBrick[i, hor].DestroyBrick();
                }
            }
        }

        private void OnBurstVer(int ver)
        {
            for (int i = 0; i < lsBrick.GetLength(1); i++)
            {
                if (!lsBrick[ver, i]) continue;
                if (lsBrick[ver, i].CanDieOnBottom())
                {
                    lsBrick[ver, i].DestroyBrick();
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
            for (int i = hor - 1; i <= hor + 1; i++)
            {
                for (int j = ver - 1; j <= ver + 1; j++)
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
    }
}