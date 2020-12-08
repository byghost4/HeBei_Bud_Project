using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.XR;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Newtonsoft.Json;
using HighlightingSystem;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditor;

public static class Utils
{

    /// @ https://www.cnblogs.com/huangtailang/p/4435765.html
    /// <summary>
    /// AES加密 
    /// </summary>
    /// <param name="text">加密字符</param>
    /// <param name="key">加密的密码 16倍数</param>
    /// <param name="iv">密钥 16倍数</param>
    /// <returns></returns>
    public static string AESEncrypt(string text, string key, string iv)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length) len = keyBytes.Length;
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        rijndaelCipher.IV = ivBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(text);
        byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);
        return Convert.ToBase64String(cipherBytes);
    }

    /// <summary>
    /// AES解密       
    /// </summary>
    /// <param name="text"></param>
    /// <param name="key">16倍数</param>
    /// <param name="iv">16倍数</param>
    /// <returns></returns>
    public static string AESDecrypt(string text, string key, string iv)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;
        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] encryptedData = Convert.FromBase64String(text);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length) len = keyBytes.Length;
        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
        rijndaelCipher.IV = ivBytes;
        ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
        byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }

    public static Dictionary<string, GameObject> FindGameobjectCache = new Dictionary<string, GameObject>();
    //public static GameObject FindGameobjectByName(string name, bool catchError = true)
    //{

    //    if (name == null) return null;
    //    if (FindGameobjectCache.TryGetValue(name, out var cacheResult)) return cacheResult;
    //    string rawName = name;

    //    string[] names = name.Split('/');
    //    int len = names.Length;
    //    name = names[len - 1];

    //    GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
    //    Transform tResult = null;
    //    foreach (var g in gos)
    //    {
    //        g.transform.TraversalTransform(m =>
    //        {
    //            if (m.GetComponent<FindGameobjectByNameIgnore>()) return true;
    //            if (m.name == name)
    //            {
    //                int l = len - 2;
    //                Transform t = m;
    //                while (l >= 0)
    //                {
    //                    t = t.parent;
    //                    if (t == null) return false;
    //                    if (names[l] != t.name) return false;
    //                    l--;
    //                }
    //                tResult = m;
    //                return true;
    //            }
    //            else
    //            {
    //                return false;
    //            }
    //        });
    //    }

    //    if (tResult != null)
    //    {
    //        FindGameobjectCache.Add(rawName, tResult.gameObject);
    //        return tResult.gameObject;
    //    }
    //    else
    //    {
    //        if (catchError) throw new Exception($"物体[{name}]没找到");
    //        return null;
    //    }
    //}
    //public static List<GameObject> GetGameobjectsByString(string s)
    //{
    //    try
    //    {
    //        string[] f = s.Split(',');
    //        List<GameObject> fs = new List<GameObject>();
    //        for (int i = 0; i < f.Length; i++)
    //        {
    //            fs.Add(FindGameobjectByName(f[i], false));
    //        }
    //        return fs;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception($"数字输入格式不正确[ {s} ]" + ex);
    //    }
    //}
    public static List<float> GetFloatsByString(string s)
    {
        try
        {
            string[] f = s.Split(',');
            List<float> fs = new List<float>();
            for (int i = 0; i < f.Length; i++)
            {
                fs.Add(float.Parse(f[i]));
            }
            return fs;
        }
        catch
        {
            throw new Exception($"数字输入格式不正确[ {s} ]");
        }
    }
    public static Color GetColorByStrings(List<float> strings)
    {
        Color c = new Color();
        c.a = 1;
        for (int i = 0; i < strings.Count && i < 4; i++)
        {
            c[i] = strings[i] / 255;
        }
        return c;
    }

    public static string JsonSerialize<T>(this T t)
    {
        return JsonConvert.SerializeObject(t, Formatting.Indented);
    }
    public static string JsonSerializeMin<T>(this T t)
    {
        return JsonConvert.SerializeObject(t);
    }
    public static T JsonDeserialize<T>(this string s)
    {
        return JsonConvert.DeserializeObject<T>(s); ;
    }
    public static T JsonDeserializeAnonymousType<T>(this string s, T definition)
    {
        return JsonConvert.DeserializeAnonymousType<T>(s, definition); ;
    }
    public static JsonSerializerSettings JsonSetting_HandleType => new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

    public static T ObjectCopyByJson<T>(T t)
    {
        string s = JsonConvert.SerializeObject(new[] { t }, JsonSetting_HandleType);
        return JsonConvert.DeserializeObject<T[]>(s, JsonSetting_HandleType)[0];
    }

    public static bool IsNullOrWhiteSpace(this string s) => string.IsNullOrWhiteSpace(s);
    public static void PlayForward(this Animation animation, string name = "")
    {
        if (name == "") name = animation.clip.name;
        animation[name].speed = 1;
        animation[name].normalizedTime = 0;
        animation.Play(name);
    }

    public static void PlayBack(this Animation animation, string name = "")
    {
        if (name == "") name = animation.clip.name;
        animation[name].speed = -1;
        animation[name].normalizedTime = 1;
        animation.Play(name);
    }
    public static byte[] Compress(byte[] bytes)
    {
        using (MemoryStream cms = new MemoryStream())
        {
            using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(cms, System.IO.Compression.CompressionMode.Compress))
            {
                gzip.Write(bytes, 0, bytes.Length);
            }
            return cms.ToArray();
        }
    }
    public static byte[] Uncompress(byte[] bytes)
    {
        using (MemoryStream dms = new MemoryStream())
        {
            using (MemoryStream cms = new MemoryStream(bytes))
            {
                using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(cms, System.IO.Compression.CompressionMode.Decompress))
                {
                    byte[] b = new byte[1024];
                    int len = 0;
                    while ((len = gzip.Read(b, 0, b.Length)) > 0)
                    {
                        dms.Write(b, 0, len);
                    }
                }
            }
            return dms.ToArray();
        }
    }

    public static void SetPosX_World(this Transform t, float value)
    {
        Vector3 v = t.position;
        v.x = value;
        t.position = v;
    }
    public static void SetPosY_World(this Transform t, float value)
    {
        Vector3 v = t.position;
        v.y = value;
        t.position = v;
    }
    public static void SetPosZ_World(this Transform t, float value)
    {
        Vector3 v = t.position;
        v.z = value;
        t.position = v;
    }
    public static void SetPosX_Local(this Transform t, float value)
    {
        Vector3 v = t.localPosition;
        v.x = value;
        t.localPosition = v;
    }
    public static void SetPosY_Local(this Transform t, float value)
    {
        Vector3 v = t.localPosition;
        v.y = value;
        t.localPosition = v;
    }
    public static void SetPosZ_Local(this Transform t, float value)
    {
        Vector3 v = t.localPosition;
        v.z = value;
        t.localPosition = v;
    }

    public static List<T> FindSceneObjects<T>()
    {
        List<T> ts = new List<T>();
        TraversalSceneObject(t =>
        {
            var c = t.GetComponent<T>();
            if (c != null) ts.Add(c);
        });
        return ts;
    }
    public static void TraversalSceneObject(Action<Transform> act)
    {
        var goes = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var g in goes)
        {
            g.transform.TraversalTransform(act);
        }
    }
    /// <summary>
    /// 遍历所有子物体,不包括自身
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="act"></param>
    public static void TraversalChildren(this Transform parent, Action<Transform> act)
    {
        foreach (Transform child in parent)
        {
            act?.Invoke(child);
            child.TraversalTransform(act);
        }
    }
    /// <summary>
    /// 遍历所有子物体,包括自身
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="act"></param>
    public static void TraversalTransform(this Transform parent, Action<Transform> act)
    {
        act?.Invoke(parent);
        foreach (Transform child in parent)
        {
            child.TraversalTransform(act);
        }
    }
    /// <summary>
    /// 遍历所有子物体,包括自身,包含终止条件
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="act"></param>
    public static bool TraversalTransform(this Transform parent, Func<Transform, bool> act)
    {
        if (act.Invoke(parent) == true) return true;
        foreach (Transform child in parent)
        {
            if (child.TraversalTransform(act) == true) return true;
        }
        return false;
    }
    /// <summary>
    /// 遍历所有子物体,包括自身
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="fuc">满足条件</param>
    /// <param name="act">执行操作</param>
    public static void TraversalTransform<T>(this Transform parent, Func<Transform, T> func, Action<T> act)
    {
        T t = func(parent);
        if (t != null && !t.Equals(default(T)))
        {
            act?.Invoke(t);
        }
        foreach (Transform child in parent)
        {
            child.TraversalTransform(func, act);
        }
    }

    public static T AddComponent<T>(this Transform t) where T : Component
    {
        return t.gameObject.AddComponent<T>();
    }

    public static string GetMD5(byte[] buffer)
    {
        try
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message + ex.StackTrace);
        }
    }

    public static void OpenImageByExternProcess(string path, bool waitForExit = false)
    {
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "cmd.exe";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        p.StandardInput.WriteLine("rundll32.exe C:\\Windows\\System32\\shimgvw.dll,ImageView_Fullscreen " + path.Replace("/", "\\"));
        p.StandardInput.WriteLine("exit");
        p.StandardInput.AutoFlush = true;
        if (waitForExit) p.WaitForExit();
        p.Close();
    }

    public static Texture2D CaptureCamera(Rect rect, params Camera[] camera)
    {
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);

        for (int i = 0; i < camera.Length; i++)
        {
            camera[i].targetTexture = rt;
            camera[i].Render();
        }

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();

        for (int i = 0; i < camera.Length; i++)
        {
            camera[i].targetTexture = null;
        }

        RenderTexture.active = null;
        UnityEngine.Object.Destroy(rt);
        return screenShot;
    }

    public static string ToUTF8String(this byte[] data)
    {
        return Encoding.UTF8.GetString(data);
    }

    public static void AddOnStart(this MonoBehaviour m, Action action)
    {
        m.gameObject.AddComponent<OnStart>().action = action;
    }
    public class OnStart : MonoBehaviour
    {
        public Action action;
        private void Start()
        {
            action?.Invoke();
            Destroy(this);
        }
    }

    public static bool IsMouseOverUI
    {
        get
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }

    /// <summary>
    /// 获取鼠标停留处UI
    /// </summary>
    /// <param name="canvas"></param>
    /// <returns></returns>
    public static RectTransform GetOverUI(Canvas canvas)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return results[0].gameObject.GetComponent<RectTransform>();
        }
        return null;
    }

    public static RaycastResult GetOverUIRayResult(Canvas canvas)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            return results[0];
        }
        return default;
    }

    public static void Highlighting(Transform t, Color color)
    {
        if (!t) return;
        if (color != Color.clear)
        {

            (t.GetComponent<HighlightingSystem.Highlighter>() ?? t.AddComponent<HighlightingSystem.Highlighter>()).ConstantOnImmediate(Color.yellow);
        }
        else
        {
            var h = t.GetComponent<HighlightingSystem.Highlighter>();
            if (h)
            {
                h.ConstantOffImmediate();
                //if (Application.isPlaying)
                //{
                //    UnityEngine.Object.Destroy(h);
                //}
                //else
                //{
                //    UnityEngine.Object.DestroyImmediate(h);
                //}
                try
                {
                    UnityEngine.Object.DestroyImmediate(h);
                }
                catch
                {
                    if (h) UnityEngine.Object.Destroy(h);
                }
            }
    }
    }

    public static List<int> RandomNoRepeat(int min, int max, int number)
    {
        List<int> result = new List<int>();
        List<int> temp = new List<int>();
        for (int i = min; i < max; i++)
        {
            temp.Add(i);
        }
        for (int i = 0; i < number; i++)
        {
            int r = UnityEngine.Random.Range(0, temp.Count);
            result.Add(temp[r]);
            temp.RemoveAt(r);
        }
        return result;
    }

    public static string NewGUID => Guid.NewGuid().ToString("N");

    public static GUILayoutOption[] GUIWidth(float width) => new GUILayoutOption[] { GUILayout.Width(width) };

    public static GUILayoutOption[] GUIHeight(float height) => new GUILayoutOption[] { GUILayout.Height(height) };

