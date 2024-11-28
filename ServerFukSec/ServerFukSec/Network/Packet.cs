using System.Runtime.InteropServices;
using System;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class Packet
{
    private short _size;
    private short _protocolID;

    public short Size => _size;
    public short ProtocolID => _protocolID;

    public Packet(short protocolID)
    {
        _size = (short)Marshal.SizeOf(this);
        _protocolID = protocolID;
    }

    public byte[] ToByte()
    {
        // Send Packet;
        byte[] buffer = new byte[Marshal.SizeOf(this)];

        // unsafe ������ ����ϱ� ����
        // ���� ���� allow unsafe code, Unity : Player - OtherSetting
        // fixed unsafe �������� ��밡�� �޸𸮸� �����ϰ� ����Ҽ� �ִ�
        unsafe
        {
            fixed (byte* fixed_buffer = buffer)
            {
                Marshal.StructureToPtr(this, (IntPtr)fixed_buffer, false);
            }
        }

        return buffer;
    }

    public void ToPacket(byte[] buffer)
    {
        unsafe
        {
            fixed (byte* fixed_buffer = buffer)
            {
                Marshal.PtrToStructure((IntPtr)fixed_buffer, this);
            }
        }
    }
}