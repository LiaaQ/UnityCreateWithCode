using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;
    public float rotationSpeed = 10.0f;

    [Header("Scale Settings")]
    public float baseScale = 1.3f;
    public float scaleVariation = 0.3f;
    public float scaleSpeed = 2f;

    [Header("Position Settings")]
    public Vector3 position = new Vector3(3, 4, 1);

    [Header("Color Settings")]
    public float hueSpeed = 0.1f;    // how fast hue cycles
    public float saturation = 0.9f;  // 0..1
    public float value = 0.9f;       // 0..1
    public float alpha = 0.8f;

    private Material material;
    private float hueOffset; // per-instance phase

    void Start()
    {
        if (Renderer == null) Renderer = GetComponent<MeshRenderer>();

        transform.position = position;
        material = Renderer.material;          // instance material
        hueOffset = Random.value;              // 0..1
    }

    void Update()
    {
        // rotate
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // scale “breathing”
        float scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * scaleVariation;
        transform.localScale = Vector3.one * (baseScale + scaleOffset);

        // smooth color over time via hue
        float h = (hueOffset + Time.time * hueSpeed) % 1f;
        Color c = Color.HSVToRGB(h, saturation, value);
        c.a = alpha;
        material.color = c;
    }
}
