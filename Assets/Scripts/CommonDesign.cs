using System;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

/// <summary>
/// 订单信息类
/// </summary>
[Serializable]
public class OrderInfo
{
    public int value; // 订单价值（完成后获得的金钱）
    [SerializeField]
    public Dictionary<Food, int> orderContent; // 订单包含的食物及数量
    public float lifeTime; // 订单的生命周期，单位为秒
    public int orderID; // 订单ID，用于UI显示和识别

    // 新增：订单生成时间（用于计算已存在时长）
    public float creationTime;

    public OrderInfo()
    {
        // 初始化字典
        orderContent = new Dictionary<Food, int>();
        // 自动设置创建时间和ID
        creationTime = Time.time;
        orderID = System.DateTime.Now.Millisecond;
    }

    /// <summary>
    /// 获取订单的总食物数量
    /// </summary>
    public int GetTotalFoodCount()
    {
        int count = 0;
        foreach (var item in orderContent)
        {
            count += item.Value;
        }
        return count;
    }
}

/// <summary>
/// 食物的枚举类型，方便管理，可随时添加
/// </summary>
public enum Food
{
    肉夹馍,
    窝窝头,
    麻婆豆腐,
    火锅,
    烤全羊,
    烤鸡,
    红烧肉,
    酸菜鱼
}

/// <summary>
/// 食物信息类，用于配菜时使用
/// </summary>
public class FoodInfo
{
    
    public Food food;
    public int sellPrice;
    public Sprite icon;
}


/// <summary>
/// 商店信息类，用于商店中使用
/// </summary>
public class ShopItem
{
    public Food food;

    public int bugPrice;

    public string description;

    public Sprite Icon;

}
