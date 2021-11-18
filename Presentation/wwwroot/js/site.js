// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



function RESTCALL(url, datas, method, contenttype, process, succ_callback, fail_callback, token, enctype) {
    $.ajax({
        xhr: function () {
            enablespiner();
            var xhr  = new window.XMLHttpRequest();
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

$(document).ready(function () {
    $('.select-mul').change(function (e) {
        $('.select-input').val($(this).val());
    });
});

$(document).ready(function () {
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
});
var file = null;
var selectedFile = [];
var imgDisphtml = "";
var parent = "<div class='customdisp attach'>";
$(document).ready(function () {
    $('.File').change(function (e) {
       
        file = e.target.files
        if (file != undefined) {
            for (var i = 0; i < file.length; i++) {
                //compress file before pushing (image particularly)
                const options = {
                    maxSizeMB: 1.6,          // (default: Number.POSITIVE_INFINITY)
                    maxWidthOrHeight: 720,   // compressedFile will scale down by ratio to a point that width or height is smaller than maxWidthOrHeight (default: undefined)
                    onProgress: Function,       // optional, a function takes one progress argument (percentage from 0 to 100) 
                    useWebWorker: true,
                }
                if (file[i].size / 1024 > 3000 && file[i].type.indexOf('image') != -1) {
                    enablespiner("Compressing")
                    imageCompression(file[i], options).then(function (res) {
                        var file = new File([res], res.name);
                        selectedFile.push(file);
                        disablespinner();
                    });

                } else {
                    selectedFile.push(file[i]);
                }
                imgDisphtml += "<div class='photo_disp' style='display:inline-table;margin:5px;'><span class='OpenImageDisplayModal' style='word-break: break-word;'>";
                if (file[i].type.indexOf('image') != -1) {
                    imgDisphtml += "<img class='zoom-img' data-original='" + URL.createObjectURL(event.target.files[i]) + "' height='150' src='" + URL.createObjectURL(event.target.files[i]) + "' width='200'>";
                }
                imgDisphtml += file[i].name + "</span> <input class='btn btn-sm btn-danger mt-1' onclick='removefile(event);' type = 'button' value = 'Delete'></div>";

            }
            if (parent != "") {
                $('#file_selected').append(parent + imgDisphtml);
                $('#file_selected').append("</div><p class='text-danger'>Photo uploads are not added until work order is saved.</p>");
        }
            else {
             $('.attach').append(imgDisphtml);
            
            }
            bindZoomImg();
            parent = "";
            imgDisphtml = "";
            $("input[type=file]").replaceWith($("input[type=file]").val('').clone(true));
        }
    })
});
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
$(document).ready(function () {
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
});
//assign primery property if has value



//setting primary property
$(function () {
    //--
    //--set 
    var prop = $('.select-input ').val().split(',');
    $('input:checkbox[name=SelectedProperty]').each(function (key) {
        if (prop.indexOf($('input:checkbox[name=SelectedProperty]')[key].value)!=-1) {
            $('input:checkbox[name=SelectedProperty]')[key].checked= true;
        }
    });
    $('input:radio[name=PrimaryProperty]').val([$('.primary_span')[0].innerText]);
    $('input:radio[name=PrimaryProperty]').trigger('change');
    
   
});
$(document).ready(function () {
    $('input[name$="PrimaryProperty"]').on('change', function () {
        var index = arr.lastIndexOf($(this).val());
        if (index == -1) {
            arr.push($(this).val());
            $(this).closest("div").find('input[type="checkbox"]').prop("checked", true);
            $('.select-input').val(arr);
        }
        $('.primary_span').text($(this).val());
    });
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
function enablespiner(text = "Processing") {
    if ($('.progress').length == 0) {
        msg = alertify.message("<div class='progress' style='margin-bottom:10px;display:none'><div class='progress-bar' style='width:0%' role='progressbar' aria-valuemin='0' aria-valuemax='100'></div></div>" +
            "<div style='display:-webkit-box'>" + text + "&nbsp;<i style = 'display:block;' class= 'fas fa-circle-notch fa-2x fa-spin' ></i></div>", 0);
    }
}
function disablespinner() {
    if ($('.progress').length != 0)
        msg.dismiss();

}
$(document).ready(function () {
    $('#adduser, #wocreate, #addprop, #wocreaterecurring').submit(function (e) {
        e.preventDefault();
        if ($(this).is('#wocreaterecurring')) {
            GenarateCron();
        }
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
                var data = res.responseText.split('@');
                    if (res.status == 302) {
                        alertify.alert('Success', '<p>' + data[1] + '</p>', function () {
                            window.location.href = data[0];
                        });
                    }
                    else {
                        alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
                            
                        });
                    }


            }, "");
        }
    });
});

$(function () {
    
    $("#commentpost, #commentpost1 ").submit(function (e) {
       
            e.preventDefault();
            var url = $(this).attr('action');
            var parentNode = $(this).attr('parent');
            var formData = new FormData(this);
            if ($(this).valid()) {
                $("form :input").prop("disabled", true);
                RESTCALL(url, formData, 'POST', false, false, function (res) {
                    disablespinner();
                    //if success then response contains html
                    if (res.indexOf('div') != -1) {
                        //append this in parent
                        $(parentNode).html(res);
                        bindButtonClick();
                        $('#close').click();

                        $("form :input").prop("disabled", false);
                        if ($('#commentpost').length!=0)
                            document.getElementById("commentpost").reset();
                        if ($('#commentpost1').length!=0)
                        document.getElementById("commentpost1").reset();
                        alertify.alert('Info', '<p>Updated Successfully</p>', function () {

                        });
                    }
                }, function (res) {
                    disablespinner();
                    $("form :input").prop("disabled", false);
                    alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {

                    });
                });

        }
        e.stopPropagation();
        e.stopImmediatePropagation();
        });
    });


