using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoWebShell.Server.Interop
{
    public class ConnectionStatus
    {
        //=================================================================================
        //
        //  PROPERTIES
        //
        //=================================================================================

        public enum State { DISCONNECTED, CONNECTING, CONNECTED, FAILED }

        /// <summary>
        /// The address (or whatever it is) the user told us to connect to.
        /// </summary>
        public string OriginalConnectionString { get; set; }

        /// <summary>
        /// The reason the connection failed, supplied by MongoSession.
        /// </summary>
        public string FailureReason { get; private set; }

        public State CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                currentState = value;
                if ( StateChanged != null ) // Fire the event!
                    StateChanged( );
            }
        }


        //=================================================================================
        //
        //  EVENTS
        //
        //=================================================================================

        public event VoidDelegate StateChanged;

        public delegate void VoidDelegate( );

        //=================================================================================
        //
        //  PRIVATE VARIABLES
        //
        //=================================================================================

        private State currentState = State.DISCONNECTED;

        //=================================================================================
        //
        //  PUBLIC METHODS
        //
        //=================================================================================

        public void Disconnect( )
        {
            CurrentState = State.DISCONNECTED;
        }

        public void Fail( string reason )
        {
            FailureReason = reason;
            CurrentState = State.FAILED;
        }

    }
}
