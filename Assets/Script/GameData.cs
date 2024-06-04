using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameData
{
    public static float levelID=1;
    public static float coins;
    public static int enemyCount; // 剩余怪物数量
    public static int killCount; // 已经杀怪物数量
    public static float HP; // 水晶血量数量
    public static float time; // 游戏开始时间
    public static float allEnemyCount;

    public Setting se = new Setting();

    // 根据关卡数据的去初始化UI界面
    public static void SetLevelData(int level)
    {
        levelID = level;
        coins = 5000 * level;
        enemyCount = 1 + 100 * level;
        allEnemyCount = enemyCount;
        killCount = 0;
        time = 0;
        HP = 100 * level;
    }
}