using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureLoaderFromBytes : MonoBehaviour
{
    private string rootPath;
    private float span = 3f;
    private int currentImageNum;
    private int maxImageNum = 2;
    void Start() {
        rootPath = Application.dataPath + "/Textures/";
        // StartCoroutine(ChangeTexture());

        

        byte[] readBinary = ReadFile(rootPath + "sample-image-1.jpeg");
        
        // Debug.Log(readBinary[0]);
        // var texture = CreateTexture(readBinary);
        // GetComponent<Renderer>().material.mainTexture = texture;
    }

    IEnumerator ChangeTexture(){
        while (true) {
            var texture = ReadTexture("sample-image-" + currentImageNum.ToString() + ".jpeg");
            GetComponent<Renderer>().material.mainTexture = texture;

            currentImageNum ++;
            if (currentImageNum > maxImageNum-1) currentImageNum = 0;
            
            yield return new WaitForSeconds(span);
            
        }
    }

    public void Set(byte[] bytes) {

        var texture = new Texture2D(1, 1);

        texture.LoadImage(bytes);

        // Image内に割り当てられたスクリプト内なら下記を呼ぶだけ。
        GetComponent<Renderer>().material.mainTexture = texture;
    }

    public void ChangeTexture(string data) {
        byte[] b = Encoding.UTF8.GetBytes(data);
        var texture = CreateTexture(b);
        GetComponent<Renderer>().material.mainTexture = texture;
    }

    Texture CreateTexture(byte[] data)
    {
        byte[] readBinary = data;
        
        // Debug.Log(Encoding.UTF8.GetString(readBinary));
        Debug.Log(readBinary.Length);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(readBinary);

        return texture;
    }

    Texture ReadTexture(string path)
    {
        byte[] readBinary = ReadFile(rootPath + path);
        
        // Debug.Log(Encoding.UTF8.GetString(readBinary));
        // Debug.Log(readBinary.Length);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(readBinary);

        return texture;
    }

    byte[] ReadFile(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        BinaryReader bin = new BinaryReader(fileStream);
        byte[] values = bin.ReadBytes((int)bin.BaseStream.Length);

        bin.Close();

        return values;
    }
}