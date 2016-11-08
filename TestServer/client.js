var io_client = require('socket.io-client')
var ipAddress = "localhost"
var port = "3000";

var socket = io_client.connect('http://' + ipAddress + ':' + port);
socket.emit('USER_CONNECT');
socket.emit('PLAY',{"name" : "luigi"});
socket.on('USER_CONNECTED', function (data) {
    console.log("Hello " + data.name);
   });

socket.emit('DRAW_CARDS', {"number": "2", "type":"word"});

socket.on('SEND_CARDS',function(data){

	data.forEach(function(f){
		console.log(f);
	});

});

socket.emit('PLAY_CARD', {id: "1"});
console.log("emitted play card");
socket.on('CHECK_CARD',function(data){
	console.log(data);
});
console.log("emitted check card");
