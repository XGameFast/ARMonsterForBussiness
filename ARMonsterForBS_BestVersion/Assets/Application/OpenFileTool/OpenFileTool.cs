using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Reflection;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;
using SFB;
public class OpenFileTool : MonoBehaviour {

    public Image s;
    public Button button;
    private Texture2D texture;
    public Renderer tmText2d;
    public string OpenFileWin()
    {
        //GameObject.FindWithTag("moviePlayer").GetComponent<VideoPlayer>().Pause(); OpenFileName ofn = new OpenFileName(); ofn.structSize = Marshal.SizeOf(ofn); ofn.filter = "All Files\0*.*\0\0"; ofn.file = new string(new char[256]); ofn.maxFile = ofn.file.Length; ofn.fileTitle = new string(new char[64]); ofn.maxFileTitle = ofn.fileTitle.Length; string path = Application.streamingAssetsPath; path = path.Replace('/', '\\'); //默认路径  ofn.initialDir = path; //ofn.initialDir = "D:\\MyProject\\UnityOpenCV\\Assets\\StreamingAssets";  ofn.title = "Open Project"; ofn.defExt = "JPG";//显示文件的类型  //注意 一下项目不一定要全选 但是0x00000008项不要缺少  ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR  if (WindowDll.GetOpenFileName(ofn)) { Debug.Log("Selected file with full path: {0}" + ofn.file); } //此处更改了大部分答案的协程方法，在这里是采用unity的videoplayer.url方法播放视频； // /*而且我认为大部分的其他答案，所给的代码并不全，所以，想要其他功能的人，可以仿照下面的代码，直接在此类中写功能。
        string[] s = StandaloneFileBrowser.OpenFilePanel("anda" , "Desktop" , "png" , false );
        Debug.Log("string:" + s[0]);
        return s[0];
    }


    private  byte[] getImageByte()
    {
        string imagePath = OpenFileWin();
        //读取到文件
        FileStream files = new FileStream(imagePath, FileMode.Open);
        //新建比特流对象
        byte[] imgByte = new byte[files.Length];
        //将文件写入对应比特流对象
        files.Read(imgByte, 0, imgByte.Length);
        //关闭文件
        files.Close();
        //返回比特流的值
        return imgByte;
    }
  

    private IEnumerator LoadAssetOutSide()
    {
        string url = OpenFileWin();

//        FileInfo fileInfo = new FileInfo(url);
        Debug.Log("fullName" + url);
        string path =  url;
        WWW www = new WWW(path);
        Debug.Log("path//" + path);
        yield return www;

        texture = new Texture2D(www.texture.height,www.texture.height);

        texture.SetPixels(www.texture.GetPixels());
        texture.Apply(true);
        texture.filterMode = FilterMode.Trilinear;

        tmText2d.material.mainTexture = texture;

        s.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                                 new Vector2(0.5f,0.5f));

    }

    public void TestButton()
    {
        StartCoroutine(LoadAssetOutSide());
        return;
        Texture2D tmp2d = new Texture2D(1024,1024);
        byte[] text = getImageByte();
        tmp2d.LoadImage(text);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, tmp2d.width, tmp2d.height),
                                      Vector2.zero);
        s.sprite = sprite;
    }
}
public class WindowDll {[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)] public static extern bool GetOpenFileName([In, Out] OpenFileName ofn); public static bool GetOpenFileName1([In, Out] OpenFileName ofn) { return GetOpenFileName(ofn); } }

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)] public class OpenFileName { public int structSize = 0; public IntPtr dlgOwner = IntPtr.Zero; public IntPtr instance = IntPtr.Zero; public String filter = null; public String customFilter = null; public int maxCustFilter = 0; public int filterIndex = 0; public String file = null; public int maxFile = 0; public String fileTitle = null; public int maxFileTitle = 0; public String initialDir = null; public String title = null; public int flags = 0; public short fileOffset = 0; public short fileExtension = 0; public String defExt = null; public IntPtr custData = IntPtr.Zero; public IntPtr hook = IntPtr.Zero; public String templateName = null; public IntPtr reservedPtr = IntPtr.Zero; public int reservedInt = 0; public int flagsEx = 0; }