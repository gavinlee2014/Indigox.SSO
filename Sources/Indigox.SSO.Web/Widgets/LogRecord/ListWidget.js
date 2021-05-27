define("/SSO/Widgets/LogRecord/ListWidget",
    [
        "/SSO/Widgets/LogRecord/SearchLogRecordContentType",
        "/SSO/Widgets/LogRecord/SearchLogRecordSchema",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.Util",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.ListController",
        "Indigox.Web.JsLib.Controllers.FormController",
        "Indigox.Web.JsLib.Controls.Selection.ItemMode",
        "Indigox.Web.JsLib.Controls.PageUrlMonitor",
        "Indigox/SSO/Application/LogRecord"
    ],
    function (
        SearchLogRecordContentType,
        SearchLogRecordSchema,
        StringUtil,
        UrlUtil,
        Util,
        AutoBatch,
        ArrayProxy,
        InstructionProxy,
        RecordManager,
        ListController,
        FormController,
        ItemMode,
        PageUrlMonitor
    ) {
        var exports = function (widget) {
            var limit = 10;

            initSearchPanel(widget);

            $(widget).DataList("LogRecordList").first().getItemTemplate().configureChildren({
                "Action": {
                    binding: {
                        mapping: {
                            value: function (record) {
                                var action = record.get("Action");
                                var t = "";
                                switch (action) {
                                    case "0":
                                        t = "登录";
                                        break;
                                    case "1":
                                        t = "登出";
                                        break;
                                    default:
                                        t = "其他";
                                }
                                return t;
                            }
                        }
                    }
                }
            });
            function initSearchPanel(widget) {
                var contentType = SearchLogRecordContentType;
                var searchPanel = $(widget).Content("SearchPanel").first();
                searchPanel.setControls(contentType.controls);

                searchPanel.configure({
                    controller: new FormController({
                        model: RecordManager.getInstance().createRecordSet("SearchLogRecord", {
                            proxy: new ArrayProxy({
                                array: [{}]
                            })
                        })
                    })
                });
            }

            $(widget).Button("btnSearch").first().on("clicked", function () {
                var searchPanel = $(widget).Content("SearchPanel").first();
                var controller = searchPanel.getController();
                controller.updateRecord();
                var specification = Util.copy({}, controller.getModel().getRecord(0).data);
                var paging = $(widget).Paging("Paging").first();
                SetSearchParam(paging.getArrayController(), specification);
                paging.reset();
            });

            var controller = new ListController({
                model: RecordManager.getInstance().createRecordSet('LogRecord', {
                    proxy: new InstructionProxy({
                        query: "LogRecordListQuery"
                    })
                }),
                params: {
                    AccountName: '',
                    IP: '',
                    LogTimeBegin: '',
                    LogTimeEnd: '',
                    FetchSize: limit
                }
            });

            var listControl = $(widget).DataList("LogRecordList").first();

            listControl.configure({
                controller: controller
            });

            $(widget).Paging("Paging").configure({
                pageSize: limit,
                arrayController: controller
            });

        };

        function SetSearchParam(arrController, specification) {
            for (var p in specification) {
                if (p) {
                    var v = specification[p];

                    switch (p) {
                        case 'LogTimeBegin':
                            arrController.setParam("LogTimeBegin", v);
                            break;

                        case 'IP':
                            arrController.setParam("IP", v);
                            break;

                        case 'AccountName':
                            arrController.setParam("AccountName", v);
                            break;

                        case 'LogTimeEnd':
                            arrController.setParam("LogTimeEnd", v);
                            break;
                    }
                }

            }
        }
        return exports;
    });