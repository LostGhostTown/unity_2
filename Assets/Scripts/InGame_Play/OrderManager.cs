using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 订单管理器：负责订单生成、更新和超时逻辑
/// </summary>
public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [Header("订单生成设置")]
    [SerializeField] private float initialOrderGenerationTime = 15f; // 初始订单生成间隔（秒）
    [SerializeField] private float minOrderGenerationTime = 5f; // 最短订单生成间隔（秒）
    [SerializeField] private float orderGenerationAcceleration = 0.95f; // 订单生成加速率
    [SerializeField] private int maxSimultaneousOrders = 4; // 最大同时存在的订单数

    [Header("订单属性设置")]
    [SerializeField] private float baseOrderLifetime = 60f; // 基础订单存活时间（秒）
    [SerializeField] private int minOrderValue = 10; // 最小订单价值
    [SerializeField] private int maxOrderValue = 50; // 最大订单价值
    
    // 已解锁食品列表（可用于）
    private List<Food> unlockedFoods = new List<Food>();
    
    // 订单生成计时器
    private float orderGenerationTimer;
    private float currentGenerationInterval;
    
    // 失败的订单记录
    private int failedOrders = 0;

    // 订单超时事件
    public delegate void OrderTimeoutEvent(OrderInfo order);
    public event OrderTimeoutEvent OnOrderTimeout;

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
    }

    private void Start()
    {
        // 初始化订单生成间隔
        currentGenerationInterval = initialOrderGenerationTime;
        orderGenerationTimer = currentGenerationInterval;

        // 生成初始订单
        if (GameManager.instance != null && GameManager.instance.orderList.Count == 0)
        {
            GenerateNewOrder();
        }

        // 更新已解锁食品列表
        RefreshUnlockedFoodsList();
    }

    private void Update()
    {
        // 更新所有订单的剩余时间
        UpdateOrderLifetimes();

        // 订单生成逻辑
        if (GameManager.instance.orderList.Count < maxSimultaneousOrders)
        {
            orderGenerationTimer -= Time.deltaTime;
            
            if (orderGenerationTimer <= 0)
            {
                GenerateNewOrder();
                
                // 计算下次生成间隔，但不低于最小值
                currentGenerationInterval *= orderGenerationAcceleration;
                currentGenerationInterval-=GameManager.instance.waiterNum;
                currentGenerationInterval = Mathf.Max(currentGenerationInterval, minOrderGenerationTime);
                
                orderGenerationTimer = currentGenerationInterval;
            }
        }
    }

    /// <summary>
    /// 刷新可用于生成订单的已解锁食品列表
    /// </summary>
    public void RefreshUnlockedFoodsList()
    {
        unlockedFoods.Clear();
        
        foreach (var foodPair in GameManager.instance.foodLock)
        {
            if (foodPair.Value > 0) // 如果食品已解锁
            {
                unlockedFoods.Add(foodPair.Key);
            }
        }
    }

    /// <summary>
    /// 生成新订单并添加到GameManager的订单列表中
    /// </summary>
    public void GenerateNewOrder()
    {
        // 确保有解锁的食品可用
        if (unlockedFoods.Count == 0)
        {
            RefreshUnlockedFoodsList();
            if (unlockedFoods.Count == 0) return;
        }

        // 创建新订单
        OrderInfo newOrder = new OrderInfo();
        
        // 随机选择1-3种解锁的食品
        int foodTypeCount = Random.Range(1, Mathf.Min(4, unlockedFoods.Count + 1));
        
        // 计算难度：食品种类和数量影响价值和时间
        int totalFoodCount = 0;
        
        // 随机从已选择的食品中添加
        List<Food> selectedFoods = unlockedFoods.OrderBy(x => Random.value).Take(foodTypeCount).ToList();
        newOrder.orderContent.Clear();
        foreach (Food food in selectedFoods)
        {
            int count = Random.Range(1, 3); // 每种食品1-2个
            newOrder.orderContent.Add(food, count);
            totalFoodCount += count;
        }
        
        // 设置订单价值（基于复杂度）
        newOrder.value = minOrderValue + (totalFoodCount * 5) + (foodTypeCount * 3);
        newOrder.value = Mathf.Min(newOrder.value, maxOrderValue);
        
        // 设置订单完成期限（基于复杂度）
        newOrder.lifeTime = baseOrderLifetime + (totalFoodCount * 5);
        
        // 添加到订单列表
        GameManager.instance.orderList.Add(newOrder);
        
        // 触发订单更新事件
        //OnOrdersUpdated?.Invoke(GameManager.instance.orderList);
        
        Debug.Log($"生成了新订单, {string.Join(',', newOrder.orderContent.Select(f => $"{f.Key} x {f.Value}"))}，包含 {foodTypeCount} 种食品，共计 {totalFoodCount}，价值 {newOrder.value}，剩余时间 {newOrder.lifeTime} 秒");
    }

    /// <summary>
    /// 更新所有订单的剩余时间
    /// </summary>
    private void UpdateOrderLifetimes()
    {
        if (GameManager.instance.orderList.Count == 0) return;
        
        for (int i = GameManager.instance.orderList.Count - 1; i >= 0; i--)
        {
            OrderInfo order = GameManager.instance.orderList[i];
            order.lifeTime -= Time.deltaTime;
            
            // 处理订单超时
            if (order.lifeTime <= 0)
            {
                // 触发订单超时事件
                OnOrderTimeout?.Invoke(order);
                
                // 记录失败的订单
                failedOrders++;
                
                // 从列表中移除
                GameManager.instance.orderList.RemoveAt(i);
                
                Debug.Log("订单已超时！");
            }
        }
        
        // 触发订单更新事件
        //OnOrdersUpdated?.Invoke(GameManager.instance.orderList);
    }

    /// <summary>
    /// 获取当前失败的订单数量
    /// </summary>
    public int GetFailedOrdersCount()
    {
        return failedOrders;
    }
    
    /// <summary>
    /// 重置订单系统
    /// </summary>
    public void ResetOrderSystem()
    {
        // 清除所有当前订单
        GameManager.instance.orderList.Clear();
        
        // 重置订单生成计时器和间隔
        currentGenerationInterval = initialOrderGenerationTime;
        orderGenerationTimer = currentGenerationInterval;
        
        // 重置失败订单计数
        failedOrders = 0;
        
        // 刷新可用食品列表
        RefreshUnlockedFoodsList();
        
        // 生成初始订单
        GenerateNewOrder();
        
        // 触发订单更新事件
        //OnOrdersUpdated?.Invoke(GameManager.instance.orderList);
    }
}
