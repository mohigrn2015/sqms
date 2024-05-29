var bgColors = [
    'rgba(255,99,132,1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
];

var countersTokens_names = [];
var countersTokens_tokens = [];
var servicesWaitings_names = [];
var servicesWaitings_tokens = [];
var servicesTokens_names = [];
var servicesTokens_tokens = [];


function FilterTable() {
    index = -1;
    inp = $('#filterBox').val();

    $("#tblBranchServiceDetailList:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
};


$(document).ready(function () {
    loadData();
});

function reloadChart() {
    countersTokens_names = [];
    countersTokens_tokens = [];
    servicesWaitings_names = [];
    servicesWaitings_tokens = [];
    servicesTokens_names = [];
    servicesTokens_tokens = [];
    loadData();

    return false;
}

function loadData() {
    var id = $("#branch_id option:selected").val();
    $.ajax({
        url: '../ApiService/GetAdminDashboard',
        type: 'POST',
        data: {id:id},
        dataType: 'json',
        success: function (result) {
            $.each(result.data, function (i, data) {
                countersTokens_names.push('C' + data.counter_no);
                countersTokens_tokens.push(data.tokens);
            });

            $.each(result.servicesWaitings, function (i, data) {
                servicesWaitings_names.push(data.service_name);
                servicesWaitings_tokens.push(data.tokens);
            });

            $.each(result.servicesTokens, function (i, data) {
                servicesTokens_names.push(data.service_name);
                servicesTokens_tokens.push(data.tokens);
            });

            $("#tblBranchTokenList").find("tr:gt(0)").remove();
            $.each(result.branchTokenList, function (i, item) {
                $("#tblBranchTokenList")
                    .append($('<tr>')
                        .append($('<td>')
                            .append(item.service_name)
                        )
                        .append($('<td>')
                            .append(item.total)
                        )
                        .append($('<td>')
                            .append(item.served)
                        )
                        .append($('<td>')
                            .append(item.serving)
                        )
                        .append($('<td>')
                            .append(item.waiting)
                        )
                        .append($('<td>')
                            .append(item.missing)
                        )
                    );
            });
            $('#tblBranchTokenList tr:contains("TOTAL")').attr('id', 'Hedding');

            $("#tblBranchServiceList").find("tr:gt(0)").remove();
            $.each(result.branchServiceList, function (i, item) {
                $("#tblBranchServiceList")
                    .append($('<tr>')
                        .append($('<td>')
                            .append(item.service_name)
                        )
                        .append($('<td>')
                            .append(item.served)
                        )
                    );
            });
            $('#tblBranchServiceList tr:contains("TOTAL")').attr('id', 'Hedding');

            $("#tblBranchServiceDetailList").find("tr:gt(0)").remove();
            $.each(result.branchServiceDetailList, function (i, item) {
                $("#tblBranchServiceDetailList")
                    .append($('<tr>')
                        .append($('<td>')
                            .append(item.branch_name)
                        )
                        .append($('<td>')
                            .append(item.counter)
                        )
                        .append($('<td>')
                            .append(item.token_no_formated)
                        )
                        .append($('<td>')
                            .append(item.customer_type)
                        )
                        .append($('<td>')
                            .append(item.service)
                        )
                        .append($('<td>')
                            .append(item.issue_time_formated)
                        )
                        .append($('<td>')
                            .append(item.start_time_formated)
                        )
                        .append($('<td>')
                            .append(item.end_time_formated)
                        )
                        .append($('<td>')
                            .append(item.service_status)
                        )
                    );
            });

            generateCountersTokensChart();
            generateServicesWaitingsChart();
            generateServicesTokensChart();
            $("#currentDate").html(getCurrentDate());
        }
    });

}

function generateCountersTokensChart() {

    var helpers = Chart.helpers;
    new Chart(document.getElementById("countersTokenChart"), {
        "type": "pie",
        "data": {
            "labels": countersTokens_names,
            "datasets": [{
                "label": "Counter Wise Report",
                "data": countersTokens_tokens,
                "backgroundColor": bgColors
            }]
        },
        options: {
            responsive: true,
            legend: {
                position: 'bottom',
                labels: {
                    generateLabels: function (chart) {
                        var data = chart.data;
                        if (data.labels.length && data.datasets.length) {
                            return data.labels.map(function (label, i) {
                                var meta = chart.getDatasetMeta(0);
                                var ds = data.datasets[0];
                                var arc = meta.data[i];
                                var custom = arc && arc.custom || {};
                                var valueAtIndexOrDefault = helpers.valueAtIndexOrDefault;
                                var arcOpts = chart.options.elements.arc;
                                var fill = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(ds.backgroundColor, i, arcOpts.backgroundColor);
                                var stroke = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(ds.borderColor, i, arcOpts.borderColor);
                                var bw = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(ds.borderWidth, i, arcOpts.borderWidth);

                                return {
                                    text: label + " : " + ds.data[i],
                                    fillStyle: fill,
                                    strokeStyle: stroke,
                                    lineWidth: bw,
                                    hidden: isNaN(ds.data[i]) || meta.data[i].hidden,

                                    // Extra data used for toggling the correct item
                                    index: i
                                };
                            });
                        }
                        return [];
                    }
                }
            },
            title: {
                display: true,
                text: 'Counter Wise Report'
            },
            animation: {
                animateScale: true,
                animateRotate: true
            }
        }
    });
}

function generateServicesWaitingsChart() {

    var helpers = Chart.helpers;
    new Chart(document.getElementById("servicesWaitingChart"), {
        "type": "bar",
        "data": {
            "labels": servicesWaitings_names,
            "datasets": [{
                "label": "Service Wise Waiting Token Status",
                "data": servicesWaitings_tokens,
                "backgroundColor": bgColors
            }]
        },
        options: {
            responsive: true,
            legend: {
                position: 'bottom',
                labels: {
                    generateLabels: function (chart) {
                        var data = chart.data;
                        if (data.labels.length && data.datasets.length) {
                            return data.labels.map(function (label, i) {
                                var meta = chart.getDatasetMeta(0);
                                var ds = data.datasets[0];
                                var arc = meta.data[i];
                                var custom = arc && arc.custom || {};
                                var valueAtIndexOrDefault = helpers.valueAtIndexOrDefault;
                                var arcOpts = chart.options.elements.arc;
                                var fill = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(ds.backgroundColor, i, arcOpts.backgroundColor);
                                var stroke = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(ds.borderColor, i, arcOpts.borderColor);
                                var bw = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(ds.borderWidth, i, arcOpts.borderWidth);

                                return {
                                    text: label + " : " + ds.data[i],
                                    fillStyle: fill,
                                    strokeStyle: stroke,
                                    lineWidth: bw,
                                    hidden: isNaN(ds.data[i]) || meta.data[i].hidden,

                                    // Extra data used for toggling the correct item
                                    index: i
                                };
                            });
                        }
                        return [];
                    }
                }
            },
            title: {
                display: true,
                text: 'Service Wise Waiting Token Status'
            },
            animation: {
                animateScale: true,
                animateRotate: true
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}


function generateServicesTokensChart() {

    var helpers = Chart.helpers;
    new Chart(document.getElementById("servicesTokenChart"), {
        "type": "pie",
        "data": {
            "labels": servicesTokens_names,
            "datasets": [{
                "label": "Service Wise Report",
                "data": servicesTokens_tokens,
                "backgroundColor": bgColors
            }]
        },
        options: {
            responsive: true,
            legend: {
                display: false
                //position: 'bottom',
                //labels: {
                //    generateLabels: function (chart) {
                //        var data = chart.data;
                //        if (data.labels.length && data.datasets.length) {
                //            return data.labels.map(function (label, i) {
                //                var meta = chart.getDatasetMeta(0);
                //                var ds = data.datasets[0];
                //                var arc = meta.data[i];
                //                var custom = arc && arc.custom || {};
                //                var valueAtIndexOrDefault = helpers.valueAtIndexOrDefault;
                //                var arcOpts = chart.options.elements.arc;
                //                var fill = custom.backgroundColor ? custom.backgroundColor : valueAtIndexOrDefault(ds.backgroundColor, i, arcOpts.backgroundColor);
                //                var stroke = custom.borderColor ? custom.borderColor : valueAtIndexOrDefault(ds.borderColor, i, arcOpts.borderColor);
                //                var bw = custom.borderWidth ? custom.borderWidth : valueAtIndexOrDefault(ds.borderWidth, i, arcOpts.borderWidth);

                //                return {
                //                    text: label + " : " + ds.data[i],
                //                    fillStyle: fill,
                //                    strokeStyle: stroke,
                //                    lineWidth: bw,
                //                    hidden: isNaN(ds.data[i]) || meta.data[i].hidden,

                //                    // Extra data used for toggling the correct item
                //                    index: i
                //                };
                //            });
                //        }
                //        return [];
                //    }
                //}
            },
            title: {
                display: true,
                text: 'Service Wise Report'
            },
            animation: {
                animateScale: true,
                animateRotate: true
            }
        }
    });
}