using UnityEngine;
using UnityEngine.UI;

public class SetButtonAction : MonoBehaviour
{
    public AudioSource soundEffectPlayer; // 音效播放源
    public GameObject settingPanel; // 设置控制面板

    private void Start()
    {
        // 获取按钮组件
        Button setButton = GetComponent<Button>();
        if (setButton != null)
        {
            // 为按钮的点击事件添加监听器
            setButton.onClick.AddListener(OnSetButtonClick);
        }
    }

    private void OnSetButtonClick()
    {
        // 播放音效
        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.Play();
        }

        // 切换设置控制面板的显示状态
        if (settingPanel != null)
        {
            settingPanel.SetActive(!settingPanel.activeSelf);
        }
    }
}