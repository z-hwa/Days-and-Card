using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("全局屬性")]
    public int cardId;
    public string cardName;
    public int attack;
    public int defense;

    [Header("暫存屬性")]
    public GameObject obj;  //組件所屬於的object
    public Card recCard;    //紀錄的卡片
    public TextMeshProUGUI textMeshProUGUI_name;

    /// <summary>
    /// 複製全局屬性
    /// </summary>
    /// <param name="card">要複製的卡牌</param>
    public void Copy(Card card)
    {
        cardId = card.cardId;
        cardName = card.cardName;
        attack = card.attack;
        defense = card.defense;
    }

    /// <summary>
    /// 用於讓卡牌遊戲物件能夠告訴戰鬥系統
    /// 當前使用的卡牌在手牌中的編號
    /// </summary>
    /// <param name="index">編號</param>
    public void UseCard(int keeper)
    {
        if (keeper == ((int)Keeper.Player)) BattleSystem.Instance.UseCard(recCard, Keeper.Player);
        else BattleSystem.Instance.UseCard(this, Keeper.Enemy);

        Destroy(this.gameObject);
        if (keeper == ((int)Keeper.Player)) BattleSystem.Instance.UpdateHand(); //keep deck size right
    }
}


