using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MongoWebShell.Server
{
    class Program
    {
        static void Main( string[] args )
        {
            PrintBanner( );

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
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write( "mongoDB Web Shell" );
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write( " v1.00" );
            Console.ForegroundColor = color;
            Console.WriteLine( );
        }
    }
}
