function FilterTable() {
    index = -1;
    inp = $('#filterBox').val();
    $("#data:visible tr:not(:has(>th))").each(function () {
        if (~$(this).text().toLowerCase().indexOf(inp.toLowerCase())) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
    $('#Hedding').show();
}

$(document).ready(function () {
    inp = $("#branch_name option:selected").text();
    //if (inp == "All Branch") {
    //    $("#branch_name").attr('disabled', false);
    //}
    //else $("#branch_name").attr('disabled', true);

    FilterTable2();
    $("#branch_name").change(function () {
        // var selectedBranch = $("#branch_name option:selected").text();
        FilterTable2();

    });
});

function FilterTable2() {
    index = -1;
    inp = $("#branch_name option:selected").text();
    $("#data:visible tr").hide();
    if (inp == "All Branch") {
        inp = "";
    }

    $("#data:visible tr:not(:has(>th))").each(function (i, el) {

        let elVal = $.trim($("#data:visible tr:nth-child(" + (i+1) + ") td:nth-child(2)").text());
        console.log(elVal);

        if (elVal.toLowerCase().includes(inp.toLowerCase())) {
            $(this).show();
        } else {
            //$(this).hide();
        }
    });
    $('#Hedding').show();
};


function SetActivationStatus(user_id, is_activate) {
    $.ajax({
        url: '../BranchUsers/SetActivationStatus',
        type: 'POST',
        dataType: 'json',
        data: { user_id: user_id, is_activate: is_activate },
        success: function (data) {
            if (data.success == false) {

                alert(data.Message);
            }
            else {
               
                var viewURL = '../BranchUsers/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function UntagBranchVerify(s_user_id, s_branch_id, s_transfer_by, s_userBranchId, s_branchName) {
    SetUntagInfo(s_user_id, s_branch_id, s_transfer_by, s_userBranchId);
    modalConfirm("Do you want to Un Tag Branch Admin of " + s_branchName + "?", UntagBranch, "");
}

var user_id = "";
var branch_id = "";
var transfer_by = "";
var userBranchId = 0;

function SetUntagInfo(s_user_id, s_branch_id, s_transfer_by, s_userBranchId) {
    user_id = s_user_id;
    branch_id = s_branch_id;
    transfer_by = s_transfer_by;
    userBranchId = s_userBranchId;
}

function UntagBranch() {
    $.ajax({
        url: '../BranchUsers/UntagBranch',
        type: 'POST',
        dataType: 'json',
        data: { _user_id: user_id, _branch_id: branch_id, _transfer_by: transfer_by, _userBranchId: userBranchId },
        success: function (data) {
            if (data.success == "1") {
                alert(data.message);
            }
            else {

                var viewURL = '../BranchUsers/index';
                window.location = viewURL;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}

function SyncUser() {
    $.ajax({
        url: '../BranchUsers/SyncUsers',
        type: 'Get',
        dataType: 'json',
        success: function (result) {

            if (result.sync) {
                modalAlert("User synced.");
            }

            setTimeout(function () {
                location.reload();
            }, 2000);
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
        }
    });
}