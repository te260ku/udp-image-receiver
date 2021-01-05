using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SimpleUDPReceiver : MonoBehaviour
{
    static UdpClient udp;
    IPEndPoint remoteEP = null;
    private int i;
    
    void Start () {
        int LOCA_LPORT = 12345;

        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 2000;
    }

    
    void Update () {
        IPEndPoint remoteEP = null;
        byte[] data = udp.Receive(ref remoteEP);
        string text = Encoding.UTF8.GetString(data);
        Debug.Log(text);
    }
}
