using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Credit());
    }
    IEnumerator Credit()
    {
        yield return new WaitForSeconds(17.0f);
        SceneManager.LoadScene(0);
    }
}
