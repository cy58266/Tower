using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attact = 1;//攻击力
    public float attackSpeed = 1;//攻击速度
    public float attackRange = 10;//10米半径
    public Transform target;//目标对象
    public float rotateSpeed = 5;//旋转速度
    public float lastShootTime;//最后射击时间
    public float recharge = 1;//射击间隔
    public int expend = 10;//创建、升级消耗金币
    public List<Transform> muzzle;//枪口
    public Transform turret;//炮身
    public GameObject bullet;//子弹
    EnemyManager enemyManager;//敌人管理器

    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    //计算攻击目标
    private void GetAttackTarget()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, this.transform.position) <= attackRange)
            {
                return;
            }
        }
        for (int i = 0; i < enemyManager.enemys.Count; i++)
        {
            var e = enemyManager.enemys[i];
            if (Vector3.Distance(e.transform.position, this.transform.position) <= attackRange)
            {
                target = e.transform;
                return;
            }
        }
    }

    //旋转 瞄准攻击目标
    private void Rotate()
    {
        if (target != null)
        {
            var qua = Quaternion.FromToRotation(Vector3.forward, target.transform.position - turret.position);
            var r = Quaternion.Lerp(turret.rotation, qua, Time.deltaTime * rotateSpeed * attackSpeed);
            //底座 Y轴 旋转到目标位置
            //枪座 X轴 控制上下旋转
            //turret.rotation
            this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, r.eulerAngles.y, transform.eulerAngles.z);
            turret.eulerAngles = new Vector3(r.eulerAngles.x, turret.eulerAngles.y, turret.eulerAngles.z);
        }
    }

    //开火射击
    private void Shooting()
    {
        if (target != null)
        {
            var a = turret.position - turret.position + turret.forward;
            var b = target.position - turret.position;

            if (Vector3.Angle(a, b) <= 10)
            {
                //如果还在攻击冷却中 则不进行射击
                if (Time.time - lastShootTime <= recharge / attackSpeed)
                {
                    //射击冷却中
                    return;
                }
                if (bullet != null)
                {
                    lastShootTime = Time.time;
                    for (int i = 0; i < muzzle.Count; i++)
                    {
                        var go = GameObject.Instantiate(bullet);
                        go.tag = "Bullet";
                        go.transform.position = muzzle[i].transform.position;
                        go.transform.localScale *= 0.1f;
                        go.transform.eulerAngles = muzzle[i].eulerAngles;
                        AudioSource audio = go.AddComponent<AudioSource>();
                        audio.clip = Resources.Load<AudioClip>("audio/attack");
                        audio.Play();
                    }
                }
            }
        }
    }


    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        //如果没有炮身 则不需要做处理相关的逻辑
        if (turret == null)
        {
            return;
        }
        GetAttackTarget();
        Rotate();
        Shooting();
    }
}