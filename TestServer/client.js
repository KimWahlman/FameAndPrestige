var io_client = require('socket.io-client');
var ipAddress = "localhost";
var port = "3000";

var socket = io_client.connect('http://' + ipAddress + ':' + port);
socket.emit('USER_CONNECT');
socket.emit('PLAY',{"name" : "NODE"});
socket.on('USER_CONNECTED', function (data) {
    console.log("Hello " + data.name);
   });
/*
socket.emit('DRAW_CARDS', {"number": "2", "type":"word"});

socket.on('SEND_CARDS',function(data){

	data.cards.forEach(function(f){
		console.log(f);
	});

});

socket.emit('PLAY_CARD', {id: "1"});
socket.on('CHECK_CARD',function(data){
	console.log(data);
});
*/

var rooms;
var room_name;
socket.emit("LIST_ROOMS");
socket.on("ROOMS", (data)=>{
	console.log(JSON.stringify(data));
	rooms = JSON.parse(JSON.stringify(data.rooms));

	if(typeof rooms === 'undefined' || rooms.length == 0){
		console.log("SENDED_CREATE ROOM");
		room_name = "pippo";
		socket.emit("CREATE_ROOM",{name: room_name});
	}else{
		console.log("SENDED JOIN ROOM");
		room_name = rooms[0];
		socket.emit("JOIN_ROOM", {name: room_name})
		socket.emit("HI_ROOM");
	}

});

socket.on("HI",function(){
	console.log("HI");
});
//socket.emit("LIST ROOMS");
