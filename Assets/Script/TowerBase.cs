using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public Transform towerPosition;//塔创建后放在哪个位置
    public GameObject tower;//创建的塔

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