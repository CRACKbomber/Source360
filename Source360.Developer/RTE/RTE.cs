using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Source360.Common;
using Microsoft.Test.Xbox.XDRPC;
using XDevkit;
using XRPCLib;
namespace Source360.Development.RTE
{
    public class RTE
    {
        private XboxConsole m_IXboxConsole;
        private XRPC m_XRPC;
        private GameNames m_gameName;
        private bool m_bIsXRPC;
        /// <summary>
        /// Current Xbox Console if using XDRPC
        /// </summary>
        public XboxConsole XConsole { get { return this.m_IXboxConsole; } }
        /// <summary>
        /// XRPC object
        /// </summary>
        public XRPC RPC { get { return this.m_XRPC; } }
        /// <summary>
        /// RTE constructor
        /// </summary>
        /// <param name="XRPC">Use XRPC as connection protocol</param>
        public RTE(bool XRPC)
        {
            if (XRPC)
            {
                /// Try to connect with XRPC
                try
                {
                    this.m_XRPC = new XRPC();
                    this.m_XRPC.Connect();
                    this.m_XRPC.Notify(XRPCLib.XRPC.XNotiyLogo.XBOX_LOGO, "Source 360 development tools loaded!");
                }
                catch (System.Exception ex)
                {
                    throw new Exception(string.Format("XRPC could connect!, with the error {0}", ex.Message));
                }
                finally
                {
                    this.m_bIsXRPC = true;
                }
            }
            else
            {
                /// Use XDRPC
                try
                {
                    XboxManager manager = (XDevkit.XboxManager)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342")));
                    this.m_IXboxConsole =  manager.OpenConsole(manager.DefaultConsole);
                }
                catch (System.Exception ex)
                {
                    throw new Exception(string.Format("XDRPC could connect!, with the error {0}", ex.Message));
                }
                finally
                {
                    this.m_bIsXRPC = false;
                }
            }
            m_gameName = XamGetCurrentTitleId(); 
        }
        /// <summary>
        /// Gets the current title ID
        /// </summary>
        /// <returns>current title ID</returns>
        public GameNames XamGetCurrentTitleId()
        {
            uint curID;
            if (this.m_bIsXRPC)
                curID = this.m_XRPC.SystemCall(new object[] { this.m_XRPC.ResolveFunction("xam.xex", 463) });
            else
                curID = this.m_IXboxConsole.ExecuteRPC<uint>(XDRPCMode.Title, "xam.xex", 463, new object[] { });
            switch (curID)
            {
                case (0x45410830):
                    return GameNames.Left4Dead;
                case (0x454108D4):
                    return GameNames.Left4Dead2;
                case (0x4541080f):
                    return GameNames.OrangeBox;
                case (0x5841125A):
                    return GameNames.CSGO;
                case (0x45410912):
                    return GameNames.Portal2;
                default:
                    return GameNames.NoGame;
            }
        }
        /// <summary>
        /// Reads raw data from the xbox memory
        /// </summary>
        /// <param name="addr">address to read</param>
        /// <param name="len">length to read</param>
        /// <returns>raw data</returns>
        public byte[] ReadMemory(uint addr, uint len)
        {
            byte[] mem = new byte[len];
            uint outBytes;
            if (m_bIsXRPC)
                m_XRPC.GetMemory(addr, len);
            else
                m_IXboxConsole.DebugTarget.GetMemory(addr, len, mem, out outBytes);
            return mem;
        }
        /// <summary>
        /// Sends a command to the specified game
        /// </summary>
        /// <param name="command">string command</param>
        public void SendCommand(string command)
        {
            object[] parameters;
            if (this.m_gameName == GameNames.OrangeBox)
            {
                if (this.m_bIsXRPC)
                {
                    parameters = new object[] { command };
                    this.m_XRPC.Call(GetCBuf_AddText(this.m_gameName), parameters);
                    this.m_XRPC.Call(GetCBuf_Execute(this.m_gameName), new object[] { null });
                }
                else
                {
                    parameters = new object[] { new XDRPCStringArgumentInfo(command) };
                    CallFunction<uint>(GetCBuf_AddText(this.m_gameName), parameters);
                    CallFunction<uint>(GetCBuf_Execute(this.m_gameName));
                }
            }
            else if (this.m_bIsXRPC)
            {
                parameters = new object[] { 0, command, 0 };
                this.m_XRPC.Call(GetCBuf_AddText(this.m_gameName), parameters);
                this.m_XRPC.Call(GetCBuf_Execute(this.m_gameName), new object[] { null });
            }
            else
            {
                parameters = new object[] { new XDRPCArgumentInfo<uint>(0), new XDRPCStringArgumentInfo(command), new XDRPCArgumentInfo<uint>(0) };
                CallFunction<uint>(GetCBuf_AddText(this.m_gameName), parameters);
                CallFunction<uint>(GetCBuf_Execute(this.m_gameName));
            }
        }
        /// <summary>
        /// Calls a remote function
        /// </summary>
        /// <param name="addr">address to call</param>
        /// <param name="parameters">function parameters</param>
        /// <returns>function output</returns>
        public T CallFunction<T>(uint addr, object[] parameters)
            where T : struct
        {
            if (this.m_bIsXRPC)
                throw new Exception("Call function not supported with XRPC");
            return this.m_IXboxConsole.ExecuteRPC<T>(XDRPCMode.Title, addr, parameters);
        }
        /// <summary>
        /// Calls a remote function
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="addr">address to call</param>
        /// <returns>function output</returns>
        public T CallFunction<T>(uint addr)
            where T : struct
        {
            if (this.m_bIsXRPC)
                throw new Exception("Call function not supported with XRPC");
            return m_IXboxConsole.ExecuteRPC<T>(XDRPCMode.Title, addr);
        }
        /// <summary>
        /// CBuf_AddText
        /// </summary>
        /// <param name="gameName">Current Game</param>
        /// <returns>address of CBuf_AddText</returns>
        public static uint GetCBuf_AddText(GameNames gameName)
        {

            switch (gameName)
            {
                case (GameNames.CSGO):
                    return 0x86A1A330;
                case (GameNames.Left4Dead):
                    return 0x8642C4A8;
                case (GameNames.Left4Dead2):
                    return 0x86BCDDF0;
                case (GameNames.OrangeBox):
                    return 0x860992AC;
                case (GameNames.Portal2):
                    return 0x82D077C8;
                default:
                    throw new Exception("UNKNOWN ERROR OCCURED, GAME NAME IS UNDEFINED");
            }
        }
        /// <summary>
        /// CBuf_Execute
        /// </summary>
        /// <param name="gameName">current game</param>
        /// <returns>address of CBuf_Execute</returns>
        public static uint GetCBuf_Execute(GameNames gameName)
        {
            switch (gameName)
            {
                case (GameNames.CSGO):
                    return 0x86A1AFB8;
                case (GameNames.Left4Dead):
                    return 0x8642ECF0;
                case (GameNames.Left4Dead2):
                    return 0x86BD01D0;
                case (GameNames.OrangeBox):
                    return 0x8609B670;
                case (GameNames.Portal2):
                    return 0x82D08390;
                default:
                    throw new Exception("UNKNOWN ERROR OCCURED, GAME NAME IS UNDEFINED");
            }
        }
    }
}
