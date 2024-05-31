using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    Camera _camera;
    public Vector3 startPosition = new Vector3(39.9f, 9.2f, 139.6f);
    public float speed = 3;//设置速度
    bool isGameStart = false;//是否开始游戏
    // Start is called before the first frame update
    void Start()
    {
        _camera = transform.GetComponent<Camera>();
        transform.position = startPosition;
        _camera.fieldOfView = 60;

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStart == false)
        {
            return;
        }
        //移动
        Move();
        //缩放
        ScaleView();
    }
    public void GameStart()
    {
        isGameStart = true;

    }
    public void GameStop()
    {
        isGameStart = false;

    }

    private void Move()
    {
        if (Input.GetMouseButton(0))
        {
            var x = Input.GetAxis("Mouse X");
            var y = Input.GetAxis("Mouse Y");
            if (x != 0 || y != 0)
            {
                Vector3 target =
                    this.transform.position + new Vector3(x, 0, y) * speed;
                this.transform.position =
                    Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
            }
        }
    }

    private void ScaleView()
    {
        float mouseScrollWheel =
            Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollWheel != 0)
        {
            _camera.fieldOfView += mouseScrollWheel * speed;
            if (_camera.fieldOfView <= 38)
            {
                _camera.fieldOfView = 38;
            }
            else if (_camera.fieldOfView >= 95)
            {
                _camera.fieldOfView = 95;
            }

        }
    }
}
