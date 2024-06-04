using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> prefab = new List<GameObject>();//引用的敌人预设
    public WaitForSeconds creationSpeed = new WaitForSeconds(1);//创建间隔 1秒
    public Transform[] path;//路径点
    //当前活着的敌人
    public List<GameObject> enemys = new List<GameObject>();
    bool isStop = false;//是否停止
    UIManager uiManager;//Ul管理器
    int flag = 0;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        for (int i = 0; i < prefab.Count; i++)
        {
            var config = prefab[i].GetComponent<Enemy>();
            config.id = i;
        }
        StartCoroutine(Create());
    }
    public IEnumerator Create()
    {
        while (isStop == false)
        {
            if (GameData.enemyCount > 0)
            {
                //更新UI
                GameData.enemyCount -= 1;
                flag++;
                uiManager.UpdateBattleLevelData();
                //随机创建 UnityEngine.Random.Range(0,10)
                var index = UnityEngine.Random.Range(0, prefab.Count - 1);
                var enemy = GameObject.Instantiate(prefab[index].gameObject);
                var config = enemy.GetComponent<Enemy>();
                enemy.transform.position = path[0].position;
                enemy.transform.eulerAngles = path[0].eulerAngles;
                config.setData(path, this);
                enemys.Add(enemy);
            }
            if (GameData.enemyCount <= 0)
            {
                break;
            }
            yield return creationSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (flag == GameData.killCount && GameData.HP > 0)
        //{
        //    uiManager.ShowGameResult(true);
        //}
    }
    internal void Stop()
    {
        isStop = true;
        for (int i = 0; i < enemys.Count; i++)
        {
            var config = enemys[i].GetComponent<Enemy>();
            config.Stop();
        }
        
        //清除敌人
        enemys.Clear();

    }
}
