using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class BytesReceiver : MonoBehaviour
{
    private int LOCA_LPORT = 12345;
    static UdpClient udp;
    Thread thread;
    
    public TextureLoaderFromBytes textureLoader;
    private bool completePacket;
    
    List<byte> byteList;
    
    void Start ()
    {

        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start(); 

        byteList = new List<byte> {};
        
    }

    void OnApplicationQuit()
    {
        thread.Abort();
    }

    void Update() {
        if (completePacket) {
            SetByteToTexture();  
            completePacket = false;
        } 

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetByteToTexture();
        }
    }

    public void SetByteToTexture() {
        byte[] bytes = byteList.ToArray();
        textureLoader.SetTexture(bytes);

        byteList.Clear();
    }

    private void ThreadMethod()
    {
        while(true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);

            string text = Encoding.UTF8.GetString(data);
            
            if (text == "__end__") {
                Debug.Log("end");
                completePacket = true;
            } else {
                Debug.Log("receive");
                foreach (var item in data)
                {
                    byteList.Add(item);
                }

            }
        }
    } 
}
