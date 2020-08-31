function GetStateIdByStateName(stateName) { var id = 0; $.each(GetCityStateList(), function (i, item) { if (item.StateName == stateName) { id = item.Id; return !1 } }); return id }
function GetCityIdByCityName(stateId, cityName) { var id = 0; $.each(GetCityStateList(), function (i, item) { if (item.Id == stateId) { $.each(item.CityList, function (j, city) { if (city.CityName == cityName) { id = city.Id; return !1 } }) } }); return id }
function fillStateCityList(stateDDl, cityDDl) { $('#' + stateDDl).empty(); $('#' + cityDDl).empty(); var stateList = "<option value=''>Select State</option>"; $.each(GetCityStateList(), function (i, item) { stateList += "<option value=" + item.Id + ">" + item.StateName + "</option>" }); $('#' + cityDDl).append("<option>Select City</option>"); $('#' + stateDDl).append(stateList) }
function ddlStateChangeFillCity(obj, fillDDl) { var status = !1; var str = "<option>Select City</option>"; $('#' + fillDDl).empty(); $.each(GetCityStateList(), function (i, item) { if (item.Id == $(obj).val()) { $.each(item.CityList, function (j, n) { str += "<option value=" + n.Id + ">" + n.CityName + "</option>" }); status = !0 } }); $('#' + fillDDl).append(str) }
var lineChart1 = function (obj) { var areaChartData = { labels: obj.LineChartLbl, datasets: [{ label: 'BBA', fillColor: 'rgba(210, 214, 222, 1)', strokeColor: 'rgba(210, 214, 222, 1)', pointColor: 'rgba(210, 214, 222, 1)', pointStrokeColor: '#c1c7d1', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(220,220,220,1)', data: obj.line[0].value }, { label: 'MBA', fillColor: 'rgba(60,141,188,0.9)', strokeColor: 'rgba(60,141,188,0.8)', pointColor: '#3b8bba', pointStrokeColor: 'rgba(60,141,188,1)', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(60,141,188,1)', data: obj.line[1].value }, { label: 'PHT', fillColor: 'rgba(60,141,188,0.9)', strokeColor: 'rgb(237, 239, 112)', pointColor: 'rgb(237, 239, 112)', pointStrokeColor: 'rgba(60,141,188,1)', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(60,141,188,1)', data: obj.line[2].value }] }; var lineChartCanvas = $('#lineChart').get(0).getContext('2d'); var lineChart = new Chart(lineChartCanvas); var lineChartOptions = { showScale: !0, scaleShowGridLines: !1, scaleGridLineColor: 'rgba(0,0,0,.05)', scaleGridLineWidth: 1, scaleShowHorizontalLines: !0, scaleShowVerticalLines: !0, bezierCurve: !0, bezierCurveTension: 0.3, pointDot: !1, pointDotRadius: 4, pointDotStrokeWidth: 1, pointHitDetectionRadius: 20, datasetStroke: !0, datasetStrokeWidth: 2, datasetFill: !0, legendTemplate: '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].lineColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>', maintainAspectRatio: !0, responsive: !0 }; lineChartOptions.datasetFill = !1; lineChart.Line(areaChartData, lineChartOptions) }; var BarChart = function (obj123) { var areaChartData = { labels: obj123.BarChartLbl, datasets: [{ label: 'Electronics', fillColor: 'rgba(210, 214, 222, 1)', strokeColor: 'rgba(210, 214, 222, 1)', pointColor: 'rgba(210, 214, 222, 1)', pointStrokeColor: '#c1c7d1', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(220,220,220,1)', data: obj123.DataY }] }; var barChartCanvas = $('#barChart').get(0).getContext('2d'); var barChart = new Chart(barChartCanvas); var barChartData = areaChartData; barChartData.datasets[0].fillColor = '#00a65a'; barChartData.datasets[0].strokeColor = '#00a65a'; barChartData.datasets[0].pointColor = '#00a65a'; var barChartOptions = { scaleBeginAtZero: !0, scaleShowGridLines: !0, scaleGridLineColor: 'rgba(0,0,0,.05)', scaleGridLineWidth: 1, scaleShowHorizontalLines: !0, scaleShowVerticalLines: !0, barShowStroke: !0, barStrokeWidth: 2, barValueSpacing: 5, barDatasetSpacing: 1, responsive: !0, maintainAspectRatio: !0 }; barChartOptions.datasetFill = !1; barChart.Bar(barChartData, barChartOptions) }; var LineChart = function (obj) { var line = new Morris.Line({ element: 'line-chart', resize: !0, data: obj, xkey: 'y', ykeys: ['item1'], labels: ['Item 1'], lineColors: ['#3c8dbc'], hideHover: 'auto' }) }; var DonutChart = function (obj) {
    pendLabel = "Pending Candidates"; if (obj[0] == 0 && obj[1] == 0 && obj[2] == 0)
        pendLabel = "No Data Found"; var donut = new Morris.Donut({ element: 'sales-chart', resize: !0, colors: ["#3c8dbc", "#f56954", "#00a65a"], data: [{ label: pendLabel, value: obj[0] }, { label: "Rejected Candidates", value: obj[1] }, { label: "No. of Admission", value: obj[2] }], hideHover: 'auto' })
}; $('#ddlReportYears').change(function () { var yr = $(this).val(); if (yr != "") { var str = "<option selected value=''>Select Month</option><option value='1'>Janaury</option><option value='2'>February</option><option value='3'>March</option>"; str += "<option value='4'>April</option><option value='5'>May</option><option value='6'>June</option><option value='7'>July</option>"; str += "<option value='8'>August</option><option value='9'>September</option><option value='10'>October</option><option value='11'>November</option><option value='12'>December</option>"; $('#ddlReportMonth').empty().append(str) } else { $('#ddlReportMonth').empty().append("<option selected value=''>Select Month</option>") } }); $('.filter').change(function () {
    var yr = $('#ddlReportYears').val(); var courseId = $('#ddlReportCourse option:selected').text() == "Select Course" ? "" : $('#ddlReportCourse').val().join(); var batchId = $('#ddlReportBatch option:selected').text() == "Select Batch" ? "" : $('#ddlReportBatch').val().join(); var payStatus = $('#ddlReportPaymentStatus').val(); var state = $('#ddlState option:selected').text() == "Select State" ? "" : $('#ddlState option:selected').text(); var city = $('#ddlCity option:selected').text() == "Select City" ? "" : $('#ddlCity option:selected').text(); var month = $('#ddlReportMonth').val(); var pageName = location.pathname.split('/').slice(-1)[0]; if (pageName == "SSAReport") { Table.columns(6).search(yr); Table.columns(7).search(month); Table.columns(14).search(courseId); Table.columns(1).search(batchId); Table.columns(10).search(state); Table.draw() } else if (pageName == "RevenueReport") { showChart() } else {
        Table.columns(6).search(yr); Table.columns(7).search(month); Table.columns(8).search(courseId); Table.columns(11).search(batchId); Table.columns(14).search(payStatus); Table.columns(10).search(state); if (state != "")
            Table.columns(12).search(city);
        Table.draw()
    }
}); function printCanvas(id) { $("#" + id).printElement() }
var showChart = function () {
    ExportFileName(); var filterModel = { CourseId: $('#ddlReportCourse option:selected').text() == "Select Course" ? "" : $('#ddlReportCourse').val().join(), BatchId: $('#ddlReportBatch option:selected').text() == "Select Batch" ? "" : $('#ddlReportBatch').val().join(), Year: $('#ddlReportYears').val(), Month: $('#ddlReportMonth').val(), State: $('#ddlState option:selected').text() == "Select State" ? "" : $('#ddlState option:selected').text(), City: $('#ddlCity option:selected').text() == "Select City" ? "" : $('#ddlCity option:selected').text(), InstallmentNo: $('#ddlInstallment option:selected').text() == "Select Installment" ? 0 : $('#ddlInstallment option:selected').val() };
    $.ajax({ url: "/Report/GetChartData", type: "POST", async: !1, data: { FilterModel: filterModel }, success: function (response) { if (response != null) { $('#divBarChart').empty().append("<div class='chart'><canvas id='barChart' style='height:281px'></canvas></div>"); $('#divLine').empty().append("<div class='chart' style='height: 300px;'><canvas id='lineChart' style='height:300px'></canvas></div>"); BarChart(response.BarChartData); BarRegChart(response.BarChartRegistrationData); DonutChart(response.DonutChartData); lineChart1(response.LineChartData); $('#hdnbarcharregjsondata').val(exportBarchart(response.BarChartRegistrationData)); $('#hdnbarcharjsondata').val(exportBarchart(response.BarChartData)); $('#hdndonutcharjsondata').val(exportDonutchart(response.DonutChartData)); $('#hdnlinecharjsondata').val(exportLineChart(response.LineChartData)) } }, error: function (error) { } })
}; function ExportFileName() {
    var result = {}; var label = ''; var yr = $('#ddlReportYears').find(':selected').text() == "Select Year" ? "" : $('#ddlReportYears').find(':selected').text(); var mon = $('#ddlReportMonth').find(':selected').text() == "Select Month" ? "" : $('#ddlReportMonth').find(':selected').text(); var batch = []; var Course = []; var batches = $('#ddlReportBatch').select2('data'); if (batches.length > 0) { $.each(batches, function (i, item) { batch.push(item.text) }) }
    var courses = $('#ddlReportCourse').select2('data'); if (courses.length > 0) { $.each(courses, function (i, item) { Course.push(item.text) }) }
    result.time = 'Years'; if (yr != "") { label += yr; result.time = "Months"; if (mon != "") { label += "_" + mon; result.time = "Days" } }
    if (batch.length > 0) { label += "_" + batch.join(',') }
    result.othertext = label; if (Course.length > 0) { label += "_" + Course.join(',') }
    result.bercharttext = label; return result
}
function exportBarchart(data) {
    var jsonstr = ""; var result = []; if (data.BarChartLbl.length > 0) {
        var subtext = ExportFileName().time; for (var i = 0; i < data.BarChartLbl.length; i++) {
            var arr = {}; if (subtext == "Years")
                arr.Years = data.BarChartLbl[i]; else if (subtext == "Months")
                arr.Months = data.BarChartLbl[i]; else arr.Days = data.BarChartLbl[i]; arr.INR = data.DataY[i]; result.push(arr)
        }
    }
    jsonstr = JSON.stringify(result); return jsonstr
}
function exportDonutchart(data) {
    var jsonstr = ""; var result = []; if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            var arr = {}; if (i === 0)
                arr.Type_of_candidate = "Pending Candidates"; else if (i === 1)
                arr.Type_of_candidate = "Rejected Candidates"; else arr.Type_of_candidate = "No of Admissions"; arr.Value = data[i]; result.push(arr)
        }
    }
    jsonstr = JSON.stringify(result); return jsonstr
}
function exportLineChart(data) {
    var jsonstr = ""; var result = []; if (data.LineChartLbl.length > 0) {
        var subtext = ExportFileName().time; for (var i = 0; i < data.LineChartLbl.length; i++) {
            var arr = {}; if (subtext == "Years")
                arr.Year = data.LineChartLbl[i]; else if (subtext == "Months")
                arr.Months = data.LineChartLbl[i]; else arr.Days = data.LineChartLbl[i]; arr.BBA = data.line[0].value[i]; arr.MBA = data.line[1].value[i]; arr.PHT = data.line[2].value[i]; result.push(arr)
        }
    }
    jsonstr = JSON.stringify(result); return jsonstr
}
function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData; var CSV = ''; CSV += ReportTitle + '\r\n\n'; if (ShowLabel) {
        var row = ""; for (var index in arrData[0]) { row += index + ',' }
        row = row.slice(0, -1); CSV += row + '\r\n'
    }
    for (var i = 0; i < arrData.length; i++) {
        var row = ""; for (var index in arrData[i]) { row += '"' + arrData[i][index] + '",' }
        row.slice(0, row.length - 1); CSV += row + '\r\n'
    }
    if (CSV == '') { alert("Invalid data"); return }
    var fileName = ReportTitle; var uri = 'data:text/csv;charset=utf-8,' + escape(CSV); var link = document.createElement("a"); link.href = uri; link.style = "visibility:hidden"; link.download = fileName + ".csv"; document.body.appendChild(link); link.click(); document.body.removeChild(link)
}

