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
                    // ��� ����
                    byte[] data = Encoding.UTF8.GetBytes(cmd);
                    await stream.WriteAsync(data, 0, data.Length);

                    // ���� �޽��� ���� (��: �������� 3�� �� �޽��� ����)
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    Debug.Log("���� ����: " + response);

                    // ���⼭ ���� �޽����� ���� Unity���� ���ϴ� ���� ����
                    if (response.Contains("Hello"))
                    {
                        // ����: ���� �ؽ�Ʈ ����, ���� ���� ��
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("TCP �񵿱� ����: " + ex.Message);
        }
    }

    

    public void OnClickStartStop()
    {
        SendAndReceiveAsync("start");
    }

}

