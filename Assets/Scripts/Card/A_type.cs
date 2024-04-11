using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A001_Attack : Card
{
    int attack = 1; //傷害數值

    public A001_Attack() {
        cardId = 0;
        cardName = "攻擊";
        cardInfo = $"造成{attack}傷害";
        cardEffect = $"造成{attack}傷害";
    }

    public override void Using(Keeper keeper)
    {
        base.Using(keeper);

        if(keeper == Keeper.Player)
        {
            BattleSystem.Instance.HpChange(Keeper.Enemy, -attack);
            Debug.Log($"對敵人造成{attack}點傷害");
        }else if(keeper == Keeper.Enemy) {
            BattleSystem.Instance.HpChange(Keeper.Player, -attack);
            Debug.Log($"受到{attack}點傷害");
        }
    }
}

public class A002_Defense : Card
{
    readonly string _cardName = "防禦";
    
    int defense = 1;

    public A002_Defense() {  
        cardName = _cardName;
    }
}
