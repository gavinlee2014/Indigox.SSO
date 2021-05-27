define("/SSO/Widgets/Common/MainMenuWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Utils.StringUtil",
        "Indigox.Web.JsLib.CQRS.AutoBatch",
        "Indigox.Web.JsLib.CQRS.Batch",
        "Indigox.Web.JsLib.Proxy.ArrayProxy",
        "Indigox.Web.JsLib.Proxy.InstructionProxy",
        "Indigox.Web.JsLib.Models.RecordManager",
        "Indigox.Web.JsLib.Controllers.HierarchyController",
        "Indigox.Web.JsLib.Controls.Html.Menu",
        "Indigox.Web.JsLib.Controls.Html.Tree",
        "Indigox.Web.JsLib.Controls.Binding.PropertyBinding"
    ],
    function (
        UrlUtil,
        StringUtil,
        AutoBatch,
        Batch,
        ArrayProxy,
        InstructionProxy,
        RecordManager,
        HierarchyController,
        Menu,
        Tree,
        PropertyBinding
    ) {

        function bindMenu(widget) {
            var accordions = [];

            var workMenuData = [
                { id: "001", text: "登录日志", href: "#/LogRecord/List.htm" },
                { id: "002", text: "访问日志", href: "#/AccessRecord/List.htm" },
                { id: "003", text: "外接系统管理", href: "#/RegisteredService/List.htm" }

            ];
            var workMenu = new Menu();
            workMenu.configure({
                menuItemType: "linkmenuitem",
                orientation: Menu.ORIENTATION_VERTICAL,
                staticDisplayLevels: 2,
                childNodes: workMenuData
            });
            accordions.push({ "text": "工作区", "children": [workMenu] });

            var accordion = $(widget).Accordion("mainmenu").first();
            accordion.configure({
                items: accordions
            });
        }


        var exports = function (widget) {
            bindMenu(widget);
        };

        return exports;
    });