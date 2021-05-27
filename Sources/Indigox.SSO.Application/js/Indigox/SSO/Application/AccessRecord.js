define("Indigox/SSO/Application/AccessRecord",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('AccessRecord', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'AccountName', text: '用户名', type: String },
            { name: 'IP', text: '登录IP', type: String },
            { name: 'LogTime', ext: '登录时间', type: String },
            { name: 'ServiceName', text: '系统名', type: String },
            { name: 'ServiceID', text: '系统编号', type: String },
            { name: 'ServiceSerialID', text: '系统ID', type: String }
        ],
        primaryKey: ['ID']
    });

});