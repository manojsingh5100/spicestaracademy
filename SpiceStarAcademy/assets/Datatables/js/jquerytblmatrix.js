TableFilter = function RejectedCandidateFilter(selector, filterIndex) {
    if (selector === "tblReport")
        $('#' + selector + '_filter').append("&nbsp;&nbsp;<label>Show <select name='tblAddmissionView_length' onchange='pagelength(this);'  aria-controls='tblAddmissionView' style='height:30px;'><option value='10'>10</option><option value='25'>25</option><option value='50'>50</option><option value='10000000' selected>All</option></select> entries</label>");
    else {
        $('#' + selector + '_filter').append("&nbsp;&nbsp;<label>Show <select name='tblAddmissionView_length' onchange='pagelength(this);'  aria-controls='tblAddmissionView' style='height:30px;'><option value='10'>10</option><option value='25'>25</option><option value='50'>50</option><option value='100' >100</option></select> entries</label>");
        if (selector === "tblRejectRegistration")
            $('#' + selector + '_filter').prepend("<select id='ddlwithdrawal' onclick='filterWithdrawalCandidate(this)' style='height:32px;width:175px;display:none;'><option value=''>Withdrawn Candidates</option><option value='All'>All Candidates</option></select>  &nbsp;<select id='ddlcoursefilter' onclick='coursefilter(this," + filterIndex + ")' style='height:32px;width:125px;'><option value=''>Select Course</option><option value='BBA'>BBA</option><option value='MBA'>MBA</option><option value='PHT'>PHT</option></select>&nbsp;&nbsp;&nbsp;&nbsp;");
        else {
            if (selector === "tblScreening") {
                var str = '<div class="dateFilter" style="display:inline-block; overflow:hidden; position:absolute;left:100px;"><button type="button" style="width:210px;" class="btn btn-info" data-toggle="collapse" data-target="#demo">Date Time Filter</button><div id = "demo" class="collapse" style = "width: 210px; height:170px !important; padding: 15px; background-color: #e4e4e4;" ><div class="col-md-12 form-group"><input class="form-control" type="text" id="txtfromDate" value="" placeholder="From Date"></div><div class="col-md-12 form-group"><input class="form-control" type="text" id="txttoDate" placeholder="To Date"></div><div class="col-md-12 form-group"><input class="btn btn-default" type="button" name="Clear" value="Clear" onClick="dateClear()">&nbsp;&nbsp;<input class="btn btn-success" type="button" name="Submit" id="btndatefilter" onclick="daterangefilterfunc()" value="Submit"></div></div></div>&nbsp;&nbsp;';
                $('#' + selector + '_filter').prepend(str + "<select id='ddlcoursefilter' onchange='coursefilter(this," + filterIndex + ")' style='height:32px;width:125px;'><option value=''>Select Course</option><option value='BBA'>BBA</option><option value='MBA'>MBA</option><option value='PHT'>PHT</option></select>&nbsp;&nbsp;<select onchange='screenningstatusfilter(this,11)' id='ddlstatusfilter' style='height:32px;width:125px;margin-right: 5px;'><option value=''>Select Status</option><option value='Pending'>Pending</option><option value='Selected'>Selected</option><option value='Rejected'>Rejected</option><option value='Stand-By'>Stand-By</option><option value='Withdrwan'>Withdrawn</option></select><select onchange='screenningbatchfilter(this,8)' style='height:32px;width:125px;margin-right: 0px;' id='ddlbatchfilter'><option value=''>Select Batch</option></select>&nbsp;&nbsp;");

                $('#txtfromDate').datepicker({
                    autoclose: true,
                    format: 'dd/mm/yyyy'
                });
                $('#txttoDate').datepicker({
                    autoclose: true,
                    format: 'dd/mm/yyyy'
                });
            }
            else {
                if (selector == "tblMedical") {
                    $('#' + selector + '_filter').prepend("<select onchange='coursefilter(this," + filterIndex + ")' id='ddlcoursefilter' style='height:32px;width:125px;'><option value=''>Select Course</option><option value='BBA'>BBA</option><option value='MBA'>MBA</option><option value='PHT'>PHT</option></select>&nbsp;&nbsp;<select onchange='screenningbatchfilter(this,8)' style='height:32px;width:125px;margin-right: 0px;' id='ddlbatchfilter'><option value=''>Select Batch</option></select>&nbsp;&nbsp;");
                } else {
                    $('#' + selector + '_filter').prepend("<select onchange='coursefilter(this," + filterIndex + ")' id='ddlcoursefilter' style='height:32px;width:125px;'><option value=''>Select Course</option><option value='BBA'>BBA</option><option value='MBA'>MBA</option><option value='PHT'>PHT</option></select>&nbsp;&nbsp;&nbsp;&nbsp;");
                }
                if (selector == "tblAddmissionView") {
                    $('#' + selector + '_filter').prepend("<select onchange='trfilter(this)' id='ddltrfilter' style='height:32px;width:155px;'><option value=''>All Candidates</option><option value='Termination'>Terminated Candidates</option><option value='Resignation'>Resigned Candidates</option></select>&nbsp;&nbsp;<select onchange='screenningbatchfilter(this,8)' style='height:32px;width:125px;margin-right: 0px;' id='ddlbatchfilter'><option value=''>Select Batch</option></select>&nbsp;&nbsp;");
                }
            }
        }
        if (selector === "tblFeeCandidatesList") { $('#' + selector + '_filter').prepend("<select onclick='feeStatusFilter(this,11)' style='height:32px;width:125px;margin-right: 13px;' id='ddlfeestatus' ><option value=''>Select Fee Status</option><option value='Unpaid'>Unpaid</option><option value='Partially'>Partially Paid</option><option value='Fully'>Fully Paid</option></select>&nbsp;&nbsp;<select onchange='screenningbatchfilter(this,8)' style='height:32px;width:125px;margin-right: 0px;' id='ddlbatchfilter'><option value=''>Select Batch</option></select>&nbsp;&nbsp;"); }
    }
};

