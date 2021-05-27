define("/SSO/Widgets/RegisteredService/ListWidget",
    [
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.ErrorHandler",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/SSO/Application/RegisteredService"
    ],
    function (
        StringUtil,
        UrlUtil,
        ErrorHandler,
        InstructionProxy,
        RecordManager,
        ListController,
        Batch,
        ItemMode,
        PageUrlMonitor
    ) {
        var exports = function (widget) {
            var limit = 10;

            $(widget).DataList("RegisteredServiceList").first().getItemTemplate().configureChildren({
                "IsEnabled": {
                    binding: {
                        mapping: {
                            value: function (record) {
                                var enabled = record.get("IsEnabled");
                                return enabled == true ? "是" : "否";
                            }
                        }
                    }
                }
            });
            $(widget).DataList("RegisteredServiceList").on("itemAdded", function (source, index, item) {
                $(item).Button("btnEdit").on("clicked", editHandler);
            });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('RegisteredService', {
                    proxy: new InstructionProxy({
                        query: "RegisteredServiceListQuery"
                    })
                }),
                params: {
                    "FetchSize": limit
                }
            });

            var listControl = $(widget).DataList("RegisteredServiceList").first();

            listControl.configure({
                controller: controller
            });

            $(widget).Paging("Paging").configure({
                pageSize: limit,
                arrayController: controller
            });

        };

        var editHandler = function (source) {
            var record = source.parent.getRecord();
            var id = record.get("ID");
            var url = "#/RegisteredService/Edit.htm?ID=" + id;
            UrlUtil.goTo(url);
        };
        
        return exports;
    });