$(document).ready(function () {
    $('#edituser, #editwo, #editworecurring').submit(function (e) {
        e.preventDefault();
        if ($(this).is('#editworecurring')) {
            GenarateCron();
        }
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
                    
                });
            }, function (res) {
                disablespinner();
                $("input[type=password]").val("");
                    $("form :input").prop("disabled", false);
                    var data = res.responseText.split('@');
                    if (res.status == 302) {
                        alertify.alert('Success', '<p>' + data[1] + '</p>', function () {
                            window.location.href = data[0];
                        });
                    }
                    else {
                        alertify.alert('Error', '<p>' + res.responseText + '</p>', function () {
                            location.reload();
                        });
                    }
            }, "");
        }
    });
});

$(document).ready(function () {
    $('#Role').change(function () {
        if ($(this).children("option:selected").val() == "Master Admin") {
            $('.mainprop').prop("hidden", true);
        }
        else
            $('.mainprop').prop("hidden", false);
    });
});

$(document).ready(function () {
$('#FilterActive').on('change', function (e) {
    console.log('called');
    console.log(e);
    if ($(this).prop("checked") == true) {
        $('.wofilter').show();
        $('.mwofilter').show();
    }
});

$('.closefilter').click(function () {

    $('#FilterActive').prop("checked", false);
    $('.wofilter').hide(100);
    $('.mwofilter').hide(100);
});
});

$(document).ready(function () {
    $('#Export').change(function () {
        $('input[type="submit"]').click();
    })
});

/*mobile workorder detail*/
$('#Cancel').click(function () {
    $('.pop-up').hide(100);
});
$('.edit-link').click(function () {
    $('.pop-up').show(100);
});

$(window).on('beforeunload', function () {
    if (!isdownload)
        alertify.message("<div style='display:-webkit-box'>Processing &nbsp;<i style = 'display:block;' class= 'fas fa-circle-notch fa-2x fa-spin' ></i></div>", 0);
    else
        isdownload = false;
});
$(window).on('unload', function () {

    this.disablespinner();
});

