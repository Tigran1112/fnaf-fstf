using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class N1Base : MonoBehaviour
{
    public GameObject[] pos;
    protected int index;

    [Range(0, 20)]
    public int AI;
    protected float _minDelay, _maxDelay, _chance;

    public GameObject Camera;
    public GameObject trans;
    public Animator run, jumpscare;

    public GameObject tab;
    public Cameras cameras;

    public UIDocument doc;
    protected VisualElement dead;

    protected bool isAttacking;
    protected bool returning;

    protected virtual void Awake()
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

        var root = doc.rootVisualElement;
        dead = root.Q<VisualElement>("Dead");

        InvokeRepeating(nameof(Walk), Random.Range(_minDelay, _maxDelay), Random.Range(_minDelay, _maxDelay));
    }

    protected IEnumerator TransEffect()
    {
        trans.SetActive(true);
        yield return new WaitForSeconds(1f);
        trans.SetActive(false);
    }

    protected virtual void Walk()
    {
        if (Random.Range(0, 101) <= _chance && index != 0 && !returning)
        {
            StartCoroutine(TransEffect());
            pos[index].SetActive(false);
            index--;
            pos[index].SetActive(true);
        }

        TryAttack();
    }

    // Метод для запуска атаки — наследники переопределяют
    protected virtual void TryAttack()
    {
        if (index == 0 && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(Attack());
        }
    }

    protected virtual IEnumerator Attack()
    {
        float delay = Random.Range(20f, 60f);
        yield return new WaitForSeconds(delay);

        run.SetTrigger("Run");
        yield return new WaitForSeconds(2f);

        cameras.Close();
        Destroy(tab);

        jumpscare.SetTrigger("Scream");
        yield return new WaitForSeconds(1.5f);

        dead.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("Dead");
    }

    protected void GoBack()
    {
        pos[index].SetActive(false);
        index = pos.Length - 1;
        pos[index].SetActive(true);
    }
}