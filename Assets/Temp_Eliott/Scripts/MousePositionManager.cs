using UnityEngine;
using UnityEngine.InputSystem;

public class MousePositionManager : MonoBehaviour
{
    public static MousePositionManager Instance { get; private set;}
    public Vector3 MouseWorldPos { get; private set; }
    public bool HasMouseWorldPos { get;  private set; }
    public bool useObjectYForPlane = true;
    public float planeY = 0f;
    public Transform planeObject;

    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null)
        {
            HasMouseWorldPos = false;
            return;
        }
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        float y = useObjectYForPlane && planeObject != null ? planeObject.position.y : planeY;

        Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));
        Ray ray = cam.ScreenPointToRay(mouseScreenPos);
        float enter;
        if (plane.Raycast(ray, out enter))
        {
            MouseWorldPos = ray.GetPoint(enter);
            HasMouseWorldPos = true;
        }
        else
        {
            HasMouseWorldPos = false;
        }
    }
}
