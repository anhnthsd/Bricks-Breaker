using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Script;
using UnityEngine;

public class ModeTower : BaseModePlay
{
    [SerializeField] private int brickOnTurn = 0;
    [SerializeField] private bool isSpecialTurn = false;
    [SerializeField] private int sumBallSpecial = 9;
    private readonly int[,] _lsMap = new int[,]
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
        { 0, 1, 1, 1, 1, 3, 1 },
        { 0, 0, 3, 0, 1, 3, 1 },
        { 7, 8, 9, 10, 11, 1, 1 },
        { 0, 1, 2, 1, 1, 5, 1 },
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
        { 1, 3, 1, 3, 2, 1, 3 },
        { 1, 2, 1, 1, 1, 1, 1 },
        { 1, 1, 100, 100, 100, 100, 10 },
        { 100, 2, 1, 1, 1, 1, 3 },
    };
    
    public override void StartGame()
    {
        ballController.CreateBall(startBall, new Vector2(0, -3.33f));
        ballController.CreateDotBall(10, new Vector2(0, -3.33f));
        brickController.CreateBrickWithMap(_lsMap, _lsNumber,currentRow);
    }

    public override void AfterTurn()
    {
        brickOnTurn = 0;
        if (isSpecialTurn)
        {
            BallController.ins.AfterSpecialTurn(sumBallSpecial);
            isSpecialTurn = false;
        }
        BrickController.ins.AfterTurn();

        BallController.ins.AfterTurn();

        if (BrickController.ins.IsSpecialTurn())
        {
            isSpecialTurn = true;
            SpecialTurn(2);
        }
    }

    public override void SpecialTurn(int rows = 1)
    {
        for (int r = 0; r < rows; r++)
        {
            DOVirtual.DelayedCall(0.2f, () => { BrickController.ins.AfterTurn(); });
        }

        BallController.ins.SpecialTurn(sumBallSpecial);
    }

    public override void EndGame()
    {
        Debug.Log("ENDGAME");
    }

    public override void IncreaseScore()
    {
        brickOnTurn++;
        Score += 10 * brickOnTurn;
    }
}