TableSSAReportFilter = function SSReportingList() { $('#tblSSAReport_filter').append("&nbsp;&nbsp;<label>Show <select name='tblReport_length' id='ddlssareport' onchange='pagelength(this);'  aria-controls='tblReport' style='height:30px;'><option value='10'>10</option><option value='25'>25</option><option value='50'>50</option><option value='10000000' selected>All</option></select> entries</label>") };

function rejectedCol() {
    return [{ "data": "SrNo" }, { "data": "RegistartionNo" }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" },
    //{ "data": "DateOfBirth" },
    { "data": "CourseName" },
    { "data": "MedicalRemark" },
    { "data": "RegisterDate" }
    //{
    //    "data": "IsFeePayment", "render": function (data, type, row) {
    //        if (row.IsFeePayment) {
    //            if (row.RefundStatus == "success")
    //                return "Fully paid";
    //            else if (row.RefundStatus == "orange")
    //                return "Partially paid";
    //            else
    //                return "Unpaid";
    //        }
    //        else {
    //            return "Unpaid"
    //        }
    //    }
    //}
    //{ "mRender": function (data, type, row) { var render = withdrawalRenderAction(row); return render }, orderable: !1 }
        //{ "render": function (data, type, row) { return ragistrationStatus(row) } }
    ]
}

function withdrawalRenderAction(obj) {
    var action = '';
    if (obj.MedicalRemark == null || obj.MedicalRemark == "") {
        action += '<span id="spnIsMedicalClear_' + obj.Id + '" style="float:left; width:70%;display:block;"> <input type="button" onclick="openRemarkModel(' + obj.Id + ',this,' + obj.RegistartionNo + ')" value="Withdrawn" class="btn btn-default btn-xs" /></span>';
    } else {
        action += '<span id="spnIsMedicalClear_' + obj.Id + '" style="float:left; width:70%;display:block;"> <input type="button" onclick="openRemarkModel(' + obj.Id + ',this,' + obj.RegistartionNo + ')" value="Withdrawn" class="btn btn-warning btn-xs" /></span>';
    }
    return action;
}


function ragistrationStatus(obj) {
    var str = ""; if (obj.IsScreenningClear) { str += "<span class='label label-success'>Screening &nbsp;<i class='fa fa-check' aria-hidden='true'></i></span>&nbsp;"; if (obj.IsMedicalClear != null && !obj.IsMedicalClear) { str += "<span class='label label-danger'>Medical &nbsp;<i class='fa fa-close' aria-hidden='true'></i></span>&nbsp;" } } else { str += "<span class='label label-danger'>Screening &nbsp;<i class='fa fa-close' aria-hidden='true'></i></span>&nbsp;" }
    return str
}

