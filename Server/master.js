const http = require('http');
const url = require('url');
const cp = require('child_process');
const server =  require('./app.js')

var connection = 0;
var port = 1999; 

function onRequest(request, response) {
    var pathname = url.parse(request.url).pathname;
    if( pathname == '/connect' ){
        //console.log('New connection request');
        if(connection == 0){
            port ++;
            server(port);
            console.log("Started instance of the server on: " + port);
            var Json = {};
            Json.port = port;
        }else{
            var Json = {};
            Json.port = port;
        }
        response.writeHead(200, {'Content-Type': 'json'});
        response.write(JSON.stringify(Json));
        response.end();
        
        connection ++;
        //console.log(connection);
        if(connection == 4)
            connection = 0;
        
    }

}
http.createServer(onRequest).listen(8080);
console.log('Server started');