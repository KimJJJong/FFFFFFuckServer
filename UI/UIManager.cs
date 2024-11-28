using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 유니티에서 연결
    public GameObject startUI;
    public GameObject lobbyUI;

    public TMP_InputField inputID;
    public TMP_InputField inputIP;

   // public Button buttonID; //필요 없지 않을까?
    //public Button buttonIP; //필요 없지 않을까?

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
                Debug.Log("유효하지 않은 ID입니다.");
                return;
            }
            GameManager.Instance.UserID = inputID.text;

            string ip = inputIP.text;
            if (string.IsNullOrEmpty(id))
            {
                Debug.Log("유효하지 않은 IP입니다.");
                return;
            }

            FindObjectOfType<Client>().StartClient(inputIP.text);

        });

        SetUIState(EUIState.Start);
    }




    public enum EUIState
    {
        Start,      // 접속 하자 마자
        Lobby,      // IP, ID 입력 후 나오는 GameRoom이 있는 로비
        Game        // 찐으로 게임시작ㅇㅇ
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