var BarRegChart = function (obj123) {
    $('#barChartReg').empty().append("<div class='chart'><canvas id='barChart1' style='height:280px'></canvas></div>");
    var areaChartData = {
        labels: obj123.BarChartLbl,
        datasets: [
            {
                label: 'Digital Goods',
                fillColor: 'rgba(60,141,188,0.9)',
                strokeColor: 'rgba(60,141,188,0.8)',
                pointColor: '#3b8bba',
                pointStrokeColor: 'rgba(60,141,188,1)',
                pointHighlightFill: '#fff',
                pointHighlightStroke: 'rgba(60,141,188,1)',
                data: obj123.DataY 
            }
        ]
    }
    var barChartCanvas = $('#barChart1').get(0).getContext('2d')
    var barChart = new Chart(barChartCanvas)
    var barChartData = areaChartData
    var barChartOptions = {
        scaleBeginAtZero: true,
        scaleShowGridLines: true,
        scaleGridLineColor: 'rgba(0,0,0,.05)',
        scaleGridLineWidth: 1,
        scaleShowHorizontalLines: true,
        scaleShowVerticalLines: true,
        barShowStroke: true,
        barStrokeWidth: 1,
        barValueSpacing: 5,
        barDatasetSpacing: 1,
        legendTemplate: '<ul class="<%=name.toLowerCase()%>-legend"><% for (var i=0; i<datasets.length; i++){%><li><span style="background-color:<%=datasets[i].fillColor%>"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>',
        responsive: true,
        maintainAspectRatio: true
    }
    barChartOptions.datasetFill = false
    barChart.Bar(barChartData, barChartOptions)
}