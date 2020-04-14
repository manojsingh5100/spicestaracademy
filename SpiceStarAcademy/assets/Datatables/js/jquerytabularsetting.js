function exportOpt() { return [{ extend: 'excelHtml5', className: 'btn-primary' }] }
function getAjax(tbl, url) {
    if (tbl == "tblScreening" || tbl == "tblMedical")
        return {
            url: url, type: "POST", datatype: "json", data: { Tag: (tbl == "tblScreening" ? "Screen" : "Medical") }, error: function (jqXHR, exception) { } };
    else return { url: url, type: "POST", datatype: "json", error: function (jqXHR, exception) { } }
}
function tblSetting(exportBtn, url, cols, orderByIndex, tblName, orderBy) {
    return {
        responsive: true, "searchDelay": 1000, "page": 1, "serverSide": !0, stateSave: true, "filter": !0, "orderMulti": !1, "dom": 'Bfrtip', "buttons": exportBtn, "ajax": getAjax(tblName, url), "pageLength": deafultAllPaging(tblName),
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false
        }],
        "initComplete": function (settings, json) {
            StopLoading();
            checkMsg();
        },
        "columns": cols, "order": [[orderByIndex, orderBy]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex)
        {
            if (tblName == "tblAddmissionView") {
                if (aData.IsTermResgCandidate == "Termination")
                    $(nRow).css("background-color", "rgba(251, 132, 132, 0.16)");
                else if (aData.IsTermResgCandidate == "Resignation")
                    $(nRow).css("background-color", "rgba(251, 132, 132, 0.16)");
            }
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        }, orderCellsTop: !0, fixedHeader: !0,
        "fnInitComplete": function (oSettings, json) {
            StopLoading();
            if (TableCourseFilter != undefined && TableCourseFilter != "" && TableCourseFilter != null) {
                TableFilter(tblName, TableCourseFilter);
            }
            else if (tblName === "tblReport") {
                TableFilter(tblName, TableCourseFilter);
            }
            if (tblName == "tblScreening" || tblName == "tblMedical" || tblName == "tblAddmissionView") {
                addbatchOption('ddlbatchfilter');
                $("#ddlbatchfilter option[value='19']").remove();
            } else if (tblName == "tblFeeCandidatesList") {
                addbatchOption('ddlbatchfilter');
            }
        }
    };
}

function deafultAllPaging(tblName) {
    if (tblName == "tblSSAReport" || tblName == "tblReport")
        return 10000000;
    else
        return 10;
}
function coursefilter(obj, colnum) { Table.columns(colnum).search(obj.value).draw() }
function filterWithdrawalCandidate(obj) { Table.columns(7).search(obj.value).draw() }
function trfilter(obj) { Table.columns(3).search(obj.value).draw() }
function filterBatch(obj, colnum) { var objName = $("option:selected", obj).text() == "Fiter By Batch" ? "" : $("option:selected", obj).text().trim(); Table.columns(colnum).search(objName).draw() }
function filterSSAReportBatch(obj, colnum) { var objName = $("option:selected", obj).text() == "Fiter By Batch" ? "" : $("option:selected", obj).text().trim(); Table.columns(colnum).search(objName).draw() }
function feeStatusFilter(obj, colnum) { Table.columns(colnum).search(obj.value).draw(); }

function pagelength(obj) {
    Table.page.len($(obj).val()).draw(); 
}
function screenningstatusfilter(obj) { Table.columns(10).search(obj.value).draw(); }
function screenningbatchfilter(obj) { Table.columns(7).search(obj.value).draw(); }
function filterAction(obj) { Table.columns(10).search(obj.value).draw(); }

function btnexport() {
    $('.buttons-excel').trigger('click');
}
