using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public enum ItemType { Food, Waiter }

    [System.Serializable]
    public class ShopItem
    {
        public Food itemKey; // 必须完全匹配Food枚举命名（如"Hot_pot"）
        public int price;
        public ItemType type;
        public Button buyButton; // 父对象Button
        public Image iconImage;  // 子对象Image（DishXIcon）
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI priceText;
        public GameObject boughtMark;
    }

    [Header("UI Settings")]
    public TextMeshProUGUI moneyText;
    public Button exitButton;
    public Button nextLevelButton;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip buySuccessSound;
    public AudioClip buyFailSound;

    [Header("Shop Items")]
    public ShopItem[] shopItems;

    private GameManager _gameManager;


    void Start()
    {
        _gameManager = GameManager.instance;
        InitializeReferences(); // 新增的初始化引用方法

        InitializeUI();
        SetupButtons();
    }

    // 新增方法：确保正确获取嵌套的图标引用
    private void InitializeReferences()
    {
        foreach (var item in shopItems)
        {
            if (item.buyButton != null && item.iconImage == null)
            {
                // 从按钮的子对象中查找图标
                item.iconImage = item.buyButton.GetComponentInChildren<Image>();

                // 禁用图标的事件接收
                if (item.iconImage != null)
                {
                    item.iconImage.raycastTarget = false;
                }
            }
        }
    }



    private bool IsBasicFood(Food itemKey)
    {
        return itemKey == Food.火锅 || itemKey == Food.烤全羊 || itemKey == Food.烤鸡;
    }



    private void InitializeUI()
    {
        moneyText.text = $"{_gameManager.money}";

        foreach (var item in shopItems)
        {
            bool isBought = IsPurchased(item.itemKey);

            item.boughtMark.SetActive(isBought);
            item.buyButton.interactable = !isBought;
            item.priceText.text = $"${item.price}";

            // 配置透明按钮
            var colors = item.buyButton.colors;
            colors.normalColor = Color.clear;
            colors.highlightedColor = new Color(1, 1, 1, 0.2f);
            colors.pressedColor = new Color(1, 1, 1, 0.4f);
            colors.disabledColor = Color.clear;
            item.buyButton.colors = colors;
        }
    }

    private bool IsPurchased(Food itemKey)
    {

        if (_gameManager.foodLock[itemKey] == 1) return true;

        return false;
    }

    private void SetupButtons()
    {
        foreach (var item in shopItems)
        {
            item.buyButton.onClick.RemoveAllListeners();
            item.buyButton.onClick.AddListener(() =>
            {
                StartCoroutine(ButtonPressEffect(item.iconImage.transform));
                TryPurchase(item);
            });
        }

        exitButton.onClick.AddListener(() =>
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        nextLevelButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("InGame");
        });
    }

    private IEnumerator ButtonPressEffect(Transform iconTransform)
    {
        if (iconTransform != null)
        {
            iconTransform.localScale = Vector3.one * 0.95f;
            yield return new WaitForSeconds(0.08f);
            iconTransform.localScale = Vector3.one;
        }
    }

    private void TryPurchase(ShopItem item)
    {
        if (IsBasicFood(item.itemKey))
        {
            PlaySound(buyFailSound);
            ShowMessage("基础食物已解锁");
            return;
        }

        if (IsPurchased(item.itemKey))
        {
            PlaySound(buyFailSound);
            ShowMessage("已购买");
            return;
        }

        if (_gameManager.money < item.price)
        {
            PlaySound(buyFailSound);
            ShowMessage("金钱不足");
            return;
        }

        _gameManager.money -= item.price;
        _gameManager.foodLock[item.itemKey] = 1;

        if (item.type == ItemType.Food && System.Enum.TryParse(item.itemKey.ToString(), out Food food))
        {
            _gameManager.foodLock[food] = 1;
        }
        else if (item.type == ItemType.Waiter)
        {
            _gameManager.waiterNum++;
        }

        item.boughtMark.SetActive(true);
        item.buyButton.interactable = false;
        moneyText.text = $"{_gameManager.money}";

        PlaySound(buySuccessSound);
        ShowMessage("购买成功");
        GameManager.instance.UpdateInspector();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void ShowMessage(string msg)
    {
        Debug.Log(msg);
    }
}