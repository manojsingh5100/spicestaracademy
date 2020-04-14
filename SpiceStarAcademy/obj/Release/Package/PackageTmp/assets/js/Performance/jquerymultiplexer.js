var performanceIndicator = { Needs: 1, Meets: 2, Exceeds: 3, properties: { 1: { name: "Needs Improvements", value: 1, color: "#ff0000" }, 2: { name: "Meets Expectations", value: 2, color: "#ffff00" }, 3: { name: "Exceeds Expectations", value: 3, color: "#008000" } } }; var getRate = function (ind) {
    if (ind == 2)
        return 1; else if (ind == 1)
        return 2; else if (ind == 0)
        return 3; else return 0
}; function clearAddParameterTypeCtrl() { $('#Id').val(0); $('#Name').val(''); $('#IsActive').prop('checked', !0); $('#ddladdstatus').val(0); }
function AddUpadteParameterType() {
    var status = !0;
    if ($('#Id').val() === "0") {
        if ($('#Name').val() == "") { $('#Name').css("border", "1px solid red"); status = !1 } else { $('#Name').css("border", "") }

        if ($('#ddladdstatus option:selected').val() === "0") { $('#ddladdstatus').css("border", "1px solid red"); status = !1 } else { $('#ddladdstatus').css("border", "") }
    } else
        status = true;

    if (status) {
        var model = {}; model.Id = $('#Id').val(); model.Name = $('#Name').val();
        if (model.Id == 0) {
            if ($('#ddladdstatus').val() === "1")
                model.IsActive = true;
            else
                model.IsActive = false;
        }
        else
            model.IsActive = $('#IsActive').prop('checked');
        $.ajax({
            url: "/PerformanceCard/PerformanceParameter/AddUpadteParameterType", type: "POST", async: !1, data: { Model: model }, success: function (response) {
                if (response != null) {
                    Table.draw();
                    //  $('#btnaddparameter').text("Add Parameter");
                    clearAddParameterTypeCtrl();
                    $("#lblMsg").text(response);
                    if ($("#lblMsg").text() != "") {
                        $("#dvMsg").css("display", "block");
                        setTimeout(function () { $("#dvMsg").css("display", "none"); $("#lblMsg").text(''); }, 8000); Table.draw();  // clearAddParameterTypeCtrl()
                    }
                }
            }, error: function (error) { }
        })
    }
}
function editParameterType(id) { $.ajax({ url: "/PerformanceCard/PerformanceParameter/GetParameterTyeInfoById", type: "POST", async: !1, data: { Id: parseInt(id) }, success: function (response) { if (response != null) { $('#Id').val(response.Id); $('#Name').val(response.Name); $('#IsActive').prop('checked', response.IsActive); $('#btnaddparameter').text("Update Parameter") } }, error: function (error) { } }) }

function getKeyCode() { var keyid = $('#tblParameterList tbody tr:last-child td:eq(1)').text(); if (keyid !== "") { var newsubid = parseInt($('#tblParameterList tbody tr:last-child td:eq(1)').text().charAt($('#tblParameterList tbody tr:last-child td:eq(1)').text().length - 1)) + 1; $('#KeyId').val($('#tblParameterList tbody tr td:eq(1)').text().slice(0, -1) + newsubid) } else { $('#KeyId').val("SSA" + $('h5').text().trim().charAt(0).toUpperCase() + "00" + ($('#tblParameterList tbody tr td:eq(1)').length > 1 ? $('#tblParameterList tbody tr').length + 1 : 1)) } }

