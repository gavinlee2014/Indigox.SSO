define("/SSO/Widgets/LogRecord/SearchLogRecordContentType", function () {

    var SearchLogRecordContentType = {
        controls: [
        {
            controlType: "fieldset",
            rowFields: 2,
            items: [
                {
                    controlType: "fieldcontainer",
                    text: "用户名",
                    children: [{
                        controlType: "textbox",
                        name: "AccountName"
                    }]
                },
                {
                    controlType: "fieldcontainer",
                    text: "登录IP",
                    children: [{
                        controlType: "textbox",
                        name: "IP"
                    }]
                }
            ]
        }, 
         {
                controlType: "fieldset",
                items: [{
                    controlType: "fieldcontainer",
                    text: "操作时间",
                    children: [{
                        controlType: "datepicker",
                        editHMS: false,
                        name: "LogTimeBegin",
                        width: "40%"
                    }, {
                        controlType: "label",
                        value: "至",
                        width: "5%"
                    }, {
                        controlType: "datepicker",
                        editHMS: false,
                        name: "LogTimeEnd",
                        width: "40%"
                    }]
                }]
            }]
    };

    return SearchLogRecordContentType;
});