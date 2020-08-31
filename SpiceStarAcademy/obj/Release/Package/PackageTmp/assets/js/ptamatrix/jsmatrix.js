function getpilotcandidatelist() {
    $.ajax({
        cache: false,
        type: "Get",
        url: "/PTA/Registration/GetPilotCandidateList",
        async: false,
        success: function (response) {
            if (response != null && response.length > 0) {
                var tblbody = '';
                $.each(response, function (i, item) {
                    tblbody += '<tr>'
                    tblbody += '<td>' + (i + 1) + '</td> <td>' + item.PilotRegistartionNo + '</td> <td>' + item.StudentName + '</td> <td>' + item.Email + '</td> <td>' + item.Mobile + '</td> <td>' + item.DOBStr + '</td> <td>' + item.Gender + '</td> <td>' + item.SourceOfCandidate + '</td> <td>' + item.CourseName + '</td> <td>' + item.RegisterDate + '</td> <td style="text-align:center"><input name=' + (item.IsScrreningExamFeeEmailNotificationNo == 0 ? "yes" : "no") + '  class=' + (item.IsScrreningExamFeeEmailNotificationNo == 0 ? (item.ScreeningTestResultCount > 0 ? "hasbeen" : 'initial') : "hasbeen") + '  ' + (item.ScreeningTestResultCount > 0 ? "checked disabled" : '') + '  type="checkbox" regno=' + item.Id + '  /></td> </tr>';
                });
                $('#tbpilotregistrationdata').append(tblbody);
            }
        },
        complete: function () {
            $("#tblPilotRegistration").DataTable({
                "order": [],
                "columnDefs": [{
                    "targets": 'no-sort',
                    "orderable": false,
                }],
                "pageLength": 50
            });
            StopLoading();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed!');
        }
    });
}

function getpilotcandidatesadmissionlist() {
    $.ajax({
        cache: false,
        type: "Get",
        url: "/PTA/Admission/GetPilotAdmissionCandidateList",
        async: false,
        success: function (response) {
            if (response != null && response.length > 0) {
                var tblbody = '';
                $.each(response, function (i, item) {
                    tblbody += '<tr>';
                    tblbody += '<td>' + (i + 1) + '</td> <td>' + item.PilotRegistartionNo + '</td><td>' + (item.ApplicationNo != null ? item.ApplicationNo : "") + '</td> <td>' + item.StudentName + '</td> <td>' + item.Email + '</td> <td>' + item.Mobile + '</td> <td>' + item.DOBStr + '</td> <td>' + item.Gender + '</td><td>' + item.CourseName + '</td> <td><input type="button" onclick="Message(' + item.Id + ');" value="Payment" class="btn btn-info btn-xs" /> </td> <td>' + item.RegisterDate + '</td> <td><input type="button" onclick="OpenPilotMedicalPopUp(' + item.PilotRegistrationId + ');" value="Medical" class="btn ' + (item.MedicalImagesCount < 2 ? "btn-warning" : "btn-success") + ' btn-xs" /> <input type="button" onclick="OpenPilotOtherDocsPopUp(' + item.PilotRegistrationId + ',' + item.Id + ');" value="Other" class="btn btn-info btn-xs" /></td>';
                    tblbody += '</tr>';
                });
                $('#tbpilotadmissiondata').append(tblbody);
            }
        },
        complete: function () {
            $("#tblPilotAdmission").DataTable({
                "order": [],
                "columnDefs": [{
                    "targets": 'no-sort',
                    "orderable": false,
                }],
                "pageLength": 50
            });
            StopLoading();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed!');
        }
    });
}

function Message(Id) {
    alert("Payment page under construction!");
    return false;
    window.location.href = '/PTA/FeeCollection?Id=' + Id;
}
function getpilotcandidatescreeninglist() {
    $.ajax({
        cache: false,
        type: "Get",
        url: "/PTA/Registration/GetPilotCandidateScreeningList",
        async: false,
        success: function (response) {
            console.log(response);
            if (response != null && response.length > 0) {
                var tblbody = '';
                $.each(response, function (i, item) {
                    tblbody += '<tr>';
                    tblbody += '<td>' + (i + 1) + '</td><td>' + item.PilotRegistartionNo + '</td><td>' + (item.ApplicationNo != null ? item.ApplicationNo : "") + '</td><td>' + item.StudentName + '</td> <td>' + item.Email + '</td> <td>' + item.Mobile + '</td> <td>' + item.DOBStr + '</td> <td>' + item.Gender + '</td><td>' + (item.CreatedBy == 4 ? "Selected" : (item.IsActive ? "Rejected" : "Pending")) + '</td><td>' + item.CourseName + '</td><td>' + item.RegisterDate + '</td> <td><input type="button" id="btn_' + item.RegistartionNo + '" onclick="getpilotScreenInitialInfo(' + item.RegistartionNo + ');" value="  ' + (item.CreatedBy == 4 ? "Selected" : (item.IsActive ? "Rejected" : "Screening")) + '" class="btn ' + (item.CreatedBy == 4 ? "btn-success" : (item.IsActive ? "btn-danger" : "btn-info")) + ' btn-xs"  ' + (item.ScreeningExamFeeNo == 0 ? "disabled" : "") + ' /></td>  <td style = "text-align:center" > <input type="checkbox" regno=' + item.Id + ' name=' + (item.LastExamFailedStatus ? (item.ScreeningExamFeeNo == 3 ? "no" : "yes") : "no") + ' term=' + item.ScreeningExamFeeNo + '  class=' + (item.LastExamFailedStatus ? (item.ScreeningExamFeeNo == 3 ? "hasbeen" : "initial") : "hasbeen") + ' term=' + item.ScreeningExamFeeNo + ' /></td>';
                    tblbody += '</tr>';
                });
                $('#tbpilotregistrationdata').append(tblbody);
            }
        },
        complete: function () {
            $("#tblPilotRegistration").DataTable({
                "order": [],
                "columnDefs": [{
                    "targets": 'no-sort',
                    "orderable": false,
                }],
                "pageLength": 50
            });
            StopLoading();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed!');
        }
    });
}

function openmedical() {
    $('#myMedicalModal').modal('show');
}

function getpilotScreenInitialInfo(regNo) {
    $.ajax({
        cache: false,
        type: "POST",
        url: "/PTA/Registration/GetPartialScreeningInfo",
        data: { RegNo: regNo },
        async: false,
        success: function (response) {
            if (response != null > 0) {
                $('#divscreenmodel').empty().append(response);
            }
        },
        complete: function () {
            StopLoading();
            $('#myModal').modal('show');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed!');
        }
    });
}

function OpenPilotMedicalPopUp(Id) {
    $('#divmedicalmodel').empty();
    $.ajax({
        cache: false,
        type: "GET",
        url: "/PTA/Registration/GetPartialMedicalInfo",
        data: { Id: Id },
        success: function (data) {
            $('#divmedicalmodel').empty().append(data);
            $('#myMedicalModal').modal('show');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed!');
        }
    });
}

function OpenPilotOtherDocsPopUp(Id, Adm) {
    $('#divmedicalmodel').empty();
    $.ajax({
        cache: false,
        type: "GET",
        url: "/PTA/Registration/GetPartialOtherDocsInfo",
        data: { Id: Id, AdmissionId: Adm },
        success: function (data) {
            $('#divOtherDocsmodel').empty().append(data);
            $('#myOtherDocsModal').modal('show');
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Failed!');
        }
    });
}


