using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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

    //player setting
    public List<Card> playerDeck;
    public List<Card> playerHand;
    public List<Card> playerField;

    public GameObject deckObject_player;
    int playerHp = 20;
    
    //enemy setting
    public List<Card> enemyDeck;
    public List<Card> enemyHand;
    public List<Card> enemyField;

    public GameObject deckObject_enemy;
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
        // 初始化玩家和敌人的卡组
        InitializeDeck(playerDeck, deckObject_player);
        InitializeDeck(enemyDeck, deckObject_enemy);

        // 玩家和敌人抽卡
        DrawStartingHand(playerDeck, playerHand);
        DrawStartingHand(enemyDeck, enemyHand);

        // 开始游戏
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// 初始化卡組
    /// </summary>
    /// <param name="deck">卡組</param>
    void InitializeDeck(List<Card> deck, GameObject deckObject)
    {
        // 这里可以添加初始化卡组的逻辑
     
        //隨機生成卡組
        for(int i =0;i<10;i++)
        {
            Card newCard = CardCreater.Instance.RandomCreate(0, CardCreater.Instance.cardAdds.Count, deckObject);
            deck.Add(newCard);
        }

        string deckCardInfo = "Deck: ";

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
    void DrawStartingHand(List<Card> deck, List<Card> hand)
    {
        for (int i = 0; i < maxHandSize; i++)
        {
            DrawCard(deck, hand);
        }
    }

    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="deck">卡組</param>
    /// <param name="hand">手牌</param>
    void DrawCard(List<Card> deck, List<Card> hand)
    {
        if (deck.Count > 0 && hand.Count < maxHandSize)
        {
            Card drawnCard = deck[Random.Range(0, deck.Count)];
            hand.Add(drawnCard);
            deck.Remove(drawnCard);
        }
    }

    IEnumerator GameLoop()
    {
        while (playerDeck.Count > 0 || enemyDeck.Count > 0)
        {
            // 玩家回合
            yield return StartCoroutine(PlayerTurn());

            // 敌人回合
            yield return StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerTurn()
    {
        // 这里可以添加玩家回合的逻辑
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        // 这里可以添加敌人回合的逻辑
        yield return null;
    }
}
