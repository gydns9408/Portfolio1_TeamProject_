using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLoadingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text1 = null;
    [SerializeField] Slider slider = null;
    [SerializeField] TextMeshProUGUI text2 = null;
    [SerializeField] TextMeshProUGUI texx3 = null;

    //public static LoadingText instance;
    //private Action<float> loadingdeli;

    // 로딩 씬 중 플레이어의 체력이 깍이는 것을 방지하기위한 델리게이트
    public Action isload;

    PlayerBase player;

    private float nowTime;              // 현재

    void Start()
    {
        player = FindObjectOfType<PlayerBase>();
        slider.value = 0.0f;
        StartCoroutine(ChnageText(1.5f, text1, text2, texx3));
        StartCoroutine(ChargeSlider());
        isload?.Invoke();
    }

    //private void Update()
    //{
    //    //if(player != null)
    //    //{
    //    //    player.transform.position = Vector3.zero;
    //    //    player.transform.rotation = Quaternion.Euler(0, 0, 0);
    //    //}
    //}

    public IEnumerator ChnageText(float time, TextMeshProUGUI i, TextMeshProUGUI j, TextMeshProUGUI k)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        j.color = new Color(j.color.r, j.color.g, j.color.b, 0);
        k.color = new Color(k.color.r, k.color.g, k.color.b, 0);

        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / time));
            yield return null;
        }
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);

        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / time));
            yield return null;
        }

        while (j.color.a < 1.0f)
        {
            j.color = new Color(j.color.r, j.color.g, j.color.b, j.color.a + (Time.deltaTime / time));
            yield return null;
        }
        j.color = new Color(j.color.r, j.color.g, j.color.b, 1);

        while (j.color.a > 0.0f)
        {
            j.color = new Color(j.color.r, j.color.g, j.color.b, j.color.a - (Time.deltaTime / time));
            yield return null;
        }

        while (k.color.a < 1.0f)
        {
            k.color = new Color(k.color.r, k.color.g, k.color.b, k.color.a + (Time.deltaTime / time));
            yield return null;
        }
        k.color = new Color(k.color.r, k.color.g, k.color.b, 1);

        while (k.color.a > 0.0f)
        {
            k.color = new Color(k.color.r, k.color.g, k.color.b, k.color.a - (Time.deltaTime / time));
            yield return null;
        }
    }

    IEnumerator ChargeSlider()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        operation.allowSceneActivation = false;


        while (!operation.isDone)
        {

            yield return null;

            nowTime += Time.deltaTime;

            if (nowTime < 9.0f)
            {
                slider.value = nowTime / 9.0f;
            }
            else
            {
                slider.value = 9.0f;
                operation.allowSceneActivation = true;
                yield break;
            }

        }
    }
}