function gotoaddparameter(id) { window.location.href = "/PerformanceCard/PerformanceParameter/CreateParameter?Id=" + id }
function gotoperformance(regNo) { window.location.href = "/PerformanceCard/Performance?RegNo=" + regNo }
function clearParameterControls() {
    $('#ParameterId').val(0); $('#KeyId').val(''); $('#Name').val(''); $('#IsActive').prop('checked', !0); $('#ddladdstatus').val(0); $('#ddlGender').val(0); $('#ddlreview').val(0); getKeyCode();
}
$.sum = function (arr) { var r = 0; $.each(arr, function (i, v) { r += +v }); return r }; function editParameter(id) { $.ajax({ url: "/PerformanceCard/PerformanceParameter/GetParameterInfoById", type: "POST", async: !1, data: { Id: id }, success: function (response) { if (response != null) { $('#ParameterId').val(response.ParameterId); $('#KeyId').val(response.KeyId); $('#Name').val(response.Name); $('#IsActive').prop('checked', response.IsActive); $('#btnsubparameter').text("Update Sub Parameter") } }, error: function (error) { } }) }
function parameterkeyidexist() {
    var status = !0; var keyid = $('#KeyId').val(); $.ajax({
        url: "/PerformanceCard/Performance/IsParmeterKeyExist", type: "POST", async: !1, data: { KeyId: keyid }, success: function (response) {
            if (response) {
                $('#KeyId').attr('disabled', !1); $("#lblMsg").text('This Key Code already exist please change it!'); if ($("#lblMsg").text() != "") { $("#dvMsg").css("display", "block"); setTimeout(function () { $("#dvMsg").css("display", "none") }, 8000) }
                status = !1
            } else { $('#KeyId').attr('disabled', 'disabled') }
        }, error: function (error) { }
    }); return status
}
function parameterAddUpdate() {
    var status = !0; if ($('#KeyId').val() == "") { $('#KeyId').css("border", "1px solid red"); status = !1 } else { if ($('#tblParameterList tbody tr:last-child td:eq(1)').text() == "" && !parameterkeyidexist()) { $('#KeyId').css("border", "1px solid red"); status = !1 } else { $('#KeyId').css("border", "") } }

    if ($('#ParameterId').val() === "0") {
        if ($('#Name').val() == "") { $('#Name').css("border", "1px solid red"); status = !1 } else { $('#Name').css("border", "") }
        if ($('#ddladdstatus').val() === "0") { $('#ddladdstatus').css("border", "1px solid red"); status = !1 } else { $('#ddladdstatus').css("border", "") }
    }

    if (!status)
        return !1;
    var model = {}; model.ParameterId = $('#ParameterId').val(); model.KeyId = $('#KeyId').val(); model.Name = $('#Name').val();
    if (model.ParameterId == 0) {
        if ($('#ddladdstatus').val() === "1")
            model.IsActive = true;
        else
            model.IsActive = false;
    }
    else
        model.IsActive = $('#IsActive').prop('checked');
    model.tblParameterTypeId = $('#tblParameterTypeId').val();
    model.GenderId = $('#ddlGender').val();
    model.ReviewId = $('#ddlreview').val();
    $.ajax({
        url: "/PerformanceCard/PerformanceParameter/ParameterSaveUpdate", type: "POST", async: !1, data: { Model: model },
        success: function (response) {
            if (response != null) {
                Table.draw();
                clearParameterControls();
                //$('#btnsubparameter').text("Add Sub Parameter");
                $("#lblMsg").text(response.Message); if ($("#lblMsg").text() != "") {
                    $("#dvMsg").css("display", "block"); setTimeout(function () { $("#dvMsg").css("display", "none"); $("#lblMsg").text(''); }, 8000); Table.draw(); getKeyCode(); //clearParameterControls(); $('#btnsubparameter').text("Add Sub Parameter") }
                }
            }
        }, complete: function () { setTimeout(function () { getKeyCode(); }, 1000) }, error: function (error) { }
    });
}
function FillWeeklyTermList(id) {
    if (id == 1) {
        $('#divWeekly').css('display', 'block'); $.ajax({
            url: "/PerformanceCard/Performance/GetWeeklyTermList", type: "POST", async: !1, data: { Id: id }, success: function (response) {
                var str = '<option value="0">Select Weekly Type</option>'; if (response != null && response.length > 0) { $.each(response, function (i, index) { str += '<option value=' + index.Id + '> ' + index.Name + '</option>' }) }
                $('#WeeklyTypeList').empty().append(str); var str1 = $('#WeeklyArr').val(); if (str != "") { $.each(str1.split(','), function (i, item) { $("#WeeklyTypeList option[value=" + item + "]").addClass('dis').attr('disabled', 'disabled') }) }
            }, error: function (error) { }
        })
    } else { $('#divWeekly').css('display', 'none'); $('#WeeklyTypeList').empty() }
}
function initialperformanceindicator() { $('.clperform').each(function () { $(this).text(performanceIndicator.properties[performanceIndicator.Needs].name); $(this).css('color', performanceIndicator.properties[performanceIndicator.Needs].color) }) }
function getResultIndicator(obj) {
    var mySize = performanceIndicator.Needs; if (obj == "Exceeds Expectations")
        mySize = performanceIndicator.Exceeds; else if (obj == "Meets Expectations")
        mySize = performanceIndicator.Meets; return mySize
}
function chkmsg() { if ($("#lblMsg").text() != "") { $("#dvMsg").css("display", "block"); setTimeout(function () { $("#dvMsg").css("display", "none") }, 8000) } }
function getindicator(rate, entity) {
    var data = $.parseJSON($('#hdnpercentageInfo').val());
    var mySize = performanceIndicator.Needs; var percentage = (rate * 100) / (entity * 3); if (percentage >= data[2].FromPercenage) { mySize = performanceIndicator.Exceeds; return mySize }
    else if (percentage >= data[1].FromPercenage) { mySize = performanceIndicator.Meets; return mySize }
    else return mySize
}
function getindicator1(obj) {
    var data = $.parseJSON($('#hdnpercentageInfo').val());
    var s = data[0].FromPercenage;
    var mySize = performanceIndicator.Needs; if (obj >= data[2].FromPercenage) { mySize = performanceIndicator.Exceeds; return mySize }
    else if (obj >= data[1].FromPercenage) { mySize = performanceIndicator.Meets; return mySize }
    else return mySize
}
function overAllPerformanceResult() {
    var arrresult = [];
    var status = !0;
    $('.divstars').each(function () {
        var check = 0;
        $(this).children('input').each(function (index) {
            if ($('#chk_' + $(this).attr('identity')).prop('checked'))
                return false;
            if ($(this).prop('checked')) {
                check = getRate(index);
            }
        });
        if (check == 0) { status = !1; return !1 }
        arrresult.push(check * 100 / 3);
    });
    if (status) {
        var indicator = getindicator1($.sum(arrresult) / arrresult.length);
        $('#lblperformanceResult').text("(" + performanceIndicator.properties[indicator].name + ")");
        $('#lblperformanceResult').css('color', performanceIndicator.properties[indicator].color);
        $('#spnpercentage').html(($.sum(arrresult) / arrresult.length).toFixed(2) + " %");
        $('#spnpercentage').css('color', performanceIndicator.properties[indicator].color);
        $('#spnpercentage1').text(performanceIndicator.properties[indicator].name);
        $('#spnpercentage1').css('color', performanceIndicator.properties[indicator].color);
    }
    else {
        $('#lblperformanceResult').text(''); $('#lblperformanceResult').css('color', 'black');
        $('#spnpercentage').html('');
        $('#spnpercentage1').html('');
    }
}
function ShowCardByReview(regNo, ReviewId, WeekId) { var url = "RegNo=" + regNo + "&Review=" + (ReviewId !== 1 ? ReviewId : (ReviewId + "-" + WeekId)); return window.location.href = '/PerformanceCard/Performance?' + url }