#if UNITY_EDITOR
    public static List<Type> GetAttributeTypes<T>()
    {
        System.Reflection.Assembly asm = System.Reflection.Assembly.GetAssembly(typeof(T));
        Type[] types = asm.GetExportedTypes();
        bool IsMyAttribute(Attribute[] o)
        {
            foreach (Attribute a in o)
            {
                if (a is T) return true;
            }
            return false;
        }
        return types.Where(o => IsMyAttribute(Attribute.GetCustomAttributes(o, true))).ToList();
    }

    [UnityEditor.MenuItem("Tools/清楚物体查找缓存")]
    public static void ClearFindGameobjectCache()
    {
        FindGameobjectCache.Clear();
    }

#endif

    public static void MeshToFile(MeshFilter mf, string filename, float scale)
    {
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            streamWriter.Write(Utils.MeshToString(mf, scale));
        }
    }

    public static string MeshToString(MeshFilter mf, float scale)
    {
        Mesh mesh = mf.mesh;
        Material[] sharedMaterials = mf.GetComponent<Renderer>().sharedMaterials;
        Vector2 textureOffset = mf.GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
        Vector2 textureScale = mf.GetComponent<Renderer>().material.GetTextureScale("_MainTex");
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("mtllib design.mtl").Append("\n");
        stringBuilder.Append("g ").Append(mf.name).Append("\n");
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vector = vertices[i];
            stringBuilder.Append(string.Format("v {0} {1} {2}\n", vector.x * scale, vector.z * scale, vector.y * scale));
        }
        stringBuilder.Append("\n");
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        if (mesh.subMeshCount > 1)
        {
            int[] triangles = mesh.GetTriangles(1);
            for (int j = 0; j < triangles.Length; j += 3)
            {
                if (!dictionary.ContainsKey(triangles[j]))
                {
                    dictionary.Add(triangles[j], 1);
                }
                if (!dictionary.ContainsKey(triangles[j + 1]))
                {
                    dictionary.Add(triangles[j + 1], 1);
                }
                if (!dictionary.ContainsKey(triangles[j + 2]))
                {
                    dictionary.Add(triangles[j + 2], 1);
                }
            }
        }
        for (int num = 0; num != mesh.uv.Length; num++)
        {
            Vector2 vector2 = Vector2.Scale(mesh.uv[num], textureScale) + textureOffset;
            if (dictionary.ContainsKey(num))
            {
                stringBuilder.Append(string.Format("vt {0} {1}\n", mesh.uv[num].x, mesh.uv[num].y));
            }
            else
            {
                stringBuilder.Append(string.Format("vt {0} {1}\n", vector2.x, vector2.y));
            }
        }
        for (int k = 0; k < mesh.subMeshCount; k++)
        {
            stringBuilder.Append("\n");
            if (k == 0)
            {
                stringBuilder.Append("usemtl ").Append("Material_design").Append("\n");
            }
            if (k == 1)
            {
                stringBuilder.Append("usemtl ").Append("Material_logo").Append("\n");
            }
            int[] triangles2 = mesh.GetTriangles(k);
            for (int l = 0; l < triangles2.Length; l += 3)
            {
                stringBuilder.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}\n", triangles2[l] + 1, triangles2[l + 1] + 1, triangles2[l + 2] + 1));
            }
        }
        return stringBuilder.ToString();
    }
}
