using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleCarController : MonoBehaviour
{
    [Header("移動速度（前進力）")]
    public float moveForce = 1000f;

    [Header("回転速度（ステアリング力）")]
    public float turnTorque = 100f;

    [Header("ブレーキの強さ")]
    public float brakeForce = 3000f;

    [Header("慣性を残す係数（0〜1）")]
    [Range(0, 1)]
    public float inertiaPreserve = 0.95f;

    private Rigidbody rb;
    private float moveInput;
    private float turnInput;
    private bool isBraking;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0); // 安定化
    }

    void Update()
    {
        // 入力取得
        moveInput = Input.GetKey(KeyCode.W) ? 1f : 0f;

        turnInput = 0f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;
        else if (Input.GetKey(KeyCode.D)) turnInput = 1f;

        isBraking = Input.GetKey(KeyCode.LeftShift);
    }

    void FixedUpdate()
    {
        // 前進
        if (moveInput > 0)
        {
            Vector3 forwardForce = transform.forward * moveInput * moveForce * Time.fixedDeltaTime;
            rb.AddForce(forwardForce, ForceMode.Force);
        }

        // 回転（スピードに関係なく）
        Vector3 torque = Vector3.up * turnInput * turnTorque * Time.fixedDeltaTime;
        rb.AddTorque(torque, ForceMode.Force);

        // ブレーキ処理（減速）
        if (isBraking)
        {
            Vector3 brakeVelocity = rb.velocity * (1 - (brakeForce * Time.fixedDeltaTime / rb.mass));
            rb.velocity = brakeVelocity;
        }

        // 横滑りの抑制（完成の法則をやや残す）
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x *= inertiaPreserve;
        rb.velocity = transform.TransformDirection(localVelocity);
    }
}
