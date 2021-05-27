define("/SSO/Widgets/RegisteredService/ListToolBarWidget",
    [
        "Indigox.Web.JsLib.Utils.UrlUtil",
        "Indigox.Web.JsLib.Controls.Html.Menu"
    ],
    function (
        UrlUtil,
        Menu
    ) {
        var exports = function (widget) {
            $(widget).Menu("toolbar").first().configure({
                menuItemType: "buttonmenuitem",
                orientation: Menu.ORIENTATION_HORIZONTAL,
                staticDisplayLevels: 1,
                childNodes: [{
                    name: "btnCreateRegisteredService",
                    value: "新建外接系统",
                    events: {
                        clicked: function (src, e) {
                            var url = "#/RegisteredService/Create.htm";
                            UrlUtil.goTo(url);
                        }
                    }
                }]
            });
        };

        return exports;
    });