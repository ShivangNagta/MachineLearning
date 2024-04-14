using UnityEngine;

public class DrawingController : MonoBehaviour
{
    public int drawResolution = 1024;
    public int outputResolution = 28;

    public BoxCollider2D canvasCollider;
    public ComputeShader drawCompute;
    public float brushRadius;
    [Range(0, 1)]
    public float smoothing;
    public Material canvasMaterial;

    RenderTexture canvas;
    RenderTexture outputCanvas;

    Camera cam;
    Vector2Int brushCentreOld;

    void Start()
    {
        cam = Camera.main;
        CreateRenderTexture(ref canvas, drawResolution, drawResolution, FilterMode.Bilinear, "Draw Canvas");
        canvasMaterial.mainTexture = canvas;
    }

    void CreateRenderTexture(ref RenderTexture texture, int width, int height, FilterMode filterMode, string name)
    {
        texture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        texture.filterMode = filterMode;
        texture.name = name;
        texture.enableRandomWrite = true;
        texture.Create();
    }

    public RenderTexture RenderOutputTexture()
    {
        CreateRenderTexture(ref outputCanvas, outputResolution, outputResolution, FilterMode.Point, "Draw Output");

        Graphics.Blit(canvas, outputCanvas);

        return outputCanvas;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Clear();
        }

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Bounds canvasBounds = canvasCollider.bounds;

        float tx = Mathf.InverseLerp(canvasBounds.min.x, canvasBounds.max.x, mouseWorld.x);
        float ty = Mathf.InverseLerp(canvasBounds.min.y, canvasBounds.max.y, mouseWorld.y);

        Vector2Int brushCentre = new Vector2Int((int)(tx * drawResolution), (int)(ty * drawResolution));

        drawCompute.SetInts("brushCentre", brushCentre.x, brushCentre.y);
        drawCompute.SetInts("brushCentreOld", brushCentreOld.x, brushCentreOld.y);
        drawCompute.SetFloat("brushRadius", brushRadius);
        drawCompute.SetFloat("smoothing", smoothing);
        drawCompute.SetInt("resolution", drawResolution);
        drawCompute.SetInt("mode", (Input.GetMouseButton(0)) ? 0 : 1);

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            int kernelHandle = drawCompute.FindKernel("CSMain");
            drawCompute.SetTexture(kernelHandle, "Canvas", canvas);
            drawCompute.Dispatch(kernelHandle, drawResolution / 8, drawResolution / 8, 1);
        }

        brushCentreOld = brushCentre;
    }

    void Clear()
    {
        RenderTexture.active = canvas;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
    }
}
