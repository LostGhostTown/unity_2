using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpdateMoney : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // UI文本组件

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$" + GameManager.instance.money.ToString(); // 更新UI文本
    }
}
