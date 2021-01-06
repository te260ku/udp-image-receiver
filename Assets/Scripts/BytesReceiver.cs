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

    string texts;

    
    public TextureLoaderFromBytes textureLoader;
    private bool init;
    
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
        if (init) {
            
            byte[] bytes = System.Convert.FromBase64String(texts);
            textureLoader.Set(bytes);
            
            init = false;
        } 

        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("end");
            init = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            // byte[] bytes = System.Convert.FromBase64String(texts);
            // Debug.Log(texts);   

            byte[] bytes = byteList.ToArray();
            textureLoader.Set(bytes);
            
        }
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
                
                init = true;
            } else {
                
                // string s = Convert.ToBase64String(data);
                // Debug.Log(s);
                // texts += s;

                foreach (var item in data)
                {
                    byteList.Add(item);
                }
                
            }
            
        }
    } 
}
