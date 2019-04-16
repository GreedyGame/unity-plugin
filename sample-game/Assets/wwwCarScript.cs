using UnityEngine;
using System.Collections;

public class wwwCarScript : MonoBehaviour
{
    public string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
    IEnumerator Start()
    {
        using (WWW www = new WWW(url))
        {
            yield return www;
            Renderer renderer = GetComponent<Renderer>();
            Debug.Log("GGNIXAC BEFORE APPLYING TEXTURE ");
            renderer.material.mainTexture = www.texture;
            Debug.Log("GGNIXAC AFTER APPLYING TEXTURE ");
        }

    }
}