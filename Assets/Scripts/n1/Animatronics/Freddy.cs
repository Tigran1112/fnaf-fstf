using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Freddy : MonoBehaviour
{
    public GameObject[] pos;
    private int index;

    public int AI;
    private float _minDelay, _maxDelay, _chance;

    public GameObject Camera;

    public UIDocument doc;
    private VisualElement trans;

    public Animator run, jumpscare;

    private int timeToRun;
    void Awake()
    {
        if (AI == 0) return;
        float t = (AI - 1f) / 19f;
        float s = Mathf.SmoothStep(15f, 1f, t);
        _minDelay = Mathf.Lerp(15f, 1f, s);
        _maxDelay = Mathf.Lerp(25f, 7f, s);
        _chance = Mathf.RoundToInt(Mathf.Lerp(30f, 100f, s));
        index = pos.Length - 1;
        foreach (var c in pos) c.SetActive(false);
        pos[index].SetActive(true);
        trans = doc.rootVisualElement.Q<VisualElement>("Trans");

        InvokeRepeating("Walk", Random.Range(_minDelay, _maxDelay), Random.Range(_minDelay, _maxDelay));

    }
    IEnumerator Trans()
    {
        trans.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(1f);
        trans.style.display = DisplayStyle.None;
    }
    void Update()
    {
        if (Camera.activeSelf)
        {
            timeToRun += (int)Mathf.Round(Time.deltaTime);
            if (timeToRun == 3)
            {
                StartCoroutine(Trans());
                pos[index].SetActive(false);
                index = pos.Length - 1;
                pos[index].SetActive(true);
            }
        }
    }
    void Walk()
    {
        if (Random.Range(0, 101) <= _chance && index != 0)
        {
            StartCoroutine(Trans());
            pos[index].SetActive(false);
            index--;
            pos[index].SetActive(true);
        }
        if (index == 0)
        {
            Invoke("Attack", Random.Range(25, 60));
        }
    }
    IEnumerator Attack()
    {
        run.SetTrigger("Run");
        yield return new WaitForSeconds(2f);
        jumpscare.SetTrigger("Scream");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Dead");
    }
}