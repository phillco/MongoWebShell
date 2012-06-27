using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;
using MongoWebShell.Server.Interop;
using System.Net.Sockets;
using System.Diagnostics;

namespace MongoWebShell.Server
{
    public class Util
    {
        /// <summary>
        /// Parses and returns a RemoteHost (IP+port+host) from a string (eg, "localhost", "mongo.myserver.org:7777", "192.168.1.5:10666").
        /// </summary>
        public static RemoteHost Resolve( string addressString, int defaultPort = 0 )
        {
            string[] parts = addressString.Split( ':' );

            // First resolve the DNS part of the address.
            try
            {
                IPHostEntry host = Dns.GetHostEntry( parts[0] );

                IPAddress address = host.AddressList[0];
                foreach ( IPAddress a in host.AddressList )
                    if ( a.AddressFamily == AddressFamily.InterNetwork ) // Favor IPv4 (temporary, should run mongo.exe with --ipv6 instead)
                        address = a;

                // (Optional) Parse the port.
                int port = defaultPort;
                if ( parts.Length > 1 )
                {
                    if ( !int.TryParse( parts[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port ) )
                        port = defaultPort;
                }

                return new RemoteHost( address, port, host.HostName );
            }
            catch ( SocketException )
            {
                return null;
            }
        }

        /// <summary>
        /// Starts the program with the given arguments. A shortcut for System.Diagnostics.
        /// </summary>
        public static void LaunchProgram( string fileName, string arguments = "", string workingDirectory = "", bool waitForExit = false )
        {
            using ( Process process = new Process( ) )
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WorkingDirectory = workingDirectory;

                process.Start( );
                if ( waitForExit )
                    process.WaitForExit( );
            }
        }

        /// <summary>
        /// Opens a web browser to connect to the given URL.
        /// </summary>       
        public static void LaunchBrowser( string address )
        {
            LaunchProgram( address );
        }
    }
}