$(document).ready(function () {
    $('#LocationId').change(function () {
        enablespiner("Loading...");
        $.get("/Property/GetSubLocation?id=" + $(this).val(),
            function (data) {
               
                $("#SubLocationId").html("<option value=''>Select Sublocation</option>")
                if (data != null || data != undefined) {
                    for (var i = 0; i < data.length; i++) {
                        $("#SubLocationId").append('<option value=' + data[i].id + '>' + data[i].propertyName + '</option>')
                    }
                }
            });
        
        //item
        $.get("/Property/GetAsset?locId=" + $(this).val(),
            function (data) {
                disablespinner();
                $("#ItemId").html("<option value=''>Select Item</option>")
                if (data != null || data != undefined) {
                    for (var i = 0; i < data.length; i++) {
                        $("#ItemId").append('<option value=' + data[i].id + '>' + data[i].propertyName + '</option>')
                    }
                }
            });
    });
});
$(document).ready(function () {
    $('#ItemId').change(function () {
        enablespiner("Loading...");
        $.get("/WorkOrder/GetIssue?id=" + $(this).val(),
            function (data) {
                disablespinner();
                $("#IssueId").html("<option value=''>Select Issue</option><option value='-1'>Other</option>")
                if (data != null || data != undefined) {
                    for (var i = 0; i < data.length; i++) {
                        $("#IssueId").append('<option value=' + data[i].id + '>' + data[i].propertyName + '</option>')
                    }
                }
            });
    });
});
$(document).ready(function () {
    $('#PropertyId').change(function () {
        enablespiner("Loading...");
        $.get("/workorder/GetLocation?id=" + $(this).val(),
            function (data) {
                disablespinner();
                $("#LocationId").html("<option value=''>Select Location</option>")
                $("#SubLocationId").html("<option value=''>Select SubLocation</option>")
                if (data != null || data != undefined) {
                    for (var i = 0; i < data.length; i++) {
                        $("#LocationId").append('<option value=' + data[i].id + '>' + data[i].propertyName + '</option>')
                    }
                }
            });
    });
});
$(document).ready(function () {
    $('#Category').change(function () {
        enablespiner("Loading...");
        var self = $(this).val();
        $.get("/WorkOrder/GetDataByCategory?category=" + $(this).val(),
            function (data) {
                disablespinner();
                if (data != null || data != undefined) {
                    $("#OptionId").html("<option value=''>Please Choose Option</option>")
                    for (var prop in data) {
                        var result = ""
                        if (prop != "")
                            result += "<optgroup label='" + prop + "'>";

                        for (var i = 0; i < data[prop].length; i++) {
                            result += '<option value=' + data[prop][i].id;
                            if (data[prop][i].id == 3 && self == "department")
                                result += ' selected ';
                                result+='>' + data[prop][i].propertyName + '</option>';

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
    if ($('#OptionId').val() == "" || $('#OptionId').val() == undefined)
         $('#Category').trigger('change');

});
$('#history_button').click(function (e) {
    e.preventDefault();
    enablespiner();
    var value = $(this).data("custom-value");
    $.get('/WorkOrder/GetHistory?entity=workorder&rowId='+value , function (res) {
        $('#history').html(res);
        disablespinner();
    })
    
    
})

$(document).ready(function () {
    $('#type-select').change(function () {
        EnableReqInput();
    })
});
$('input[name="sub-radio"]').click(function () {
    EnableReqInput();
})
EnableReqInput();
function EnableReqInput () {
    var value = '#' + $('#type-select').val();
    var nodes = $('#type-select > option').each(function (key) {
        var valuetemp = '#' + $('#type-select > option')[key].value;
        if (value != valuetemp) {
            $(valuetemp).hide();
            $(valuetemp + ' :input[type="number"]').each(function (e) {
                $(valuetemp + ' :input[type="number"]')[e].disabled = true;
            })
            $(valuetemp + ' select').each(function (e) {
                $(valuetemp + ' select')[e].disabled = true;
            })
        }
    });
    if (value == '#Yearly') {
        $(value + ' :input').each(function (e) {
            $(value + ' :input')[e].disabled = false;
        });
    }
    $(value).show();
    
    $('input[name="sub-radio"]').each(function (e) {
        
        if ($('input[name="sub-radio"]')[e].checked) {
            var id = '.' + $('input[name="sub-radio"]')[e].id;
            $(id + ' :input').each(function (key) {
                $(id + ' :input')[key].disabled = false;
            });
            $(id + ' select').each(function (key) {
                $(id + ' select')[key].disabled = false;
            });
        }
        else {
            var id = '.' + $('input[name="sub-radio"]')[e].id;
            $(id + ' :input').each(function (key) {
                $(id + ' :input')[key].disabled = true;
            });
            $(id + ' select').each(function (key) {
                $(id + ' select')[key].disabled = true;
            });
        }
        
    })
    
   
}

$(document).ready(function () {
    $('input[name="end-select"]').change(function () {
        EndInputEnabler();
    });
});
EndInputEnabler();
function EndInputEnabler() {
    $('input[name="end-select"]').each(function (e) {

        if ($('input[name="end-select"]')[e].checked) {
            var id = '.' + $('input[name="end-select"]')[e].id;
            $(id + ' :input').each(function (key) {
                $(id + ' :input')[key].disabled = false;
            });
        }
        else {
            var id = '.' + $('input[name="end-select"]')[e].id;
            $(id + ' :input').each(function (key) {
                $(id + ' :input')[key].disabled = true;
            });
        }

    })
}


function GenarateCron() {
    var cron = '*1 *2 *3 *4 *5';
    cron = cron.replace('*1', '0').replace('*2', '0');
    
    // find day
    //if the value is daily
    if ($('input[name="type-select"]:checked').val() == "Daily" || $('#type-select').val() == "Daily") {

        if ($('input[name="sub-radio"]:checked').val() == "Every") {
            var value = $('#daily-occurence').val() != '' ? '*/'+$('#daily-occurence').val() : '*';
            cron = cron.replace('*3', value);

        }
        else if ($('input[name="sub-radio"]:checked').val() == "Weekdays") {
            cron = cron.replace('*5', '2-6');
        }
    }
    else if ($('input[name="type-select"]:checked').val() == "Weekly" || $('#type-select').val() == "Weekly") {
        var dayofweek = "";
        $.each($("input[name='exampleCheck1']:checked"), function () {
            dayofweek = dayofweek + $(this).val() + ",";
        });
      
        dayofweek = dayofweek.substr(0, dayofweek.length - 1);
        if (dayofweek == "") {
            dayofweek = '1';
        }
        cron = cron.replace('*5', dayofweek);

    }
    else if ($('input[name="type-select"]:checked').val() == "Monthly" || $('#type-select').val()== "Monthly") {
        if ($('input[name="sub-radio"]:checked').val() == "Day") {
            var value = $('#month').val() != '' ? '*/' + $('#month').val() : '*';
            var value1 = $('#day').val() != '' ? $('#day').val() : "*";
            cron = cron.replace('*3', value1).replace('*4', value);
        }
        else if ($('input[name="sub-radio"]:checked').val() == "Month") {
            var capturedday = $('#select-day-option').val();
            if ($('#select-day-option').val() == "L") {
                var months = [1, 3, 5, 7, 8, 10, 12]
                if (months.indexOf($('#input-month').val()) != -1)
                    capturedday = 31;
                else {
                    if ($('#input-month').val() == 2)
                        capturedday = 28;
                    else
                        capturedday = 30;
                }
            }
            var value = capturedday != 'L' && capturedday != '' ? capturedday : '*';
            var value1 = $('#select-week-option').val() != '' ? $('#select-week-option').val() : '*';
            var value2 = $('#input-month').val() != '' ? '*/'+$('#input-month').val() : '*';
            cron = cron.replace('*3', value).replace('*4', value2).replace('*5', value1);
        }

    }
    else if ($('input[name="type-select"]:checked').val() == "Yearly" || $('#type-select').val() == "Yearly") 
        {
           
        if ($('#year-day').val() != '' && $('#year-day').val() != undefined)
            cron = cron.replace('*3', $('#year-day').val())
        if ($('#year-month').val() != '' && $('#year-month').val() != undefined)
            cron = cron.replace('*4', $('#year-month').val())
      
            
    }

    cron = cron.replace('*1', '0').replace('*2', '0').replace('*3', '*').replace('*4', '*').replace('*5', '*');
   
    $('#CronExpression').val(cron);
};

$(document).ready(function () {
    $('#RecurringStartDate').on('change', function () {
        $('#RecurringEndDate').prop('min', $(this).val());
    });
});
var isdownload = false;
$("a[name='exportlink']").click(function () {
    var href = this.href;
    event.preventDefault();

    alertify.confirm(this.innerText, 'Export currently filtered list (all pages)?', function () {
        isdownload = true;
        alertify.closeAll(); window.location = href;
    }
        , function () {
            //window.location = href;
            //isdownload = true;
            //href = href.replace("&IsCurrent=False", "&IsCurrent=true");
        });
});
$(document).ready(function () {
    $('#IssueId').on('change', function () {
        if ($(this).val() == '-1') {
            $('.CustomIssueDiv').show();
            $('.CustomIssueDiv').prop("required", 'true');
        }
        else {
            $('.CustomIssueDiv').hide();
            $('.CustomIssueDiv').removeProp("required");
        }
    });
    $('#IssueId').trigger('change');

});




function AddItem(e) {
    var val = $('#FilesRemoved').val();
    if (val == "" || val == undefined)
        $('#FilesRemoved').val(e.target.name)
    else
        $('#FilesRemoved').val(val + ',' + e.target.name);
    $('.removed').show();
    e.target.remove();

}

$(function () {
    bindButtonClick();
})


function bindButtonClick() {
    $('.reply, #addcomment').on('click', function (e) {
        e.preventDefault();

        var con = $(this).closest('div.mycon');
        var parent = $(this).closest('div.parent');
        if ($('[name="Comment"]').length == value) {
            con.append(
                "<div class='card reply popup top-buffer' style='margin-left:24px'>" +
                "<div class='card-body'>" +
                "<textarea  id='Comment' name='Comment' class='note' required></textarea>" +
                "<input type='text' id='ParentId' name='ParentId' value='" + parent.prop('id') + "'hidden required>" +
                "<input type='text' id='WorkOrderId' name='WorkOrderId' value='" + $('#WorkOrderId').val() + "'hidden required>" +
                "<input type='text' id='RepliedTo' name='RepliedTo' value='" + $(this).siblings('.CommentBy').val() + "'hidden required>" +
                "<button type='submit' class='btn btn-outline-primary btn-sm top-buffer'>Post</button>" +
                "<input type='button' id='close' class='btn btn-outline-danger ml-1 btn-sm top-buffer' value='Close' onclick='closePopUp();;'>" +
                "</div>" +
                "</div>"
            );
            var x = document.getElementById("Comment");
            x.scrollIntoView()
            x.focus();
        }
    });
}





function deleteProp() {
    var value = $('#LocationId').find(":selected").val();
    if (value != "") {
        alertify.confirm('Do you want to delete Location', function () {
            enablespiner("Deleting...");
            $.ajax(
                {
                    type: "POST", //HTTP POST Method
                    url: "/property/DeleteLocation", // Controller/View 
                    data: { //Passing data
                        Id: value, //Reading text box values using Jquery 

                    }, success: function () { disablespinner(); alertify.success('Deleted Successfully'); $('#LocationId').val(""); $('#LocationId option[value=' + value + ']').remove(); $('#SubLocation').val(''); }, error: function (res) { disablespinner(); alertify.error(res.responseText); }

                });
            
        });
    } else {
        alertify.alert('Info', '<p> Please Choose Location </p>')

    }
}
function deleteAsset() {
    var value = $('#AssetId').find(":selected").val();
    if (value != "") {
        alertify.confirm('Do you want to delete Asset', function () {
            enablespiner("Deleting...");
            $.ajax(
                {
                    type: "POST", //HTTP POST Method
                    url: "/property/DeleteAsset", // Controller/View 
                    data: { //Passing data
                        Id: value, //Reading text box values using Jquery 

                    }, success: function () { disablespinner(); alertify.success('Deleted Successfully'); $('#AssetId option[value=' + value + ']').remove(); $('#Issues').val(''); }, error: function (res) {
                        disablespinner(); alertify.error(res.responseText); $('#AssetId').val("");

 }

                });

        });
    } else {
        alertify.alert('Info', '<p> Please Choose Asset </p>')

    }
}

//hide show location

function ShowNewLocation() {
    var value = document.querySelector('#flexCheckDefault').checked;
    if (value) {
        $('#new-location').show();
        $('#available-location').hide();
        $('#SubLocation').val('');
        $('#LocationId').val('');
    }
    else {
        $('#new-location').hide();
        $('#NewLocation').val('')
        $('#available-location').show();
    }
}
function ShowNewAsset() {
    var value = document.querySelector('#flexCheckDefault').checked;
    if (value) {
        $('#new-asset').show();
        $('#available-asset').hide();
        $('#Issues').val('');
        $('#AssetId').val('');
    }
    else {
        $('#new-asset').hide();
        $('#NewAsset').val('')
        $('#available-asset').show();
    }
}





$(document).ready(function () {
    $('#dohiddenreset').click(function () {
        document.getElementById("search").value = "";
        $('#form1').submit();
        $('.form2').submit();
    });
});
$(document).ready(function () {
$('.room').click(function () {
    //get sublocation
   
    $.get('/Home/Sublocation?Id=' + $(this).val(), function (res) {
        $('.subloc').html(res);
       
    });
})
});


$(document).ready(function () {
    $('.toggle').change(function () {
        var falseval = ".false" + $(this).val();
        var trueval = ".true" + $(this).val();
        if ($(this)[0].checked) {
            $(falseval).prop("hidden", false)
            $(trueval).prop("hidden", true)

        }
        else {
            $(trueval).prop("hidden", false)
            $(falseval).prop("hidden", true)
        }
    })
    $('#updatefeat').submit(function (e) {
        e.preventDefault();
        var url = $(this).attr('action');
        var form = $(this).serialize();
        RESTCALL(url, form, 'POST', 'application/x-www-form-urlencoded; charset=UTF-8', true, function (res) {
            alertify.alert('Info', '<p>' + res + '</p>', function () {
                disablespinner();
            });
        }, function (res) {
            $('.features').html(res.responseText);
            alertify.alert('Error', '<p> Some error Occured </p>', function () {
                disablespinner();
            });
        }, "");
    });
});