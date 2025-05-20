using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;


public class ShopControl : MonoBehaviour
{
    public Transform shopButtons;
    public int[] price={500,200,300,400,500,600,700,800};   

    public ShopButton _waiter;
    public bool isWaiterCanBuy=true;
    public TextMeshProUGUI _moneyShow;

    //购买失败的音效（mp3格式）
    public AudioSource _buyFailAudio;

    //购买成功的音效（mp3格式）
    public AudioSource _buySuccessAudio;


    public void Start()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _nextButton.onClick.AddListener(OnNextButtonClick);
        _buyButton1.onClick.AddListener(OnBuyButton1Click);
        _buyButton2.onClick.AddListener(OnBuyButton2Click);
        _buyButton3.onClick.AddListener(OnBuyButton3Click);
        _buyButton4.onClick.AddListener(OnBuyButton4Click);
        _buyButton5.onClick.AddListener(OnBuyButton5Click);
        _buyButton6.onClick.AddListener(OnBuyButton6Click);
        _buyButton7.onClick.AddListener(OnBuyButton7Click);
        _buyButton8.onClick.AddListener(OnBuyButton8Click);
        _WaiterButton.onClick.AddListener(OnWaiterButtonClick);

        UpdateFoodState();
        if(GameManager.instance.waiterNum <2)
        {
            _waiter._flag.SetActive(false);
            _waiter._price.text="1000";
            _waiter.isBought=false;
        }
        else
        {
            _waiter._flag.SetActive(true);
            _waiter._price.text="Bought";
            _waiter.isBought=true;
        }

    }

    void Update()
    {
        _moneyShow.text = GameManager.instance.money.ToString();
    }
    public void UpdateFoodState()
    {
        for(int i = 0; i < shopButtons.childCount; i++)

        {
            Food food = shopButtons.GetChild(i).GetComponent<ShopButton>().food;
            if(GameManager.instance.foodLock[food]==1)
            {
                shopButtons.GetChild(i).GetComponent<ShopButton>()._flag.SetActive(true);
                shopButtons.GetChild(i).GetComponent<ShopButton>()._price.text = "Bought";
                shopButtons.GetChild(i).GetComponent<ShopButton>().isBought=true;
            }
            else
            {
                shopButtons.GetChild(i).GetComponent<ShopButton>()._price.text = price[i].ToString();
            }

        }
    }


    

    #region 按钮
    public Button _exitButton;
    public Button _nextButton;
    public Button _buyButton1;
    public Button _buyButton2;
    public Button _buyButton3;
    public Button _buyButton4;
    public Button _buyButton5;
    public Button _buyButton6;
    public Button _buyButton7;
    public Button _buyButton8;
    public Button _WaiterButton;

    public void OnExitButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNextButtonClick()
    {
        SceneManager.LoadScene("InGame");
    }

    public void OnBuyButton1Click()
    {
        if(shopButtons.GetChild(0).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[0])
            {
            //播放成功音效
            //_buySuccessAudio.Play();
            GameManager.instance.foodLock[shopButtons.GetChild(0).GetComponent<ShopButton>().food] = 1;
            GameManager.instance.money -= price[0];
            GameManager.instance.UpdateInspector();
            shopButtons.GetChild(0).GetComponent<ShopButton>().isBought = true;
            //更新食物状态
            UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnBuyButton2Click()
    {
        if(shopButtons.GetChild(1).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[1])
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.foodLock[shopButtons.GetChild(1).GetComponent<ShopButton>().food] = 1;
                GameManager.instance.money -= price[1];
                GameManager.instance.UpdateInspector();
                shopButtons.GetChild(1).GetComponent<ShopButton>().isBought = true;
                //更新食物状态
                UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnBuyButton3Click()
    {
        if(shopButtons.GetChild(2).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[2])
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.foodLock[shopButtons.GetChild(2).GetComponent<ShopButton>().food] = 1;
                GameManager.instance.money -= price[2];
                GameManager.instance.UpdateInspector();
                shopButtons.GetChild(2).GetComponent<ShopButton>().isBought = true;
                //更新食物状态
                UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnBuyButton4Click()
    {
        if(shopButtons.GetChild(3).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[3])
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.foodLock[shopButtons.GetChild(3).GetComponent<ShopButton>().food] = 1;
                GameManager.instance.money -= price[3];
                GameManager.instance.UpdateInspector();
                shopButtons.GetChild(3).GetComponent<ShopButton>().isBought = true;
                //更新食物状态
                UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnBuyButton5Click()
    {
        if(shopButtons.GetChild(4).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[4])
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.foodLock[shopButtons.GetChild(4).GetComponent<ShopButton>().food] = 1;
                GameManager.instance.money -= price[4];
                GameManager.instance.UpdateInspector();
                shopButtons.GetChild(4).GetComponent<ShopButton>().isBought = true;
                //更新食物状态
                UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnBuyButton6Click()
    {
        if(shopButtons.GetChild(5).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[5])
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.foodLock[shopButtons.GetChild(5).GetComponent<ShopButton>().food] = 1;
                GameManager.instance.money -= price[5];
                GameManager.instance.UpdateInspector();
                shopButtons.GetChild(5).GetComponent<ShopButton>().isBought = true;
                //更新食物状态
                UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnBuyButton7Click()
    {
        if(shopButtons.GetChild(6).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[6])
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.foodLock[shopButtons.GetChild(6).GetComponent<ShopButton>().food] = 1;
                GameManager.instance.money -= price[6];
                GameManager.instance.UpdateInspector();
                shopButtons.GetChild(6).GetComponent<ShopButton>().isBought = true;
                //更新食物状态
                UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnBuyButton8Click()
    {
        if(shopButtons.GetChild(7).GetComponent<ShopButton>().isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > price[7])
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.foodLock[shopButtons.GetChild(7).GetComponent<ShopButton>().food] = 1;
                GameManager.instance.money -= price[7];
                GameManager.instance.UpdateInspector();
                shopButtons.GetChild(7).GetComponent<ShopButton>().isBought = true;
                //更新食物状态
                UpdateFoodState();
            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }

    public void OnWaiterButtonClick()
    {
        if(_waiter.isBought)
        {
            //播放失败音效
            //_buyFailAudio.Play();
        }
        else
        {
            if(GameManager.instance.money > 1000)
            {
                //播放成功音效
                //_buySuccessAudio.Play();
                GameManager.instance.waiterNum += 2;
                GameManager.instance.money -= 1000;
                GameManager.instance.UpdateInspector();
                _waiter.isBought = true;
                _waiter._flag.SetActive(true);
                _waiter._price.text="Bought";

            }
            else
            {
                //播放失败音效
                //_buyFailAudio.Play();
            }
        }
    }
    #endregion
    
    
    
    
    
    
    
    
    

    
}
