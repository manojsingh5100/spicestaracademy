function GetListOfAxisPoint() { var e = $("#hdnmappoint").val(); return null != e && "" != e ? $.parseJSON(e) : "" }
function chartHeader() {
    var e = new Date, t = e.getDate(), r = e.getMonth() + 1, n = e.getFullYear(); t < 10 && (t = "0" + t), r < 10 && (r = "0" + r), $("#headStr").text(["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"][r - 1] + ", " + n
        + "  (Day/No of Registration)")
}

function convertNumberToWords(e) { var t = new Array; t[0] = "", t[1] = "One", t[2] = "Two", t[3] = "Three", t[4] = "Four", t[5] = "Five", t[6] = "Six", t[7] = "Seven", t[8] = "Eight", t[9] = "Nine", t[10] = "Ten", t[11] = "Eleven", t[12] = "Twelve", t[13] = "Thirteen", t[14] = "Fourteen", t[15] = "Fifteen", t[16] = "Sixteen", t[17] = "Seventeen", t[18] = "Eighteen", t[19] = "Nineteen", t[20] = "Twenty", t[30] = "Thirty", t[40] = "Forty", t[50] = "Fifty", t[60] = "Sixty", t[70] = "Seventy", t[80] = "Eighty", t[90] = "Ninety"; var r = (e = e.toString()).split(".")[0].split(",").join(""), n = r.length, a = ""; if (n <= 9) { for (var u = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0), l = new Array, i = 0; i < n; i++)l[i] = r.substr(i, 1); i = 9 - n; for (var o = 0; i < 9; i++ , o++)u[i] = l[o]; for (i = 0, o = 1; i < 9; i++ , o++)0 != i && 2 != i && 4 != i && 7 != i || 1 == u[i] && (u[o] = 10 + parseInt(u[o]), u[i] = 0); value = ""; for (i = 0; i < 9; i++)value = 0 == i || 2 == i || 4 == i || 7 == i ? 10 * u[i] : u[i], 0 != value && (a += t[value] + " "), (1 == i && 0 != value || 0 == i && 0 != value && 0 == u[i + 1]) && (a += "Crores "), (3 == i && 0 != value || 2 == i && 0 != value && 0 == u[i + 1]) && (a += "Lakhs "), (5 == i && 0 != value || 4 == i && 0 != value && 0 == u[i + 1]) && (a += "Thousand "), 6 == i && 0 != value && 0 != u[i + 1] && 0 != u[i + 2] ? a += "Hundred and " : 6 == i && 0 != value && (a += "Hundred "); a = a.split("  ").join(" ") } return a }
var dtChartAndHeader = function () { chartHeader(); setTimeout(function () { BarChart(GetListOfAxisPoint()) }, 300); StopLoading() }; var regViewSetting = function () { TableCourseFilter = 8; Table = $("#tblRegistration").DataTable(registertblSetting("tblRegistration", "GetRagisterList", "Registration", 1)); $.fn.dataTable.ext.errMode = 'throw' };
function getDOB() { var e = new Date; return e.setMonth(e.getMonth() - 9), e.setFullYear(e.getFullYear() - 17), e.getDate() + "/" + (e.getMonth() + 1) + "/" + e.getFullYear() } function getDOB1() { var e = new Date; return e.setMonth(e.getMonth() - 0), e.setFullYear(e.getFullYear() - 27), e.getDate() + "/" + (e.getMonth() + 1) + "/" + e.getFullYear() }
function checkMsg() { if ($("#lblMsg").text().trim() != "") { $("#dvMsg").css("display", "block"); setTimeout(function () { $("#dvMsg").css("display", "none") }, 8000) } }
function GetPaymentByRazorId(id, amount) { var status = !1; $.ajax({ url: "/FeeManagement/GetPaymentById", type: "GET", async: !1, data: { Id: id }, success: function (response) { if (response != null && response != undefined && response != "") { var object = JSON.parse(response).Attributes; if (object != null && object != undefined) { if (object.error_code !== null && object.status !== "failed") { if (object.status === "authorised" && object.amount === amount * 100) { status = !0 } } else { $("#lblMsg").text('Error Code: ' + object.error_code + '  Error Description: ' + object.error_description); $("#dvMsg").css("display", "block"); setTimeout(function () { $("#lblMsg").text(''); $("#dvMsg").css("display", "none") }, 8000) } } } }, error: function (error) { $("#lblMsg").text(error.statusText); $("#dvMsg").css("display", "block"); setTimeout(function () { $("#lblMsg").text(''); $("#dvMsg").css("display", "none") }, 8000) } }); return status }
function HideShowTableColumn(tblname) {
    if ($("#showhidecolumn" + tblname).length) { return }
    if ($("#" + tblname).length > 0) {
        var tblCommonApi = new $.fn.dataTable.Api("#" + tblname); var action = " &nbsp;<div class='btn-group tblshowhide' ><button type='button' class='btn btn-primary btn-sm dropdown-toggle' data-toggle='dropdown'><b><i class='glyphicon glyphicon-expand'></i></b></button>"; action += "<ul id='showhidecolumn" + tblname + "' class='dropdown-menu ' role='menu'>"; var numCols = $('#' + tblname).dataTable().fnSettings().aoColumns.length; for (var i = 0; i < numCols; i++) { if (tblCommonApi.column(i).visible() && $(tblCommonApi.columns(i).header()).html() != "") { action += "<li class='pull-left'><div class='col-md-12 form-group'> <input  onclick='HideShowTableColumnOnClick(" + i + ",\"" + tblname + "\")' checked  type='checkbox'> <span>" + $(tblCommonApi.columns(i).header()).html() + "</span></div></li>" } }
        action += "  </ul></div>"; $("#" + tblname + "_filter").append(action)
    }
}
function HideShowTableColumnOnClick(i, tblname) { var tblCommonApi = new $.fn.dataTable.Api("#" + tblname); var column = tblCommonApi.column(i); column.visible(!column.visible()) }
function getValue(id) { return $('#' + id).val() }
function DateFormat(jsonDate) { var value = new Date(parseInt(jsonDate.replace(/(^.*\()|([+-].*$)/g, ''))); var dat = value.getDate() + 1 + "/" + value.getMonth() + "/" + value.getFullYear(); return dat }
function DateFormat1(jsonDate) { var value = new Date(parseInt(jsonDate.replace(/(^.*\()|([+-].*$)/g, ''))); var dat = value.getDate() + "/" + (value.getMonth() + 1) + "/" + value.getFullYear(); return dat }
function isNumber(evt) {
    evt = (evt) ? evt : window.event; var charCode = (evt.which) ? evt.which : evt.keyCode; if (charCode > 31 && (charCode < 48 || charCode > 57)) { return !1 }
    return !0
}
function isDecimalNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if ((charCode != 46 || $(element).val().indexOf('.') != -1) && (charCode < 48 || charCode > 57))
        return !1; return !0
}
function deleteConfirmation(regno) {
    $.confirm({
        animation: 'news', closeAnimation: 'news', title: 'Delete Confirmation!', content: 'Are you sure, you want to delete this record!', buttons: {
            confirm: function () {
                $.ajax({ url: "/Registration/DeleteRegistrationByRegNo", type: "POST", async: !1, data: { RegNo: regno } }).done(function (response) {
                    if (response != "")
                        $.alert(response); else Table.draw()
                })
            }, cancel: function () { }
        }
    })
}

function sendFeeEmail(regNo) {
    $.ajax({ url: "/FeeManagement/SendFeeEmailOneByOne", type: "POST", async: !1, data: { RegNo: regNo } }).done(function (response) {
        if (response != "")
            alert("");
    });
}

function addbatchOption(id) {
    $.ajax({ url: "/Registration/GetBatchOptionString", type: "GET", async: !1 }).done(function (response) {
        var str = '<option value="">Select Batch</option>';
        if (response != "" && response.length > 0) {
            $.each(response, function (i, item) {
                str += '<option value="'+ item.Id +'">'+ item.BatchName +'</option>';
            });
        }
        $('#' + id).empty().append(str);
    });
}

function GetDateFromJsonString(MyDate_String_Value) {
    var value = new Date
        (
            parseInt(MyDate_String_Value.replace(/(^.*\()|([+-].*$)/g, ''))
        );
    var dat = value.getMonth() +
        1 +
        "/" +
        value.getDate() +
        "/" +
        value.getFullYear();
    return dat;
}

function IsDateSixMonthAgo(date) {
    var d = new Date();
    var oldDate = new Date(GetDateFromJsonString(date));
    d.setDate(d.getDate() - 180);
    if (oldDate > d)
        return true;
    else
        return false;
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function calculateBMI(height, weight, id) { 
    if (height == null || height == "" || height <= 0 || height == undefined) {
        $('#' + id).text('');
        $('#divbmi').css('display', 'none');
        return false;
    }
    else if (weight == null || weight == "" || weight <= 0 || weight == undefined) {
        $('#' + id).text('');
        $('#divbmi').css('display', 'none');
        return false;
    }
    var bmi = weight / (height / 100 * height / 100);
    $('#divbmi').css('display', 'block');
    $('#' + id).text("Candidate BMI: " + bmi.toFixed(2));
}