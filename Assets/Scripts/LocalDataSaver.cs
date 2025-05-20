using System.IO;
using UnityEngine;

public static class LocalDataSaver
{

    // 保存排行榜数据到本地 JSON 文件
    public static void SaveRankData(int newScore)
    {
        RankData data = LoadRankData();
        data.scores.Add(newScore);
        //将data里的list按照分数从高到低排序
        data.scores.Sort((a, b) => b.CompareTo(a));
        // 取前10个分数
        if (data.scores.Count > 10)
        {
            data.scores = data.scores.GetRange(0, 10);
        }
        // 将数据序列化为 JSON 格式
        string json = JsonUtility.ToJson(data);
        // 将 JSON 数据放在resources/MainMenu/data文件夹下
        string path = Path.Combine(Application.dataPath, "Resources/MainMenu/data/rankData.json");
        // 如果文件夹不存在，则创建
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        }
        // 将 JSON 数据写入文件
        File.WriteAllText(path, json);




    }
    //写一个用于测试保存的方法
    public static void SaveRankDataTest()
    {
        int newScore = Random.Range(0, 1000);
        SaveRankData(newScore);
    }
    // 从本地 JSON 文件加载排行榜数据
    public static RankData LoadRankData()
    {
        string path = Path.Combine(Application.dataPath, "Resources/MainMenu/data/rankData.json");
        if (File.Exists(path))
        {
            // 读取 JSON 数据
            string json = File.ReadAllText(path);
            // 将 JSON 数据反序列化为 RankData 对象
            RankData data = JsonUtility.FromJson<RankData>(json);
            return data;
        }
        else
        {
            Debug.LogError("Rank data file not found: " + path);
            return new RankData(); // 返回一个空的 RankData 对象
        }
    }

}