var app = require('express')();
var server = require('http').createServer(app);
var io = require('socket.io')(server);

var clients = [];
var cards = {};

cards["1"]= {
	name: "1",
	description: "1",
	type: "word"
}
cards["2"]= {
	name: "2",
	description: "2",
	type: "word"
}
cards["3"]= {
	name: "3",
	description: "3",
	type: "word"
}
cards["4"]= {
	name: "4",
	description: "4",
	type: "action"
}
cards["5"]= {
	name: "5",
	description: "5",
	type: "event"
}

var available_cards = JSON.parse(JSON.stringify(cards));
var discard_cars = {};

io.on('connection', function(socket){ 

	var currentUser;

	socket.on("USER_CONNECT", function(){
		console.log("-----On USER_CONNECT-----");
		console.log("User Connected ");
		console.log(socket.client.id);
	});

  	socket.on('PLAY', function (data) {
  		console.log("-----On PLAY-----");
  		currentUser = {
  			name: data.name
  		};
  		clients.push(currentUser)
    	console.log("his name is " + currentUser.name);
    	socket.emit("USER_CONNECTED", currentUser);
    	socket.broadcast.emit("USER_CONNECTED", currentUser);

    	socket.emit("ROLL_DICE",{roll: Math.floor(Math.random() * 20)+""})
  	});

  	/*
	socket.on('disconnect', function(){
		console.log("-----On USER_DISCONNECTED-----");
		socket.broadcast.emit("USER_DISCONNECTED", currentUser);
		for (var i = 0; i < clients.length; i++) {
			if(clients[i].name === currentUser.name){
				console.log("User " + currentUser.name +  " disconnect");
				clients.splice(i,1);
			}
		}
    });
	*/
	
    socket.on("DRAW_CARDS", function(data){
    	console.log("-----On DRAW_CARDS-----");
    	console.log(data);
    	card_number = Number(data.number);
    	var send_card = {cards:[]};
    	for (var i = 0; i < card_number; i++) {
    		keys = Object.keys(available_cards);
    		do 
    			rand = keys[Math.floor(Math.random() * keys.length)];
    		while(available_cards[rand].type !== data.type)

    		console.log("Sending");
    		console.log(available_cards[rand])
    		send_card.cards.push(available_cards[rand]);
    		delete available_cards[rand];
    	}
    	console.log("--------SEND_CARDS-------");
    	console.log("Sending");
    	console.log(JSON.stringify(send_card));
    	socket.emit("SEND_CARDS", send_card);
    	console.log("--------SEND_CARDS_DONE-------");
    });


    socket.on("PLAY_CARD", function(data){
    	console.log("-----On PLAY_CARDS-----");
    	console.log("play card event");
    	card_id = data.id;
    	if(Math.random()>= 0.5){
    		console.log("--------CHECK_CARD-------");
    		socket.emit("CHECK_CARD",{check: "true"});
    	}
    	else{
    		console.log("--------CHECK_CARD-------");
    		socket.emit("CHECK_CARD",{check: "false"});
    	}


    });


});
server.listen(3000, function(){
	console.log('listening on *:3000');
});