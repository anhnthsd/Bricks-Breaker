using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game.Script
{
    public class GameController : MonoBehaviour
    {
        public static GameController ins;
        private Camera cam;
        public BallScript ball;
        public GameObject direcBall;
        public List<GameObject> listDirecBall;
        public List<GameObject> listBricks;

        public List<Sprite> lsBrickSprites;
        public TextMeshProUGUI textPrefab;
        public Transform parentText;
        public List<BallScript> lsBalls;
        private bool isFly = false;
        public GameObject[,] lsBrick;

        public int[,] lsMap = new int[,]
        {
            { 0, 1, 1, 0, 1, 2, 1 },
            { 3, 1, 0, 0, 1, 2, 1 },
            { 0, 1, 2, 1, 1, 1, 3 },
            { 1, 0, 1, 0, 1, 1, 1 },
            // { 0, 1, 1, 0, 1, 2, 1 },
            // { 3, 1, 0, 0, 1, 2, 1 },
            // { 0, 1, 2, 1, 1, 1, 3 },
            // { 1, 0, 1, 0, 1, 1, 1 },
        };

        public LayerMask wallMaskHor;
        public LayerMask wallMaskVer;

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            cam = Camera.main;
            CreateBrick();
            CreateBall(10, new Vector2(0, -3.35f));
        }

        void Update()
        {
            if (isFly) return;
            if (Input.GetMouseButtonUp(0))
            {
                isFly = true;
                var point = cam.ScreenToWorldPoint(Input.mousePosition);
                point.z = 0;
                var ballPos = lsBalls[0].transform.position;
                Vector2 direction = new Vector2(point.x - ballPos.x, point.y - ballPos.y);
                for (int i = 0; i < listDirecBall.Count; i++)
                {
                    if (listDirecBall[i].activeInHierarchy)
                        listDirecBall[i].SetActive(false);
                }

                StartCoroutine(ShootBall(direction));
            }

            if (Input.GetMouseButton(0))
            {
                var point = cam.ScreenToWorldPoint(Input.mousePosition);
                point.z = 0;
                var posStart = lsBalls[0].transform.position;
                Vector2 direction = new Vector2(point.x - posStart.x, point.y - posStart.y);

                var ray = Physics2D.Raycast(posStart, direction, 20, wallMaskHor);
                bool check = true;
                int balls = 0;
                if (ray.collider != null)
                {
                    if (ray.collider.CompareTag("WallHor"))
                    {
                        for (int i = 0; i < listDirecBall.Count; i++)
                        {
                            SetDirecBall(i, direction, posStart);

                            Vector2 direcReflect = Vector2.Reflect(direction, Vector2.down);
                            if (listDirecBall[i].transform.position.y > ray.point.y)
                            {
                                SetDirecReflectBall(ref check, i, direcReflect, ray, ref balls);
                            }

                            bool disActive = listDirecBall[i].transform.position.y < posStart.y;
                            DisActiveDirecBall(disActive, i);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < listDirecBall.Count; i++)
                        {
                            SetDirecBall(i, direction, posStart);

                            if (ray.point.x < posStart.x)
                            {
                                Vector2 direcReflect = Vector2.Reflect(direction, Vector2.right);
                                if (listDirecBall[i].transform.position.x < ray.point.x)
                                {
                                    SetDirecReflectBall(ref check, i, direcReflect, ray, ref balls);
                                }

                                bool disActive = listDirecBall[i].transform.position.x > (0 - ray.point.x - 0.5f) ||
                                                 listDirecBall[i].transform.position.y > 3.5f;

                                DisActiveDirecBall(disActive, i);
                            }
                            else
                            {
                                Vector2 direcReflect = Vector2.Reflect(direction, Vector2.left);
                                if (listDirecBall[i].transform.position.x > ray.point.x)
                                {
                                    SetDirecReflectBall(ref check, i, direcReflect, ray, ref balls);
                                }

                                bool disActive = listDirecBall[i].transform.position.x < (0 - ray.point.x + 0.5f) ||
                                                 listDirecBall[i].transform.position.y > 3.5f;
                                DisActiveDirecBall(disActive, i);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listDirecBall.Count; i++)
                    {
                        SetDirecBall(i, direction, posStart);
                    }
                }
            }
        }

        void SetDirecBall(int i, Vector2 direction, Vector3 posStart)
        {
            if (!listDirecBall[i].activeInHierarchy)
            {
                listDirecBall[i].SetActive(true);
            }

            listDirecBall[i].transform.position = (Vector3)(direction.normalized * (i * 0.5f)) + posStart;
        }

        void DisActiveDirecBall(bool disActive, int i)
        {
            if (disActive)
            {
                listDirecBall[i].SetActive(false);
            }
            else
            {
                if (i == listDirecBall.Count - 1 && listDirecBall.Count < 30)
                {
                    listDirecBall.Add(Instantiate(direcBall));
                }
            }
        }

        void SetDirecReflectBall(ref bool check, int i, Vector2 direcReflect, RaycastHit2D ray, ref int balls)
        {
            if (check)
            {
                check = false;
                balls = i - 1;
            }

            listDirecBall[i].transform.position = direcReflect.normalized * ((i - balls) * 0.5f) + ray.point;
        }

        IEnumerator ShootBall(Vector2 direction)
        {
            for (int i = 0; i < lsBalls.Count; i++)
            {
                lsBalls[i].Fly(direction.normalized * 400);
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void CreateBall(int ballCount, Vector2 position)
        {
            for (int i = 0; i < ballCount; i++)
            {
                var newBall = Instantiate(ball);
                newBall.transform.position = position;
                lsBalls.Add(newBall);
                var direcB = Instantiate(direcBall);
                direcBall.SetActive(false);
                listDirecBall.Add(direcB);
            }
        }

        public void CreateBrick()
        {
            lsBrick = new GameObject[lsMap.GetLongLength(1), lsMap.GetLongLength(0)];
            for (int i = 0; i < lsBrick.GetLongLength(0); i++)
            {
                for (int j = 0; j < lsBrick.GetLongLength(1); j++)
                {
                    GameObject brickNew = null;
                    TextMeshProUGUI txt;
                    int indexSprite = 0;
                    switch (lsMap[j, i])
                    {
                        case 1:
                            brickNew = Instantiate(listBricks[0]);

                            brickNew.transform.position = new Vector3(-2.45f + 0.8f * i, 2.5f - 0.8f * j, 0);
                            indexSprite = Random.Range(0, 10);
                            txt = Instantiate(textPrefab);
                            txt.GetComponent<RectTransform>().anchoredPosition =
                                cam.WorldToScreenPoint(brickNew.transform.position);

                            brickNew.GetComponent<NormalBrick>().textBrick = txt;
                            txt.transform.SetParent(parentText);
                            break;
                        case 2:
                            brickNew = Instantiate(listBricks[1]);
                            brickNew.transform.position = new Vector3(-2.45f + 0.8f * i, 2.5f - 0.8f * j, 0);
                            indexSprite = Random.Range(11, 21);
                            txt = Instantiate(textPrefab);
                            txt.GetComponent<RectTransform>().anchoredPosition =
                                cam.WorldToScreenPoint(brickNew.transform.position + new Vector3(0.1f, -0.1f, 0));

                            brickNew.GetComponent<NormalBrick>().textBrick = txt;
                            txt.transform.SetParent(parentText);
                            break;
                        case 3:
                            brickNew = Instantiate(listBricks[2]);
                            brickNew.transform.position = new Vector3(-2.45f + 0.8f * i, 2.5f - 0.8f * j, 0);
                            indexSprite = Random.Range(22, 28);
                            break;
                        default:
                            break;
                    }

                    if (brickNew)
                    {
                        brickNew.GetComponent<SpriteRenderer>().sprite = lsBrickSprites[indexSprite];
                        brickNew.GetComponent<BaseBrick>().OnSpawn(Random.Range(1, 10));
                        brickNew.GetComponent<BaseBrick>().i = i;
                        brickNew.GetComponent<BaseBrick>().j = j;
                        lsBrick[i, j] = brickNew;
                    }
                }
            }
        }

        public BallScript ballFirstFall;
        private int ballFall = 0;

        public void OnBallFall()
        {
            for (int i = 0; i < lsBalls.Count; i++)
            {
                if (lsBalls[i].state == StateBall.Done)
                {
                    ballFall++;
                    if (ballFirstFall == null)

                    {
                        ballFirstFall = lsBalls[i];
                    }
                    else
                    {
                        lsBalls[i].ChangePosition(ballFirstFall.transform.position);
                    }

                    lsBalls[i].state = StateBall.Start;
                    break;
                }
            }

            if (ballFall != lsBalls.Count) return;
            isFly = false;
            ballFirstFall = null;
            ballFall = 0;
        }
    }
}