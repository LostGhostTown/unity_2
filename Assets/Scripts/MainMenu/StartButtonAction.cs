using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtonAction : MonoBehaviour
{
    public AudioSource soundEffectPlayer; // 音效播放源

    private void Start()
    {
        // 获取按钮组件
        Button startButton = GetComponent<Button>();
        if (startButton != null)
        {
            // 为按钮的点击事件添加监听器
            startButton.onClick.AddListener(OnStartButtonClick);
        }
    }

    private void OnStartButtonClick()
    {
        // 播放音效
        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.Play();
        }
        // 加载 InGame_Play 场景
        SceneManager.LoadScene("InGame");
    }
}