$(function () {
    $('#filterList').keyup(function (e)
    {
        var key = e.keyCode;
        if((key < 65 || key >90 ) && (key < 48 ||key > 57 )&& (key!= 222 && key!= 32 && key!= 8 && key!=16))
            return false;
        var filter = {filter: $('#filterList').val().toString()};
        $.ajax( {url: '/Movie/Filter',
            type: 'post',
            data: filter
        }).success(function (data) {
            debugger;
            $(".movieList").find(".movieDetailRow").remove();
            $(".movieList").append(data);
        })
        return false;
    })
})
