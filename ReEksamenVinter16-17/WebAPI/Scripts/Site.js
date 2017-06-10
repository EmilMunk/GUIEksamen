$(function () {

    $(".dropdown-menu li a").click(function () {

        $(".btn:first-child").text($(this).text());
        $(".btn:first-child").val($(this).text());
        $(".form-control:first.child").text($(this).text());

    });

});

$(function ()
{
    $(".dropdown-menu li a").click(function () {
        $('#datebox').val($(this).text());
    });
})

