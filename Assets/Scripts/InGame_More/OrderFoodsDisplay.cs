using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;

public class OrderFoodsDisplay : MonoBehaviour
{
    public void UpdateOrderFoods(Dictionary<Food, int> foodsDic)
    {
        int foodCount = this.transform.childCount; // 获取当前子物体数量
        int foodInUse = foodsDic.Count;
        List<Food> foodList = new List<Food>(foodsDic.Keys); // 获取食物类型列表
        for (int i = 0; i < foodCount; i++)
        {
            Transform food = this.transform.GetChild(i); // 获取第i个子物体的Transform组件
            if (i < foodInUse)
            {
                food.gameObject.SetActive(true); // 激活子物体
                Food foodType = foodList[i]; // 获取食物类型
                int foodNum = foodsDic[foodType]; // 获取食物数量
                food.GetChild(0).GetComponent<Image>().sprite = GetFoodImage(foodType); // 获取食物图片
                food.GetChild(1).GetComponent<TextMeshProUGUI>().text = foodNum.ToString(); // 设置食物数量
            }
            else
            {
                food.gameObject.SetActive(false); // 关闭子物体
            }
        }

    }

    public Sprite GetFoodImage(Food food)
    {
        Sprite foodImage = Resources.Load<Sprite>("InGame_Play/Images/" + food.ToString());
        return foodImage;
    }
}