function registrationlistCol() {
    return [{ "data": "Id" }, { "data": "RegistartionNo", "render": function (data, type, row) { return "<a href='/Registration/Create?Id=" + row.Id + "'>" + row.RegistartionNo + "</a>" } }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "DateOfBirth" }, { "data": "Gender", visible: !1 },
    {
        "data": "SourceOfCandidate", "render": function (data, type, row) {
            if (row.SourceOfCandidate === "Empty") {
                if (row.PaymentStatusStr === "HR Candidate")
                    return "<span style='color:#999'>SpiceJet HR</span>";
                else if (row.PaymentStatusStr === "Consultent Candidate")
                    return "<span style='color:#999'>Consultant Candidate</span>";
                else
                    return "-";
            } else {
                if (row.PaymentStatusStr === "HR Candidate")
                    return "<span style='color:#999'>SpiceJet HR</span>";
                else if (row.PaymentStatusStr === "Consultent Candidate")
                    return "<span style='color:#999'>Consultant Candidate</span>";
                else
                    return "<span style='color:#999'>" + row.SourceOfCandidate + "</span>";
            }
        }
    },
    {
        "data": "ModOfPayment", "render": function (data, type, row) {
            if (row.ModOfPayment != null) {
                return "<span style='color:#999'>" + row.ModOfPayment + "</span>";
            } else {
                return "-";
            }
            //if (row.PaymentStatusStr === "HR Candidate")
            //    return "<span style='color:#999'>SpiceJet HR</span>";
            //else if (row.PaymentStatusStr === "Consultent Candidate")
            //    return "<span style='color:#999'>Consultant Candidate</span>";
            //else
            //    return "<span style='color:#999'>" + row.ModOfPayment + "</span>";

            //if (row.PaymentStatusStr === "Success")
            //    return "<span style='color:green'>Success</span>";
            //else if (row.PaymentStatusStr === "Pending")
            //    return "<span style='color:red'>Pending</span>";
            //else {
            //    if (row.PaymentStatusStr === "HR Candidate")
            //        return "<span style='color:#999'>SpiceJet HR</span>";
            //    else
            //        return "<span style='color:#999'>Consultant</span>"
            //}
        }
    }
        , { "data": "CourseName" }, { "data": "RegisterDate" }, {
        "mRender": function (data, type, row) {
            if (IsDateSixMonthAgo(row.RegistrationDate)) {
                var str = '';
                if (row.IsResistrationHistory)
                    str = '<button data-toggle="tooltip" title="Registration History!" onclick="OpenReregistrationHistory(\'' + row.Email + '\',\'' + row.Mobile + '\')" class="btn btn-info btn-xs" ><i class="fa fa-info-circle" aria-hidden="true"></i></button>&nbsp;';
                return str + '<button type="button" onclick="deleteConfirmation(' + row.RegistartionNo + ')" class="btn btn-danger btn-xs" >Delete</button>';
            }
            else {
                var str1 = '';
                if (row.IsResistrationHistory)
                    str1 = '<button data-toggle="tooltip" title="Registration History!" onclick="OpenReregistrationHistory(\'' + row.Email + '\',\'' + row.Mobile + '\')" class="btn btn-info btn-xs" ><i class="fa fa-info-circle" aria-hidden="true"></i></button>&nbsp;';
                return str1 + '<button type="button" onclick="deleteConfirmation(' + row.RegistartionNo + ')" class="btn btn-danger btn-xs" >Delete</button>';
                //return str1 + '<button type="button" onclick="ReRegisterCandidate(' + row.RegistartionNo + ')" class="btn btn-warning btn-xs" >Re-Register</button>&nbsp;<button type="button" onclick="deleteConfirmation(' + row.RegistartionNo + ')" class="btn btn-danger btn-xs" >Delete</button>';
            }
        }, orderable: !1
    }];
}

function ReRegisterCandidate(id) {
    return window.location.href = "/Registration/ReRegisterCandidate?id=" + id;
}

