using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCreater : MonoBehaviour
{
    private static CardCreater _CardCreater;

    /// <summary>
    /// instance mode
    /// </summary>
    public static CardCreater Instance
    {
        get
        {
            if (!_CardCreater)
            {
                _CardCreater = FindObjectOfType(typeof(CardCreater)) as CardCreater;
                if (!_CardCreater) Debug.LogError($"There needs to be one active script on a Gameobject"); //找不到輸出錯誤訊息
                else
                {
                    _CardCreater.Init();
                }
            }

            return _CardCreater;
        }
    }

    /// <summary>
    /// 創建卡牌的委託
    /// </summary>
    /// <returns></returns>
    public delegate Card CardAdd(GameObject deck);

    //可以使用的委託字典
    public List<CardAdd> cardAdds;

    /// <summary>
    /// init this card creater
    /// </summary>
    void Init()
    {
        DontDestroyOnLoad(gameObject);
        
        cardAdds = new List<CardAdd>();
        cardAdds.Add(CreateAttack);
        cardAdds.Add(CreateDefense);
    }

    /// <summary>
    /// 隨機生成卡片
    /// </summary>
    /// <param name="stPos">可生成的方法開始位置</param>
    /// <param name="edPos">結束位置</param>>
    /// <param name="deck">卡組物件</param>
    /// <returns></returns>
    public Card RandomCreate(int stPos, int edPos, GameObject deck)
    {
        int randomIndex = Random.Range(stPos, edPos);   //隨機選取卡片生成方法
        CardAdd creator = cardAdds[randomIndex];    //指定生成方法
        return creator(deck);   //透過creater 創建卡片 回傳生成後的卡片
    }

    Card CreateAttack(GameObject deck)
    {
        Card newCard = deck.AddComponent<A001_Attack>();
        return newCard;
    }

    Card CreateDefense(GameObject deck)
    {
        Card newCard = deck.AddComponent<A002_Defense>();
        return newCard;
    }
}
