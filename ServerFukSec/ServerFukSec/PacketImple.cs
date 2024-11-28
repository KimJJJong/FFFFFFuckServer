// SC : 서버->클라, CS : 클라->서버, REL : 릴레이
using System.Runtime.InteropServices;

public enum EProtocolID
{
    SC_REQ_USERINFO,         // 서버 -> 클라 : 유저 정보 요청
    CS_ANS_USERINFO,         // 클라 -> 서버 : 유저 정보 응답
    REL_GAME_READY,          // 서버 -> 클라 : 게임 준비 상태 릴레이
    CS_GAME_READY_OK,        // 클라 -> 서버 : 게임 준비 완료
    SC_GAME_START,           // 서버 -> 클라 : 게임 시작
    REL_UNIT_SUMMONED,       // 서버 -> 클라 : 유닛 소환 완료 릴레이
    REL_UNIT_ACTION,         // 서버 -> 클라 : 유닛 행동 이벤트 릴레이
    REL_GRID_UPDATE,         // 서버 -> 클라 : 맵 격자 상태 업데이트
    SC_GAME_END,             // 서버 -> 클라 : 게임 종료
    SC_ERROR_MESSAGE         // 서버 -> 클라 : 에러 메시지 전달
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
    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 2)] // 최대 2명
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
            if (players[i].uid == 0) // 빈 슬롯 찾기
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
    public int xPosition; // 격자 좌표
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
    public int actionType; // 예: 이동(1), 공격(2) 등
    public int targetUID;  // 공격 또는 상호작용 대상

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