function screenninglistingCol() {
    return [{ "data": "Id" }, { "data": "RegistartionNo", "render": function (data, type, row) { return "<a href='/Registration/Create?Id=" + row.Id + "'>" + row.RegistartionNo + "</a>" } }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "DateOfBirth" }, { "data": "Gender", visible: !1 }, {
        "data": "PaymentStatusStr", visible: !1, "render": function (data, type, row) {
            if (row.PaymentStatusStr === "Success")
                return "<span style='color:green'>Success</span>"; else if (row.PaymentStatusStr === "Pending")
                return "<span style='color:red'>Pending</span>"; else {
                if (row.PaymentStatusStr === "HR Candidate")
                    return "<span style='color:#999'>HR Candidate</span>"; else return "<span style='color:#999'>Consultant Candidate</span>"
            }
        }
    }, { "data": "CourseName" }, { "data": "ModOfPayment" }, {
        "data": "BatchName", "render": function (data, type, row) {
            if (row.IsStandBy || (row.IsScreenningClear != null && row.IsScreenningClear == false))
                return "";
            else
                return row.BatchName == "Batch 0" ? "" : row.BatchName;
        }
    }, { "data": "RegisterDate" }, { "mRender": function (data, type, row) { var render = RenderAction(row); return render }, orderable: !1 },]
}
function RenderAction(obj) {
    var action = ''; if (obj.PaymentStatus || obj.IsConsultantCandidate || obj.IsHRCandidate) {
        if (obj.IsScreenningClear == null) { action += ' <button type="button" onclick="openScreenPopUpModel(' + obj.RegistartionNo + ',this)" class="btn btn-info btn-xs" >Screening</button>' }
        else if (obj.IsStandBy) { action += ' <button type="button" onclick="openScreenPopUpModel(' + obj.RegistartionNo + ',this)" class="btn btn-warning btn-xs" >Stand-By &nbsp;  <i class="fa fa-check" aria-hidden="true"></i></button>' }
        else if (obj.MedicalStatus == "Withdrawn") {
            action += ' <button type="button" onclick="openScreenPopUpModel(' + obj.RegistartionNo + ',this)" style="background-color: purple;border:1px solid purple !important;" class="btn btn-warning btn-xs" >Withdrawn </button>'
        }
        else if (obj.IsScreenningClear == !0) { action += ' <button type="button" onclick="openScreenPopUpModel(' + obj.RegistartionNo + ',this)" class="btn btn-success btn-xs" >Selected &nbsp;  <i class="fa fa-check" aria-hidden="true"></i></button>' }
        else if (obj.IsScreenningClear == !1) { action += ' <button type="button" onclick="openScreenPopUpModel(' + obj.RegistartionNo + ',this)" class="btn btn-danger btn-xs" >Rejected &nbsp;  <i class="fa fa-close" aria-hidden="true"></i></button>' }

        if (obj.IsResistrationHistory)
            action += '&nbsp;<button data-toggle="tooltip" title="Registration History!" onclick="OpenReregistrationHistory(\'' + obj.Email + '\',\'' + obj.Mobile + '\')" class="btn btn-primary btn-xs" ><i class="fa fa-info-circle" aria-hidden="true"></i></button>';

        if (obj.IsScreenningClear == !0 && obj.ShowMedicalConsultPopUp) {
            action += '&nbsp;<button data-toggle="tooltip" title="Medical!" onclick="OpenMedicalPopup(\'' + obj.RegistartionNo + '\')" class="btn btn-warning btn-xs" ><i class="fa fa-medkit" aria-hidden="true"></i></button>';
        }
    }
    var role = $('#hdnRoleType').val();
    if (role != null && role != undefined && role == 'Admin') {
        action += '&nbsp;<button type="button" onclick="deleteConfirmation(' + obj.RegistartionNo + ')" class="btn btn-danger btn-xs" >Delete</button>';
    }
    return action
}
function isRejected(Id, obj) {
    var status = !0; if ($(obj).attr('value') == "Reject")
        status = !1; $.post('/Registration/IsStudentScreenningMedicalClearance', { Id: Id, status: status, Tag: "Screen" }, function (response) {
            if (response != null)
                Table.draw()
        }).error(function (ex) { })
}

