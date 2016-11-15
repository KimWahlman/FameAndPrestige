var cards = {};
var pC = require('./pointCounter.js');

cards["0"]= {
    name: "0",
    description: "1",
    theme : ["horror", "history"],
    point: "1",
    type: "mixed"
}

cards["1"]= {
	name: "1",
	description: "1",
    theme : ["horror", "nature"],
    point: "1",
	type: "mixed"
}
cards["2"]= {
	name: "2",
	description: "2",
    theme : ["horror"],
    point: "1",
	type: "mixed"
}
cards["3"]= {
	name: "3",
	description: "3",
    theme : ["nature", "history"],
    point: "1",
	type: "mixed"
}
cards["4"]= {
	name: "4",
	description: "4",
    theme : ["horror"],
    point: "2",
	type: "pure"
}
cards["5"]= {
	name: "5",
	description: "5",    
    theme : ["nature"],
    point: "2",
	type: "pure"
}
cards["6"]= {
    name: "5",
    description: "5",    
    theme : ["folklore"],
    point: "2",
    type: "pure"
}



var c = [cards["3"],cards["1"],cards["3"],cards["1"]];

if(pc.checkLinks(c))
    console.log(pc.countPoints(c, "horror"))
