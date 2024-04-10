using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 當前的掌控者
/// </summary>
public enum Keeper
{
    Player,
    Enemy
}

public class BattleSystem : MonoBehaviour
{
    private static BattleSystem _battleSystem;

    /// <summary>
    /// instance mode
    /// </summary>
    public static BattleSystem Instance
    {
        get
        {
            if (!_battleSystem)
            {
                _battleSystem = FindObjectOfType(typeof(BattleSystem)) as BattleSystem;
                if (!_battleSystem) Debug.LogError($"There needs to be one active script on a Gameobject"); //找不到輸出錯誤訊息
                else
                {
                    _battleSystem.Init();
                }
            }

            return _battleSystem;
        }
    }

    [Header("公用資訊")]
    public int nowTurn = 0;
    public int turnLimit = 10;  //回合數限制

    //player setting
    [Header("玩家資訊")]
    public List<Card> playerDeck;
    public List<Card> playerHand;
    public List<Card> playerField;

    public GameObject deckObject_player;
    public GameObject handUI_content;   //手牌的UI顯示 content的物件
    public GameObject cardPrefabs;  //卡牌預製體
    bool isPlayerDone = true; //check is player makes its decision
    int playerHp = 20;

    //enemy setting
    [Header("敵人資訊")]
    public List<Card> enemyDeck;
    public List<Card> enemyHand;
    public List<Card> enemyField;

    public GameObject deckObject_enemy;
    bool isEnemyDone = true; //check is enemy make its decision
    int enemyHP = 20;

    public int maxHandSize = 5; //maximum card numbers in hand

    /// <summary>
    /// init this system
    /// </summary>
    void Init()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("game init...");

        // 初始化玩家和敌人的卡组
        InitializeDeck(playerDeck, deckObject_player, Keeper.Player);
        InitializeDeck(enemyDeck, deckObject_enemy, Keeper.Enemy);

        // 玩家和敌人抽卡
        DrawStartingHand(playerDeck, playerHand, Keeper.Player);
        DrawStartingHand(enemyDeck, enemyHand, Keeper.Enemy);

        Debug.Log("game init done.");

        // 开始游戏
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// 初始化卡組
    /// </summary>
    /// <param name="deck">卡組</param>
    void InitializeDeck(List<Card> deck, GameObject deckObject, Keeper keeper)
    {
        // 这里可以添加初始化卡组的逻辑
     
        //隨機生成卡組
        for(int i =0;i<10;i++)
        {
            int id = Random.Range(0, CardCreater.Instance.cardAdds.Count);
            Card newCard = CardCreater.Instance.CreateCard(id, deckObject);
            deck.Add(newCard);

            if (keeper == Keeper.Player) handUI_content.GetComponent<AdvancedGridLayoutGroupHorizontal>().cellNum = deck.Count; //keep deck size right
        }

        string deckCardInfo = $"{keeper.ToString()} Deck: ";

        for (int i = 0; i < 10; i++)
        {
            deckCardInfo += deck[i].cardName;
            deckCardInfo += ", ";
        }

        Debug.Log(deckCardInfo);
    }

    /// <summary>
    /// 抽取最初的手牌
    /// </summary>
    /// <param name="deck">卡組</param>
    /// <param name="hand"><手牌/param>
    void DrawStartingHand(List<Card> deck, List<Card> hand, Keeper keeper)
    {
        for (int i = 0; i < maxHandSize; i++)
        {
            DrawCard(deck, hand, keeper);
        }
    }

    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="deck">卡組</param>
    /// <param name="hand">手牌</param>
    void DrawCard(List<Card> deck, List<Card> hand, Keeper keeper)
    {
        if (deck.Count > 0 && hand.Count < maxHandSize)
        {
            Card drawnCard = deck[Random.Range(0, deck.Count)];
            hand.Add(drawnCard);
            deck.Remove(drawnCard);

            if(keeper == Keeper.Player)
            {
                GameObject cardObject = Instantiate(cardPrefabs, handUI_content.transform);   //生成卡牌遊戲物體
                Card card = cardObject.GetComponent<Card>();
                card.Copy(drawnCard);   //複製卡牌資料
                card.textMeshProUGUI_name.text = card.cardName; //設置卡牌名稱
                card.handId = hand.Count - 1;   //設置卡牌在手牌中的位置
            }

            if (keeper == Keeper.Player) handUI_content.GetComponent<AdvancedGridLayoutGroupHorizontal>().cellNum = deck.Count; //keep deck size right
        }
    }

    IEnumerator GameLoop()
    {
        Debug.Log("game start");

        while (true)
        {
            nowTurn++;
            Debug.Log($"進入第{nowTurn}回合，回合數限制為{turnLimit}");

            // 玩家回合
            isPlayerDone = false;
            yield return StartCoroutine(PlayerTurn());

            // 敌人回合
            isEnemyDone = false;
            yield return StartCoroutine(EnemyTurn());

            // 檢查是否結束遊戲
            if (enemyHP <= 0)
            {
                Debug.Log("玩家獲勝");
                break;
            }
            else if (playerHp <= 0)
            {
                Debug.Log("生命值歸0，玩家失敗");
                break;
            }
            else if(nowTurn >= turnLimit)
            {
                Debug.Log("回合數達到上限，玩家失敗");
                break;
            }
        }
    }

    IEnumerator PlayerTurn()
    {
        // 这里可以添加玩家回合的逻辑
        Debug.Log("玩家回合");

        yield return new WaitUntil(() => { return isPlayerDone; }) ;
    }

    IEnumerator EnemyTurn()
    {
        // 这里可以添加敌人回合的逻辑
        Debug.Log("enem turn");

        yield return new WaitUntil(() => { return isEnemyDone; });
    }
}
