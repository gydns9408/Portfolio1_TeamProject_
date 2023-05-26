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
        inputField = GetComponentInChildren<TMP_InputField>();  // ������Ʈ ã��
        inputField.onEndEdit.AddListener(OnNameInputEnd);       // �Է��� ������ �� ����� �Լ� ���
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
        inputField.transform.parent.parent.gameObject.SetActive(false); // �Է� �Ϸ�Ǿ����� �ٽ� �Ⱥ��̰� �����
        SceneManager.LoadScene(2);
    }
}
