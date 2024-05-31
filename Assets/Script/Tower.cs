using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attact = 1;//������
    public float attackSpeed = 1;//�����ٶ�
    public float attackRange = 10;//10�װ뾶
    public Transform target;//Ŀ�����
    public float rotateSpeed = 5;//��ת�ٶ�
    public float lastShootTime;//������ʱ��
    public float recharge = 1;//������
    public int expend = 10;//�������������Ľ��
    public List<Transform> muzzle;//ǹ��
    public Transform turret;//����
    public GameObject bullet;//�ӵ�
    EnemyManager enemyManager;//���˹�����

    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    //���㹥��Ŀ��
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

    //��ת ��׼����Ŀ��
    private void Rotate()
    {
        if (target != null)
        {
            var qua = Quaternion.FromToRotation(Vector3.forward, target.transform.position - turret.position);
            var r = Quaternion.Lerp(turret.rotation, qua, Time.deltaTime * rotateSpeed * attackSpeed);
            //���� Y�� ��ת��Ŀ��λ��
            //ǹ�� X�� ����������ת
            //turret.rotation
            this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, r.eulerAngles.y, transform.eulerAngles.z);
            turret.eulerAngles = new Vector3(r.eulerAngles.x, turret.eulerAngles.y, turret.eulerAngles.z);
        }
    }

    //�������
    private void Shooting()
    {
        if (target != null)
        {
            var a = turret.position - turret.position + turret.forward;
            var b = target.position - turret.position;

            if (Vector3.Angle(a, b) <= 10)
            {
                //������ڹ�����ȴ�� �򲻽������
                if (Time.time - lastShootTime <= recharge / attackSpeed)
                {
                    //�����ȴ��
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
        //���û������ ����Ҫ��������ص��߼�
        if (turret == null)
        {
            return;
        }
        GetAttackTarget();
        Rotate();
        Shooting();
    }
}