// SC : ����->Ŭ��, CS : Ŭ��->����, REL : ������
using System.Runtime.InteropServices;

public enum EProtocolID
{
    SC_REQ_USERINFO,         // ���� -> Ŭ�� : ���� ���� ��û
    CS_ANS_USERINFO,         // Ŭ�� -> ���� : ���� ���� ����
    REL_GAME_READY,          // ���� -> Ŭ�� : ���� �غ� ���� ������
    CS_GAME_READY_OK,        // Ŭ�� -> ���� : ���� �غ� �Ϸ�
    SC_GAME_START,           // ���� -> Ŭ�� : ���� ����
    REL_UNIT_SUMMONED,       // ���� -> Ŭ�� : ���� ��ȯ �Ϸ� ������
    REL_UNIT_ACTION,         // ���� -> Ŭ�� : ���� �ൿ �̺�Ʈ ������
    REL_GRID_UPDATE,         // ���� -> Ŭ�� : �� ���� ���� ������Ʈ
    SC_GAME_END,             // ���� -> Ŭ�� : ���� ����
    SC_ERROR_MESSAGE         // ���� -> Ŭ�� : ���� �޽��� ����
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketReqUserInfo : Packet
{
    public int uid;

    public PacketReqUserInfo()
        : base((short)EProtocolID.SC_REQ_USERINFO)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketAnsUserInfo : Packet
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
    public string id;

    public PacketAnsUserInfo()
        : base((short)EProtocolID.CS_ANS_USERINFO)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketGameReady : Packet
{
    public PacketGameReady()
        : base((short)EProtocolID.REL_GAME_READY)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketGameReadyOk : Packet
{
    public PacketGameReadyOk()
        : base((short)EProtocolID.CS_GAME_READY_OK)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct GameStartInfo
{
    public int uid;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
    public string id;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketGameStart : Packet
{
    public int playerCount;
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 2)] // �ִ� 2��
    public GameStartInfo[] players;

    public PacketGameStart()
        : base((short)EProtocolID.SC_GAME_START)
    {
        players = new GameStartInfo[2];
    }

    public void AddPlayerInfo(int uid, string id)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].uid == 0) // �� ���� ã��
            {
                players[i] = new GameStartInfo { uid = uid, id = id };
                playerCount++;
                break;
            }
        }
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketUnitSummoned : Packet
{
    public int unitUID;
    public int cardID;
    public int ownerUID;
    public int xPosition; // ���� ��ǥ
    public int yPosition;

    public PacketUnitSummoned()
        : base((short)EProtocolID.REL_UNIT_SUMMONED)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketUnitAction : Packet
{
    public int unitUID;
    public int actionType; // ��: �̵�(1), ����(2) ��
    public int targetUID;  // ���� �Ǵ� ��ȣ�ۿ� ���

    public PacketUnitAction()
        : base((short)EProtocolID.REL_UNIT_ACTION)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketGridUpdate : Packet
{
    public int xPosition;
    public int yPosition;
    public int ownerUID;

    public PacketGridUpdate()
        : base((short)EProtocolID.REL_GRID_UPDATE)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketGameEnd : Packet
{
    public int winningUID;

    public PacketGameEnd()
        : base((short)EProtocolID.SC_GAME_END)
    {
    }
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class PacketErrorMessage : Packet
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
    public string errorMessage;

    public PacketErrorMessage()
        : base((short)EProtocolID.SC_ERROR_MESSAGE)
    {
    }
}
