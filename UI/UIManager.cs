using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // ����Ƽ���� ����
    public GameObject startUI;
    public GameObject lobbyUI;

    public TMP_InputField inputID;
    public TMP_InputField inputIP;

   // public Button buttonID; //�ʿ� ���� ������?
    //public Button buttonIP; //�ʿ� ���� ������?

    public Button buttonStart;


    private Client _client;

    private void Awake()
    {
        _client = FindObjectOfType<Client>();

      
        buttonStart.onClick.AddListener(() =>
        {
            string id = inputID.text;
            if (string.IsNullOrEmpty(id))
            {
                Debug.Log("��ȿ���� ���� ID�Դϴ�.");
                return;
            }
            GameManager.Instance.UserID = inputID.text;

            string ip = inputIP.text;
            if (string.IsNullOrEmpty(id))
            {
                Debug.Log("��ȿ���� ���� IP�Դϴ�.");
                return;
            }

            FindObjectOfType<Client>().StartClient(inputIP.text);

        });

        SetUIState(EUIState.Start);
    }




    public enum EUIState
    {
        Start,      // ���� ���� ����
        Lobby,      // IP, ID �Է� �� ������ GameRoom�� �ִ� �κ�
        Game        // ������ ���ӽ��ۤ���
    }

    public void SetUIState(EUIState state)
    {
        switch (state)
        {
            case EUIState.Start:
                startUI.SetActive(true);
                lobbyUI.SetActive(false);
                break;
            case EUIState.Lobby:
                startUI.SetActive(false);
                lobbyUI.SetActive(true);
                break;
            case EUIState.Game:
                startUI.SetActive(false);
                lobbyUI.SetActive(false);
                break;
        }
    }

}
