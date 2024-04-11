using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 當前的掌控者
/// </summary>
[System.Serializable]
public enum Keeper
{
    Player,
    Enemy
}

/// <summary>
/// 以玩家為視角的遊戲狀態
/// </summary>
[System.Serializable]
public enum GameStatus
{
    Battle,
    Win,
    Lose
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

    public int maxHandSize = 100; //maximum card numbers in hand
    public int initHandSize = 5;

    public GameStatus gameStatus; //遊戲狀態

    //player setting
    [Header("甲板相關")]
    public List<Card> playerDeck;
    public List<Card> playerHand;
    public List<Card> playerField;
    public List<Card> playerCemetery;

    //enemy setting
    public List<Card> enemyDeck;
    public List<Card> enemyHand;
    public List<Card> enemyField;
    public List<Card> enemyCemetery;

    [Header("玩家資訊")]
    public GameObject deckObject_player;    //玩家的卡組 遊戲物件
    public GameObject handUI_content;   //負責手牌的UI顯示 content遊戲物件
    public GameObject cardPrefabs;  //卡牌預製體
    public bool isPlayerDone = true; //check is player makes its decision
    public int playerHp = 20;   //玩家生命

    [Header("敵人資訊")]
    public GameObject deckObject_enemy;
    public bool isEnemyDone = true; //check is enemy make its decision
    public int enemyHP = 20;

    /// <summary>
    /// init this system
    /// </summary>
    void Init()
    {
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("game init...");

        // 初始化玩家和敌人的卡组
        InitializeDeck(Keeper.Player);
        InitializeDeck(Keeper.Enemy);

        // 玩家和敌人抽卡
        DrawStartingHand(Keeper.Player);
        DrawStartingHand(Keeper.Enemy);

        Debug.Log("game init done.");

        // 开始游戏
        StartCoroutine(GameLoop());
    }

    /// <summary>
    /// 初始化卡組
    /// </summary>
    /// <param name="deck">卡組</param>
    void InitializeDeck(Keeper keeper)
    {
        //卡組、手牌、卡組遊戲物件
        List<Card> deck;
        GameObject deckObject;

        //根據keeper設置上述三個變數
        if (keeper == Keeper.Player)
        {
            deck = playerDeck;
            deckObject = deckObject_player;
        }
        else
        {
            deck = enemyDeck;
            deckObject = deckObject_enemy;
        }

        // 这里可以添加初始化卡组的逻辑
        RandomInitDeck(deckObject, keeper, deck);
    }

    /// <summary>
    /// 抽取最初的手牌
    /// </summary>
    /// <param name="keeper">使用者</param>
    void DrawStartingHand(Keeper keeper)
    {
        for (int i = 0; i < initHandSize; i++)
        {
            DrawCard(keeper);
        }
    }

    /// <summary>
    /// 抽卡
    /// </summary>
    /// <param name="keeper">使用者</param>
    void DrawCard(Keeper keeper)
    {
        //卡組以及手牌
        List<Card> deck, hand, cemetery;

        //根據keeper決定上述兩個變數
        if(keeper == Keeper.Player)
        {
            deck = playerDeck;
            hand = playerHand;
            cemetery = playerCemetery;
        }else
        {
            deck = enemyDeck;
            hand = enemyHand;
            cemetery = enemyCemetery;
        }

        //如果卡組沒牌 且墓地存在卡牌 把墓地卡牌回收至卡組
        if(deck.Count <= 0 && cemetery.Count > 0)
        {
            while(cemetery.Count > 0)
            {
                Card card = cemetery[0];
                deck.Add(card);
                cemetery.Remove(card);
            }
        }

        //如果卡組有牌 且手牌未超過限制 則抽牌
        if (deck.Count > 0 && hand.Count < maxHandSize)
        {
            Card drawnCard = deck[Random.Range(0, deck.Count)]; //抽出卡牌
            hand.Add(drawnCard);    //添加進手牌
            deck.Remove(drawnCard); //從卡組中移除該手牌

            //if player, updated its deck
            if(keeper == Keeper.Player)
            {
                GameObject cardObject = Instantiate(cardPrefabs, handUI_content.transform);   //生成卡牌遊戲物體
                Card card = cardObject.GetComponent<Card>();    //從遊戲物體中獲取卡牌組件

                card.Copy(drawnCard);   //複製卡牌資料
                card.recCard = drawnCard;   //紀錄當前卡牌是甚麼
                card.textMeshProUGUI_name.text = card.cardName; //更新UI卡牌名稱

                //更新ui顯示空間
                UpdateHand();
            }
        }
    }

