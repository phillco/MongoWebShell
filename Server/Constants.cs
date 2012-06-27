using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoWebShell.Server
{
    /// <summary>
    /// Store application-wide constants here.
    /// </summary>
    class Constants
    {
        /// <summary>The default port that mongod listens on.</summary>
        public const int DefaultMongoServerPort = 27017;

        /// <summary>The default port that we listen on.</summary>
        public const int DefaultMongoWebPort = 29017;
    }
}
