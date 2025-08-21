using System.Collections;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chica : MonoBehaviour
{
    [Range(0, 20)]
    public int AI;
    private float _minDelay, _maxDelay, _chance;

    public GameObject[] pos;
    private int index;

    public GameObject[] trans, cam;
    public Animator run, jumpscare;

    private bool isAttacking;
    private float timeToRun;

    public GameObject tab;
    public Cameras cameras;
    public GameObject UI;

    void Awake()
    {
        foreach (var c in pos) c.SetActive(false);

        index = pos.Length - 1;
        pos[index].SetActive(true);

        if (AI == 0) return;

        float t = (AI - 1f) / 19f;
        float s = Mathf.SmoothStep(15f, 1f, t);
        _minDelay = Mathf.Lerp(15f, 1f, s);
        _maxDelay = Mathf.Lerp(25f, 7f, s);
        _chance = Mathf.RoundToInt(Mathf.Lerp(30f, 100f, s));

        InvokeRepeating("Walk", Random.Range(_minDelay, _maxDelay), Random.Range(_minDelay, _maxDelay));
    }

    IEnumerator TransEffect()
    {
        foreach (var t in trans) t.SetActive(true);
        yield return new WaitForSeconds(1f);
        foreach (var t in trans) t.SetActive(false);
    }
    void Walk()
    {
        if (Random.Range(0, 101) <= _chance && index != 0)
        {
            StartCoroutine(TransEffect());
            pos[index].SetActive(false);
            index = Random.Range(1, pos.Length - 1);
            pos[index].SetActive(true);
            isAttacking = true;
        }
    }
    void Update()
    {
        if (isAttacking)
        {
            timeToRun += Time.deltaTime;
            if (cam[index].activeSelf)
            {
                StartCoroutine(GoBack());
            }
        }
    }
    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(2f);
        pos[index].SetActive(false);
        index = pos.Length - 1;
        pos[index].SetActive(true);
    }
}