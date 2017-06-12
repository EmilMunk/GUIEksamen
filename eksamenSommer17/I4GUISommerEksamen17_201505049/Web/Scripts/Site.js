$('#ToDo').css('background-color', 'Yellow');


$('.navbar-inverse').click(function () {
    var tmp = ["red", "green", "blue", "yellow", "black"];
    var tmpnumber = Math.floor(Math.random() * 5);
    console.log(tmpnumber);
    $(this).css('background-color', tmp[tmpnumber]);
});

$('.PartyButton').click(function() {
    var tmp = ["red", "green", "blue", "yellow", "black"];
        var tmpnumber = Math.floor(Math.random() * 5);
        $('.body-content').css('background-color', tmp[tmpnumber]);
})

