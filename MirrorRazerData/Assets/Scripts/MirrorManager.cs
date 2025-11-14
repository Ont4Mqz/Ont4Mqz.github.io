using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MirrorManager : MonoBehaviour
{
    [Header("レーザー設定")]
    public Transform laserStartPos;
    public float laserDistance = 100f;
    public int maxReflections = 10;

    [Header("レイヤー設定")]
    public LayerMask reflectMask;

    [Header("ターゲット判定")]
    public Transform targetObject;   // 当てたいオブジェクト
    public GameObject targetPanel;   // 当たったらONにするパネル

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        if (targetPanel != null) targetPanel.SetActive(false); // 最初はOFF
    }

    void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        Vector3 startPos = laserStartPos.position;
        Vector3 direction = laserStartPos.forward;

        line.positionCount = 1;
        line.SetPosition(0, startPos);

        int count = 1;
        float remainingDistance = laserDistance;

        for (int i = 0; i < maxReflections; i++)
        {
            if (Physics.Raycast(startPos, direction, out RaycastHit hit, remainingDistance, reflectMask))
            {
                count++;
                line.positionCount = count;
                line.SetPosition(count - 1, hit.point);

                // ★ターゲットに当たったらパネルON
                if (hit.transform == targetObject)
                {
                    if (targetPanel != null)
                        targetPanel.SetActive(true);
                }

                // 反射処理
                direction = Vector3.Reflect(direction, hit.normal);
                remainingDistance -= hit.distance;
                startPos = hit.point;
            }
            else
            {
                // これ以上反射しない
                count++;
                line.positionCount = count;
                line.SetPosition(count - 1, startPos + direction * remainingDistance);
                break;
            }
        }
    }
}
