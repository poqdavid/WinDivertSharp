/*
 * WinDivertNative.cs
 * (C) 2018, all rights reserved,
 *
 * This file is part of WinDivertSharp.
 *
 * WinDivertSharp is free software: you can redistribute it and/or modify it under
 * the terms of the GNU Lesser General Public License as published by the
 * Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
 * License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * WinDivertSharp is free software; you can redistribute it and/or modify it under
 * the terms of the GNU General Public License as published by the Free
 * Software Foundation; either version 2 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * for more details.
 *
 * You should have received a copy of the GNU General Public License along
 * with this program; if not, write to the Free Software Foundation, Inc., 51
 * Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace WinDivertSharp;

internal static unsafe partial class WinDivertNative
{
    /// <summary>
    /// Static initializer to get our paths correct.
    /// </summary>
    static WinDivertNative()
    {
        // Modify PATH var to include our WinDivert DLL's so that the LoadLibrary function will
        // find whatever WinDivert dll required for the current architecture.
        var path = new[] { Environment.GetEnvironmentVariable("PATH") ?? string.Empty };

        var dllSearchPaths = new[]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "x86"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "x64"),
        };

        string newPath = string.Join(Path.PathSeparator.ToString(), path.Concat(dllSearchPaths));

        Environment.SetEnvironmentVariable("PATH", newPath);
    }

    /// Return Type: HANDLE->void*
    ///filter: char*
    ///layer: WINDIVERT_LAYER->Anonymous_13846946_b76a_4250_9642_c2122691f126
    ///priority: INT16->short
    ///flags: UINT64->unsigned __int64
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertOpen", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial IntPtr WinDivertOpen([MarshalAs(UnmanagedType.LPStr)] string filter, WinDivertLayer layer, short priority, ulong flags);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///readLen: ref uint
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertRecv", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertRecv(IntPtr handle, IntPtr pPacket, uint packetLen, ref WinDivertAddress pAddr, ref uint readLen);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///flags: UINT64->unsigned __int64
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///readLen: ref uint
    ///lpOverlapped: LPOVERLAPPED->_OVERLAPPED*
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertRecvEx", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertRecvEx(IntPtr handle, IntPtr pPacket, uint packetLen, ulong flags, ref WinDivertAddress pAddr, ref uint readLen, NativeOverlapped* lpOverlapped);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///writeLen: ref uint
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertSend", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertSend(IntPtr handle, IntPtr pPacket, uint packetLen, ref WinDivertAddress pAddr, ref uint writeLen);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///flags: UINT64->unsigned __int64
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///writeLen: ref uint
    ///lpOverlapped: LPOVERLAPPED->_OVERLAPPED*
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertSendEx", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertSendEx(IntPtr handle, IntPtr pPacket, uint packetLen, ulong flags, ref WinDivertAddress pAddr, ref uint writeLen, NativeOverlapped* lpOverlapped);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///flags: UINT64->unsigned __int64
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///writeLen: ref uint
    ///lpOverlapped: LPOVERLAPPED->_OVERLAPPED*
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertSendEx", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertSendEx(IntPtr handle, IntPtr pPacket, uint packetLen, ulong flags, ref WinDivertAddress pAddr, IntPtr ignoredLenPtr, IntPtr ignoredOverlappedPtr);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertClose", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertClose(IntPtr handle);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    ///param: WINDIVERT_PARAM->Anonymous_fff177c6_9a3b_4c59_b7f9_62aa58d87f4e
    ///value: UINT64->unsigned __int64
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertSetParam", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertSetParam(IntPtr handle, WinDivertParam param, ulong value);

    /// Return Type: BOOL->int
    ///handle: HANDLE->void*
    ///param: WINDIVERT_PARAM->Anonymous_fff177c6_9a3b_4c59_b7f9_62aa58d87f4e
    ///pValue: UINT64*
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertGetParam", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertGetParam(IntPtr handle, WinDivertParam param, out ulong pValue);

    /// Return Type: BOOL->int
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///ppIpHdr: PWINDIVERT_IPHDR*
    ///ppIpv6Hdr: PWINDIVERT_IPV6HDR*
    ///ppIcmpHdr: PWINDIVERT_ICMPHDR*
    ///ppIcmpv6Hdr: PWINDIVERT_ICMPV6HDR*
    ///ppTcpHdr: PWINDIVERT_TCPHDR*
    ///ppUdpHdr: PWINDIVERT_UDPHDR*
    ///ppData: PVOID*
    ///pDataLen: ref uint
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertHelperParsePacket", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertHelperParsePacket(IntPtr pPacket, uint packetLen, IPv4Header** ppIpHdr, IPv6Header** ppIpv6Hdr, IcmpV4Header** ppIcmpHdr, IcmpV6Header** ppIcmpv6Hdr, TcpHeader** ppTcpHdr, UdpHeader** ppUdpHdr, byte** ppData, ref uint pDataLen);

    /// Return Type: UINT->unsigned int
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///flags: UINT64->unsigned __int64
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertHelperCalcChecksums", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial uint WinDivertHelperCalcChecksums(IntPtr pPacket, uint packetLen, ref WinDivertAddress pAddr, ulong flags);

    /// Return Type: UINT->unsigned int
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///flags: UINT64->unsigned __int64
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertHelperCalcChecksums", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial uint WinDivertHelperCalcChecksums(IntPtr pPacket, uint packetLen, IntPtr ignoredAddress, ulong flags);

    /// Return Type: UINT->unsigned int
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    ///flags: UINT64->unsigned __int64
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertHelperCalcChecksums", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    public static partial uint WinDivertHelperCalcChecksums(byte* pPacket, uint packetLen, IntPtr ignoredAddress, ulong flags);

    /// Return Type: BOOL->int
    ///filter: char*
    ///layer: WINDIVERT_LAYER->Anonymous_13846946_b76a_4250_9642_c2122691f126
    ///errorStr: char**
    ///errorPos: ref uint
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertHelperCheckFilter", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertHelperCheckFilter([MarshalAs(UnmanagedType.LPStr)] string filter, WinDivertLayer layer, char** errorStr, ref uint errorPos);

    /// Return Type: BOOL->int
    ///filter: char*
    ///layer: WINDIVERT_LAYER->Anonymous_13846946_b76a_4250_9642_c2122691f126
    ///pPacket: PVOID->void*
    ///packetLen: UINT->unsigned int
    ///pAddr: PWINDIVERT_ADDRESS->Anonymous_33ad92c9_0104_417e_989a_2fdd4b3efcc1*
    [LibraryImport("WinDivert.dll", EntryPoint = "WinDivertHelperEvalFilter", SetLastError = true)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WinDivertHelperEvalFilter([MarshalAs(UnmanagedType.LPStr)] string filter, WinDivertLayer layer, IntPtr pPacket, uint packetLen, ref WinDivertAddress pAddr);
}