using System;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class BytesReceiver : MonoBehaviour
{
    private readonly int LOCA_LPORT = 12345;
    private readonly int maxDGRAM;
    private readonly int maxImgDGRAM;
    static UdpClient udp;
    Thread thread;
    
    public TextureLoaderFromBytes textureLoader;

    private bool completePacket;
    private bool isWaitingPacket = true;
    
    List<byte> byteList;

    public BytesReceiver() {
        maxDGRAM = (int)Mathf.Pow(2, 16);
        maxImgDGRAM = maxDGRAM - 64;
    }

    void Start ()
    {
        Debug.Log(maxDGRAM);
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

        
    }

    public void SetByteToTexture() {
        byte[] bytes = byteList.ToArray();
        textureLoader.SetTexture(bytes);

        byteList.Clear();
    }

    private void AddBytes(byte[] bytes) {
        foreach (var item in bytes)
        {
            byteList.Add(item);
        }
    }

    private byte[] RemoveHeader(byte[] bytes) {
        var list = new List<byte>(bytes);
        list.RemoveAt(0);
        return list.ToArray();
    }

    private void ThreadMethod()
    {
        while(true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            Debug.Log("Received Packet Size: " + data.Length);
            byte[] body = RemoveHeader(data);

            if (data.Length == maxImgDGRAM+1) {
                AddBytes(body);   
                isWaitingPacket = true;
            } else {
                if (isWaitingPacket) {
                    Debug.Log("end");
                    AddBytes(body);
                    isWaitingPacket = false;
                    completePacket = true;
                }
            }
            
        }
    } 

    
}
