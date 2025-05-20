
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VInspector;
using Image = UnityEngine.UI.Image;

public class OrderDisplay : MonoBehaviour
{
    public static OrderDisplay instance;
    private void Awake()
    {
        instance = this;
    }
    int ori=0;
    void Update()
    {
        int count = GameManager.instance.orderList.Count;
        if (count !=ori)
        {
            UpdateOrders();
            GameManager.instance.UpdateInspector();
            ori = count;
        }
    }
    public Transform orders;


    public void UpdateOrders()
    {
        int orderNum = orders.childCount;
        int orderInUse = GameManager.instance.orderList.Count;

        for (int i = 0; i < orderNum; i++)
        {
            GameObject order = orders.GetChild(i).gameObject;
            if (i < orderInUse)
            {
                order.gameObject.SetActive(true);
                OrderInfo orderInfo = GameManager.instance.orderList[i];
                order.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = orderInfo.value.ToString(); // 订单价格
                OrderFoodsDisplay orderFoodsDisplay = order.transform.GetChild(0).GetComponent<OrderFoodsDisplay>();
                orderFoodsDisplay.UpdateOrderFoods(orderInfo.orderContent);
                
            }
            else{
                order.gameObject.SetActive(false);
            }
        }

    }


}
