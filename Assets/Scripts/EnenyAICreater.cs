using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Handles;

//AI方法的委託長相
public delegate void AIFunction();

public class EnenyAICreater : MonoBehaviour
{
    private static EnenyAICreater _EnenyAICreater;

    /// <summary>
    /// instance mode
    /// </summary>
    public static EnenyAICreater Instance
    {
        get
        {
            if(!_EnenyAICreater)
            {
                _EnenyAICreater = FindObjectOfType(typeof(EnenyAICreater)) as EnenyAICreater;
                if(!_EnenyAICreater) Debug.LogError($"There needs to be one active script on a Gameobject"); //找不到輸出錯誤訊息
                else
                {
                    _EnenyAICreater.Init();
                }
            }

            return _EnenyAICreater;
        }
    }

    // 定义一个列表，用于存储不同类型的 AI 方法
    private List<AIFunction> aiFunctions;

    void Init()
    {
        aiFunctions = new List<AIFunction>();

        // 将不同类型的 AI 方法添加到列表中
        aiFunctions.Add(PerformSimpleAITurn);
        // 添加更多的 AI 方法...
    }

    // 工厂方法，根据指定的 AI 类型返回相应的 AI 方法
    public AIFunction GetAI(int aiIndex)
    {
        if (aiIndex >= 0 && aiIndex < aiFunctions.Count)
        {
            return aiFunctions[aiIndex];
        }
        else
        {
            Debug.LogError($"AI Index {aiIndex} not found!");
            return null;
        }
    }

    // 不同类型的 AI 方法
    private void PerformSimpleAITurn()
    {
        // 实现简单的 AI 决策逻辑
        Debug.Log("Enemy AI is performing simple AI turn...");

        List<Card> hand = BattleSystem.Instance.enemyHand;
        if (hand != null)
        {
            int useId = Random.Range(0, hand.Count);
            Card card = hand[useId];

            BattleSystem.Instance.UseCard(card, Keeper.Enemy);
        }
    }
}
