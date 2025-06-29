using UnityEngine;
using System.Net.Sockets;
using System.Text;
using TMPro;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class TCPClient : MonoBehaviour
{
    public string serverIP = "127.0.0.1";
    public int port = 8080;
    
    public async void SendAndReceiveAsync(string cmd)
    {
        try
        {
            using (TcpClient client = new TcpClient())
            {
                await client.ConnectAsync(serverIP, port);
                using (NetworkStream stream = client.GetStream())
                {
                    // 명령 전송
                    byte[] data = Encoding.UTF8.GetBytes(cmd);
                    await stream.WriteAsync(data, 0, data.Length);

                    // 서버 메시지 수신 (예: 서버에서 3초 후 메시지 전송)
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Debug.Log("서버 응답: " + response);

                    // 여기서 서버 메시지에 따라 Unity에서 원하는 동작 실행
                    if (response.Contains("Hello"))
                    {
                        // 예시: 상태 텍스트 변경, 색상 변경 등
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("TCP 비동기 오류: " + ex.Message);
        }
    }

    

    public void OnClickStartStop()
    {
        SendAndReceiveAsync("start");
    }

}

