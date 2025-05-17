using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour
{
    private void Start()
    {
        //DelayMethod‚ğ2•bŒã‚ÉŒÄ‚Ño‚·
        Invoke(nameof(DelayMethod), 2f);
    }

    void DelayMethod()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void OnDestroy()
    {
        // Destroy‚É“o˜^‚µ‚½Invoke‚ğ‚·‚×‚ÄƒLƒƒƒ“ƒZƒ‹
        CancelInvoke();
    }
}