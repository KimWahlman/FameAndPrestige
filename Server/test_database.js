
// If you use require('nosql'):
var DB = require('nosql');
var deck = DB.load('deck.nosql');
var jsonfile = require('jsonfile');
var crypto = require('crypto');
var CryptoJS = require("crypto-js");
var moment = require('moment');


	
//console.log("ciao bello");
var counter;

function insertCard(name, type, firstTheme,secondTheme, value, description, img){
	
		card = {}
		key = 'fame&prestige123';
		// Encrypt 
		var now = moment()
		message = now.toString();
		console.log(message);
		var ciphertext = CryptoJS.AES.encrypt(message, key);
		var key = ciphertext.toString().slice(6,15);
		console.log(key);
		
		card.id = key
		card.name = name;
		card.type = type;
		card.theme = [2];
		card.theme.push(firstTheme, secondTheme);
		card.point = value
		card.description = description;
		card.img = img;
		deck.insert(card);
		return key;
}
 
//insertCard("card1","mixed","horror","nature",2,"description1","urlpicture");

deck.find().make(function(builder) {
	builder.callback(function(err, response, count){
		console.log(response);
	});
});





 




/*
deck.find().make(function(builder) {
    builder.callback(function(err, response, count){
		insertCard(count,"mixed","horror",2,"description1","urlpicture");
	});
});
*/




 


/*



nosql.find().make(function(filter, fun) {
    filter.where("id","=","card0");
    filter.callback(fun);
});

*/
//console.log(nosql.find.filter.where('name','=','rosario'));
