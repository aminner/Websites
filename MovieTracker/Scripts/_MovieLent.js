$(function () {
    $('#datepicker').datepicker({ onSelect: function () { validateLentOutInput(); } })
});

$(function () {
    $('#lentToInput').keyup(function(e)
    {
        var key = e.keyCode;
        if ((key < 65 || key > 90) && (key < 48 || key > 57) && (key != 222 && key != 32 && key != 8 && key != 16))
            return false;
        validateLentOutInput();
    })
})
$(function () {
    $("#lentOut").attr("disabled", "disabled");
    var lentOut = $('#lentToInput').val() != "";
    if (lentOut) {
        $('#lentToInput').attr("disabled", "disabled");
        $('#datepicker').datepicker('disable'); 
        $('#lentOut').hide();
    }
    else {
        $('#lentToInput').removeAttr("disabled");
        $('#datepicker').datepicker("enable");
        $('#lentOut').show();
    }
});
function MovieReturned(sentObject) {
    var movieId = {
        movieId: $(sentObject).data("movieId"),
        reloadLoc: $(sentObject).data("reloadLoc")
    };
    debugger;
    $.ajax({url: '/Movie/Returned',
            type: 'post',
            data: movieId
}).success(function (data) {
    debugger;
    var dataLoadTo;
    if (movieId.reloadLoc.indexOf('Detail') > 0)
        dataLoadTo = $('.movieLentStatus');
    else {
        dataLoadTo = $("#lentOutDetails" + movieId.movieId);
        $('#lentOutStatus' + movieId.movieId).attr("src", "../../Content/available.png");
    }
    dataLoadTo.empty();
    dataLoadTo.append(data);
})
}
   
function MovieLentOut() {
    var borrower = $('#lentToInput').val();
    var borrowedDate = $('#datepicker').datepicker({ dateFormat: 'yy-mm-dd' }).val();
    var passData = {
        movieId: $('#lentOut').data("movieId"),
        borrower: borrower,
        borrowedDate: borrowedDate
    };
    debugger;
    $.ajax({url: '/Movie/LentOut',
        type: 'post',
        data: passData
    }).success(function (data) {
        debugger;
        $(".movieLentStatus").empty();
        $(".movieLentStatus").append(data);
    })
}
function validateLentOutInput()
{
    var validBorrowerName = validateBorrower($('#lentToInput').val());
    var validBorrowedDate = $("#datepicker").datepicker("getDate") != null;
    if (!validBorrowerName)
        $('#lentToValidation').text("Input must be alphanumeric.");
    else
        $('#lentToValidation').text("");

    if (!validBorrowedDate)
        $('#lentOnValidation').text("Please enter the lent out date.");
    else
        $('#lentOnValidation').text("");

    if (!validBorrowerName || !validBorrowedDate)
        $('#lentOut').attr("disabled", "disabled");
    else
        $('#lentOut').removeAttr("disabled");
}
function validateBorrower(borrowerName)
{
    var regexMatch = /^[\w\-\s]+$/.test(borrowerName);
    return (borrowerName.trim() != "" && /^[\w\-\s]+$/.test(borrowerName));
}