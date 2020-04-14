var showChart = function (regno) {
    var model = {}; model.BatchNo = $('#BatchNo').val(); model.RegNo = $('#ddlReportCandidate').val(); model.ReviewId = $('#ReviewId').val(); if ($('#ddlWeeklyId').length > 0) { model.ddlWeeklyId = $('#ddlWeeklyId').val() }
    $.ajax({ url: "/PerformanceCard/Report/GetAssesmentChartInfo", type: "POST", async: !1, data: { Model: model }, success: function (response) { if (response != null) { $('#hdnsubchartinfo').val(response.SubchartString); $('#divBarChart').empty().append("<div class='chart'><canvas id='barChart' style='height:230px'></canvas></div>"); BarChart(response) } }, error: function (error) { } })
};


var showChartnew = function (regno) {
    var model = {};
    model.BatchNo = $('#BatchNo').val();
    model.RegNo = $('#ddlReportCandidate').val();
    model.ReviewId = $('#ReviewId').val();
    if ($('#ddlWeeklyId').length > 0) { model.ddlWeeklyId = $('#ddlWeeklyId').val(); }
    $.ajax({
        url: "/PerformanceCard/Report/GetAssesmentChartInfo",
        type: "POST",
        async: !1,
        data: { Model: model },
        success: function (response) {
            if (response != null) {
                newbarchart(response);
                $('#hdnsubchartinfo').val(response.SubchartString);
            }
        }, error: function (error) {
        }
    });
};

function newbarchart(obj) {
    $('#divNodeNodeParameter').css('display', 'block');
    $('#divNodeParameter').empty().append("<canvas id='opcanalysis'></canvas>");
    var db = [];

    if (obj.DataY != null) {
        db.push({
            label: 'Mid Term',
            backgroundColor: "#1b90fd",
            data: obj.DataY
        });
    }
    if (obj.DataX != null) {
        db.push({
            label: 'End Term',
            backgroundColor: "#faa",
            data: obj.DataX
        });
    }
    var barChartData1 = {
        labels: obj.BarChartLbl,
        datasets: db
    };
    var canvas = document.getElementById("opcanalysis");
    var ctx1 = canvas.getContext("2d");
    var myBar1 = new Chart(ctx1, {
        type: 'bar',
        data: barChartData1,
        options: {
            responsive: true,
            scales: {
                xAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Parameters'
                    },
                    barPercentage: 0.9,
                    maxRotation: 90,
                    minRotation: 90,

                    categoryPercentage: 0.8 / 10 * barChartData1.datasets[0].data.length
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Percentage(%)'
                    },
                    ticks: {
                        beginAtZero: true,
                        min: 0,
                        max: 100,
                        callback: function (value) {
                            return value + "%"
                        }
                    }
                }]
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        var label = data.datasets[tooltipItem.datasetIndex].label || '';

                        if (label) {
                            label += ': ';
                        }
                        label += Math.round(tooltipItem.yLabel * 100) / 100;
                        return label + " %";
                    }
                }
            }
        }
    });

    document.getElementById("opcanalysis").onclick = function (e) {
        var activePoint = myBar1.getElementAtEvent(e)[0];
        var data = activePoint._chart.data;
        var datasetIndex = activePoint._index;
        var label = data.labels[datasetIndex];
        newshowparameterchart(label);
    };
}


