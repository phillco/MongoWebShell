using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using MongoDB.Driver;

namespace MongoWebShell.Server
{
    class Program
    {
        private static ConsoleColor originalColor;
        private static MongoServer upstream;

        static void Main( string[] args )
        {
            originalColor = Console.ForegroundColor;
            PrintBanner( );

            // Connect to the upstream server.
            Console.Write( "Connecting to upstream server..." );
            if ( ConnectToUpstream( ) )
                Console.WriteLine( "success!" );
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( "failure.");
                Console.ForegroundColor = originalColor;
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
        /// Just prints the startup banner.
        /// </summary>
        private static void PrintBanner( )
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write( "mongoDB Web Shell" );
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write( " v1.00" );
            Console.ForegroundColor = originalColor;
            Console.WriteLine( );
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
