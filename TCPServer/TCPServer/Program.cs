using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main()
    {
        int port = 8080;
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Console.WriteLine("서버 시작! 포트: " + port);

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

            Console.WriteLine("수신: " + message);

            // 예시: 받은 메시지에 따라 응답
            string response = "OK";
            if (message == "hello") response = "안녕 Unity!";
            if (message == "stop") response = "정지 명령 수신!";
            if (message.StartsWith("speed: ")) response = "속도 변경: " + message.Substring(6);

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }
}