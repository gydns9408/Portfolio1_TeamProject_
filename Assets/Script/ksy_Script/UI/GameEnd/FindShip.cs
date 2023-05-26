using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FindShip : MonoBehaviour
{
    private bool isShip = false;
    private PlayerInput inputActions;
    CraftingWindow craft;
    private void Awake()
    {
        inputActions = new PlayerInput();
        craft = FindObjectOfType<CraftingWindow>();
    }

    private void OnEnable()
    {
        inputActions.OpenWindow.Enable();
        craft.makeShip += OnMakeShip;
        inputActions.OpenWindow.OpenCraftWindow.performed += OnShip;
    }


    private void OnDisable()
    {
        inputActions.OpenWindow.OpenCraftWindow.performed -= OnShip;
        inputActions.OpenWindow.Disable();
    }
    private void Start()
    {
        gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        isShip = false;
    }

    private void OnMakeShip()
    {
        gameObject.SetActive(true);
        StartCoroutine(EffectDuration());
    }

    IEnumerator EffectDuration()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(15.0f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isShip = true;
        }
    }

    private void OnShip(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(isShip == true)
        {
            SceneManager.LoadScene(3);
        }
    }
}
