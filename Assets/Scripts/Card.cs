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
    public int handId;
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
}


