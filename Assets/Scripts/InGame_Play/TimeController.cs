using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 引入场景管理命名空间

public class TimeController : MonoBehaviour
{
    public Text countdownText; // 倒计时文本
    public float totalTime = 300f; // 总时间（5 分钟 = 300 秒）
    private float currentTime; // 当前剩余时间
    public GameManager gameManager; // 引用 GameManager 脚本

    [SerializeField] private OrderManager orderManager; // 引用 OrderManager 脚本

    public delegate void GameOverEvent();
    public event GameOverEvent OnGameOver;

    void Start()
    {
        currentTime = totalTime;
        UpdateCountdownText();
        
        // 获取OrderManager引用
        if (orderManager == null)
        {
            orderManager = FindObjectOfType<OrderManager>();
            if (orderManager == null)
            {
                Debug.LogError("场景中没有OrderManager组件");
            }
        }
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateCountdownText();

            if (currentTime <= 0)
            {
                currentTime = 0;
                GameOver();
            }
        }
    }

    void UpdateCountdownText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        // 触发游戏结束事件
        if (OnGameOver != null)
        {
            OnGameOver();
        }
        Debug.Log("游戏结束，开始结算");
        LocalDataSaver.SaveRankData((int)gameManager.money);
        // 记录失败的订单数量
        int failedOrders = 0;
        if (orderManager != null)
        {
            failedOrders = orderManager.GetFailedOrdersCount();
            // 可以为失败订单添加惩罚逻辑
            Debug.Log($"未完成的订单数量: {failedOrders}");
        }

        // 读取 GameManager 中的 money 成员
        if (gameManager != null)
        {
            float money = gameManager.money;
            Debug.Log("当前拥有的金钱: " + money);
        }

        // 跳转至 shop 场景
        SceneManager.LoadScene("shop");
    }
}