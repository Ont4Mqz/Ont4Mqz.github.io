using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaniWaniManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private TimerManager timerManager;

    [Header("ワニ")]
    [SerializeField] private List<Transform> waniList = new();

    [Header("移動距離")]
    [SerializeField] private float moveDistance = 1.0f;

    [Header("レベル別 MoveSpeed")]
    [SerializeField] private List<float> moveSpeedLevels = new()
    {
        3.0f,
        6.0f,
        9.5f,
        12.0f,
        15.0f
    };

    [Header("レベル別 StayTime")]
    [SerializeField] private List<float> stayTimeLevels = new()
    {
        2.0f,
        1.5f,
        1.0f,
        0.5f,
        0.2f
    };

    [Header("レベル別 出現頻度（秒）")]
    [SerializeField] private List<float> spawnIntervalLevels = new()
    {
        2.0f,
        1.5f,
        1.0f,
        0.7f,
        0.4f
    };

    [Header("その他")]
    [SerializeField] private float fastReturnSpeed = 20f;

    [Header("ヒット演出")]
    [SerializeField] private Material hitMaterial;
    [SerializeField] private float hitMaterialTime = 0.1f;

    private Dictionary<Transform, Vector3> defaultPositions = new();
    private Dictionary<Transform, Coroutine> moveCoroutines = new();
    private Dictionary<Transform, Material> defaultMaterials = new();

    private Coroutine spawnLoop;

    void Start()
    {
        foreach (Transform wani in waniList)
        {
            defaultPositions[wani] = wani.position;

            Renderer r = wani.GetComponent<Renderer>();
            if (r != null)
                defaultMaterials[wani] = r.material;

            wani.tag = "Untagged";

            Collider col = wani.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;
        }
    }

    void Update()
    {
        if (timerManager.IsGameStarted && spawnLoop == null)
        {
            spawnLoop = StartCoroutine(WaniSpawnLoop());
        }
    }

    IEnumerator WaniSpawnLoop()
    {
        while (true)
        {
            Transform target = GetRandomIdleWani();

            if (target != null)
            {
                Coroutine c = StartCoroutine(MoveWani(target));
                moveCoroutines[target] = c;
            }

            yield return new WaitForSeconds(GetCurrentSpawnInterval());
        }
    }

    Transform GetRandomIdleWani()
    {
        List<Transform> idle = new();

        foreach (Transform w in waniList)
        {
            if (!moveCoroutines.ContainsKey(w))
                idle.Add(w);
        }

        if (idle.Count == 0) return null;

        return idle[Random.Range(0, idle.Count)];
    }

    IEnumerator MoveWani(Transform wani)
    {
        float moveSpeed = GetCurrentMoveSpeed();
        float stayTime = GetCurrentStayTime();

        Vector3 startPos = defaultPositions[wani];
        Vector3 frontPos = startPos + (-wani.forward * moveDistance);

        // 出てくる
        yield return StartCoroutine(Move(wani, startPos, frontPos, moveSpeed));

        // 叩ける状態
        wani.tag = "MovingWani";
        Collider col = wani.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        // ステイ
        yield return new WaitForSeconds(stayTime);

        // 引っ込む
        DisableWani(wani);
        yield return StartCoroutine(Move(wani, frontPos, startPos, moveSpeed));

        moveCoroutines.Remove(wani);
    }

    IEnumerator Move(Transform wani, Vector3 from, Vector3 to, float speed)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            wani.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        wani.position = to;
    }

    void DisableWani(Transform wani)
    {
        wani.tag = "Untagged";
        Collider col = wani.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    public void HitWani(Transform wani)
    {
        if (!moveCoroutines.ContainsKey(wani)) return;

        StopCoroutine(moveCoroutines[wani]);
        DisableWani(wani);

        StartCoroutine(HitAndFastReturn(wani));
        moveCoroutines.Remove(wani);
    }

    IEnumerator HitAndFastReturn(Transform wani)
    {
        Renderer r = wani.GetComponent<Renderer>();

        if (r != null && hitMaterial != null)
        {
            r.material = hitMaterial;
            yield return new WaitForSeconds(hitMaterialTime);
            r.material = defaultMaterials[wani];
        }

        yield return StartCoroutine(
            Move(wani, wani.position, defaultPositions[wani], fastReturnSpeed)
        );
    }

    float GetCurrentMoveSpeed()
    {
        int i = Mathf.Clamp(timerManager.SpeedLevel - 1, 0, moveSpeedLevels.Count - 1);
        return moveSpeedLevels[i];
    }

    float GetCurrentStayTime()
    {
        int i = Mathf.Clamp(timerManager.SpeedLevel - 1, 0, stayTimeLevels.Count - 1);
        return stayTimeLevels[i];
    }

    float GetCurrentSpawnInterval()
    {
        int i = Mathf.Clamp(timerManager.SpeedLevel - 1, 0, spawnIntervalLevels.Count - 1);
        return spawnIntervalLevels[i];
    }
}
