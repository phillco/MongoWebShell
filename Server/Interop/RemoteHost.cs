using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MongoWebShell.Server.Interop
{
    /// <summary>
    /// Just stores an IPEndPoint (IP address+port) and a hostname together.
    /// </summary>
    public class RemoteHost
    {
        public IPEndPoint EndPoint { get; set; }
        public string HostName { get; set; }

        public RemoteHost( IPAddress address, int port, string hostName )
        {
            EndPoint = new IPEndPoint( address, port );
            HostName = hostName;
        }

        public override string ToString( )
        {
            return EndPoint.ToString( );
        }
    }
}