function getratingentity(obj) {
    var clName = $(obj).attr("name").split('-')[0];
    var entity = 0;
    var rate = 0;
    $.each($("." + clName), function () {
        entity = entity + 1;
        $.each($("input[name=" + $(this).val() + "]"), function (index) {
            if ($(this).prop('checked')) {
                rate = rate + getRate(index)
            }
        })
    });
    var indicator = getindicator(rate, entity); $('#lblperformance' + $(obj).attr("identity").split('-')[0]).text(performanceIndicator.properties[indicator].name); $('#lblperformance' + $(obj).attr("identity").split('-')[0]).css('color', performanceIndicator.properties[indicator].color); overAllPerformanceResult()
}

function disableReviewOption() { var str = $('#ReviewArr').val(); if (str != "") { $.each(str.split(','), function (i, item) { $("#ReviewId option[value=" + item + "]").addClass('dis').attr('disabled', 'disabled') }) } }

function performanceCardValidation() {
    var status = !0; var ReviewId = $('#ReviewId').val(); if (ReviewId == undefined || ReviewId == null || ReviewId == "") { $('#ReviewId').css("border", "1px solid red"); status = !1 } else { $('#ReviewId').css("border", ""); if (ReviewId == 1) { var weeklytermid = $('#WeeklyTypeList').val(); if (weeklytermid == undefined || weeklytermid == null || weeklytermid == "" || weeklytermid == 0) { $('#WeeklyTypeList').css("border", "1px solid red"); status = !1 } else { $('#WeeklyTypeList').css("border", "") } } }
    return status
}

