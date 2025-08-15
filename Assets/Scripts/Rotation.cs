using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float XRot;
    public float sensitivity;
    public float min, max;

    public GameObject FL, FF, FR;

    void Update()
    {
        XRot += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        XRot = Mathf.Clamp(XRot, min, max);

        var angles = transform.eulerAngles;
        angles.y = XRot;
        transform.eulerAngles = angles;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (XRot < 45f) OnFlashlight(FL);
            else if (45f < XRot && XRot < 135f) OnFlashlight(FF);
            else if (135f < XRot) OnFlashlight(FR);
        }
        else OnFlashlight(null);
    }
    void OnFlashlight(GameObject flashlight)
    {
        FL.SetActive(false);
        FF.SetActive(false);
        FR.SetActive(false);

        if(flashlight != null) flashlight.SetActive(true);
    }
}
