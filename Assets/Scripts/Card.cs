using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("全局屬性")]
    public int cardId;  //對應儲存在卡牌列表中的值
    public string cardName;
    public string cardInfo;
    public string cardEffect;

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
        cardInfo = card.cardInfo;
        cardEffect = card.cardEffect;
    }

    /// <summary>
    /// 用於讓卡牌遊戲物件能夠告訴戰鬥系統
    /// 當前使用的卡牌在手牌中的編號
    /// </summary>
    /// <param name="index">編號</param>
    public void UseCard(int keeper)
    {
        //檢測是否能夠使用卡牌
        if (keeper == ((int)Keeper.Player) && BattleSystem.Instance.isPlayerDone == true)
        {
            Debug.Log("現在不是你的回合");
            return;
        }
        else if(keeper == ((int)Keeper.Enemy) && BattleSystem.Instance.isEnemyDone == true) return;
     
        if (keeper == ((int)Keeper.Player))
        {
            BattleSystem.Instance.UseCard(recCard, Keeper.Player);
        }
        else
        {
            BattleSystem.Instance.UseCard(recCard, Keeper.Enemy);
        }

        Destroy(this.gameObject);
        if (keeper == ((int)Keeper.Player)) BattleSystem.Instance.UpdateHand(); //keep deck size right
    }

    /// <summary>
    /// 使用卡牌效果
    /// </summary>
    /// <param name="keeper">使用者</param>
    public virtual void Using(Keeper keeper)
    {

    }
}


