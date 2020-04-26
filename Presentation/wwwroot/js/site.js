﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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
$('#Role').change(function (e) {
    if ($(this).val() == "Master Admin" || $(this).val()=="User")
        $('.mainprop').hide();
    else
        $('.mainprop').show();
})
/**/
if ($('#Role').val() == "Master Admin" || $('#Role').val() == "User")
    $('.mainprop').hide();
else
    $('.mainprop').show();
/**/

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

var d = new Date();
var month = d.getMonth() + 1;
var day = d.getDate();
month = month < 10 ? +"0" + month.toString() : month;
day = day < 10 ? +"0" + day.toString() : day;
var datestring = d.getFullYear() + "-" + month + "-" + day;
$('#CreationEndDate').prop("max", datestring);
$('#CreationStartDate').prop("max", datestring);
if ($('#CreationEndDate').val() == "" || $('#CreationEndDate').val() == undefined) {
    $('#CreationEndDate').val(datestring);
}
$('#CreationEndDate').change(function () {
    $('#CreationStartDate').prop("max", $('#EndDate').val());
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
                $("input[type=file]").replaceWith($("input[type=file]").val('').clone(true));
                $("input[type=reset]").click();
            });
        }, function (res) {
            $('.fa-spinner').prop("hidden", true);
            alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
                $("input[type=password]").val("");
            });
        }, "");
    }
});
$('input[type="reset"]').click(function (e) {
    $('.wofilter :input').each(function (x) {
        var node = $('.wofilter :input')[x];
        if (node.type == "text" || node.type == "date") {
            node.setAttribute("value", "");
        }
    })
    $("input[type=file]").replaceWith($("input[type=file]").val('').clone(true));
    $("img").prop("src", "");
    $('.photo_disp').hide();
    $('#Status').val("");
    $('.browsebutton')[0].innerText = "Browse";
    $('#Priority').val("");
    $("input[type=file]").replaceWith($("input[type=file]").val('').clone(true));
    $('input[type="submit"]').click();
    $('.primary_span').text("");
})

$('.adduser').submit(function (e) {
    e.preventDefault();
    var url = $(this).attr('action');
    var form = $(this).serialize();
    var formData = new FormData(this);
    if ($(this).valid()) {
        $('.fa-spinner').prop("hidden", false);
        RESTCALL(url, formData, 'POST', false, false, function (res) {
            alertify.alert('Info', '<p>' + res + '</p>', function () {
                $("input[type=reset]").click();
            });

        }, function (res) {
            $('.fa-spinner').prop("hidden", true);
            alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
                $("input[type=password]").val("");
            });

        }, "");
    }
});

$('.edituser').submit(function (e) {
    e.preventDefault();
    var url = $(this).attr('action');
    var form = $(this).serialize();
    var formData = new FormData(this);
    if ($(this).valid()) {
        $('.fa-spinner').prop("hidden", false);
        RESTCALL(url, formData, 'POST', false, false, function (res) {
            $('.fa-spinner').prop("hidden", true);
            alertify.alert('Info', '<p>' + res + '</p>', function () {
                location.reload();
            });
        }, function (res) {
            $('.fa-spinner').prop("hidden", true);
            alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
                $(this).find("input[type=password]").val("");
            });
        }, "", "multipart/form-data");

    }
});

var arr = [];
$('input[name$="SelectedProperty"]').on('change', function () {
    if ($(this).prop("checked") == true) {
        var index = arr.lastIndexOf($(this).val());
        if (index == -1)
            arr.push($(this).val());
    }
    else {
        var index = arr.lastIndexOf($(this).val());
        if ($('input[name$="PrimaryProperty"]:checked').val() == $(this).val()) {
            $('.primary_span').text("");
            $('input[name$="PrimaryProperty"]:checked').prop("checked", false);
        }
        arr.splice(index, 1);
    }
    $('.select-input').val(arr);
})
$('input[name$="PrimaryProperty"]').on('change', function () {
    var index = arr.lastIndexOf($(this).val());
    if (index == -1) {
        arr.push($(this).val());
        $(this).closest("div").find('input[type="checkbox"]').prop("checked", true);
        $('.select-input').val(arr);
    }
    $('.primary_span').text($(this).val());
});
if ($('.select-input').val() != "" && $('.select-input').val() != undefined) {
    var val = $('.select-input').val();
    if (val.indexOf(',') != -1) {
       val=val.split(',')
    }
    arr = val;
    $('input[name$="SelectedProperty"]').each(function () {
        if (arr.indexOf($(this).val() != -1)) {
            $(this).prop("checked", true);
        }
    });
    $('input[name$="PrimaryProperty"]').each(function () {
        if ($(".primary_span").text().trim() == $(this).val()) {
            $(this).prop("checked", true);
        }
    });
}
