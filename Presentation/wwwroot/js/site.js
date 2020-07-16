// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function RESTCALL(url, datas, method, contenttype, process, succ_callback, fail_callback, token, enctype) {
    $.ajax({
        xhr: function () {
            enablespiner();
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener("progress", function (evt) {
                $('.progress').show();
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total;
                    percentComplete = parseInt(percentComplete * 100);
                    $('.progress-bar').css("width", percentComplete + '%')
                    $('.progress-bar').text(percentComplete + '%');
                    if (percentComplete === 100) {
                        $('.progress').hide();
                        $('.progress-bar').css("width", '0%')
                    }
                }
            }, false);

            return xhr;
        },
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

$('.Photo').change(function (e) {
    const file = e.target.files[0]
    if (file != undefined) {
        const reader = new FileReader();
        var src = "";
        reader.onload = e => {
            src = e.target.result;
            $(".photo_disp img").prop("src", src);
        };
        reader.readAsDataURL(file);
        $('.browsebutton')[0].innerText = "Selected";
        $('.photo_disp').show();
    }
})
var file = null;
var selectedFile = [];
$('.File').change(function (e) {
    file = e.target.files
    if (file != undefined) {
        for (var i = 0; i < file.length; i++) {
            selectedFile.push(file[i]);
            $('#file_selected').append("<span class='text-info'>&nbsp;" + file[i].name + "&nbsp; <input type='button' class='btn btn-sm btn-danger' onclick='removefile(event);' name='" + file[i].name + "' value='Delete'></span>");
        }
        $("input[type=file]").replaceWith($("input[type=file]").val('').clone(true));
    }
})
function removefile(e) {
    var val = e.target.name.trim();
    if (selectedFile != null) {
        for (var i = 0; i < selectedFile.length; i++) {
            if (selectedFile[i].name == val) {
                selectedFile.splice(i, 1);
                break;
            }
        }
        e.currentTarget.parentNode.remove();
    }
};

$('.clear').click(function () {
    $("input[type=text]").val("");
})

var d = new Date();
function ConvertDatetoString(d) {
    var month = d.getMonth() + 1;
    var day = d.getDate();
    month = month < 10 ? +"0" + month.toString() : month;
    day = day < 10 ? +"0" + day.toString() : day;
    var datestring = d.getFullYear() + "-" + month + "-" + day;
    return datestring;
}

var datestring = ConvertDatetoString(d);
$('#CreationEndDate').prop("max", datestring);
$('#CreationStartDate').prop("max", datestring);
if ($('#CreationEndDate').val() == "" || $('#CreationEndDate').val() == undefined) {
    $('#CreationEndDate').val(datestring);   
}
if ($('#DueDate').val() != undefined || $('#DueDate').val() != '')
    $('.DueDatelabel').text($('#DueDate').val());

$('#CreationEndDate').change(function () {
    $('#CreationStartDate').prop("max", $('#EndDate').val());
})

$('input[type="reset"]').click(function (e) {
    $('.wofilter :input').each(function (x) {
        var node = $('.wofilter :input')[x];
        if (node.type == "text" || node.type == "date") {
            node.setAttribute("value", "");
        }
    })
    $('.mwofilter :input').each(function (x) {
        var node = $('.mwofilter :input')[x];
        if (node.type == "text" || node.type == "date") {
            node.setAttribute("value", "");
        }
    })
    
    $("input[type=file]").replaceWith($("input[type=file]").val('').clone(true));
    $("photo-display").prop("src", "");
    $("#DueDate").prop("value", "");
    selectedFile = [];
    $("#file_selected").load(location.href + " #file_selected>*", "");
    $("input[type=checkbox]").prop("checked", false);
    $('.photo_disp').hide();
    $('#Status').val("");
    if ($('.browsebutton')[0] != undefined)
        $('.browsebutton')[0].innerText = "Browse";
    $('#Priority').val("");
    $('.primary_span').text("");
    $('input[type="submit"]').click();
   
})

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
        val = val.split(',')
        arr = val;
    }
    else
        arr.push(val);

    $('input[name$="SelectedProperty"]').each(function () {
        if (arr.indexOf($(this).val()) != -1) {
            $(this).prop("checked", true);
        }
    });
    $('input[name$="PrimaryProperty"]').each(function () {
        if ($(".primary_span").text().trim() == $(this).val()) {
            $(this).prop("checked", true);
        }
    });
}


var msg = null
function enablespiner() {
    msg = alertify.message("<div class='progress' style='margin-bottom:10px;display:none'> <div class='progress-bar' style='width:0%' role='progressbar' aria-valuemin='0' aria-valuemax='100'></div></div>" +
        "<div style='display:-webkit-box'>Processing &nbsp;<i style = 'display:block;' class= 'fas fa-circle-notch fa-2x fa-spin' ></i></div>", 0);
}
function disablespinner() {
    msg.dismiss();
}

$('#adduser, #wocreate, #addprop').submit(function (e) {
    e.preventDefault();
    var url = $(this).attr('action');
    var formData = new FormData(this);
    for (var i = 0; i < selectedFile.length; i++) {
        formData.append('File', selectedFile[i]);
    };
   
    if ($(this).valid()) {
        $("form :input").prop("disabled", true);
        RESTCALL(url, formData, 'POST', false, false, function (res) {
            disablespinner();
            $("form :input").prop("disabled", false);
            alertify.alert('Info', '<p>' + res + '</p>', function () {
            $("input[type=reset]").click();
            });
        }, function (res) {
            disablespinner();
            $("input[type=password]").val("");
            $("form :input").prop("disabled", false);
            alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
            });
        }, "");
    }
});

