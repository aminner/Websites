$(function () {
    $(".clearRating").click(function () {
        var $td = $(this).closest("td");
        var movieToClearRating = { movieId: $(this).closest(".movieRating").data('movieId') };
        $.ajax({
            url: '/Movie/ClearRating',
            type: 'post',
            data: movieToClearRating
        }).success(function (data) {
            $td.empty().append(data);
        });
    });
    $(".star").click(function () {
        var $td = $(this).closest("td");
        var rating = $(this).index() + 1;
        var movieToRate = {
            movieId: $(this).closest(".movieRating").data('movieId'),
            rating: rating
        };
        $.ajax({
            url: '/Movie/AddRating',
            type: 'post',
            data: movieToRate
        }).success(function (data) {
            $td.empty().append(data);
        });
    });
    $(".star").hover(function () {
        $(this).prevAll(".star").andSelf().addClass("hovered");

    }, function () {
        $(this).siblings(".star").andSelf().removeClass("hovered");
    });
})