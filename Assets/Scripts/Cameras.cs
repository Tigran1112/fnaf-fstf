using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Cameras : MonoBehaviour
{
    public GameObject[] cameras;
    public int currentCameraIndex;
    public GameObject MainCamera;
    public Animator anim;

    public UIDocument doc;
    private VisualElement tabletButton, minimap, translate;
    private Button next, prev;
    private Button Text;
    void Awake()
    {
        var root = doc.GetComponent<UIDocument>().rootVisualElement;
        tabletButton = root.Q<VisualElement>("TabletButton");
        minimap = root.Q<VisualElement>("Minimap");
        next = root.Q<Button>("Next");
        prev = root.Q<Button>("Back");
        translate = root.Q<VisualElement>("Trans");
        Text = root.Q<Button>("CamNum");

        minimap.style.display = DisplayStyle.None;
        translate.style.display = DisplayStyle.None;

        tabletButton.RegisterCallback<PointerEnterEvent>(ChangeVisible);
        next.clicked += () => ChangeCamera(true);
        prev.clicked += () => ChangeCamera(false);
        Text.text = "CAM:" + currentCameraIndex + 1;
    }
    void ChangeVisible(PointerEnterEvent env)
    {
        if (MainCamera.activeSelf) StartCoroutine(Open());
        else Close();
    }
    IEnumerator Open()
    {
        anim.SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.2f);

        cameras[currentCameraIndex].SetActive(true);
        MainCamera.SetActive(false);
        minimap.style.display = DisplayStyle.Flex;
    }
    void Close()
    {
        MainCamera.SetActive(true);
        cameras[currentCameraIndex].SetActive(false);
        minimap.style.display = DisplayStyle.None;

        anim.SetBool("IsOpen", false);
    }
    void ChangeCamera(bool next)
    {
        translate.style.display = DisplayStyle.Flex;
        cameras[currentCameraIndex].SetActive(false);

        if (next) currentCameraIndex++;
        else currentCameraIndex--;

        if (currentCameraIndex >= cameras.Length) currentCameraIndex = 0;
        else if (currentCameraIndex < 0) currentCameraIndex = cameras.Length - 1;

        Text.text = "CAM:" + currentCameraIndex + 1;
        cameras[currentCameraIndex].SetActive(true);

        Invoke("OffTrans", 1f);
    }
    void OffTrans() => translate.style.display = DisplayStyle.None;
}
