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
        // �����ŉE�����֐i��
        Vector3 movement = new Vector3(1, 0).normalized;

        // �ʒu�X�V
        transform.position += new Vector3(autoScrollSpeed, 0) * Time.deltaTime;
    }
}
