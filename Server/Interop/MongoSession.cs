using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
using MongoDB.Driver;
using System.Net;
using System.Net.Sockets;

namespace MongoWebShell.Server.Interop
{
    /// <summary>
    /// Represents a mongo administration session.
    /// </summary>
    public class MongoSession
    {
        //=================================================================================
        //
        //  PROPERTIES
        //
        //=================================================================================

        /// <summary>
        /// The status of this session.
        /// </summary>
        public ConnectionStatus Status { get; private set; }

        /// <summary>
        /// The address of the server we're connected to.
        /// </summary>
        public RemoteHost Address { get; private set; }

        /// <summary>
        /// The local mongo.exe connection to the serer.
        /// </summary>
        public ProcessWrapper Client { get; private set; }

        /// <summary>
        /// The driver connection to the server.
        /// </summary>
        public MongoServer Server { get; private set; }

        //=================================================================================
        //
        //  CONSTRUCTORS
        //
        //=================================================================================

        public MongoSession( )
            : this( "localhost" )
        {
        }

        public MongoSession( string address )
        {
            Status = new ConnectionStatus
            {
                OriginalConnectionString = address
            };
            Start( );
        }

        //=================================================================================
        //
        //  PUBLIC METHODS
        //
        //=================================================================================

        public void Start( )
        {
            Status.CurrentState = ConnectionStatus.State.CONNECTING;

            ThreadPool.QueueUserWorkItem( delegate
            {
                // First resolve the DNS name of the address.
                this.Address = Util.Resolve( Status.OriginalConnectionString, 27017 );
                if ( Address == null )
                {
                    Status.Fail( "Could not resolve that address." );
                    return;
                }

                // Create the clients.
                Client = ProcessWrapper.Start( "mongo.exe", Address.EndPoint.ToString( ) );                
                Server = MongoServer.Create( new MongoServerSettings { Server = new MongoServerAddress( Address.HostName, Address.EndPoint.Port ) } );
                Client.Start( );
                
                try
                {
                    // Attempt to verify the connection.
                    Server.Ping( );
                    Status.CurrentState = ConnectionStatus.State.CONNECTED;
                }
                catch ( SocketException )
                {
                    Status.Fail( "Could not connect to the server. (Socket error)" );
                }
            } );
        }

        public void Stop( )
        {            
            if ( Server != null )
                Server.Disconnect( );
            if ( Client != null )
                Client.Stop( );

            Status.Disconnect( );
        }

        public override string ToString( )
        {
            if ( Address == null )
                return Status.OriginalConnectionString;
            else
            {
                if ( Address.EndPoint.Port == Constants.DefaultMongoServerPort )
                    return Address.HostName;
                else
                    return Address.HostName + ":" + Address.EndPoint.Port;
            }
        }
    }
}
