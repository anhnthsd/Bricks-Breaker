using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Script;
using UnityEngine;

public class ModeClassic : BaseModePlay
{
    // [SerializeField] private int brickOnTurn = 0;
    [SerializeField] private bool isSpecialTurn = false;
    [SerializeField] private int sumBallSpecial = 9;
    private readonly int[,] _lsMap = new int[,]
    {
        { 1, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
        { 1, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
        { 1, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
    };
    
    private readonly int[,] _lsNumber = new int[,]
    {
        { 1, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 2, 1, 1 },
        { 0, 2, 1, 3, 1, 1, 2 },
        { 0, 1, 1, 1, 2, 3, 1 },
        { 4, 1, 3, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 3, 1, 2 },
        { 1, 1, 1, 1, 1, 1, 1 },
        { 3, 1, 3, 1, 1, 1, 2 },
        { 1, 1, 1, 1, 1, 1, 1 },
    };
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void StartGame()
    {
        ballController.CreateBall(startBall, new Vector2(0, -3.33f));
        ballController.CreateDotBall(10, new Vector2(0, -3.33f));
        brickController.CreateBrick(currentRow);
    }

    public override void AfterTurn()
    {
        if (isSpecialTurn)
        {
            BallController.ins.AfterSpecialTurn(sumBallSpecial);
            isSpecialTurn = false;
        }
        
        BrickController.ins.AfterTurn();

        BallController.ins.AfterTurn();
        IncreaseScore();
    }

    public override void SpecialTurn(int rows = 1)
    {
    }

    public override void EndGame()
    {
        Debug.Log("ENDGAME");
    }

    public override void IncreaseScore()
    {
    }

    public void CreateMap()
    {
        
    }
}
