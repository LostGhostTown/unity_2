using UnityEngine;
using UnityEngine.UI;

public class SettingPanelController : MonoBehaviour
{
    public AudioSource musicPlayer; // 背景音乐播放器
    public AudioSource soundEffectPlayer; // 音效播放器
    public Button musicButton; // 音乐控制按钮
    public Button soundButton; // 音效控制按钮

    // 按你的图片名称修改变量名（或直接使用原变量名，仅需在Inspector中拖放正确图片）
    public Sprite musicOnSprite; // 音乐开启图片（对应你的"musicon"）
    public Sprite musicOffSprite; // 音乐关闭图片（对应你的"musicoff"）
    public Sprite soundOnSprite; // 音效开启图片（对应你的"soundon"）
    public Sprite soundOffSprite; // 音效关闭图片（对应你的"sounoff"）

    private bool isMusicOn = true; // 默认音乐开启
    private bool isSoundOn = true; // 默认音效开启

    private void Start()
    {
        musicButton.onClick.AddListener(ToggleMusic);
        soundButton.onClick.AddListener(ToggleSound);
        UpdateMusicButtonSprite(); // 初始化音乐按钮图片
        UpdateSoundButtonSprite(); // 初始化音效按钮图片
    }

    private void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        musicPlayer.enabled = isMusicOn; // 或使用 Play()/Stop()
        UpdateMusicButtonSprite();
    }

    private void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        soundEffectPlayer.mute = !isSoundOn; // 直接切换静音状态
        UpdateSoundButtonSprite();
    }

    private void UpdateMusicButtonSprite()
    {
        Image img = musicButton.GetComponent<Image>();
        if (img != null)
        {
            img.sprite = isMusicOn ? musicOnSprite : musicOffSprite;
        }
    }

    private void UpdateSoundButtonSprite()
    {
        Image img = soundButton.GetComponent<Image>();
        if (img != null)
        {
            img.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
        }
    }
}