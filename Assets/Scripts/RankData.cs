using System.Collections.Generic;

// 排行榜数据类
[System.Serializable]
public class RankData
{
    public List<int> scores;

    public RankData()
    {
        scores = new List<int>();
    }
}