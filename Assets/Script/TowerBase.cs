using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public Transform towerPosition;//������������ĸ�λ��
    public GameObject tower;//��������

    public void Awake()
    {
        towerPosition = transform.Find("pos");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}