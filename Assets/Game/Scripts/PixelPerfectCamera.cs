using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PixelPerfectCamera : MonoBehaviour
{
    [SerializeField]
    private int pixelsPerUnit = 1;

    private new Camera camera;

    [ReadonlyField, SerializeField]
    private float lastVerticalResolution;

    private	void OnEnable ()
    {
        camera = GetComponent<Camera>();
        SetOrthographicSize();
    }
	
	// Update is called once per frame
	private void Update ()
    {
        if (lastVerticalResolution == Screen.height) return;

        SetOrthographicSize();
    }

    private void SetOrthographicSize()
    {
        lastVerticalResolution = Screen.height;
        camera.orthographicSize = lastVerticalResolution / (2 * pixelsPerUnit);
    }
}
