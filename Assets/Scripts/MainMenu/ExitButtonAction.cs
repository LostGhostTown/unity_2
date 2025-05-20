using UnityEngine;
using UnityEngine.UI;

public class ExitButtonAction : MonoBehaviour
{
    public AudioSource soundEffectPlayer; // 音效播放源

    private void Start()
    {
        // 获取按钮组件
        Button exitButton = GetComponent<Button>();
        if (exitButton != null)
        {
            // 为按钮的点击事件添加监听器
            exitButton.onClick.AddListener(OnExitButtonClick);
        }
    }

    private void OnExitButtonClick()
    {
        // 播放音效
        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.Play();
        }

        // 关闭游戏
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}