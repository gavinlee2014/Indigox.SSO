define("/SSO/Widgets/AccessRecord/SearchAccessRecordSchema",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register("SearchAccessRecord", {
        "columns": [
        {
            "name": "AccountName",
            "text": "用户名"
        }, {
            "name": "IP",
            "text": "登录IP"
        }, {
            "name": "ServiceName",
            "text": "系统名"
        }, {
            "name": "ServiceID",
            "text": "系统编号"
        }, {
            "name": "LogTimeBegin",
            "text": "操作时间Begin"
        }, {
            "name": "LogTimeEnd",
            "text": "操作时间End"
        }]
    });
});