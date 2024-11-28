using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetClient
{
    public enum EConnectionState
    {
        DisConnected,
        Connecting,
        Connected,
    }

    private Socket _socket;
    private UserToken _userToken;
    private EConnectionState _connectionState;

    public EConnectionState ConnectionState => _connectionState;

    public event System.Action<bool, UserToken> onConnected;    // ���Ӽ��� ����, UserToken

    public void Start(string ip)
    {
        _connectionState = EConnectionState.Connecting;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), NetDefine.PORT);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // �񵿱� ����
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += OnConnected;
        args.RemoteEndPoint = endPoint; // ������ EndPoint;
        bool pending = _socket.ConnectAsync(args);
        if (!pending)
        {
            OnConnected(null, args);
        }
        Debug.Log("Ŭ���̾�Ʈ ����");
    }

    public void End()
    {
        _connectionState = EConnectionState.DisConnected;
        if (_userToken != null)
        {
            _userToken.Close();
        }
    }

    private void OnConnected(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.Success)
        {
            _connectionState = EConnectionState.Connected;
            _userToken = new UserToken(_socket, PacketMessageDispatcher.Instance);
            _userToken.OnConnected();
            _userToken.onSessionClosed += OnSessionClosed;
            _userToken.StartReceive();
            Debug.Log("���� ���� ����");

            MainThreadDispatcher.Instance.Add(() =>
            {
                onConnected?.Invoke(true, _userToken);
            });
        }
        else
        {
            _connectionState = EConnectionState.DisConnected;
            Debug.LogError($"���� ���� ����: {e.SocketError}");

            MainThreadDispatcher.Instance.Add(() =>
            {
                onConnected?.Invoke(false, null);
            });
        }
    }

    private void OnSessionClosed(UserToken token)
    {
        _connectionState = EConnectionState.DisConnected;
    }
}