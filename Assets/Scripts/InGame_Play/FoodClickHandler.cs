using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.InGame_Play;
using UnityEngine;
using UnityEngine.UI;

public class FoodClickHandler : MonoBehaviour
{

    public GameObject targetArea; // 餐盘区域
    private List<GameObject> plateFoods = new List<GameObject>(); // 保存餐盘里的所有食物
    private float padding = 10f; // 食物之间的间距
    private float minScale = 0.01f; // 最小缩放比例
    private float maxScale = 1.2f; // 最大缩放比例



    // 当前餐盘中的食物
    private Dictionary<Food, int> plateFoodsDict
        => plateFoods.GroupBy(f => f.GetComponent<FoodTypeComponent>().FoodType).ToDictionary(f => f.Key, f => f.Count());

    // 提交按钮
    [SerializeField] private Button submitButton;

    // 点击音效
    [SerializeField] private AudioSource foodClickSound;
    [SerializeField] private AudioSource submitSound;

    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager 未赋值");
            return;
        }
        RectTransform targetRect = targetArea.GetComponent<RectTransform>();
        Debug.Log($"targetArea 宽度: {targetRect.sizeDelta.x}, 高度: {targetRect.sizeDelta.y}");
        // 打印 foodLock 的大小
        int foodLockCount = GameManager.instance.foodLock.Count;
        Debug.Log($"foodLock 中的键值对数量: {foodLockCount}");

        foreach (KeyValuePair<Food, int> pair in GameManager.instance.foodLock)
        {
            Food food = pair.Key;
            int unlockStatus = pair.Value;
            Debug.Log($"食物: {food}, 解锁状态: {unlockStatus}");
        }

        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foods)
        {
            Button button = food.GetComponent<Button>();
            if (button == null)
            {
                button = food.AddComponent<Button>();
            }
            string foodName = food.name;
            if (System.Enum.IsDefined(typeof(Food), foodName))
            {
                Food foodEnum = (Food)System.Enum.Parse(typeof(Food), foodName);
                if (GameManager.instance.foodLock.TryGetValue(foodEnum, out int lockState))
                {
                    if (lockState == 0)
                    {
                        button.enabled = false;
                        food.SetActive(false); // 隐藏未解锁食物
                    }
                    else
                    {
                        GameObject localFood = food;
                        food.AddComponent<FoodTypeComponent>().FoodType = foodEnum;
                        button.onClick.AddListener(() => OnLeftFoodClick(localFood));
                    }
                }
                else
                {
                    Debug.LogWarning($"FoodLock 中缺少 {foodName} 的键");
                }
            }
            else
            {
                Debug.LogWarning($"{foodName} 不是有效的 Food 枚举值");
            }
        }



        // 添加提交按钮点击事件
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(SubmitPlate);
        }
        //audioSource.Play();
    }

    void OnLeftFoodClick(GameObject food)
    {//点击左侧食物的事件
        // 实例化食物并设置参数
        GameObject foodCopy = Instantiate(food, targetArea.transform);
        foodCopy.GetComponent<Button>().enabled = true;
        Button copyButton = foodCopy.GetComponent<Button>();
        copyButton.onClick.AddListener(() => OnPlateFoodClick(foodCopy));
        plateFoods.Add(foodCopy);
        if (foodClickSound != null)
        {
            foodClickSound.Play();
        }
        UpdateFoodPositionsAndScale(); // 更新排列位置和缩放
    }

    void OnPlateFoodClick(GameObject plateFood)
    {//点击餐盘里食物的事件
        plateFoods.Remove(plateFood);
        Destroy(plateFood);
        if (foodClickSound != null)
        {
            foodClickSound.Play();
        }
        UpdateFoodPositionsAndScale(); // 移除后重新排列和缩放
    }

    void ClearPlate()
    {
        foreach (GameObject food in plateFoods)
        {
            Destroy(food);
        }
        plateFoods.Clear();
        plateFoodsDict.Clear();
        UpdatePlateUI();
        Debug.Log("餐盘已清空");
    }

    void UpdateFoodPositionsAndScale()
    {
        RectTransform targetRect = targetArea.GetComponent<RectTransform>();
        float availableWidth = targetRect.sizeDelta.x;
        float availableHeight = targetRect.sizeDelta.y;

        int numRows = 1;
        float currentRowWidth = 0;
        float scaleFactor = 1f;
        List<List<GameObject>> rows = new List<List<GameObject>> { new List<GameObject>() };

        // 先尝试不缩放排列
        for (int i = 0; i < plateFoods.Count; i++)
        {
            RectTransform foodRect = plateFoods[i].GetComponent<RectTransform>();
            float foodWidth = foodRect.sizeDelta.x;

            if (currentRowWidth + foodWidth + padding > availableWidth)
            {
                numRows++;
                currentRowWidth = 0;
                rows.Add(new List<GameObject>());
            }

            currentRowWidth += foodWidth + padding;
            rows[rows.Count - 1].Add(plateFoods[i]);
        }

        if (plateFoods.Count != 0)
        {
            // 检查高度是否超出，如果超出则进行缩放
            float totalHeight = numRows * (plateFoods[0].GetComponent<RectTransform>().sizeDelta.y + padding);
            if (totalHeight > availableHeight)
            {
                scaleFactor = availableHeight / totalHeight;
                scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);
            }

            // 应用缩放
            foreach (GameObject food in plateFoods)
            {
                food.transform.localScale = Vector3.one * scaleFactor;
            }

            // 重新计算排列
            rows.Clear();
            rows.Add(new List<GameObject>());
            currentRowWidth = 0;
            numRows = 1;

            for (int i = 0; i < plateFoods.Count; i++)
            {
                RectTransform foodRect = plateFoods[i].GetComponent<RectTransform>();
                float foodWidth = foodRect.sizeDelta.x * scaleFactor;

                if (currentRowWidth + foodWidth + padding > availableWidth)
                {
                    numRows++;
                    currentRowWidth = 0;
                    rows.Add(new List<GameObject>());
                }

                currentRowWidth += foodWidth + padding;
                rows[rows.Count - 1].Add(plateFoods[i]);
            }

            // 设置位置
            float startY = availableHeight / 2 - (plateFoods[0].GetComponent<RectTransform>().sizeDelta.y * scaleFactor + padding) / 2;
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                float currentRowStartX = -availableWidth / 2 + (rows[rowIndex][0].GetComponent<RectTransform>().sizeDelta.x * scaleFactor + padding) / 2;
                for (int colIndex = 0; colIndex < rows[rowIndex].Count; colIndex++)
                {
                    RectTransform foodRect = rows[rowIndex][colIndex].GetComponent<RectTransform>();
                    float xPos = currentRowStartX + colIndex * (foodRect.sizeDelta.x * scaleFactor + padding);
                    float yPos = startY - rowIndex * (foodRect.sizeDelta.y * scaleFactor + padding);
                    foodRect.localPosition = new Vector3(xPos, yPos, 0);
                }
            }
        }
    }

    /// <summary>
    /// 添加食物到餐盘
    /// </summary>
    public void AddFoodToPlate(Food foodType)
    {
        // 检查食物是否已解锁
        if (GameManager.instance.foodLock[foodType] == 0)
        {
            Debug.Log($"食物 {foodType} 尚未解锁！");
            return;
        }

        // 播放点击音效
        if (foodClickSound != null)
        {
            foodClickSound.Play();
        }

        // 添加食物到餐盘
        if (plateFoodsDict.ContainsKey(foodType))
        {
            plateFoodsDict[foodType]++;
        }
        else
        {
            plateFoodsDict.Add(foodType, 1);
        }

        // 更新UI显示
        UpdatePlateUI();

        Debug.Log($"添加食物 {foodType} 到餐盘，当前数量: {plateFoodsDict[foodType]}");
    }

    /// <summary>
    /// 更新餐盘的UI显示
    /// </summary>
    private void UpdatePlateUI()
    {
    }

    /// <summary>
    /// 提交餐盘进行订单匹配
    /// </summary>
    public void SubmitPlate()
    {
        // 播放提交音效
        if (submitSound != null)
        {
            submitSound.Play();
        }

        // 如果餐盘为空，直接返回
        if (plateFoodsDict.Count == 0)
        {
            Debug.Log("餐盘为空，无法提交！");
            return;
        }

        // 检查订单匹配
        int matchedOrderIndex = OrderMatchingSystem.Instance.CheckOrderMatch(plateFoodsDict);

        if (matchedOrderIndex >= 0)
        {
            // 匹配成功
            OrderMatchingSystem.Instance.HandleSuccessfulMatch(matchedOrderIndex);
            OrderDisplay.instance.UpdateOrders();

            Debug.Log($"匹配到订单: {matchedOrderIndex}");
        }
        else
        {
            // 匹配失败
            OrderMatchingSystem.Instance.HandleFailedMatch();

            Debug.Log("没有匹配的订单");
        }

        // 无论成功与否，清空餐盘
        ClearPlate();
    }
}