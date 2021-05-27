define("Indigox/SSO/Application/LogRecord",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('LogRecord', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'AccountName', text: '用户名', type: String },
            { name: 'IP', text: '登录IP', type: String },
            { name: 'LogTime', text: '登录时间', type: String },
            { name: 'Action', text: '类别', type: String }
        ],
        primaryKey: ['ID']
    });

});