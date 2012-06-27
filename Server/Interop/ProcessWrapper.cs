using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace MongoWebShell.Server.Interop
{
    /// <summary>
    /// Wraps the mongo.exe client session and handles starting it, redirescting its input & output, and checking for its failure.
    /// </summary>
    public class ProcessWrapper
    {
        //=================================================================================
        //
        //  EVENTS
        //
        //=================================================================================

        /// <summary>Occurs when a new line of input has been received.</summary>
        public event InputReceivedDelegate InputReceived;

        public delegate void InputReceivedDelegate( string text );

        //=================================================================================
        //
        //  PRIVATE VARIABLES
        //
        //=================================================================================

        private Process process;
        private StreamReader input;
        private StreamWriter output;

        //=================================================================================
        //
        //  CONSTRUCTORS
        //
        //=================================================================================

        /// <summary>
        /// Creates the process wrapper around an existing mongo process.
        /// </summary>
        public ProcessWrapper( Process process )
        {   
            process.Start( );
            this.process = process;
            this.input = process.StandardOutput;
            this.output = process.StandardInput;
        }

        /// <summary>
        /// Starts a new mongo process with the given parameters and wraps it.
        /// </summary>
        public static ProcessWrapper Start( string path, string arguments )
        {
            // Create the mongo console process.
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "mongo.exe",
                    Arguments = arguments,
                    ErrorDialog = false,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                }
            };

            return new ProcessWrapper( process );
        }

        //=================================================================================
        //
        //  PUBLIC METHODS
        //
        //=================================================================================

        public void Start( )
        {
            // Create the polling thread.
            var thread = new Thread( InputThreadLoop );
            thread.IsBackground = true;
            thread.Start( );
        }

        /// <summary>
        /// Sends the given command to the console.
        /// </summary>
        /// <param name="command"></param>
        public void Send( string command )
        {
            output.WriteLine( command );
        }

        /// <summary>
        /// Stops the process.
        /// </summary>
        public void Stop( )
        {
            try
            {
                process.Kill( );
            }
            catch ( InvalidOperationException ) { }
        }

        public override string ToString( )
        {
            return "mongo.exe PID#" + process.Id;
        }

        //=================================================================================
        //
        //  PRIVATE METHODS
        //
        //=================================================================================

        /// <summary>
        /// Loops forever, polling for input.
        /// </summary>
        private void InputThreadLoop( )
        {
            Thread.CurrentThread.Name = ( this + " input thread" );

            while ( !process.HasExited )
            {
                string line = input.ReadLine( ); // Does not block; returns null if no input.
                if ( !string.IsNullOrEmpty( line ) )
                {
                    if ( InputReceived != null )
                        InputReceived( line + Environment.NewLine );
                }
                else
                    Thread.Sleep( 15 );
            }
        }
    }
}