function medicallistingCol() {
    return [{ "data": "Id" }, { "data": "RegistartionNo" }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "DateOfBirth" }, { "data": "Gender", visible: !1 }, {
        "data": "IsFeePayment", "render": function (data, type, row) {
            if (row.IsFeePayment) {
                if (row.IsMedicalClear != null && row.IsMedicalClear == !1) { return '<input type="button" value="Payment Done" class="btn btn-success btn-xs payment" onclick="redirectOnPaymentPage(' + row.RegistartionNo + ',' + row.SessionId + ',' + row.CourseId + ',1)" >' }
                else { return '<input type="button" value="Payment Done" class="btn btn-success btn-xs payment" onclick="redirectOnPaymentPage(' + row.RegistartionNo + ',' + row.SessionId + ',' + row.CourseId + ',1)" >' }
            }
            else {
                if (row.IsMedicalClear != null && row.IsMedicalClear == !1) { return '<input type="button" value="Take Payment" class="btn btn-info btn-xs payment" onclick="redirectOnPaymentPage(' + row.RegistartionNo + ',' + row.SessionId + ',' + row.CourseId + ',1)" >' } else {
                    if (row.IsFeePayStandBy)
                        return '<input type="button" value="Stand-By" style="width:85px;" class="btn btn-warning btn-xs payment" onclick="redirectOnPaymentPage(' + row.RegistartionNo + ',' + row.SessionId + ',' + row.CourseId + ',1)" >'; else return '<input type="button" value="Take Payment" class="btn btn-info btn-xs payment" onclick="redirectOnPaymentPage(' + row.RegistartionNo + ',' + row.SessionId + ',' + row.CourseId + ',1)" >'
                }
            }
        }
    }, { "data": "CourseName" }, { "data": "BatchName" }, { "data": "MedicalStatus" }, { "data": "MedicalRemark" }, { "data": "RegisterDate", visible: !1 }, { "mRender": function (data, type, row) { var render = medicalRenderAction(row); return render }, orderable: !1 },]
}
function redirectOnPaymentPage(regNo, sessionId, courseId, page) {
    $.ajax({
        url: "/Registration/GetFeeTypeExist", type: "GET", async: !1, data: { SessionId: sessionId, CourseId: courseId }, success: function (response) {
            if (response) {
                if (page == 1)
                    window.location.href = '/FeeManagement/AddFeeCollection?Id=' + regNo + '&PT=1'; else window.location.href = '/FeeManagement/AddFeeCollection?Id=' + regNo
            } else { $("#dvMsg").css("display", "block"); $("#lblMsg").text("Please firstly, create or active Fee type info for admission from Fee Mangement/Add Fee Type Tab!"); setTimeout(function () { $("#dvMsg").css("display", "none"); $("#lblMsg").text('') }, 8000) }
        }, error: function (error) { }
    })
}
function medicalRenderAction(obj) {
    var action = ''; if (obj.PaymentStatus || obj.IsConsultantCandidate || obj.IsHRCandidate) {
        action += '<span id="spnIsMedicalClear_' + obj.Id + '" style="float:left; width:70%;display:' + (obj.IsMedicalClear == null ? "block" : "none") + '"><input type="button" onclick="isRejectedMedical(' + obj.Id + ',this)" value="Clear" class="btn btn-success btn-xs"  />'; action += '&nbsp; <input type="button" onclick="openRemarkModel(' + obj.Id + ',this,' + obj.RegistartionNo + ')" value="Not clear" class="btn btn-danger btn-xs" /></span>'; if (obj.IsMedicalClear == !0) { action += '<span  id="spnClear_' + obj.Id + '" class="label label-success" style="float:left; width:50%; margin-right:5px; display:' + (obj.IsMedicalClear == !0 ? "block" : "none") + '"> Cleared &nbsp; <i class="fa fa-check" aria-hidden="true"></i></span>' }
        else if (obj.IsMedicalClear == !1) { action += '<span id="spnNotClear_' + obj.Id + '" style="float:left; width:50%; margin-right:5px; display:' + (obj.IsMedicalClear == !1 ? "block" : "none") + '" class="label ' + (obj.IsMedicalStandBy ? 'label-warning' : 'label-danger') + '" title="' + obj.MedicalRemark + '">' + (obj.IsMedicalStandBy ? 'Stand-By' : 'Not cleared') + '&nbsp;' + (obj.IsMedicalStandBy ? '' : '<i class="fa fa-close" aria-hidden="true"></i>') + ' </span>' }
        if (obj.IsMedicalClear !== null && $('#hdnRoleType').val() === "Admin") { action += '<input type="checkbox"  onchange="changeMadicalStage(this,' + obj.IsMedicalClear + ',' + obj.Id + ')" checked> Lock' }
    }
    return action
}
function changeMadicalStage(obj, status, id) { var state = $(obj).prop('checked'); var sts = status === !1 ? "spnNotClear_" : "spnClear_"; if (!state) { $('#spnIsMedicalClear_' + id).css("display", "block"); $('#' + sts + id).css("display", "none") } else { $('#spnIsMedicalClear_' + id).css("display", "none"); $('#' + sts + id).css("display", "block") } }
function isRejectedMedical(Id, obj) {
    var status = !0; $.post('/Registration/IsStudentScreenningMedicalClearance', { Id: Id, status: status, Tag: "Medical" }, function (response) {
        if (response != null)
            Table.draw(false);
    }).error(function (ex) { })
}
function admissionlistingCol() { return [{ "data": "Id" }, { "data": "RegistartionNo" }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "DateOfBirth" }, { "data": "Gender" }, { "data": "CourseName" }, { "mRender": function (data, type, row) { var render = AdmissionRenderAction(row); return render }, orderable: !1 },] }
function AdmissionRenderAction(obj) { var action = '<a href=/Addmission/Addmission?Id=' + obj.Id + ' class="btn btn-success btn-xs" > Go to Admission </a>'; return action }
function admissionViewListingCol() {
    return [{ "data": "Id" }, { "data": "RegNo" }, { "data": "StudentName", "render": function (data, type, row) { return "<a href='/Addmission/StudentProfile?Id=" + row.Id + "'> " + row.StudentName + "</a>" } }, { "data": "Email" }, { "data": "MobileNo" }, { "data": "DateOfBirth" }, { "data": "CourseName" },
    {
        "data": "BatchName", "render": function (data, type, row) {
            var htmlContent = '';
            if (row.IsBatchHistory)
                htmlContent += '&nbsp;&nbsp;&nbsp;&nbsp;<a data-toggle="tooltip" title="Batch History Info!" href="javascript:void(0)" onclick="openBatchHistory(\'' + row.Id + '\')" ><i class="fa fa-info-circle" aria-hidden="true" style="color: #28142f;"></i></a>';
            return row.BatchName + htmlContent;
        }
    },
    { "data": "SessionName" }, {
        "data": "IsValidPassport", "render": function (data, type, row) {
            if (row.IsValidPassport)
                return "<span>Yes</span>";
            else
                return "<span> No </span>";
        }
    },
    //{ "data": "MedicalStatus" },
    { "data": "DateOfAddmission" },
    {
        "data": "IsValidPassport", "render": function (data, type, row) {
            return '<div class="round-button"><div class="round-button-circle" style="' + (row.IsTermResgCandidate == "Termination" ? "background:red" : (row.IsTermResgCandidate == "P-Termination" ? "background:#ee7f2e" : (row.IsTermResgCandidate != "" ? "opacity:0.5;cursor: no-drop;" : ""))) + '"> <a href="javascript:void(0)" style="' + (row.IsTermResgCandidate == "Termination" ? "" : (row.IsTermResgCandidate == "Resignation" ? "pointer-events: none;" : (row.IsTermResgCandidate == "P-Resignation" ? "pointer-events: none;" : ""))) + '"  onclick="openTRPopup(this,' + row.Id + ');" class="round-button">T</a></div></div> <div class="round-button"><div class="round-button-circle" style="' + (row.IsTermResgCandidate == "Resignation" ? "background:red" : (row.IsTermResgCandidate == "P-Resignation" ? "background:#ee7f2e" : (row.IsTermResgCandidate != "" ? "opacity:0.5;cursor: no-drop;" : ""))) + '" ><a href="javascript:void(0)" onclick="openTRPopup(this,' + row.Id + ');" class="round-button" style="' + (row.IsTermResgCandidate == "Resignation" ? "" : (row.IsTermResgCandidate == "Termination" ? "pointer-events:none;" : (row.IsTermResgCandidate == "P-Termination" ? "pointer-events: none;" : ""))) + '" >R</a></div></div>';
        }
    }];
}

