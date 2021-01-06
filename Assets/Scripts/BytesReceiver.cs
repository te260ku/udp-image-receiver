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
    string buf;
    string texts = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAIBAQEBAQIBAQECAgICAgQDAgICAgUEBAMEBgUGBgYFBgYGBwkIBgcJBwYGCAsICQoKCgoKBggLDAsKDAkKCgr/2wBDAQICAgICAgUDAwUKBwYHCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgr/wAARCAAKAAoDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dgd3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8/T19vf4+fr/xAAfAQADAQEBAQEBAQEBAAAAAAAAAQIDBAUGBwgJCgv/xAC1EQACAQIEBAMEBwUEBAABAncAAQIDEQQFITEGEkFRB2FxEyIygQgUQpGhscEJIzNS8BVictEKFiQ04SXxFxgZGiYnKCkqNTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8/T19vf4+fr/2gAMAwEAAhEDEQA/AP5/6KKKAP/Z";
    // string texts;
    byte[] b;
    int i;
    
    public TextureLoaderFromBytes textureLoader;
    private bool init;
    

    void Start ()
    {

        


        b = new byte[1024*60];
        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start(); 



        
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
            Debug.Log("fin:   " + texts);
            init = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            
            byte[] bytes = System.Convert.FromBase64String(texts);
            Debug.Log(texts);
            // Debug.Log(bytes);
            
            textureLoader.Set(bytes);
            
        }
    }

    private void ThreadMethod()
    {
        while(true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            b = data;
           
            string text = Encoding.UTF8.GetString(data);
            
            if (text == "__end__") {
                Debug.Log("fin:   " + texts);
                
                init = true;
            } else {
                
                string s = Convert.ToBase64String(data);
                
                
                Debug.Log(s);
            
                texts += s;
                
                
                
            }
            
            
            
            // Debug.Log(data);
            // Debug.Log(data[0]);
            
        }
    } 
}
