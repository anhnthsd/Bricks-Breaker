using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Script
{
    public class BallController : MonoBehaviour
    {
        public static BallController ins;
        private bool isFly = false; 
        public BallScript ball;
        public GameObject direcBall;
        public GameObject addBall;
        public List<GameObject> listDotBall;
        public Transform parentBall;
        public Transform parentDotBall;
        public List<BallScript> lsBalls;

        public Transform parentTxtBall;
        public TextMeshProUGUI textPrefab;
        public TextMeshProUGUI textSumBall;

        public LayerMask wallMask;
        public BallScript ballFirstFall;
        private int _ballFall = 0;
        public int sumAddBall = 0;
        private Coroutine _corShootBall;

        public GameObject fxSpecialBall;

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
        }

        public void Play(int startBall)
        {
            textSumBall = Instantiate(textPrefab);
            CreateBall(startBall, new Vector2(0, -3.33f));
            CreateDotBall(10, new Vector2(0, -3.33f));
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject)
            {
                return;
            }

            if (isFly) return;
            if (Input.GetMouseButtonUp(0))
            {
                isFly = true;
                var point = GameController.ins.cam.ScreenToWorldPoint(Input.mousePosition);
                point.z = 0;

                var ballPos = lsBalls[0].transform.position;
                Vector2 direction = new Vector2(point.x - ballPos.x, point.y - ballPos.y);

                if (point.y < -3f)
                {
                    var anchor = point.x < ballPos.x ? new Vector3(-2.5f, -3f, 0) : new Vector3(2.5f, -3f, 0);
                    direction = anchor - ballPos;
                }

                for (int i = 0; i < listDotBall.Count; i++)
                {
                    if (listDotBall[i].activeInHierarchy)
                        listDotBall[i].SetActive(false);
                }

                textSumBall.gameObject.SetActive(false);
                _corShootBall = StartCoroutine(ShootBall(direction));
            }

            if (Input.GetMouseButton(0))
            {
                var point = GameController.ins.cam.ScreenToWorldPoint(Input.mousePosition);
                point.z = 0;
                var posStart = lsBalls[0].transform.position;

                Vector2 direction = point - posStart;

                if (point.y < -3f)
                {
                    var anchor = point.x < posStart.x ? new Vector3(-2.5f, -3f, 0) : new Vector3(2.5f, -3f, 0);
                    direction = anchor - posStart;
                }
                // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //
                // if (angle < 10 || angle > 170)
                // {
                //     return;
                // }

                var ray = Physics2D.Raycast(posStart, direction, 20, wallMask);
                bool check = true;
                int balls = 0;
                if (ray.collider != null)
                {
                    if (ray.collider.CompareTag("WallHor"))
                    {
                        for (int i = 0; i < listDotBall.Count; i++)
                        {
                            SetDirecBall(i, direction, posStart);

                            Vector2 direcReflect = Vector2.Reflect(direction, Vector2.down);
                            if (listDotBall[i].transform.position.y > ray.point.y)
                            {
                                SetDirecReflectBall(ref check, i, direcReflect, ray, ref balls);
                            }

                            bool disActive = listDotBall[i].transform.position.y < posStart.y;
                            DisActiveDirecBall(disActive, i);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < listDotBall.Count; i++)
                        {
                            SetDirecBall(i, direction, posStart);

                            if (ray.point.x < posStart.x)
                            {
                                Vector2 direcReflect = Vector2.Reflect(direction, Vector2.right);
                                if (listDotBall[i].transform.position.x < ray.point.x)
                                {
                                    SetDirecReflectBall(ref check, i, direcReflect, ray, ref balls);
                                }

                                bool disActive = listDotBall[i].transform.position.x > (0 - ray.point.x - 0.5f) ||
                                                 listDotBall[i].transform.position.y > 3.5f;

                                DisActiveDirecBall(disActive, i);
                            }
                            else
                            {
                                Vector2 direcReflect = Vector2.Reflect(direction, Vector2.left);
                                if (listDotBall[i].transform.position.x > ray.point.x)
                                {
                                    SetDirecReflectBall(ref check, i, direcReflect, ray, ref balls);
                                }

                                bool disActive = listDotBall[i].transform.position.x < (0 - ray.point.x + 0.5f) ||
                                                 listDotBall[i].transform.position.y > 3.5f;
                                DisActiveDirecBall(disActive, i);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < listDotBall.Count; i++)
                    {
                        SetDirecBall(i, direction, posStart);
                    }
                }
            }
        }

        void SetDirecBall(int i, Vector2 direction, Vector3 posStart)
        {
            if (!listDotBall[i].activeInHierarchy)
            {
                listDotBall[i].SetActive(true);
            }

            listDotBall[i].transform.position = (Vector3)(direction.normalized * (i * 0.5f)) + posStart;
        }

        void DisActiveDirecBall(bool disActive, int i)
        {
            if (disActive)
            {
                listDotBall[i].SetActive(false);
            }
            else
            {
                if (i == listDotBall.Count - 1 && listDotBall.Count < 30)
                {
                    listDotBall.Add(Instantiate(direcBall, parentDotBall, true));
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

            listDotBall[i].transform.position = direcReflect.normalized * ((i - balls) * 0.5f) + ray.point;
        }

        IEnumerator ShootBall(Vector2 direction)
        {
            for (int i = 0; i < lsBalls.Count; i++)
            {
                lsBalls[i].Fly(direction.normalized * 450);
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void CreateBall(int ballCount, Vector2 position)
        {
            for (int i = 0; i < ballCount; i++)
            {
                var newBall = Instantiate(ball,parentBall,true);
                newBall.transform.position = position;
                lsBalls.Add(newBall);
            }

            var pos = lsBalls[0].transform.position + new Vector3(0.4f,0,0);
            textSumBall.GetComponent<RectTransform>().anchoredPosition = GameController.ins.cam.WorldToScreenPoint(pos);
            textSumBall.transform.SetParent(parentTxtBall);
            textSumBall.gameObject.SetActive(true);
            textSumBall.text = lsBalls.Count.ToString();
        }

        public void CreateDotBall(int ballCount, Vector2 position)
        {
            for (int i = 0; i < ballCount; i++)
            {
                var direcB = Instantiate(direcBall, parentDotBall, true);
                direcBall.SetActive(false);
                listDotBall.Add(direcB);
            }
        }

        public void OnBallFall()
        {
            for (int i = 0; i < lsBalls.Count; i++)
            {
                if (lsBalls[i].state == StateBall.Done)
                {
                    _ballFall++;
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

            if (_ballFall != lsBalls.Count) return;
            GameController.ins.AfterTurn();
        }

        public void AfterTurn()
        {
            CreateBall(sumAddBall, lsBalls[0].transform.position);
            sumAddBall = 0;
            isFly = false;
            ballFirstFall = null;
            _ballFall = 0;
            textSumBall.transform.SetParent(null);
            var pos = lsBalls[0].transform.position + new Vector3(0.4f,0,0);
            textSumBall.GetComponent<RectTransform>().anchoredPosition = GameController.ins.cam.WorldToScreenPoint(pos);
            textSumBall.transform.SetParent(BrickController.ins.parentText);
        }

        public void CreateNewBall(Vector3 newPos, int sumBall)
        {
            var newBall = Instantiate(addBall);
            newBall.transform.position = newPos;

            if (ballFirstFall)
            {
                newBall.transform.DOMove(ballFirstFall.transform.position, 0.3f)
                    .OnComplete(() => Destroy(newBall));
            }
            else
            {
                newBall.transform.DOMove(lsBalls[0].transform.position, 0.3f)
                    .OnComplete(() => Destroy(newBall));
            }

            sumAddBall += sumBall;
        }
        
        public void SpecialTurn(int countAddBall)
        {
            var fx = Instantiate(fxSpecialBall);
            fx.transform.position = lsBalls[0].transform.position;
            Destroy(fx, 1);
            CreateBall(countAddBall,lsBalls[0].transform.position);
        }

        public void AfterSpecialTurn(int countAddBall)
        {
            for (int i = 0; i < countAddBall; i++)
            {
                lsBalls[lsBalls.Count - 1 - i].gameObject.SetActive(false);
                lsBalls.RemoveAt(lsBalls.Count - 1 - i);
            }
        }

        public void OnAddBall(int count, Vector3 pos)
        {
            sumAddBall += count;
            var newBall = Instantiate(addBall);
            newBall.transform.position = pos;
            newBall.transform.DOMove(new Vector3(pos.x, -3.35f, 0), 0.2f)
                .OnComplete((() => StartCoroutine(MoveAddBall(newBall))));
        }

        private IEnumerator MoveAddBall(GameObject newBall)
        {
            while (ballFirstFall == null && isFly)
            {
                yield return new WaitForSeconds(1);
            }

            if (ballFirstFall)
            {
                newBall.transform.DOMove(ballFirstFall.transform.position, 0.2f).OnComplete((() => Destroy(newBall)));
            }
            else newBall.transform.DOMove(lsBalls[0].transform.position, 0.2f).OnComplete((() => Destroy(newBall)));
        }

        public void Btn()
        {
            if (_corShootBall != null)
            {
                StopCoroutine(_corShootBall);
            }

            foreach (var itemBall in lsBalls)
            {
                itemBall.rigi.velocity = Vector2.zero;
                itemBall.state = StateBall.Stop;
            }

            if (ballFirstFall == null)
            {
                foreach (var ballScript in lsBalls)
                {
                    ballScript.ChangePosition(new Vector3(0, -3.33f, 0));
                }
            }
            else
            {
                foreach (var ballScript in lsBalls)
                {
                    ballScript.ChangePosition(ballFirstFall.transform.position);
                }
            }

            DOVirtual.DelayedCall(0.2f, () => { GameController.ins.AfterTurn(); });
        }
    }
}