function callCenterListingCol() {
    return [{ "data": "Id" }, { "data": "RegistartionNo", "render": function (data, type, row) { return "" + row.RegistartionNo + "" } }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "DateOfBirth", visible: !1 }, { "data": "Gender", visible: !1 }, { "data": "CourseName" }, {
        "data": "PaymentStatusStr", "render": function (data, type, row) {
            if (row.PaymentDate == null)
                return "<span class='label label-info'> Payment Due </span>"; else {
                if (row.PaymentStatusStr == "Success")
                    return "<span style='width:100px;' class='label label-success'> Paid &nbsp; <i class='fa fa-check' aria-hidden='true'></i> </span>"
                else return "<span class='label label-danger'> Pending &nbsp; <i class='fa fa-close' aria-hidden='true'></i></span>"
            }
        }
    }, {
        "data": "IsApproved", "render": function (data, type, row) {
            if (row.PaymentDate == null)
                return ""; else {
                if (row.IsApproved != null) {
                    if (row.IsApproved)
                        return "<span style='width:100px;' class='label label-success'> Selected &nbsp; <i class='fa fa-check' aria-hidden='true'></i> </span>"; else return "<span class='label label-danger'> Rejected &nbsp; <i class='fa fa-close' aria-hidden='true'></i></span>"
                } else { return "<span class='label label-warning'> Pending </span>" }
            }
        }
    }, {
        "data": "IsScreenningClear", "render": function (data, type, row) {
            if (row.IsScreenningClear != null) {
                if (row.IsScreenningClear)
                    return "<span style='width:100px;' class='label label-success'> Selected &nbsp; <i class='fa fa-check' aria-hidden='true'></i> </span>"; else return "<span class='label label-danger'> Rejected &nbsp; <i class='fa fa-close' aria-hidden='true'></i></span>"
            } else {
                if (row.IsApproved == null || !row.IsApproved)
                    return ""; else return "<span class='label label-warning'> Pending </span>"
            }
        }
    }, {
        "data": "IsMedicalClear", "render": function (data, type, row) {
            if (row.IsMedicalClear != null) {
                if (row.IsMedicalClear)
                    return "<span style='width:100px;' class='label label-success'> Cleared &nbsp; <i class='fa fa-check' aria-hidden='true'></i> </span>"; else return "<span class='label label-danger'> Not Clear &nbsp; <i class='fa fa-close' aria-hidden='true'></i></span>"
            } else {
                if (row.IsScreenningClear == null || !row.IsScreenningClear)
                    return ""; else return "<span class='label label-warning'> Pending </span>"
            }
        }
    }, {
        "data": "IsFeePayment", "render": function (data, type, row) {
            if (row.IsMedicalClear == null) { return "" } else {
                if (!row.IsMedicalClear && row.IsFeePayment) {
                    if (row.IsRefunded) { return "<span class='label label-warning'> Refunded </span>" }
                    else return "<span class='label label-warning'> Refund Pending </span>"
                } else { if (row.IsFeePayment) { return "<span class='label label-success'> Deposited </span>" } else { return "<span class='label label-info'> Fee Due </span>" } }
            }
        }
    }, { "data": "RegisterDate" }, { "mRender": function (data, type, row) { return '<input type="button" value="Remark" onclick="openRefundFeeModel(' + row.RegistartionNo + ');" class="btn btn-warning btn-xs" />' } }]
}

