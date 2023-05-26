using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputPlayerName : MonoBehaviour
{
    TMP_InputField inputField;

    NewButtonSceneChange change;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();  // 컴포넌트 찾고
        inputField.onEndEdit.AddListener(OnNameInputEnd);       // 입력이 끝났을 때 실행될 함수 등록
    }

    private void Start()
    {
        inputField.transform.parent.parent.gameObject.SetActive(false);
        change = FindObjectOfType<NewButtonSceneChange>();
        change.whatIsYOurNAme += YOurNAme;
    }

    private void YOurNAme()
    {
        inputField.transform.parent.parent.gameObject.SetActive(true);
    }

    private void OnNameInputEnd(string text)
    {
        inputField.text = text;
        DataController.Instance.gameData.playerName = inputField.text;
        inputField.transform.parent.parent.gameObject.SetActive(false); // 입력 완료되었으니 다시 안보이게 만들기
        SceneManager.LoadScene(2);
    }
}