function saveCandidatePerformaneParameter(model) {
    model.Percentage = $('#spnpercentage').text();
    model.PerformanceReview = $('#txtReview').val();
    $.ajax({
        url: "/PerformanceCard/Performance/SavePerformanceCardDetail", type: "POST", async: !1, data: { Model: model }, success: function (response) {
            if (response != null) {
                if (response.IsSuccess) { history.back(); /*window.location.href = "/PerformanceCard/Performance/CandidateList"*/ } else {
                    $("#lblMsg").text(response.Massage); if ($("#lblMsg").text() != "") { $("#dvMsg").css("display", "block"); setTimeout(function () { $("#dvMsg").css("display", "none") }, 8000) }
                    $('html, body').animate({ scrollTop: $("#dvMsg").offset().top }, 2000)
                }
            }
        }, error: function (error) { }
    })
}

function savePerformanceCard() {
    if (!performanceCardValidation()) { $('html, body').animate({ scrollTop: $("#dvMsg").offset().top }, 2000); return !1 }
    var model = {}; model.RegNo = $('#RegNo').val(); model.AdmissionId = $('#AdmissionId').val(); model.ReviewId = $('#ReviewId').val(); if ($('#hdnBatchId').val() != "")
        model.BatchId = $('#hdnBatchId').val(); if (model.ReviewId != null && model.ReviewId == 1)
        model.WeeklyTermId = $('#WeeklyTypeList').val(); model.PerformanceMasterId = $('#lblperformanceResult').text().trim() == "" ? 0 : performanceIndicator.properties[getResultIndicator($('#lblperformanceResult').text())].value; var parameterTypeInfoList = [];
    $('.stars').each(function () {
        var parameterTypeInfo = {};
        parameterTypeInfo.PerformanceMasterId = performanceIndicator.properties[getResultIndicator($('#lblperformance' + $(this).attr('ptypeId')).text())].value;
        parameterTypeInfo.ParameterTypeId = $(this).attr('ptypeId');
        var parameterinfolist = [];
            $(this).children('input').each(function (index) {
                var parameterInfo = {};
                var identity = $(this).attr('identity').split('-');
                parameterTypeInfo.PtId = identity[1];
                parameterTypeInfo.IsApplicable = ($('#chk_' + $(this).attr('identity')).prop('checked') ? false : true);
                if ($(this).prop('checked')) {
                    parameterInfo.TblParameterId = identity[1];
                    parameterInfo.Rating = getRate(index);
                    parameterInfo.Remarks = $('#txtreview' + $(this).attr('identity')).val();
                    parameterinfolist.push(parameterInfo);
                }
            });
        parameterTypeInfo.ParameterInfoList = parameterinfolist; parameterTypeInfoList.push(parameterTypeInfo)
    });
    $('.starwars').each(function () {
        var parameterTypeInfo = {};
        parameterTypeInfo.PerformanceMasterId = performanceIndicator.properties[getResultIndicator($('#lblperformance' + $(this).attr('ptypeId')).text())].value;
        parameterTypeInfo.ParameterTypeId = $(this).attr('ptypeId');
        var parameterinfolist = [];
        var parameterInfo = {};
        var identity = $(this).attr('identity').split('-');
        parameterTypeInfo.PtId = identity[1];
        parameterTypeInfo.IsApplicable = false;
        parameterinfolist.push(parameterInfo);
        parameterTypeInfo.ParameterInfoList = parameterinfolist; parameterTypeInfoList.push(parameterTypeInfo)
    });
    model.ParameterTypeInfoList = parameterTypeInfoList; var allratingstatus = !0; $.each(model.ParameterTypeInfoList, function (index, item) { if (item.ParameterInfoList.length == 0 && item.IsApplicable) { allratingstatus = !1; return !1 } }); if (!allratingstatus) {
        $("#lblMsg").text('Please fill all parameters rating!'); if ($("#lblMsg").text() != "") { $("#dvMsg").css("display", "block"); setTimeout(function () { $("#dvMsg").css("display", "none") }, 8000) }
        $('html, body').animate({ scrollTop: $("#dvMsg").offset().top }, 2000); return !1
    } else { saveCandidatePerformaneParameter(model) }
}

