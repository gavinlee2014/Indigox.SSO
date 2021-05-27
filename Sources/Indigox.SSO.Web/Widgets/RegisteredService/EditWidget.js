define("/SSO/Widgets/RegisteredService/EditWidget",
    [
        "Indigox.Web.JsLib.Utils.ArrayUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controls.Validation.Rules.NotBlankRule",
        "Indigox/SSO/Application/RegisteredService"
    ],
function (
        ArrayUtil,
        UrlUtil,
        AutoBatch,
        InstructionProxy,
        RecordManager,
        FormController,
        ListController,
        NotBlankRule
) {
    function exports(widget) {


        var formControl = $(widget).Content("EditArea").first();

        formControl.on('submit', function (successed) {
            if (successed) {
                alert('保存成功。');
                UrlUtil.goBack();
            }
            else {
                debug.error('保存失败。');
            }
            Page().unmask();
        });

        $(widget).Button("btnSave").first().configure({
            events: {
                clicked: function (src, e) {
                    Page().mask();
                    formControl.submit();
                }
            }
        });
        $(widget).Button("btnDelete").first().configure({
            events: {
                clicked: function (source, e) {
                    Page().mask();

                    AutoBatch.getCurrentBatch().execute({
                        name: "DeleteRegisteredServiceCommand",
                        properties: {
                            ID: Page().getUrlParam("ID")
                        }
                    })
                    .done(function (data) {
                        alert('删除成功。');
                        UrlUtil.goBack();
                    })
                    .fail(function (data) {
                        debug.error('删除失败。');
                    })
                    .always(function (data) {
                        Page().unmask();
                    });
                }
            }
        });
        $(widget).Button("btnReturn").first().configure({
            events: {
                clicked: function (src, e) {
                    UrlUtil.goBack();
                }
            }
        });

        var ID = Page().getUrlParam("ID");

        var controller = new FormController({
            model: RecordManager.getInstance().createRecordSet('RegisteredService', {
                proxy: new InstructionProxy({
                    query: "RegisteredServiceSingleQuery",
                    updateCommand: "UpdateRegisteredServiceCommand"
                })
            }),
            params: {
                "ID": ID
            }
        });

        formControl.configure({
            controller: controller
        });
    };

    return exports;
});