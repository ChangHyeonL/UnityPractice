using UnityEngine;
using System.Net.Sockets;
using System.Text;
using TMPro;

public class TCPClient : MonoBehaviour
{
    public string serverIP = "127.0.0.1";
    public int port = 8080;
    public bool isRunning = false;
    public TextMeshProUGUI startStopButtonText;

    public void SendCommand(string cmd)
    {
        try
        {
            TcpClient client = new TcpClient(serverIP, port);
            NetworkStream stream = client.GetStream();

            byte[] data = Encoding.UTF8.GetBytes(cmd);
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Debug.Log("서버 응답: " + response);

            stream.Close();
            client.Close();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("TCP 오류: " + ex.Message);
        }
    }

    public void OnClickStartStop()
    {
        if (isRunning)
        {
            SendCommand("stop");
            isRunning = false;
        }
        else
        {
            SendCommand("start");
            isRunning = true;
        }
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (startStopButtonText != null)
            startStopButtonText.text = isRunning ? "Stop" : "Start";
    }
   
    public void OnClickSpeed()
    {
        SendCommand("speed:1.5");
    }
}

