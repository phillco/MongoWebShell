using System;
using System.Collections.Generic;
using System.Text;
using HybridDSP.Net.HTTP;
using System.IO;

namespace MongoWebShell.Server
{
    class MongoWebServer
    {
        class MongoHandler : IHTTPRequestHandler
        {
            private const string RootPath = "Client\\";

            public void HandleRequest( HTTPServerRequest request, HTTPServerResponse response )
            {
                var root = Path.GetFullPath( RootPath );
                var path = Path.Combine( root, request.URI.Substring( 1 ) );

                if ( !path.StartsWith( root ) )
                {
                    response.StatusAndReason = HTTPServerResponse.HTTPStatus.HTTP_FORBIDDEN;
                    response.Send( );
                    return;
                }

                if ( request.URI == "/" )
                    response.SendFile( Path.Combine( RootPath, "index.html" ), "text/html" );
                else if ( File.Exists( path ) )
                    response.SendFile( path, "unknown" );
                else
                {
                    response.StatusAndReason = HTTPServerResponse.HTTPStatus.HTTP_NOT_FOUND;
                    response.Send( );
                }
            }
        }

        /// <summary>
        /// Justs generates an instance of our request handler.
        /// </summary>
        class RequestHandlerFactory : IHTTPRequestHandlerFactory
        {
            public IHTTPRequestHandler CreateRequestHandler( HTTPServerRequest request )
            {
                return new MongoHandler( );
            }
        }

        public static HTTPServer Create( int port = 8080 )
        {
            return new HTTPServer( new RequestHandlerFactory( ), port );
        }
    }
}
