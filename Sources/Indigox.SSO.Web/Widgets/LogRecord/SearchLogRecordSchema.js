define("/SSO/Widgets/LogRecord/SearchLogRecordSchema",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register("SearchLogRecord", {
        "columns": [
        {
            "name": "AccountName",
            "text": "用户名"
        }, {
            "name": "IP",
            "text": "登录IP"
        }, {
            "name": "LogTimeBegin",
            "text": "操作时间Begin"
        }, {
            "name": "LogTimeEnd",
            "text": "操作时间End"
        }]
    });
});