// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function RESTCALL(url, datas, method, contenttype, process, succ_callback, fail_callback, token, enctype) {
    $.ajax({
        url: url,
        type: method,
        cache: true,
        data: datas,
        processData: process,
        enctype: enctype,
        contentType: contenttype,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
            if (token != "" || token != undefined)
                xhr.setRequestHeader("Authorization", "Bearer " + token);
        },
        success: succ_callback,
        error: fail_callback
    });
}

$('.select-mul').change(function (e) {
    $('.select-input').val($(this).val());
});
$('#Role').on("change load", function (e) {
    if ($(this).val() == "Admin")
        $('.mainprop').hide();
    else
        $('.mainprop').show();
})

$('.Photo').change(function (e) {
    const file = e.target.files[0]
    if (file != undefined) {
        const reader = new FileReader();
        var src = "";
        reader.onload = e => {
            src = e.target.result;
            console.log(src);
            $(".photo_disp img").prop("src", src);
        };
        reader.readAsDataURL(file);
        $('.browsebutton')[0].innerText = "Selected";
        $('.photo_disp').show();
    }
})
$('.clear').click(function () {
    $("input[type=text]").val("");
})

$("[name='Filter']").change(function () {
    var d = new Date();
    var month = d.getMonth() + 1;
    var day = d.getDate();
    month = month < 10 ? +"0" + month.toString() : month;
    day = day < 10 ? +"0" + day.toString() : day;
    var datestring = d.getFullYear() + "-" + month + "-" + day;
    $('#search').prop("type", 'text');
    $('#search').val("");
    $('#search').show();
    $('#endDate').hide();
    $('#select').hide();
    console.log($(this).val());
    if ($(this).val() == 'ByDate') {
        $('#search').prop("type", 'date');
        $('#endDate').prop('max', datestring)
        $('#search').prop('max', datestring)
        $('#endDate').prop('disabled', true)
        $('#endDate').show()
    }
    if ($(this).val() == 'ByStatus') {
        $('#select').show();
        $('#search').hide();
    }
});
$("#search").blur(function () {
    $('#endDate').prop('disabled', false)
    $('#endDate').prop('min', $(this).val())
});
$('.clear').click(function (e) {
    e.preventDefault();
    $('#search').val("");
    $('#endDate').val("");
})

$('#wocreate').submit(function (e) {
    e.preventDefault();
    var url = $(this).attr('action');
    var form = $(this).serialize();
    var formData = new FormData(this);
    if ($(this).valid()) {
        $('.fa-spinner').prop("hidden", false);
        RESTCALL(url, formData, 'POST', false, false, function (res) {
            alertify.alert('Info', '<p>' + res + '</p>', function () {
                $('.fa-spinner').prop("hidden", true);
                $("input[type=password]").val("");
                $("input[type=text]").val("");
                $("input[type=email]").val("");
                $('select').prop('selectedIndex', 0);
            });

        }, function (res) {
            $('.fa-spinner').prop("hidden", true);
            alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
                $("input[type=password]").val("");

            });
        }, "");
    }
});

