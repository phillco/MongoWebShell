using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MongoDB.Driver;

namespace MongoWebShell.Server
{
    class Program
    {
        private static MongoServer upstream;

        static void Main( string[] args )
        {
            Util.ConsoleWriteLine( "mongoDB Web Shell v1.00\n", ConsoleColor.Cyan );

            // Connect to the upstream server.
            Console.Write( "Connecting to upstream server..." );
            if ( ConnectToUpstream( ) )
                Util.ConsoleWriteLine( "success!", ConsoleColor.Green );
            else
            {
                Util.ConsoleWriteLine( "failure.", ConsoleColor.Red );
                Console.WriteLine( "Please ensure MongoDB is running.\n" );
                return;
            }

            // Start the server.
            int port = Constants.DefaultMongoWebPort;
            var server = MongoWebServer.Create( port );
            server.Start( );

            // Kick the user to it.
            Util.LaunchBrowser( "http://localhost:" + port );

            // Stay alive until the user kills us.
            Console.Write( "\nServer started. Press any key to shut down..." );
            Console.ReadKey( );
            Console.WriteLine( "\nServer shutting down..." );
            // server.Stop( );
        }

        /// <summary>
        /// Connects to the upstream MongoDB server and returns if successful.
        /// </summary>
        private static bool ConnectToUpstream( )
        {
            try
            {
                upstream = MongoServer.Create( );
                upstream.Connect( );
                upstream.Ping( );
                return true;
            }
            catch ( MongoConnectionException )
            {
                return false;
            }
        }
    }
}
