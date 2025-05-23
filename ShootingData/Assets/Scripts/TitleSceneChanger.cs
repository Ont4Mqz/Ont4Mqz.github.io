using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour
{
    private void Start()
    {
        //DelayMethodを2秒後に呼び出す
        Invoke(nameof(DelayMethod), 2f);
    }

    void DelayMethod()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnDestroy()
    {
        // Destroy時に登録したInvokeをすべてキャンセル
        CancelInvoke();
    }
}