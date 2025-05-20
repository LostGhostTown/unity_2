using UnityEngine;
using UnityEngine.UI;

public class CloseRankPanelButton : MonoBehaviour
{
    public GameObject rankPanel;

    private void Start()
    {
        Button closeButton = GetComponent<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseRankPanel);
        }
    }

    private void CloseRankPanel()
    {
        if (rankPanel != null)
        {
            rankPanel.SetActive(false);
        }
    }
}