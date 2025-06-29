using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    public Transform product; // 이동시킬 제품
    public float speed = 1.0f; // 기본 속도 (0.5에서 1.0으로 변경)
    public bool isRunning = false; // 동작 상태
    public GameObject gameObject;

    private void Update()
    {
        if (isRunning && product != null)
        {
            product.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ToggleRunning()
    {
        isRunning = !isRunning;
    }

  
}
