using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bonnie : MonoBehaviour
{
    [Range(0, 20)]
    public int AI;
    private float _minDelay, _maxDelay, _chance;

    public GameObject[] pos;
    private int index;

    public GameObject[] flashlight;

    public GameObject trans;
    public Animator run, jumpscare;

    private bool isCheck;
    private bool isAttacking;

    public GameObject tab;
    public Cameras cameras;
    public GameObject UI;

    private float timeToRun;

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
        trans.SetActive(true);
        yield return new WaitForSeconds(1f);
        trans.SetActive(false);
    }

    void Update()
    {
        if (isCheck && !isAttacking)
        {
            timeToRun += Time.deltaTime;

            if (timeToRun >= 5f)
            {
                foreach (var f in flashlight)
                {
                    if (f.activeSelf)
                    {
                        isAttacking = true;
                        StartCoroutine(Attack());
                        break;
                    }
                }
                run.SetTrigger("Out");
                Invoke("GoBack", 2f);
            }
        }
    }

    void Walk()
    {
        if (Random.Range(0, 101) <= _chance && index != 0)
        {
            StartCoroutine(TransEffect());
            pos[index].SetActive(false);
            index--;
            pos[index].SetActive(true);
        }

        if (index == 0 && !isCheck)
        {
            isCheck = true;
            timeToRun = 0f;
        }
    }

    IEnumerator Attack()
    {
        run.SetTrigger("Run");
        yield return new WaitForSeconds(2f);

        cameras.Close();
        Destroy(tab);
        Destroy(UI);

        jumpscare.SetTrigger("Scream");
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Dead");
    }
    void GoBack()
    {
        pos[index].SetActive(false);
        index = pos.Length - 1;
        pos[index].SetActive(true);
    }
}