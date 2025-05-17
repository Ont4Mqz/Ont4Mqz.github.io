using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour
{
    private void Start()
    {
        //DelayMethod��2�b��ɌĂяo��
        Invoke(nameof(DelayMethod), 2f);
    }

    void DelayMethod()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnDestroy()
    {
        // Destroy���ɓo�^����Invoke�����ׂăL�����Z��
        CancelInvoke();
    }
}