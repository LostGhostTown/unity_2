using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using VInspector;

/// <summary>
/// 用于存储单一局内信息，一局结束后清空
/// 本脚本内只保留数据，不保留方法，方法在其他脚本中实现
/// 【单例】
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }
    public bool start = true;

    public int money = 0; // 玩家当前金钱

    [SerializeField]
    public Dictionary<Food, int> foodLock = new Dictionary<Food, int>(); // 食物解锁情况，int=1表示解锁，int=0表示未解锁
    [SerializeField]
    public List<string> foodCanUse = new List<string>(); // 食物列表，包含所有可使用的食物类型
    public int waiterNum = 1; // 当前服务员数量

    [SerializeField]
    public List<OrderInfo> orderList = new List<OrderInfo>(); // 当前订单列表
    [SerializeField]
    public List<string> orderFoodNames = new List<string>(); // 当前订单食物名称列表
    void Start()
    {
        if (start)
        {
            Init(); // 关键：调用初始化方法
        }
        else
        {

        }
    }

    public void Init()
    {
        // 初始化游戏数据
        // 初始化金钱和服务员数量
        money = 100;
        waiterNum = 1;
        start = false;
        // 初始化食物解锁情况，前三个是基础食物，一开始就解锁,其他Food枚举类型的食物未解锁
        foodLock.Clear();
        for (int i = 0; i < Enum.GetNames(typeof(Food)).Length; i++)
        {
            foodLock.Add((Food)i, 0);
        }
        foodLock[Food.火锅] = 1;
        foodLock[Food.烤全羊] = 1;
        foodLock[Food.烤鸡] = 1;
        UpdateInspector();
        // 初始化订单列表
        orderList.Clear();

        // 随机生成初始订单
        OrderManager.Instance.GenerateNewOrder();


        foreach (KeyValuePair<Food, int> pair in foodLock)
        {
            Food food = pair.Key;
            int unlockStatus = pair.Value;
            Debug.Log($"食物: {food}, 解锁状态: {unlockStatus}");
        }

        Debug.Log("游戏数据初始化完成");

        // 初始化订单管理器
        // 注意：确保场景中已经有OrderManager实例，或通过Resources加载预制体
        OrderManager orderManager = FindObjectOfType<OrderManager>();
        if (orderManager == null)
        {
            Debug.LogWarning("场景中没有OrderManager，请添加OrderManager组件到场景中");
        }
    }

    public void Again()
    {
        UpdateInspector();
        // 初始化订单列表
        orderList.Clear();

        // 随机生成初始订单
        OrderManager.Instance.GenerateNewOrder();
    }
    public void UpdateInspector()
    {
        foodCanUse.Clear();
        orderFoodNames.Clear();
        foreach (KeyValuePair<Food, int> pair in foodLock)
        {

            foodCanUse.Add(pair.Key.ToString() + " " + pair.Value);


        }

        foreach (OrderInfo order in orderList)
        {
            string temp = "";
            foreach (KeyValuePair<Food, int> pair in order.orderContent)
            {
                if (pair.Value > 0)
                {
                    temp = temp + order.orderID + pair.Key.ToString() + " x " + pair.Value.ToString();

                }
                temp = temp + " -- ";
            }
            orderFoodNames.Add(temp);
        }
    }
}
