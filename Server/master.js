const http = require('http');
const url = require('url');
const cp = require('child_process');

var connection = 0;
var ports = {};
ports.free = [2000,2001,2002,2003,2004,2005,2006,2007,2008,2009]
ports.busy = [];
var port; 

function onInstanceClose(port){

    console.log("Calling onInstanceClose");
    console.log(port);

    var index = ports.busy.indexOf(Number(port));
    if (index > -1) {
        ports.busy.splice(index, 1);
        ports.free.push(port);
    }
    console.log(ports.free)
    console.log(ports.busy)
}

function onRequest(request, response) {
    var pathname = url.parse(request.url).pathname;
    if( pathname == '/connect' ){
        //console.log('New connection request');
        if(connection == 0){
            if(ports.free.length==0){
                var Json= {};
                Json.mex = "All the instaces are busy";
            }else{
                port = ports.free[0]
                const server = cp.fork(__dirname + '/app.js', [port]);
        
                server.on('message', (code) => {
                    onInstanceClose(code.port);
                });

                console.log("Started instance of the server on: " + port);
                var Json = {};
                Json.port = port;
            }
        }else{
            var Json = {};
            Json.port = port;
        }
        response.writeHead(200, {'Content-Type': 'json'});
        response.write(JSON.stringify(Json));
        response.end();
        
        connection ++;
        if(connection == 4){
            ports.busy.push(port)
            ports.free.shift();
            connection = 0;
        }
    }

}

http.createServer(onRequest).listen(8080);
console.log('Server started');