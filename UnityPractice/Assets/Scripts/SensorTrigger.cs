using UnityEngine;

public class SensorTrigger : MonoBehaviour
{
    public UIController uiController;
    public string targetTag = "Product"; // ������ �±�
    public ConveyorController conveyorController; // Inspector���� �Ҵ�

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Debug.Log("��ǰ�� ������ ������!");
            if (uiController != null)
                uiController.UpdateStatus("Status: Detected!");
            
            Renderer renderer = other.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = Color.red; // ���ϴ� �������� ����

        }
    }
}