function newshowparameterchart(parametertypename) {
    $('#divNodeNode').css('display', 'block');
    $('#divsubNodeParameter').empty().append("<canvas id='punctualityreport'></canvas>");
    var subchartdata = $('#hdnsubchartinfo').val() != "" ? JSON.parse($('#hdnsubchartinfo').val()) : ""; if (subchartdata != "") {
        $.each(subchartdata, function (index, item) {
            if (item.Name == parametertypename) {
                var list = item.list; $.ajax({
                    url: "/PerformanceCard/Report/GetSubParameterData", type: "POST", async: !1, data: { ParameterType: parametertypename }, success: function (response) {
                        if (response != null && response.length > 0) {
                            var xAxis = []; var yAxis = []; var zAxis = []; $.each(response, function (a, t) {
                                var n = ""; if (response.length == 1)
                                    n = t.Name.length > 30 ? (t.Name.substring(0, 26) + "...") : t.Name; else if (response.length == 2)
                                    n = t.Name.length > 18 ? (t.Name.substring(0, 18) + "...") : t.Name; else n = t.Name.length > 14 ? (t.Name.substring(0, 14) + "...") : t.Name;
                                xAxis.push(n);
                                var temp = [];
                                var temp1 = [];
                                $.each(list, function (b, c) {
                                    if (t.ParameterId == c[0].TblParameterId) {
                                        if (c[0].ReviewName == "Mid Term")
                                            temp.push(c[0].Rating);
                                        if (c[0].ReviewName == "End Term")
                                            temp1.push(c[0].Rating);
                                    }
                                });
                                if (temp.length > 0) {
                                    yAxis.push((temp.reduce((a, b) => a + b, 0) * 100 / (temp.length * 3.00)).toFixed(2));
                                }

                                if (temp1.length > 0) {
                                    zAxis.push((temp1.reduce((a, b) => a + b, 0) * 100 / (temp1.length * 3.00)).toFixed(2));
                                }

                                if (temp1.length == 0 && temp.length == 0) {
                                    yAxis.push(0);
                                    zAxis.push(0);
                                }
                            });
                            var db = [];
                            var ddlinfo = $('#ReviewId option:selected').val();
                            if (ddlinfo == 2) {
                                db.push({
                                    label: 'Mid Term',
                                    backgroundColor: "#1b90fd",
                                    data: yAxis
                                });
                            } else if (ddlinfo == 3) {
                                db.push({
                                    label: 'End Term',
                                    backgroundColor: "#faa",
                                    data: zAxis 
                                });
                            } else {
                                db.push({
                                    label: 'Mid Term',
                                    backgroundColor: "#1b90fd",
                                    data: yAxis  
                                });
                                db.push({
                                    label: 'End Term',
                                    backgroundColor: "#faa",
                                    data: zAxis
                                });
                            }

                            var barChartData1 = {
                                labels: xAxis,
                                datasets: db
                            };
                            var ctx1 = document.getElementById("punctualityreport").getContext("2d");
                            var myBar1 = new Chart(ctx1, {
                                type: 'bar',
                                data: barChartData1,
                                options: {
                                    responsive: true,
                                    scales: {
                                        xAxes: [{
                                            display: true,
                                            scaleLabel: {
                                                display: true,
                                                labelString: 'Sub-Parameters'
                                            },
                                            barPercentage: 1,
                                            maxRotation: 90,
                                            minRotation: 90,

                                            categoryPercentage: 0.8 / 10 * barChartData1.datasets[0].data.length
                                        }],
                                        yAxes: [{
                                            display: true,
                                            scaleLabel: {
                                                display: true,
                                                labelString: 'Percentage(%)'
                                            },
                                            ticks: {
                                                beginAtZero: true,
                                                min: 0,
                                                max: 100,
                                                callback: function (value) {
                                                    return value + "%"
                                                }
                                            }
                                        }]
                                    },
                                    tooltips: {
                                        callbacks: {
                                            label: function (tooltipItem, data) {
                                                var label = data.datasets[tooltipItem.datasetIndex].label || '';

                                                if (label) {
                                                    label += ': ';
                                                }
                                                label += Math.round(tooltipItem.yLabel * 100) / 100;
                                                return label + " %";
                                            }
                                        }
                                    }
                                }
                            });
                            $('#divsubname').empty().text(parametertypename.toUpperCase());
                            //$('#divSubBarChart').empty().append("<div class='chart'><canvas id='subbarChart' style='height:230px'></canvas></div>"); SubBarChart(xAxis, yAxis); $('#subheading').text(parametertypename);
                        }
                        return !1
                    }, error: function (error) { }
                })
            }
        })
    } else { $('#divSubBarChart').empty().append("<div class='chart'><div id='subbarChart' style='height:230px;text-align:center;padding-top:80px;font-size:20px;'><span style='padding-top:50px;'> No Data Found </span></div></div>"); $('#subheading').text("(Review Percentage" + "/ " + parametertypename + ")") }
}







