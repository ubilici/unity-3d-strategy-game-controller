using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private int levelArea = 10;
    private int scrollArea = 25;
    private int scrollSpeed = 25;
    private int dragSpeed = 25;
    private int zoomSpeed = 25;
    private int zoomMin = 15;
    private int zoomMax = 40;
    private int panSpeed = 50;
    private int panAngleMin = 75;
    private int panAngleMax = 90;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 translation = Vector3.zero;

        float zoomDelta = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        if (zoomDelta != 0)
        {
            translation -= Vector3.up * zoomSpeed * zoomDelta;
        }

        float pan = mainCamera.transform.eulerAngles.x - zoomDelta * panSpeed;
        pan = Mathf.Clamp(pan, panAngleMin, panAngleMax);
        if (zoomDelta < 0 || GetComponent<Camera>().transform.position.y < (zoomMax / 2))
        {
            mainCamera.transform.eulerAngles = new Vector3(pan, 0, 0);
        }

        //translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetMouseButton(2))
        {
            translation -= new Vector3(Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime, 0, Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.mousePosition.x < scrollArea)
            {
                translation += Vector3.right * -scrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.x >= Screen.width - scrollArea)
            {
                translation += Vector3.right * scrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y < scrollArea)
            {
                translation += Vector3.forward * -scrollSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y > Screen.height - scrollArea)
            {
                translation += Vector3.forward * scrollSpeed * Time.deltaTime;
            }
        }

        var desiredPosition = mainCamera.transform.position + translation;

        if (desiredPosition.x < -levelArea || levelArea < desiredPosition.x)
        {
            translation.x = 0;
        }
        if (desiredPosition.y < zoomMin || zoomMax < desiredPosition.y)
        {
            translation.y = 0;
        }
        if (desiredPosition.z < -levelArea || levelArea < desiredPosition.z)
        {
            translation.z = 0;
        }

        mainCamera.transform.position += translation;
    }
}
