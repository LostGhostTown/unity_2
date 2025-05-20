using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理菜盘与订单匹配的系统
/// </summary>
public class OrderMatchingSystem : MonoBehaviour
{
    public static OrderMatchingSystem Instance;
    [SerializeField] private AudioSource matchSuccessSound;
    [SerializeField] private AudioSource matchFailSound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// 检查菜盘是否与某个订单匹配
    /// </summary>
    /// <param name="plateFoods">菜盘中的食物</param>
    /// <returns>匹配的订单索引，-1表示没有匹配</returns>
    public int CheckOrderMatch(Dictionary<Food, int> plateFoods)
    {
        List<OrderInfo> orderList = GameManager.instance.orderList;

        for (int i = 0; i < orderList.Count; i++)
        {
            if (IsMatchOrder(plateFoods, orderList[i].orderContent))
            {
                return i;
            }
        }
        return -1; // 没有匹配的订单
    }

    /// <summary>
    /// 判断菜盘食物是否与订单内容匹配
    /// </summary>
    private bool IsMatchOrder(Dictionary<Food, int> plateFoods, Dictionary<Food, int> orderContent)
    {
        // 检查菜盘中是否包含所有订单要求的食物，且数量一致
        foreach (var food in orderContent)
        {
            if (!plateFoods.ContainsKey(food.Key) || plateFoods[food.Key] != food.Value)
            {
                return false;
            }
        }

        // 检查菜盘中是否有订单不需要的食物
        foreach (var food in plateFoods)
        {
            if (!orderContent.ContainsKey(food.Key) || orderContent[food.Key] != food.Value)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 处理匹配成功的订单
    /// </summary>
    /// <param name="orderIndex">匹配的订单索引</param>
    public void HandleSuccessfulMatch(int orderIndex)
    {
        if (orderIndex >= 0 && orderIndex < GameManager.instance.orderList.Count)
        {
            OrderInfo matchedOrder = GameManager.instance.orderList[orderIndex];
            SwitchWaiter.Instance.SwitchWaiterDisplay(); // 切换服务员
            // 增加玩家金钱
            GameManager.instance.money += matchedOrder.value;

            // 播放成功音效
            if (matchSuccessSound != null)
            {
                matchSuccessSound.Play();
            }

            // 移除已完成的订单
            GameManager.instance.orderList.RemoveAt(orderIndex);
            
            // 通知订单变更
            if (OrderManager.Instance != null)
            {
                //OrderManager.Instance.RefreshUnlockedFoodsList();
                //OrderManager.Instance.OnOrdersUpdated?.Invoke(GameManager.instance.orderList);
            }

            Debug.Log($"订单匹配成功！获得 {matchedOrder.value} 金钱。当前金钱：{GameManager.instance.money}");
            
            // 如果订单数量不足，尝试生成新订单
            if (OrderManager.Instance != null && GameManager.instance.orderList.Count == 0)
            {
                OrderManager.Instance.GenerateNewOrder();
            }
        }
    }

    /// <summary>
    /// 处理匹配失败的情况
    /// </summary>
    public void HandleFailedMatch()
    {
        // 播放失败音效
        if (matchFailSound != null)
        {
            matchFailSound.Play();
        }

        Debug.Log("订单匹配失败！请检查菜盘内容是否正确。");
    }
}
