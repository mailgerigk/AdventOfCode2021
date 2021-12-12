using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float time = 0.1f;
    public float size = 6f;
    float elapsed = 0;

    public Color lerpFrom;
    public Color lerpTo;

    Material mat;
    Texture2D tex;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (time < elapsed)
            Destroy(gameObject);
        transform.localScale = Vector3.one * Mathf.Lerp(Vector3.kEpsilon, size, elapsed / time);
        mat.SetColor("_Color", Color.Lerp(lerpFrom, lerpTo, elapsed / time));
    }
}
