using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameData
{
    public static float levelID=1;
    public static float coins;
    public static int enemyCount; // ʣ���������
    public static int killCount; // �Ѿ�ɱ��������
    public static float HP; // ˮ��Ѫ������
    public static float time; // ��Ϸ��ʼʱ��
    public static float allEnemyCount;

    public Setting se = new Setting();

    // ���ݹؿ����ݵ�ȥ��ʼ��UI����
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