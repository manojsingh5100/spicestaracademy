function parameterTypeListingCol() {
    return [{ "data": "Id", "bSortable": !1 }, { "data": "Name" }, {
        "data": "IsActive", "render": function (data, type, row) {
            if (row.IsActive)
                return "Active"; else return "InActive";
        }
    },
    { "mRender": function (data, type, row) { return '<input type="button" value="ADD SUB PARAMETER" onclick="gotoaddparameter(' + row.Id + ')" class="performance-card" />' }, "bSortable": !1 },
    { "mRender": function (data, type, row) { return '<input type="image" src="/Areas/PerformanceCard/Images/edit.png" onclick="changeImage(this,' + row.Id + ')" />' }, "bSortable": !1 }
    ]
}
function parameterListingCol() {
    return [{ "data": "ParameterId", "bSortable": !1 }, { "data": "KeyId" }, { "data": "Name" },
    {
        "data": "Gender", "render": function (data, type, row) {
            if (row.Gender === 'F')
                return "Female";
            else if (row.Gender === 'M')
                return "Male";
            else
                return "All";
        }
    },
    //{
    //    "data": "ReviewId", "render": function (data, type, row) {
    //        if (row.ReviewId === 2)
    //            return "Mid Term";
    //        else if (row.ReviewId === 3)
    //            return "End Term";
    //        else
    //            return "All";
    //    }
    //},
    {
        "data": "IsActive", "render": function (data, type, row) {
            if (row.IsActive)
                return "Active"; else return "InActive";
        }, "bSortable": !1
    },
    { "mRender": function (data, type, row) { return '<input type="image" src="/Areas/PerformanceCard/Images/edit.png" onclick="changeImage(this,' + row.ParameterId + ')" />' }, "bSortable": !1 }]

}
function candidateListingCol() {
    return [{ "data": "Id", "bSortable": !1 }, { "data": "RegistartionNo" }, { "data": "Fname" }, { "data": "Email" }, { "data": "Mobile" }, { "data": "CourseName" }, { "data": "BatchName" }, {
        "mRender": function (data, type, row) {
            return row.IsResistrationHistory ? '<button type="button" class="btn btn-success" title="Relegated info." style="Padding:0px 8px !important;" onclick="openReliagtePopUp(' + row.RegistartionNo + ');" >Relegated</button>' : "";
            //return row.IsResistrationHistory ? '<button type="button" class="btn btn-success" title="Religated info." style="Padding:0px 8px !important;" onclick="openReliagtePopUp(' + row.RegistartionNo + ');" ><i class="fa fa-info" aria-hidden="true"></i></button>' : "";
        }
    },
    { "mRender": function (data, type, row) { return '<input type="button" value="PERFORMANCE CARD" onclick="gotoperformance(\'' + row.RegistartionNo + '\')" class="performance-card" />' }, "bSortable": !1 },
    { "mRender": function (data, type, row) { return '<input type="image" src="/Areas/PerformanceCard/Images/reports.png" onclick="GotoShowReport(' + row.RegistartionNo + ')" />' }, "bSortable": !1 },
    {
        "mRender": function (data, type, row) {
            var str = '';
            if (row.PerformanceCardList.length > 0) {
                $.each(row.PerformanceCardList, function (i, index) {
                    if (index.ReviewId == 2) {
                        str += '<span onclick="redirecttofilledcard(' + row.RegistartionNo + ',' + index.ReviewId + ');" style="background-color:green;border-radius:25px;color: #fff; height:10px;width:20px;padding: 5px 5px 2px 5px;font-weight: bold;cursor:pointer">M</span>';
                        if (row.PerformanceCardList.length == 1) {
                            if (index.ReviewId != 1) {
                                str += ' <span style="background-color:red;border-radius:25px;color: #fff; height:10px;width:20px;padding: 5px 5px 2px 5px;font-weight: bold;">E</span>';
                            }
                        }
                    }
                    else if (index.ReviewId == 3) {
                        if (row.PerformanceCardList.length == 1) {
                            if (index.ReviewId != 1) {
                                str += '<span style="background-color:red;border-radius:25px;color: #fff; height:10px;width:20px;padding: 5px 5px 2px 5px;font-weight: bold;">M</span>';
                            }
                        }
                        str += ' <span onclick="redirecttofilledcard(' + row.RegistartionNo + ',' + index.ReviewId + ');" style="background-color:green;border-radius:25px;color: #fff; height:10px;width:20px;padding: 5px 5px 2px 5px;font-weight: bold;cursor:pointer">E</span>';
                    }
                });
            }
            else {
                str += '<span style="background-color:red;border-radius:25px;color: #fff; height:10px;width:20px;padding: 5px 5px 2px 5px;font-weight: bold;">M</span> <span style="background-color:red;border-radius:25px;color: #fff; height:10px;width:20px;padding: 5px 5px 2px 5px;font-weight: bold;">E</span>';
            }
            return str;
        }, "bSortable": !1
    }];
}

function GotoShowReport(regno) { return window.location.href = "/PerformanceCard/Report?RegNo=" + regno }

function redirecttofilledcard(reg, review) {
    window.location.href = "/PerformanceCard/Performance?RegNo=" + reg + "&Review=" + review;
}