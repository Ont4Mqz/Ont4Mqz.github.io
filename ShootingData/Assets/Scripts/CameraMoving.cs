using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public float autoScrollSpeed = 200f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 自動で右方向へ進む
        Vector3 movement = new Vector3(1, 0).normalized;

        // 位置更新
        transform.position += new Vector3(autoScrollSpeed, 0) * Time.deltaTime;
    }
}
