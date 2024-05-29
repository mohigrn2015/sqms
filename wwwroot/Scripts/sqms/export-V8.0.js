


function ExportLocalCustomerReport(fileType) {

    var reportName = 'LocalIconCoustomerReport';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var customer_type_id = $('#customer_type_id').val();
    var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;



    var viewURL = '../Report/ExportLocalCustomerReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&customer_type_id=' + customer_type_id + '&service_sub_type_id=' + service_sub_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;
    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};



function ExportSinglevsMultipleVisitSummaryReport(fileType) {
    var reportName = 'SingleVSMultipleVisitedSummary';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();

    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;


    //int branch_id, string user_id, int counter_id, int service_sub_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = ""

    var viewURL = '../Report/ExportSinglevsMultipleVisitSummaryReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&service_sub_type_id=' + service_sub_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;
    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};



function ExportAgentWiseSummaryReport(fileType) {
    var reportName = 'AgentWiseSummary';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;
    
    var viewURL = '../Report/ExportAgentWiseSummaryReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&service_sub_type_id=' + service_sub_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;
    window.location = viewURL;
    return false;
};



function ExportServiceSummaryReport(fileType) {
    var reportName = 'ServiceWiseSummuryReport';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    var viewURL = '../Report/ExportServiceSummaryReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&service_sub_type_id=' + service_sub_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;

    window.location = viewURL;
    return false;
};


function ExportGeneralSearchReport(fileType) {
    var reportName = 'GeneralSearch';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var msisdn_no = $('#txtMSISDN').val();
    var service_sub_type_id = $('#service_sub_type_id').val();
    var token_no = $('#txtToken').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    
    var viewURL = '../Report/ExportGeneralSearchReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&msisdn_no=' + msisdn_no + '&service_sub_type_id=' + service_sub_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&token_no=' + token_no + '&report_Name=' + reportName + '&file_Type=' + fileType;


    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};


function ExportBreakReport(fileType) {
    var reportName = 'BreakReport';
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var customer_type_id = 0;
    //if ($('#service_sub_type_id').val())
    var break_type_id = $('#break_type_id').val();
    //var token_no = $('#txtToken').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    //int branch_id, string user_id, int counter_id, int break_type_id, DateTime start_date, DateTime end_date, string report_Name = "", string file_Type = ""

    var viewURL = '../Report/ExportBreakReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&break_type_id=' + break_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;


    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};



function ExportTopNServiceReport(fileType) {
    var reportName = 'TopFiveServicesReport';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();

    var topn = $('#txtTopN').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    var viewURL = '../Report/ExportTopNServiceReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&topn=' + topn + '&report_Name=' + reportName + '&file_Type=' + fileType;

    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};


function ExportTokenExceedingReport(fileType) {
    var reportName = '10MinExceedingReport';

    var flag = $('#txtFlag').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();
    var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    var viewURL = '../Report/ExportTokenExceedingReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&service_sub_type_id=' + service_sub_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&flag=' + flag + '&report_Name=' + reportName + '&file_Type=' + fileType;

    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};


function ExportLogoutDetailReport(fileType) {
    var reportName = 'LogOutDetailsReport';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var user_id = $('#txtUser').val();


    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    var viewURL = '../Report/ExportLogoutDetailReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;

    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};


function ExportLoginAttemptDetailsReport(fileType) {
    var reportName = 'LoginAttemptDetailsReport';

    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    var counter_id = $('#txtCounter').val();
    var is_success = $('#is_success').val();
    var user_id = $('#txtUser').val();


    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    var viewURL = '../Report/ExportLoginAttemptDetailsReport/?branch_id=' + branch_id + '&user_id=' + user_id + '&counter_id=' + counter_id + '&is_success=' + is_success + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;

    window.location = viewURL;
    //loadReport(viewURL, reportName);
    return false;
};

function ExportCentreWiseSummaryReport(fileType) {
    var reportName = 'ExportCentreWiseSummary';

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    var fromDate = $('#txtFromDate').val();
    var toDate = $('#txtToDate').val();
    var branch_id = $('#branch_id').val();
    if ($('#service_sub_type_id').val())
        var service_sub_type_id = $('#service_sub_type_id').val();

    if ((fromDate == "" || toDate == "")) {
        modalAlert("Please Fill To Date & From Date");
        ShowPannel(1);
        return false;
    }

    var startTime = $('#txtStartTime').val();
    var endTime = $('#txtEndTime').val();
    fromDate = fromDate + ' ' + startTime;
    toDate = toDate + ' ' + endTime;

    var viewURL = '../Report/ExportCentreWiseSummaryReport/?branch_id=' + branch_id + '&service_sub_type_id=' + service_sub_type_id + '&start_date=' + fromDate + '&end_date=' + toDate + '&report_Name=' + reportName + '&file_Type=' + fileType;
    window.location = viewURL;
    return false;
};