function rendercardvalue() {
    var info = $('#GetRatingData').val() != "" ? $.parseJSON($('#GetRatingData').val()) : ""; if (info != "") {
        $.each(info, function (index, item) { var ptype = item.Name; $.each(item.ParameterList, function (ind, obj) { $('#star-' + item.Name.replace(/\s/g, '') + '-' + obj.Rating + '-' + obj.ParameterId).prop('checked', !0).trigger('change'); $('#txtreview' + item.Id + '-' + obj.ParameterId).val(obj.Remark); if (obj.IsApplicable) { $('#chk_' + item.Id + '-' + obj.ParameterId).prop('checked', !0).trigger('change'); } }) });
        //$("#secOverlay").prepend("<div class=\"overlay\"></div>"); $(".overlay").css({ "position": "absolute", "width": $(document).width(), "height": "100%", "z-index": 99999 }).fadeTo(0, 0.8);
        $('.star').each(function () { $(this).addClass('overlay'); });
        $('#btncardsub').attr('disabled', 'disabled');
    }
}

function fillCandidatelistddl() {
    var batchId = $('#BatchNo').val(); if (batchId == "") { $('#ddlReportCandidate').empty().append('<option value="" selected > Select Trainee </option>'); return !1 }
    $.ajax({
        url: "/PerformanceCard/Report/GetCandidateListByBatchId", type: "POST", async: !1, data: { BatchId: parseInt(batchId) }, success: function (response) {
            var str = ''; str += '<option value="">Select Trainee</option>'; str += '<option value="0">All Trainee</option>'; if (response != null && response.length > 0) { $.each(response, function (index, item) { str += '<option value=' + item.Id + '>' + item.Name + '</option>' }) }
            $('#ddlReportCandidate').empty().append(str); if ($('#hdnRegNo').val() != "") { $('#ddlReportCandidate').val($('#hdnRegNo').val()).trigger('change') }
            $('#spncapacity').text(''); if (batchId > 0) { $('#divcapacity').css('display', 'block'); $('#spncapacity').text("Batch Capacity : " + (response.length > 0 ? response[0].Capacity : 0)) } else { $('#divcapacity').css('display', 'none') }
        }, error: function (error) { }
    })
}

