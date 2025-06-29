using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Button startStopButton;
    public Button resetButton;
    public Slider speedSlider;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI sliderSpeedText;

    public ConveyorController conveyorController;

    private bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 버튼 클릭 이벤트 연결
        startStopButton.onClick.AddListener(OnStartStopButtonClicked);

        resetButton.onClick.AddListener(OnResetButtonClicked);

        // 슬라이더 값 변경 이벤트 연결
        speedSlider.onValueChanged.AddListener(OnSpeedSliderChanged);

        // 슬라이더 초기값 표시
        UpdateSliderSpeedText(speedSlider.value);
        
    }

    void OnStartStopButtonClicked()
    {
        isRunning = !isRunning;
        conveyorController.isRunning = isRunning;
        statusText.text = isRunning ? "Status: Running" : "Status: Stopped";
        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = isRunning ? "Stop" : "Start";
    }

    public void OnResetButtonClicked()
    {
        if(conveyorController != null)
        {
            conveyorController.isRunning = false;
            conveyorController.ResetConveyor();
        }
        if (statusText != null)
            statusText.text = "Status: Ready";
    }
    void OnSpeedSliderChanged(float value)
    {
        conveyorController.speed = value;
        UpdateSliderSpeedText(value);
    }

    void UpdateSliderSpeedText(float value)
    {
        if (sliderSpeedText != null)
        {
            sliderSpeedText.text = $"Speed: {value:F2}";
        }
    }

    public void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
}