    /// <summary>
    /// 使用卡牌
    /// </summary>
    /// <param name="card">要使用的卡牌</param>
    /// <param name="keeper">使用者</param>
    public void UseCard(Card card, Keeper keeper)
    {
        List<Card> hand, cemetery;    //手牌

        //設置手牌
        if (keeper == Keeper.Player)
        {
            hand = playerHand;
            cemetery = playerCemetery;
        }
        else
        {
            hand = enemyHand;
            cemetery = enemyCemetery;
        }

        Debug.Log($"使用 {card.cardName} 卡牌");
        card.Using(keeper); //使用卡牌

        if (keeper == Keeper.Player) isPlayerDone = true;
        else isEnemyDone = true;

        hand.Remove(card);  //從手牌中移除
        cemetery.Add(card); //卡牌移入墓地
        //Destroy(card);  //刪除該組件
    }

    /// <summary>
    /// 更新手牌區域顯示UI
    /// </summary>
    public void UpdateHand()
    {
        handUI_content.GetComponent<AdvancedGridLayoutGroupHorizontal>().cellNum = playerHand.Count; //keep hand size right
    }

    IEnumerator GameLoop()
    {
        Debug.Log("game start");
        gameStatus = GameStatus.Battle; //進入戰鬥狀態

        while (true)
        {
            nowTurn++;
            Debug.Log($"進入第{nowTurn}回合，回合數限制為{turnLimit}");

            // 玩家回合
            yield return StartCoroutine(PlayerTurn());

            if (EndGameCheck() == true) break;

            // 敌人回合
            yield return StartCoroutine(EnemyTurn());

            if(EndGameCheck() == true) break;
        }

        EndGame();
    }

    IEnumerator PlayerTurn()
    {
        // 这里可以添加玩家回合的逻辑
        Debug.Log("玩家回合");

        Debug.Log("玩家抽牌");
        DrawCard(Keeper.Player);    //每回合先抽牌

        isPlayerDone = false;

        yield return new WaitUntil(() => { return isPlayerDone; }) ;
    }

    IEnumerator EnemyTurn()
    {
        // 这里可以添加敌人回合的逻辑
        Debug.Log("enem turn");

        Debug.Log("敵人抽牌");
        DrawCard(Keeper.Enemy); //抽牌
        
        isEnemyDone = false;

        yield return new WaitForSeconds(0.2f);  //等待0.2秒模擬AI思考
        AIFunction aIFunction = EnenyAICreater.Instance.GetAI(0);   //使用簡單AI
        aIFunction();   //執行AI方法

        isEnemyDone = true;

        yield return new WaitUntil(() => { return isEnemyDone; });
    }

    /// <summary>
    /// 改變生命值
    /// </summary>
    /// <param name="keeper">被改變者</param>
    /// <param name="value">改變的數值</param>
    public void HpChange(Keeper keeper, int value)
    {
        if(keeper == Keeper.Player)
        {
            playerHp += value;
        }else
        {
            enemyHP += value;
        }
    }

    /// <summary>
    ///隨機生成卡組的方法
    /// </summary>
    /// <param name="deckObject">卡組遊戲物件</param>
    /// <param name="keeper">使用者</param>
    /// <param name="deck">卡組</param>
    void RandomInitDeck(GameObject deckObject, Keeper keeper, List<Card> deck)
    {
        //隨機生成卡組
        for (int i = 0; i < 10; i++)
        {
            int id = Random.Range(0, CardCreater.Instance.cardAdds.Count);
            Card newCard = CardCreater.Instance.CreateCard(id, deckObject); //create card
            newCard.obj = deckObject;

            deck.Add(newCard);  //add card to deck
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
    /// 檢查遊戲是否結束
    /// </summary>
    bool EndGameCheck()
    {
        // 檢查是否結束遊戲
        if (enemyHP <= 0)
        {
            Debug.Log("玩家獲勝");
            gameStatus = GameStatus.Win;
            return true;
        }
        else if (playerHp <= 0)
        {
            Debug.Log("生命值歸0，玩家失敗");
            gameStatus = GameStatus.Lose;
            return true;
        }
        else if (nowTurn >= turnLimit)
        {
            Debug.Log("回合數達到上限，玩家失敗");
            gameStatus = GameStatus.Lose;
            return true;
        }else
        {
            return false;
        }
    }

    /// <summary>
    /// 遊戲結算
    /// </summary>
    void EndGame()
    {
        if(gameStatus == GameStatus.Win)
        {
            //獲勝收穫結算
        }else
        {
            //失敗結算
        }

        SceneManager.LoadScene("MainPage"); //回到主介面
    }
}