function getcomparereport(model) { $.ajax({ url: "/PerformanceCard/Report/GetAssesmentCompareChartInfo", type: "POST", async: !1, data: { Model: model }, success: function (response) { if (response != null) { $('#divReportChart1').empty().append("<div class='chart'><canvas id='barChart1' style='height:230px'></canvas></div>"); compareChart(response.DataX, response.DataY, response.BarChartLbl) } }, error: function (error) { } }) }
function showparameterchart(parametertypename) {
    var subchartdata = $('#hdnsubchartinfo').val() != "" ? JSON.parse($('#hdnsubchartinfo').val()) : ""; if (subchartdata != "") {
        $.each(subchartdata, function (index, item) {
            if (item.Name == parametertypename) {
                var list = item.list; $.ajax({
                    url: "/PerformanceCard/Report/GetSubParameterData", type: "POST", async: !1, data: { ParameterType: parametertypename }, success: function (response) {
                        if (response != null && response.length > 0) {
                            var xAxis = []; var yAxis = []; $.each(response, function (a, t) {
                                var n = ""; if (response.length == 1)
                                    n = t.Name.length > 30 ? (t.Name.substring(0, 26) + "...") : t.Name; else if (response.length == 2)
                                    n = t.Name.length > 18 ? (t.Name.substring(0, 18) + "...") : t.Name; else n = t.Name.length > 14 ? (t.Name.substring(0, 14) + "...") : t.Name; xAxis.push(n); var temp = []; $.each(list, function (b, c) { if (t.ParameterId == c[0].TblParameterId) { temp.push(c[0].Rating) } }); if (temp.length > 0)
                                    yAxis.push((temp.reduce((a, b) => a + b, 0) * 100 / (temp.length * 3)).toFixed()); else yAxis.push(0)
                            }); $('#divSubBarChart').empty().append("<div class='chart'><canvas id='subbarChart' style='height:230px'></canvas></div>"); SubBarChart(xAxis, yAxis); $('#subheading').text(parametertypename)
                        }
                        return !1
                    }, error: function (error) { }
                })
            }
        })
    } else { $('#divSubBarChart').empty().append("<div class='chart'><div id='subbarChart' style='height:230px;text-align:center;padding-top:80px;font-size:20px;'><span style='padding-top:50px;'> No Data Found </span></div></div>"); $('#subheading').text("(Review Percentage" + "/ " + parametertypename + ")") }
}
var BarChart = function (obj123) {
    $('#barChart').empty(); var db = []; if (obj123.DataY != null) { db.push({ label: 'Electronics', fillColor: 'rgba(210, 214, 222, 1)', strokeColor: 'rgba(210, 214, 222, 1)', pointColor: 'rgba(210, 214, 222, 1)', pointStrokeColor: '#c1c7d1', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(220,220,220,1)', data: obj123.DataY }) }
    if (obj123.DataX != null) { db.push({ label: 'Digital Goods', fillColor: 'rgba(60,141,188,0.9)', strokeColor: 'rgba(60,141,188,0.8)', pointColor: '#3b8bba', pointStrokeColor: 'rgba(60,141,188,1)', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(60,141,188,1)', data: obj123.DataX }) }

    var areaChartData = { labels: obj123.BarChartLbl, datasets: db, options: { events: ['click'] } }; var barChartCanvas = $('#barChart').get(0).getContext('2d'); var barChart = new Chart(barChartCanvas); var barChartData = areaChartData; var barChartOptions = { scaleOverride: !0, scaleStartValue: 0, scaleSteps: 10, scaleStepWidth: 10 }; barChartOptions.datasetFill = !1; var chartobj = barChart.Bar(barChartData, barChartOptions); document.getElementById("barChart").onclick = function (e) { showparameterchart(chartobj.getBarsAtEvent(e)[0].label) }; if ($('#ddlWeeklyId').length == 0) { var pieinfo = []; var TotalCandidate = obj123.PieChartData.reduce((partial_sum, a) => partial_sum + a); pieinfo.push(((obj123.PieChartData[0] * 100) / TotalCandidate).toFixed(2)); pieinfo.push(((obj123.PieChartData[1] * 100) / TotalCandidate).toFixed(2)); pieinfo.push(((obj123.PieChartData[2] * 100) / TotalCandidate).toFixed(2)); pieChart(pieinfo); setTimeout(function () { $('.anychart-credits-logo').empty(); $('.anychart-credits-text').empty() }, 200) }
};

function getpiechartinfo(obj) { return [['Mid Performance', obj[1]], ['High Performance', obj[2]], ['Low Performance', obj[0]]] }

var SubBarChart = function (xval, yval) { var areaChartData = { labels: xval, datasets: [{ label: 'Electronics', fillColor: 'rgba(223, 247, 24, 1)', strokeColor: 'rgba(223, 247, 24, 1)', pointColor: 'rgba(223, 247, 24, 1)', pointStrokeColor: '#c1c7d1', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(220,220,220,1)', data: yval }] }; var barChartCanvas = $('#subbarChart').get(0).getContext('2d'); var barChart = new Chart(barChartCanvas); var barChartData = areaChartData; var barChartOptions = { scaleOverride: !0, scaleStartValue: 0, scaleSteps: 10, scaleStepWidth: 10 }; barChartOptions.datasetFill = !1; barChart.Bar(barChartData, barChartOptions) }; function ceratecomparereportmaptitle(compare, status) { if (!status) { $('#spnreporttext1').text("Performance Card Analysis") } else { var batchno = $('#BatchNo').select2("data")[0].text === "Select Batch" ? "" : $('#BatchNo').select2("data")[0].text; var year = $('#ddlYear').select2("data")[0].text === "Select Year" ? "" : $('#ddlYear').select2("data")[0].text; var comyear = $('#ddlYear1').select2("data")[0].text === "Select Compare Year" ? "" : $('#ddlYear1').select2("data")[0].text; var month = $('#ddlMonth').select2("data")[0].text === "Select Month" ? "" : $('#ddlMonth').select2("data")[0].text; var commonth = $('#ddlMonth1').select2("data")[0].text === "Select Compare Month" ? "" : $('#ddlMonth1').select2("data")[0].text; var monthquarter = $('#ddlquarterMonth').select2("data")[0].text === "Select Quarter" ? "" : $('#ddlquarterMonth').select2("data")[0].text; var commonthquarter = $('#ddlquarterMonth1').select2("data")[0].text === "Select Compare Quarter" ? "" : $('#ddlquarterMonth1').select2("data")[0].text; if (compare == 1) { $('#spnreporttext1').text("Comparision for " + batchno + " during " + month + " & " + commonth + " " + year) } else if (compare == 2) { $('#spnreporttext1').text("Comparision for " + batchno + " during " + year + " & " + comyear) } else if (compare == 3) { $('#spnreporttext1').text("Comparision for " + batchno + " during Q1(" + monthquarter + ") & Q2(" + commonthquarter + ") " + year) } else { $('#spnreporttext1').text("Performance Card Analysis") } } }
function createweeklyreportcharttitle() {
    var batchno = $('#BatchNo').select2("data")[0].text === "Select Batch" ? "" : $('#BatchNo').select2('val'); var tarineename = $('#ddlReportCandidate').select2("data")[0].text === "Select Trainee" ? "" : $('#ddlReportCandidate').select2('val'); var weeklytermname = $('#ddlWeeklyId').select2("data")[0].text === "Select Weekly Term" ? "" : $('#ddlWeeklyId').select2('val'); var barchtext = 'Performance Card Analysis'; if (batchno !== "" && tarineename !== "" && weeklytermname !== "") {
        if (batchno === "0" && tarineename === "0" && weeklytermname === "0") { barchtext = "Overall Performance Card Analysis" }
        else if (batchno >= 0 && tarineename >= 0 && weeklytermname >= 0) {
            barchtext = $('#BatchNo').select2("data")[0].text + (tarineename === "0" ? " " : (" " + $('#ddlReportCandidate').select2("data")[0].text) + " ") + (weeklytermname === "0" ? "Overall" : $('#ddlWeeklyId').select2("data")[0].text) + " Performance Analysis"; if (batchno > 0)
                piechtext = $('#BatchNo').select2("data")[0].text + " Performance Analysis"
        }
        $('#spnbartext').text(barchtext)
    } else { $('#spnbartext').text("Performance Card Analysis") }
}
function createcharttitle() {
    var batchno = $('#BatchNo').select2("data")[0].text === "Select Batch" ? "" : $('#BatchNo').select2('val'); var tarineename = $('#ddlReportCandidate').select2("data")[0].text === "Select Trainee" ? "" : $('#ddlReportCandidate').select2('val'); var reviewname = $('#ReviewId').select2("data")[0].text === "Select Review" ? "" : $('#ReviewId').select2('val'); var barchtext = 'Performance Card Analysis'; var piechtext = 'Performance Analysis'; if (batchno !== "" && tarineename !== "" && reviewname !== "") {
        if (batchno === "0" && tarineename === "0" && reviewname === "0") { barchtext = "Overall Performance Card Analysis" }
        else if (batchno >= 0 && tarineename >= 0 && reviewname >= 0) {
            barchtext = $('#BatchNo').select2("data")[0].text + (tarineename === "0" ? " " : (" " + $('#ddlReportCandidate').select2("data")[0].text) + " ") + (reviewname === "0" ? "Overall" : $('#ReviewId').select2("data")[0].text) + " Performance Card Analysis"; if (batchno > 0)
                piechtext = $('#BatchNo').select2("data")[0].text + " Performance Analysis"
        }
        $('#spnbartext').text(barchtext); $('#spnpietext').text(piechtext)
    } else { $('#spnbartext').text("Performance Card Analysis"); $('#subheading').text("Sub-Parameter"); $('#spnpietext').text("Performance Analysis") }
}
var compareChart = function (x, y, label) {
    $('#barChart1').empty(); var db = []; if (x != null) { db.push({ label: 'Digital Goods', fillColor: 'rgba(60,141,188,0.9)', strokeColor: 'rgba(60,141,188,0.8)', pointColor: '#3b8bba', pointStrokeColor: 'rgba(60,141,188,1)', pointHighlightFill: '#fff', pointHighlightStroke: 'rgba(60,141,188,1)', data: x }) }
    var areaChartData = { labels: label, datasets: db }; var barChartCanvas = $('#barChart1').get(0).getContext('2d'); var barChart = new Chart(barChartCanvas); var barChartData = areaChartData; var barChartOptions = { scaleOverride: !0, scaleStartValue: 0, scaleSteps: 10, scaleStepWidth: 10 }; barChartOptions.datasetFill = !1; barChart.Bar(barChartData, barChartOptions)
}