function feeTypeDetailListingCol() {
    return [{ "data": "Id" }, { "data": "SessionName" }, { "data": "CourseName" }, { "data": "FeeTypeName" }, { "data": "Amount" }, {
        "data": "IsActive", "render": function (data, type, row) {
            if (row.IsActive)
                return "Yes"; else return "No"
        }
    }, { "mRender": function (data, type, row) { return '<input type="button" value="Edit" onclick="EditFeeTypeDetail(' + row.Id + ')" class="btn btn-info btn-xs" /> <input type="button" value="Installments" onclick="addUpdateInstallment(' + row.Id + ')" class="btn btn-warning btn-xs" />' } }]
}

function feeCandidateListingCol() {
    return [{ "data": "Id" }, { "data": "RegistartionNo" }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "DateOfBirth", visible: !1 }, { "data": "Gender", visible: !1 }, { "data": "CourseName" }, { "data": "BatchName" }, { "data": "RegisterDate" }, {
        "data": "IsScreenningClear", "render": function (data, type, row) {
            if (row.IsScreenningClear != null) {
                if (row.IsScreenningClear)
                    return "<span style='width:100px;' class='label label-success'> Selected &nbsp; <i class='fa fa-check' aria-hidden='true'></i> </span>"; else return "<span class='label label-danger'> Rejected &nbsp; <i class='fa fa-close' aria-hidden='true'></i></span>"
            } else {
                if (row.IsApproved == null || !row.IsApproved)
                    return ""; else return "<span class='label label-warning'> Pending </span>"
            }
        }
    }, {
        "data": "IsMedicalClear", "render": function (data, type, row) {
            if (row.IsMedicalClear != null) {
                if (row.IsMedicalClear)
                    return "<span style='width:100px;' class='label label-success'> Cleared &nbsp; <i class='fa fa-check' aria-hidden='true'></i> </span>"; else return "<span class='label label-danger'> Not Clear &nbsp; <i class='fa fa-close' aria-hidden='true'></i></span>"
            } else {
                if (row.IsScreenningClear == null || !row.IsScreenningClear)
                    return ""; else return "<span class='label label-warning'> Pending </span>"
            }
        }
    }, {
        "data": "IsFeePayment", "render": function (data, type, row) {
            if (row.IsFeePayment) {
                if (row.RefundStatus == "success")
                    return "<span class='label label-success'>Fully paid</span>";
                else if (row.RefundStatus == "orange")
                    return "<span class='label label-warning'> Partially paid </span>";
                else
                    return "<span class='label label-danger'>Unpaid</span>";
            }
            else {
                if (row.IsFeePayStandBy)
                    return "<span class='label label-warning'> Stand-By </span>"; else return "<span class='label label-danger'>Unpaid</span>"
            }
        }
    }, {
        "mRender": function (data, type, row) {
            var status = !1; if (row.IsScreenningClear != null && row.IsScreenningClear)
                status = !0; return status ? '<a class="btn btn-primary btn-xs" href="#" onclick="redirectOnPaymentPage(' + row.RegistartionNo + ',' + row.SessionId + ',' + row.CourseId + ',2)">Fee Payment</a>' : ''
        }
    }]
}

