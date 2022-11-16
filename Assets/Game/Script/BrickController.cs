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

        public List<GameObject> listBricks;

        public List<Sprite> lsBrickSprites;
        public TextMeshProUGUI textPrefab;
        public Transform parentText;
        public GameObject[,] lsBrick;

        public GameObject fxBrick;
        public GameObject fxDamage;
        public GameObject fxDamageVer;


        public int countRow = 4;

        public int[,] lsMap = new int[,]
        {
            { 1, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 2, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 4, 1, 1, 1, 3, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 1, 1, 1, 2, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 1, 1, 1, 1, 1 },
            { 0, 1, 3, 1, 1, 3, 1 },
            { 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 4, 0, 0, 0 },
            { 0, 0, 0, 0, 4, 0, 13 },
        };

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            cam = Camera.main;

            CreateBrick();
        }

        public void CreateBrick()
        {
            lsBrick = new GameObject[lsMap.GetLongLength(1), lsMap.GetLongLength(0)];
            if (countRow > lsMap.GetLongLength(0) - 1) return;
            for (int j = 0; j < countRow; j++)
            {
                for (int i = 0; i < lsBrick.GetLongLength(0); i++)
                {
                    var brickType = (TypeOfBrick)lsMap[lsMap.GetLongLength(0) - (countRow - j), i];
                    CreateNewBrick(i, j, brickType);
                }
            }
        }

        public void CreateNewBrick(int i, int j, TypeOfBrick brickType)
        {
            GameObject brickNew = null;
            TextMeshProUGUI txt;
            int indexSprite = 0;
            switch (brickType)
            {
                case TypeOfBrick.Normal:
                    brickNew = Instantiate(listBricks[0]);

                    txt = Instantiate(textPrefab);
                    txt.GetComponent<RectTransform>().anchoredPosition =
                        cam.WorldToScreenPoint(brickNew.transform.position);

                    brickNew.GetComponent<NormalBrick>().textBrick = txt;
                    txt.transform.SetParent(parentText);
                    break;
                case TypeOfBrick.Triangle:
                    brickNew = Instantiate(listBricks[1]);
                    brickNew.transform.rotation = Quaternion.Euler(0, 0, 180);
                    
                    txt = Instantiate(textPrefab);
                    txt.GetComponent<RectTransform>().anchoredPosition =
                        cam.WorldToScreenPoint(brickNew.transform.position);

                    brickNew.GetComponent<NormalBrick>().textBrick = txt;
                    txt.transform.SetParent(parentText);
                    break;
                case TypeOfBrick.DeleteHorizontal:
                    brickNew = Instantiate(listBricks[4]);

                    indexSprite = 29;
                    txt = Instantiate(textPrefab);
                    txt.GetComponent<RectTransform>().anchoredPosition =
                        cam.WorldToScreenPoint(brickNew.transform.position);

                    brickNew.GetComponent<BurstBrick>().textBrick = txt;
                    txt.transform.SetParent(parentText);
                    break;
                case TypeOfBrick.DeleteVertical:
                    brickNew = Instantiate(listBricks[4]);

                    indexSprite = 30;
                    txt = Instantiate(textPrefab);
                    txt.GetComponent<RectTransform>().anchoredPosition =
                        cam.WorldToScreenPoint(brickNew.transform.position);

                    brickNew.GetComponent<BurstBrick>().textBrick = txt;
                    txt.transform.SetParent(parentText);
                    break;
                case TypeOfBrick.DeleteBoth:
                    brickNew = Instantiate(listBricks[4]);

                    indexSprite = 31;
                    txt = Instantiate(textPrefab);
                    txt.GetComponent<RectTransform>().anchoredPosition =
                        cam.WorldToScreenPoint(brickNew.transform.position);

                    brickNew.GetComponent<BurstBrick>().textBrick = txt;
                    txt.transform.SetParent(parentText);
                    break;
                case TypeOfBrick.DeleteSurround:
                    brickNew = Instantiate(listBricks[4]);

                    indexSprite = 32;
                    txt = Instantiate(textPrefab);
                    txt.GetComponent<RectTransform>().anchoredPosition =
                        cam.WorldToScreenPoint(brickNew.transform.position);

                    brickNew.GetComponent<BurstBrick>().textBrick = txt;
                    txt.transform.SetParent(parentText);
                    break;
                case TypeOfBrick.AddBall:
                    brickNew = Instantiate(listBricks[2]);
                    // indexSprite = Random.Range(22, 28);
                    // indexSprite = 23;
                    // brickNew.GetComponent<BrickC>().type = TypeItem.DamageVer;
                    indexSprite = Random.Range(25, 27);
                    brickNew.GetComponent<ItemAddBall>().sumBall = indexSprite switch
                    {
                        25 => 1,
                        26 => 2,
                        27 => 3,
                        _ => brickNew.GetComponent<ItemAddBall>().sumBall
                    };

                    break;
                case TypeOfBrick.Damage:
                    brickNew = Instantiate(listBricks[3]);
                    indexSprite = Random.Range(22, 24);
                    brickNew.GetComponent<ItemDamage>().type = indexSprite switch
                    {
                        22 => TypeOfBrick.DamageHorizontal,
                        23 => TypeOfBrick.DamageVertical,
                        24 => TypeOfBrick.DamageBoth,
                        _ => brickNew.GetComponent<ItemDamage>().type
                    };
                    break;
                case TypeOfBrick.DamageHorizontal:
                    brickNew = Instantiate(listBricks[3]);
                    indexSprite = 22;
                    brickNew.GetComponent<ItemDamage>().type = TypeOfBrick.DamageHorizontal;
                    break;
                case TypeOfBrick.DamageVertical:
                    brickNew = Instantiate(listBricks[3]);
                    indexSprite = 22;
                    brickNew.GetComponent<ItemDamage>().type = TypeOfBrick.DamageVertical;
                    break;
                case TypeOfBrick.DamageBoth:
                    brickNew = Instantiate(listBricks[3]);
                    indexSprite = 22;
                    brickNew.GetComponent<ItemDamage>().type = TypeOfBrick.DamageBoth;
                    break;
                case TypeOfBrick.Fixed:
                    break;
                case TypeOfBrick.ShootRandom:
                    brickNew = Instantiate(listBricks[5]);
                    indexSprite = 33;
                    break;
                default:
                    break;
            }

            if (brickNew)
            {
                brickNew.GetComponent<BaseBrick>().SetPosition(new Vector2(-2.45f + 0.8f * i, 2.5f - 0.8f * j));
                // brickNew.GetComponent<SpriteRenderer>().sprite = lsBrickSprites[indexSprite];
                brickNew.GetComponent<BaseBrick>().OnSpawn(Random.Range(1, 150));
                brickNew.GetComponent<BaseBrick>().i = i;
                brickNew.GetComponent<BaseBrick>().j = j;
                lsBrick[i, j] = brickNew;
            }
        }

        public void DelBrick(int i, int j)
        {
            lsBrick[i, j] = null;
        }

        public void AfterTurn()
        {
            for (int j = lsBrick.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = 0; i < lsBrick.GetLength(0); i++)
                {
                    if (lsBrick[i, j])
                    {
                        if (lsBrick[i, j].GetComponent<ItemDamage>())
                        {
                            if (lsBrick[i, j].GetComponent<ItemDamage>().isOver)
                                lsBrick[i, j].GetComponent<BaseBrick>().OnDelete();
                        }

                        if (lsBrick[i, j].GetComponent<ItemShootBall>())
                        {
                            if (lsBrick[i, j].GetComponent<ItemShootBall>().isOver)
                                lsBrick[i, j].GetComponent<BaseBrick>().OnDelete();
                        }
                    }

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
                        lsBrick[i, j].GetComponent<BaseBrick>().SetPosition(newPos);
                        lsBrick[i, j].GetComponent<BaseBrick>().i = i;
                        lsBrick[i, j].GetComponent<BaseBrick>().j = j;
                        if (j == 7)
                        {
                            var item = lsBrick[i, j];
                            if (item.GetComponent<NormalBrick>() || item.GetComponent<BurstBrick>())
                            {
                                Debug.Log("END GAME");
                            }

                            if (lsBrick[i, j].GetComponent<ItemAddBall>())
                            {
                                var itemS = lsBrick[i, j].GetComponent<ItemAddBall>();
                                var newBall = Instantiate(BallController.ins.addBall);
                                newBall.transform.position = newPos;
                                newBall.transform.DOMove(BallController.ins.ballFirstFall.transform.position, 0.3f)
                                    .OnComplete(() => Destroy(newBall));
                                BallController.ins.sumAddBall += itemS.sumBall;
                                itemS.OnDelete();
                            }
                        }
                    }
                }
            }

            AddMap();
        }

        public void AddMap()
        {
            if (countRow > lsMap.GetLongLength(0) - 1) return;
            {
                for (int i = 0; i < lsBrick.GetLength(0); i++)
                {
                    var brickType = (TypeOfBrick)lsMap[lsMap.GetLongLength(0) - 1 - countRow, i];
                    CreateNewBrick(i, 0, brickType);
                }

                countRow++;
            }
        }

        public bool IsSpecialTurn()
        {
            for (int j = lsBrick.GetLength(1) - 1; j >= 3; j--)
            {
                for (int i = 0; i < lsBrick.GetLength(0); i++)
                {
                    var item = lsBrick[i, j];
                    if (!item) continue;
                    if (item.GetComponent<NormalBrick>() || item.GetComponent<BurstBrick>())
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public void OnDamageHor(int hor)
        {
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                var brickType = (TypeOfBrick)lsMap[hor, i];
                if (brickType != TypeOfBrick.Empty && brickType != TypeOfBrick.AddBall)
                {
                    if (lsBrick[i, hor].GetComponent<BaseBrick>())
                    {
                        lsBrick[i, hor].GetComponent<BaseBrick>().OnDamage();
                    }
                }
            }
        }

        public void OnDamageVer(int ver)
        {
            for (int i = 0; i < lsBrick.GetLength(1); i++)
            {
                var brickType = (TypeOfBrick)lsMap[i, ver];
                if (brickType != TypeOfBrick.Empty && brickType != TypeOfBrick.AddBall)
                {
                    if (!lsBrick[ver, i]) continue;
                    if (lsBrick[ver, i].GetComponent<BaseBrick>())
                    {
                        lsBrick[ver, i].GetComponent<BaseBrick>().OnDamage();
                    }
                }
            }
        }

        public void OnDamageBoth(int hor, int ver)
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
            var fx2 = Instantiate(fxDamageVer);
            fx2.transform.position = position;
            Destroy(fx2, 0.3f);
        }

        private void ShowFxHor(Vector2 position)
        {
            var fx = Instantiate(fxDamage);
            fx.transform.position = position;
            Destroy(fx, 0.3f);
        }

        public void OnBurst(int hor, int ver)
        {
            for (int i = 0; i < lsBrick.GetLength(0); i++)
            {
                var brickType = (TypeOfBrick)lsMap[hor, i];
                if (brickType == TypeOfBrick.Normal || brickType == TypeOfBrick.Triangle ||
                    brickType == TypeOfBrick.DeleteHorizontal)
                {
                    if (!lsBrick[i, hor]) continue;
                    if (lsBrick[i, hor].GetComponent<BaseBrick>())
                    {
                        lsBrick[i, hor].GetComponent<BaseBrick>().OnDelete();
                    }
                }
            }
        }
    }
}