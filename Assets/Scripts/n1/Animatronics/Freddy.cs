using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Freddy : MonoBehaviour
{
    public GameObject[] pos;
    private int index;

    [Range(0, 20)]
    public int AI;
    private float _minDelay, _maxDelay, _chance;

    public GameObject Camera;

    public GameObject trans;
    public Animator run, jumpscare;

    private bool isAttacking;
    private bool returning;

    public GameObject tab;
    public Cameras cameras;

    void Awake()
    {
        foreach (var c in pos) c.SetActive(false);
        if (AI == 0) return;

        float t = (AI - 1f) / 19f;
        float s = Mathf.SmoothStep(15f, 1f, t);
        _minDelay = Mathf.Lerp(15f, 1f, s);
        _maxDelay = Mathf.Lerp(25f, 7f, s);
        _chance = Mathf.RoundToInt(Mathf.Lerp(30f, 100f, s));

        index = pos.Length - 1;
        pos[index].SetActive(true);

        InvokeRepeating("Walk", Random.Range(_minDelay, _maxDelay), Random.Range(_minDelay, _maxDelay));
    }

    IEnumerator TransEffect()
    {
        trans.SetActive(true);
        yield return new WaitForSeconds(1f);
        trans.SetActive(false);
    }

    void Update()
    {
        if (Camera.activeSelf && !returning && index != 0)
        {
            returning = true;
            StartCoroutine(ReturnToStart());
        }
    }

    void Walk()
    {
        if (Random.Range(0, 101) <= _chance && index != 0 && !returning)
        {
            StartCoroutine(TransEffect());
            pos[index].SetActive(false);
            index--;
            pos[index].SetActive(true);
        }

        if (index == 0 && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }

    IEnumerator ReturnToStart()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(TransEffect());
        pos[index].SetActive(false);
        index = pos.Length - 1;
        pos[index].SetActive(true);
        returning = false;
    }

    IEnumerator Attack()
    {
        float delay = Random.Range(20f, 60f);
        yield return new WaitForSeconds(delay);

        run.SetTrigger("Run");
        yield return new WaitForSeconds(2f);

        cameras.Close();
        Destroy(tab);

        jumpscare.SetTrigger("Scream");
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Dead");
    }
}