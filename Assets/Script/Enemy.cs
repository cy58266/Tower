using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int id;
    public string enemyName;
    public string type;

    public int level = 1;
    public float hp = 100;//血量
    public float attack = 1;//攻击
    public float defensive = 10;//防御
    public float moveSpeed = 5;//初始速度
    public float gold = 50;//死亡掉落金币
    public float initialMoveSpeed;//初始速度
    public bool moving = false;//是否移动 判断是否需要寻路

    //动画控制器
    Animator animator;//动画控制器
    Rigidbody _rigidbody;//刚体
    int pointIndex = 0;//路点的索引
    public Transform nextPoint;//下一个路点
    public Transform[] pointList;//路点集合

    UIManager uiManager;//UI管理器  更新UI界面的数据
    EnemyManager enemyManager;
    //怪物的出生，碰撞检测（炮塔的子弹），怪物是不是到达了终点
    public void Awake()
    {
        animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        uiManager = GameObject.Find($"Canvas").GetComponent<UIManager>();


    }

    public void setData(Transform[] pointList, EnemyManager enemyManager)
    {
        if (GameData.levelID <= 0)
        {
            return;
        }
        //设置敌人属性
        level = (int)(GameData.levelID * 2);
        hp *= GameData.levelID;
        attack *= GameData.levelID;
        defensive *= GameData.levelID;
        moveSpeed *= GameData.levelID;
        gold *= GameData.levelID;

        initialMoveSpeed = moveSpeed;
        moving = true;

        this.pointList = pointList;
        this.enemyManager = enemyManager;
    }

    public void Move()
    {
        if (moving == false)
        {
            return;
        }
        //设置默认路点列表的第一个
        if (nextPoint == null)
        {
            pointIndex = 0;
            nextPoint = pointList[pointIndex];
        }
        if (Vector3.Distance(this.transform.position, nextPoint.position) >= 1f)
        {
            transform.LookAt(nextPoint.position, Vector3.up);
            this._rigidbody.velocity = transform.forward * moveSpeed;
        }
        else
        {
            pointIndex += 1;
            //到终点
            if (pointIndex >= pointList.Length)
            {
                GameObject.Destroy(this.gameObject);
                this._rigidbody.velocity = Vector3.zero;
                Debug.Log("已经到达终点！");
                moving = false;
                if (GameData.HP > 0)
                {
                    GameData.HP -= this.attack * 100;
                    //更新血量
                    uiManager.UpdateBattleLevelData();
                }
                if (GameData.HP <= 0)
                {
                    uiManager.ShowGameResult(false);
                    enemyManager.Stop();
                }
                this.gameObject.SetActive(false);
                return;
            }
            nextPoint = pointList[pointIndex];
        }
    }

    internal void Stop()
    {
        moving = false;
        this._rigidbody.Sleep();
        animator.Play("idle01");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (hp <= 0)
            {
                return;
            }
            hp -= 50;
            if (hp <= 0)
            {
                moving = false;
                animator.Play("death_1");
                if (enemyManager != null)
                {
                    enemyManager.enemys.Remove(this.gameObject);
                }
                this.GetComponent<Rigidbody>().isKinematic = true;
                this.transform.GetComponent<Collider>().enabled = false;

                Debug.Log($"...");
                GameData.killCount += 1;
                //uiManager.UpdateBattleData();

                //增加金币
                GameData.coins += this.gold;
                uiManager.UpdateBattleLevelData();
                if (GameData.killCount == GameData.allEnemyCount)
                {
                    uiManager.ShowGameResult(true);
                }
                //var coinManager = GameObject.Find("CoinManager").GetComponent.
                //var coin0bject = coinManager.Create():
                //coinObject.transform.position = this.transform. position + new
                // coinObject.gameObject.SetActive(true);
                //Game0bject.Destroy(coinObject.game0bject, 1f);

                GameObject.Destroy(this.gameObject, 2f);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Effect"))
        {
            if (initialMoveSpeed == moveSpeed)
            {
                moveSpeed /= 2;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Effect"))
        {
            moveSpeed = initialMoveSpeed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
