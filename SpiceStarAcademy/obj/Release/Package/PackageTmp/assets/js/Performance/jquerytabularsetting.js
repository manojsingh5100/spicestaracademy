function exportOpt() { return ['copyHtml5', 'excelHtml5', 'csvHtml5', { extend: 'pdfHtml5', exportOptions: { columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] } }] }
function getAjax(url) { return { url: url, type: "POST", datatype: "json", error: function (jqXHR, exception) { } } }
function getAjax1(url, id) { return { url: url, type: "POST", datatype: "json", data: { ParameterTypeId: id }, error: function (jqXHR, exception) { } } }

function tblSetting(exportBtn, url, cols, orderByIndex, tblName, orderBy) {
    return {
        "searchDelay": 1000, "pageLength": 25, "serverSide": !0, "filter": !0, "orderMulti": !1, "dom": 'Bfrtip', "buttons": exportBtn, "ajax": getAjax(url), "initComplete": function (settings, json) { StopLoading() }, "columns": cols,
        "bStateSave": true,
        "fnStateSave": function (oSettings, oData) {
            localStorage.setItem('DataTables_' + window.location.pathname, JSON.stringify(oData));
        },
        "fnStateLoad": function (oSettings) {
            var data = localStorage.getItem('DataTables_' + window.location.pathname);
            return JSON.parse(data);
        },
        "order": [[orderByIndex, orderBy]], "fnRowCallback": function (nRow, aData, iDisplayIndex) { $("td:first", nRow).html(iDisplayIndex + 1); return nRow }, orderCellsTop: !0, fixedHeader: !0
    }
}

function tblSetting1(exportBtn, url, cols, orderByIndex, tblName, orderBy, parametertypeid) {
    url = "/PerformanceCard" + url; return {
        "searchDelay": 1000, "serverSide": !0, "pageLength": 25, "filter": !0, "orderMulti": !1, "dom": 'Bfrtip', "buttons": exportBtn, stateSave: true, "ajax": getAjax1(url, parametertypeid), "initComplete": function (settings, json) {
            if ($('#tblParameterList').length > 0)
                getKeyCode(); StopLoading()
        }, "columns": cols, "order": [[orderByIndex, orderBy]], "fnRowCallback": function (nRow, aData, iDisplayIndex) { $("td:first", nRow).html(iDisplayIndex + 1); return nRow }, orderCellsTop: !0, fixedHeader: !0
    }
}
function format(d) {
    var str = '<table id="tbl" class="table table-bordered" cellpadding="5" cellspacing="0" border="0" style="width:68%" >'; if (d.PerformanceCardList.length > 0) { str += '<tr><th>Sno</th><th> Review Name </th><th> Weekly Term Name </th><th>Action</th></tr>'; $.each(d.PerformanceCardList, function (index, item) { str += '<tr><td> ' + (index + 1) + ' </td><td> ' + item.ReviewName + '  </td><td> ' + (item.WeeklyTermName != null ? item.WeeklyTermName : '') + ' </td><td> <input type="button" onclick="ShowCardByReview(' + d.RegistartionNo + ',' + item.ReviewId + ',' + item.WeeklyTermId + ');" class="btn btn-info btn-xs" value="Show Performance Card" /> </td></tr>' }) } else { str += '<tr> <td colspan=4 style="text-align:center"> No Record Found!</td> </tr>' }
    str += '</table>'; return str
}
var ShowCardDetail = $('#tblCandidateList tbody').on('click', 'td.details-control', function () {
    var tr = $(this).closest('tr'); var row = Table.row(tr); if (row.child.isShown()) { row.child.hide(); tr.removeClass('shown') }
    else { row.child(format(row.data())).show(); tr.addClass('shown') }
})