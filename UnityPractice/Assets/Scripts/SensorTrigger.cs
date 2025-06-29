using UnityEngine;

public class SensorTrigger : MonoBehaviour
{
    public UIController uiController;
    public string targetTag = "Product"; // 감지할 태그
    public ConveyorController conveyorController; // Inspector에서 할당

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("제품이 센서에 감지됨!");
            if (uiController != null)
                uiController.UpdateStatus("Status: Detected!");
            
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = Color.red; // 원하는 색상으로 변경

        }
    }
}
