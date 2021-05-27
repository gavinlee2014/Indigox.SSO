define("Indigox/SSO/Application/RegisteredService",
    [
        "Indigox.Web.JsLib.Models.RecordManager"
    ],
function (
    RecordManager
) {
    RecordManager.getInstance().register('RegisteredService', {
        columns: [
            { name: 'ID', text: '编号', type: String },
            { name: 'ServiceID', text: '系统编号', type: String },
            { name: 'Name', text: '系统名称', type: String },
            { name: 'SecretKey', text: '密钥', type: String },
            { name: 'AccessLoginUrl', text: 'WinAuth链接', type: String },
            { name: 'LoginUrl', text: '登录链接', type: String },
            { name: 'LoginOutUrl', text: '注销链接', type: String },
            { name: 'IsEnabled', text: '是否可用', type: String },
            { name: 'IsDefaultService', text: '是否默认系统', type: String },
            { name: 'IsWindowsAuthentication', text: '是否为Windows验证', type: String },
            { name: 'IsAllowedToProxy', text: '是否允许代理', type: String },
            { name: "CreateTime", text: '创建时间', type: String }
        ],
        primaryKey: ['ID']
    });

});