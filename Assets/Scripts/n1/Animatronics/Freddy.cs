using System.Collections;
using UnityEngine;

public class Freddy : N1Base
{
    protected void Update()
    {
        if (Camera.activeSelf && !returning && index != 0)
        {
            returning = true;
            StartCoroutine(ReturnToStart());
        }
    }

    protected IEnumerator ReturnToStart()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(TransEffect());
        pos[index].SetActive(false);
        index = pos.Length - 1;
        pos[index].SetActive(true);
        returning = false;
    }
}