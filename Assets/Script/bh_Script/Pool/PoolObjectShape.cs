using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectShape : MonoBehaviour
{
    // Start is called before the first frame update

    public Action onDisable;

    protected IEnumerator LifeOver(float remainingTime = 0.0f) {
        yield return new WaitForSeconds(remainingTime);
        gameObject.SetActive(false);
    }

    protected virtual void OnDisable() {
        onDisable?.Invoke();
    }

}
