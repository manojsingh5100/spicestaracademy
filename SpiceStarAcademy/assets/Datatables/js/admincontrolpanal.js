function ReportBatchSetting(t) {
    return {
        searchDelay: 1e3, serverSide: !0, filter: !0, orderMulti: !1, lengthMenu: [[5, 10, 20], [5, 10, 20]], ajax: { url: "/Admin/GetBatchListinfo", type: "POST", datatype: "json", error: function (t, e) { } }, columns: [{ data: "Id" }, { data: "Name" }, { data: "ActiveStr" },
            {
                "data": "BatchStartDate", "render": function (data, type, row) {
                    if (row.BatchStartDate != null)
                        return DateFormat1(row.BatchStartDate);
                    else
                        return "";
                }
            },
            {
                "data": "BatchEndDate", "render": function (data, type, row) {
                    if (row.BatchEndDate != null)
                        return DateFormat1(row.BatchEndDate);
                    else
                        return "";
                }
            }
            , {
                mRender: function (t, e, r) { return '<a href="#" onclick="openBatchModel(' + r.Id + ');">Edit</a>' }
        }], order: [[0, "asc"]], fnRowCallback: function (t, e, r) { return $("td:first", t).html(r + 1), t }
    }
}
