using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A001_Attack : Card
{
    string _cardName = "攻擊";
    int _attack = 1;

    public A001_Attack() {
        cardName = _cardName;
        attack = _attack;
    }
}

public class A002_Defense : Card
{
    string _cardName = "防禦";
    int _defense = 1;

    public A002_Defense() {  
        cardName = _cardName;
        defense = _defense;
    }
}