$('#edituser, #editwo').submit(function (e) {
    e.preventDefault();
    var url = $(this).attr('action');
    var formData = new FormData(this);
    //for multiple files upload
    for (var i = 0; i < selectedFile.length; i++) {
        formData.append('File', selectedFile[i]);
    };
    if ($(this).valid()) {
        $("form :input").prop("disabled", true);
        RESTCALL(url, formData, 'POST', false, false, function (res) {
            disablespinner();
            $("form :input").prop("disabled", false);
            alertify.alert('Info', '<p>' + res + '</p>', function () {
                location.reload();
            });
        }, function (res) {
            disablespinner();
            $("input[type=password]").val("");
            $("form :input").prop("disabled", false);
            alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
                location.reload();
            });
        }, "");
    }
});

$('#Role').change(function () {
    if ($(this).children("option:selected").val() == "Master Admin") {
        $('.mainprop').prop("hidden", true);
    }
    else
        $('.mainprop').prop("hidden", false);
});

/*index*/
/* regarding mobile view*/
$("#DueDate").change(function () {
    $('.DueDatelabel').text($(this).val());
    $('.mform').submit();
})
$('.datedec').click(function () {
    var val = $("#DueDate").val();
    if (val == "NaN-NaN-NaN" || val == undefined || val == '')
        val = new Date();
    else
        val = new Date(val);
    val.setDate(val.getDate() - 1);
    var datestring = ConvertDatetoString(val);
    $("#DueDate").val(datestring);
    $('.DueDatelabel').text(datestring);
    $('.mform').submit();
});
$(".DueDateMobile").datepicker();
$('.dateinc').click(function () {
    var val = $("#DueDate").val();
    if (val == "NaN-NaN-NaN" || val == undefined|| val=='')
        val = new Date();
    else
        val = new Date(val);
    val.setDate(val.getDate() + 1);
    var datestring = ConvertDatetoString(val);
    $("#DueDate").val(datestring);
    $('.DueDatelabel').text(datestring);
    $('.mform').submit();
});

$('.filter_check').click(function () {
    if ($(this).prop("checked") == true) {
        $('.wofilter').show();
        $('.mwofilter').show();
    }
});

$('.closefilter').click(function () {
    $('.filter_check').prop("checked", false);
    $('.wofilter').hide(100);
    $('.mwofilter').hide(100);
});
$('#Export').change(function () {
    $('input[type="submit"]').click();
})

/*mobile workorder detail*/
$('#Cancel').click(function () {
    $('.pop-up').hide(100);
});
$('.edit-link').click(function () {
    $('.pop-up').show(100);
});

$(window).on('beforeunload', function () {
    alertify.message("<div style='display:-webkit-box'>Processing &nbsp;<i style = 'display:block;' class= 'fas fa-circle-notch fa-2x fa-spin' ></i></div>", 0);
});

$('#LocationId').change(function () {
    $.get("/Property/GetSubLocation?id=" + $(this).val(),
        function (data) {
            $("#SubLocationId").html("<option value=''>Please select Sublocation</option>")
            if (data != null || data != undefined) {
                for (var i = 0; i < data.length; i++) {
                    $("#SubLocationId").append('<option value=' + data[i].id + '>' + data[i].propertyName + '</option>')
                }
            }
        });
});
$('#PropertyId').change(function () {
    $.get("GetLocation?id=" + $(this).val(),
        function (data) {
            $("#LocationId").html("<option value=''>Please choose Location</option>")
            $("#SubLocationId").html("<option value=''>Please choose SubLocation</option>")
            if (data != null || data != undefined) {
                for (var i = 0; i < data.length; i++) {
                    $("#LocationId").append('<option value=' + data[i].id + '>' + data[i].propertyName + '</option>')
                }
            }
        });
});
$('#Category').change(function () {
    $.get("/WorkOrder/GetDataByCategory?category=" + $(this).val(),
        function (data) {
            console.log(data);
            if (data != null || data != undefined) {
                $("#OptionId").html("<option value=''>Please Choose Option</option>")
                for (var prop in data) {
                    var result = ""
                    if (prop != "")
                        result += "<optgroup label='" + prop + "'>";
                    
                    for (var i = 0; i < data[prop].length; i++) {
                        result += '<option value=' + data[prop][i].id + '>' + data[prop][i].propertyName + '</option>';
                        
                    }
                    if (prop != "")
                        result += "</optgroup>";
                    $("#OptionId").append(result);
                }
               
            }
        });
    if ($(this).val() == "department" || $(this).val() == "user")
        $("#OptionId").prop("required", true).prop("hidden", false);
    else
        $("#OptionId").removeProp("required").prop("hidden", true);

});

$('#history_button').click(function (e) {
    e.preventDefault();
    enablespiner();
    var value = $(this).data("custom-value");
    $.get('/WorkOrder/GetHistory?entity=workorder&rowId='+value , function (res) {
        $('#history').html(res);
        disablespinner();
    })
    //disablespinner();
    
})