function reportListingCol() {
    return [{ "data": "SrNo" }, { "data": "RegistartionNo" }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "DOBStr" }, { "data": "Gender", visible: !1 }, { "data": "Education" }, { "data": "CourseName" }, { "data": "PermanentState" }, { "data": "PermanentCity" }, { "data": "BatchName" }, { "data": "SourceOfCandidate" }, { "data": "AdmissionDateStr" },
    {
        "data": "IsFeePayment", "render": function (data, type, row) {
            if (row.IsFeePayment) {
                if (row.RefundStatus == "success")
                    return "Fully paid";
                else if (row.RefundStatus == "orange")
                    return "Partially paid";
                else
                    return "Unpaid";
            }
            else {
                return "Unpaid"
            }
        }
    },
    //{
    //    "data": "IsFeePayment", "render": function (data, type, row) {
    //    return '<button type="button" class="btn btn - info" data-toggle="modal" onclick="GetFeeList(' + row.RegistartionNo +'); " data-target="#modal -default"> Fee Info</button>';
    //}
    // },
        { "data": "FeeTotalAmount" }, { "data": "FirstInstallment" }, { "data": "SecondInstallment" }, { "data": "ThirdInstallment" }, { "data": "FeeDueAmount" }, { "data": "FeeDueDate" }]
}

function refundlistingCol() {
    return [{ "data": "Id" }, { "data": "RegNo" }, { "data": "StudentName" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "Course" }, { "data": "Status" }, { "data": "Remark" }, { "data": "Amount" }, {
        "mRender": function (data, type, row) {
            return '<input type="button" value="Approved" onclick="OpenRefundPopUp(this,' + row.Id + ');" class="btn btn-success btn-xs" /> <input type="button" value="Reject" onclick="OpenRefundPopUp(this,' + row.Id + ');" class="btn btn-warning btn-xs" />';
        }
    }]
}

function ssaReportListingCol() { return [{ "data": "Id" }, { "data": "BatchName" }, { "data": "DateOfInterView" }, { "data": "Title" }, { "data": "FullName" }, { "data": "DateOfBirth" }, { "data": "JoiningDate" }, { "data": "Age" }, { "data": "Addresee1" }, { "data": "Addresee2" }, { "data": "Addresee3" }, { "data": "PinCode" }, { "data": "Contact" }, { "data": "Email" }, { "data": "Designation" }, { "data": "DepartMent" }, { "data": "Location" }, { "data": "MedicalCenter" }, { "data": "NOC_PP" }, { "data": "MedicalDate" }, { "data": "FitnessDate" }, { "data": "MedicalStatus" }, { "data": "CourseName" }] }