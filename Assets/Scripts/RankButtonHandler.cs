using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class RankButtonHandler : MonoBehaviour
{
    public Button rankButton;
    public GameObject rankPanel;
    public GameObject rankItemPrefab;
    public Transform rankContentParent;

    private void Start()
    {
        rankButton.onClick.AddListener(ShowRankPanel);
        rankPanel.SetActive(false);
    }

    private void ShowRankPanel()
    {
        rankPanel.SetActive(true);
        ClearRankItems();
        RankData rankData = LocalDataSaver.LoadRankData();
        int rank = 1;
        foreach (int score in rankData.scores)
        {
            GameObject item = Instantiate(rankItemPrefab, rankContentParent);
            TextMeshProUGUI itemText = item.GetComponent<TextMeshProUGUI>();
            itemText.text = $"NO.{rank} : {score}";
            rank++;
        }
    }

    private void ClearRankItems()
    {
        foreach (Transform child in rankContentParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void Test()
    {
        LocalDataSaver.SaveRankDataTest();
    }
}