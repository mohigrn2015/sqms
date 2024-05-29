function GlobalSettingsValidation() {
    var tokenPrefix = $('#default_token_prefix').val();
    var msgTxt = $('#msg_text').val();
    if (tokenPrefix.length > 1) {
        modalAlert("Valid Default Token Prefix is only 1 digit")
        return false;
    }
    else if (!msgTxt.includes("{0}")) {
        modalAlert("Please enter {0} in Message Text as per token number position in message")
        return false;
    }
    else {
        return true;
    }
}