$('.refreshchart').change(function () {
    $('#divNodeNode').css('display', 'none');
    $('#divNodeNodeParameter').css('display', 'none');
    $('#divNodeParameter').empty();
    $('#divsubNodeParameter').empty();

    $('#divSubBarChart').empty(); $('#subheading').text("Sub-Parameter"); var Batch = $('#BatchNo').val(); var Trainee = $('#ddlReportCandidate').val(); var Review = $('#ReviewId').val(); var status = !0; if (Batch == "") { $("#BatchNo").select2({ containerCssClass: "error" }); status = !1 } else { $("#BatchNo").select2({ containerCssClass: "" }) }
    if (Trainee == "") { $("#ddlReportCandidate").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlReportCandidate").select2({ containerCssClass: "" }) }
    if (Review == "") { $("#ReviewId").select2({ containerCssClass: "error" }); status = !1 } else { $("#ReviewId").select2({ containerCssClass: "" }) }
    if ($('#ddlWeeklyId').length > 0) { if ($('#ddlWeeklyId').val() == "") { $("#ddlWeeklyId").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlWeeklyId").select2({ containerCssClass: "" }) } }
    if (status)
        showChartnew(); else { $('#divBarChart').empty(); $('#divSubBarChart').empty(); $('#container').empty() }
    if ($('#ddlWeeklyId').length > 0)
        createweeklyreportcharttitle(); else createcharttitle();
}); $('.assesmentreportchart').click(function () {
    $('#divReportChart1').empty(); var status = !0; var compare = $('#ddlCompare').select2('val'); var batch = $('#BatchNo').select2('val'); var year = $('#ddlYear').select2('val'); var comyear = $('#ddlYear1').select2('val'); var month = $('#ddlMonth').select2('val'); var commonth = $('#ddlMonth1').select2('val'); var qurtermonth = $('#ddlquarterMonth').select2('val'); var comqurtermonth = $('#ddlquarterMonth1').select2('val'); if (compare == "") { $("#ddlCompare").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlCompare").select2({ containerCssClass: "" }) }
    if (batch == "") { $("#BatchNo").select2({ containerCssClass: "error" }); status = !1 } else { $("#BatchNo").select2({ containerCssClass: "" }) }
    if (compare != "" && compare > 0) {
        if (year == "") { $("#ddlYear").select2({ containerCssClass: "error" }); status = !1; if (compare == 2) { if (comyear == "") { $("#ddlYear1").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlYear1").select2({ containerCssClass: "" }) } } } else {
            $("#ddlYear").select2({ containerCssClass: "" }); if (compare == 1) {
                if (month == "") { $("#ddlMonth").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlMonth").select2({ containerCssClass: "" }) }
                if (commonth == "") { $("#ddlMonth1").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlMonth1").select2({ containerCssClass: "" }) }
            } else if (compare == 2) { if (comyear == "") { $("#ddlYear1").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlYear1").select2({ containerCssClass: "" }) } } else if (compare == 3) {
                if (qurtermonth == "") { $("#ddlquarterMonth").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlquarterMonth").select2({ containerCssClass: "" }) }
                if (comqurtermonth == "") { $("#ddlquarterMonth1").select2({ containerCssClass: "error" }); status = !1 } else { $("#ddlquarterMonth1").select2({ containerCssClass: "" }) }
            }
        }
    }
    var com = compare == "" ? 0 : compare; ceratecomparereportmaptitle(com, status); if (status) { var model = {}; model.Compare = compare; model.BatchNo = batch; model.Year = year; model.CompareYear = comyear; model.Month = month; model.CompareMonth = commonth; model.QuarterMonth = qurtermonth; model.CompareQuarterMonth = comqurtermonth; getcomparereport(model) } else return !1
}); function initialfuncexecution() { StopLoading() }
function defultfuncexecution() { $('#BatchNo').prepend($('<option>', { value: "", text: 'Select Batch', selected: !0 }), $('<option>', { value: 0, text: 'All Batch' })); $('#ReviewId').prepend($('<option>', { value: "", text: 'Select Review', selected: !0 }), $('<option>', { value: 0, text: 'All Review' })); $('#ddlCompare').change(function () { var compare = $(this).select2('val'); $('#ddlYear').val('').trigger('change'); $('#ddlYear1').val('').trigger('change'); $('#BatchNo').val('').trigger('change'); $('#ReviewId').val('').trigger('change'); if (compare != "") { if (compare == 1) { $('#divyearfirst').css('display', 'block'); $('#divyearSecond').css('display', 'none'); $('#divmonthfirst').css('display', 'none'); $('#divmonthSecond').css('display', 'none'); $('#ddlYear').val('').trigger('change') } else if (compare == 2) { $('#divyearfirst').css('display', 'block'); $('#divyearSecond').css('display', 'block'); $('#divmonthfirst').css('display', 'none'); $('#divmonthSecond').css('display', 'none'); $('#ddlYear').val('').trigger('change'); $('#ddlYear1').val('').trigger('change') } else if (compare == 3) { $('#divyearfirst').css('display', 'block'); $('#divyearSecond').css('display', 'none'); $('#divmonthfirst').css('display', 'none'); $('#divmonthSecond').css('display', 'none') } } else { $('#divyearfirst').css('display', 'none'); $('#divyearSecond').css('display', 'none'); $('#divmonthfirst').css('display', 'none'); $('#divmonthSecond').css('display', 'none') } }); $('#ddlYear').change(function () { $('#ddlMonth').val('').trigger('change'); $('#ddlMonth1').val('').trigger('change'); $('#ddlquarterMonth').val('').trigger('change'); $('#ddlquarterMonth1').val('').trigger('change'); if ($(this).select2('val') > 0 && $('#ddlCompare').select2('val') == 1) { $('#divmonthfirst').css('display', 'block'); $('#divmonthSecond').css('display', 'block'); $('#divquartermonthfirst').css('display', 'none'); $('#divquartermonthSecond').css('display', 'none') } else if ($(this).select2('val') > 0 && $('#ddlCompare').select2('val') == 3) { $('#divquartermonthfirst').css('display', 'block'); $('#divquartermonthSecond').css('display', 'block'); $('#divmonthfirst').css('display', 'none'); $('#divmonthSecond').css('display', 'none') } else { $('#divmonthfirst').val('').trigger('change').css('display', 'none'); $('#divmonthSecond').val('').trigger('change').css('display', 'none'); $('#divquartermonthfirst').css('display', 'none'); $('#divquartermonthSecond').css('display', 'none') } }); StopLoading() }
function atonchange() { $('#hdnRegNo').val(''); fillCandidatelistddl() }