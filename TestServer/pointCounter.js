
module.exports = function checkLinks(cards){
    if(cards.length <= 2){
        return true;
    }

    cards.forEach(function(f){
        if(f.theme.length == 1){
            var index = cards.indexOf(f);
            var tmp = cards[0];
            cards[0] = cards[index];
            cards[index] = tmp;
            return;
        }
    });

     var what = cards.every(function(element, index, arra){
        var array = element.theme;
        var array_base = cards[0].theme;

        var flag =  false;
        for (var i = 0; i < array_base.length; i++) {
            for (var j = 0; j < array.length; j++) {
                if(array_base[i] === array[j])
                    return true;
            }
        }
        return false;

     });

    return what;
}

module.exports = function countPoints(cards, theme){
    var number = cards.length;

    var comboPoint = 0;
    var multi = 0;

    if (number == 1){
        if(cards[0].theme == theme)
            return cards[0].point;
        else
            return 1;
    }

    var normalPoint = cards.reduce(function(a, b) {

    								
    								if(b.theme == theme){
    									point_b = b.point;
    								}else{
    									point_b = 1;
    								}

  									return Number(a) + Number(point_b)
								}, 0);

   /*
   console.log("------------NORMAL POINT---------");
   console.log(normalPoint);
   */

    var mixed_cards = [];
    var pure_cards = []
    cards.forEach(function(element){
    	//console.log(element);
    	if(element.type == 'mixed'){
    		mixed_cards.push(element);
    	}
    	if(element.type == 'pure'){
    		pure_cards.push(element);
    	}
    });

    /*
    console.log("------------MIXED CARDS---------");
    console.log(mixed_cards);
    console.log("------------PURE CARDS---------");
    console.log(pure_cards);
    */

    if(number >= 2){

		var result_mixed = true;
		for (var i = 0; i < mixed_cards.length; i++) {
            for (var j = 1; j < mixed_cards.length; j++) {
                if(mixed_cards[i].theme == mixed_cards[j].theme){
                    mixed_cards.splice(i,1);
                    mixed_cards.splice(j,1);
                    multi ++;
                }
            }
        }

		var result_pure = true;
		pure_cards.forEach(function(f){
			if(pure_cards[0].theme != f.theme){
				result_pure = false;
			}
		});

        /*
		console.log("MIXED " + result_mixed);
		console.log("PURE " + result_pure);
        */

		if(result_mixed && mixed_cards.length != 0)
            comboPoint += 1 * multi;
			/*(mixed_cards.length==2)? comboPoint += 1 : (number==3)? comboPoint += 3 : (number==4)? comboPoint += 5 : comboPoint = 0 */;
		if(result_pure && pure_cards.length != 0)
			(pure_cards.length==2)? comboPoint += 2 : (number==3)? comboPoint += 4 : (number==4)? comboPoint += 6 : comboPoint = 0 ;
    }

    	return {normalPoint: normalPoint, comboPoint: comboPoint}
}