


$.CreateDeptGrid = function () {

    $("#IDTblDeptGrid").jqGrid({
        caption: "Employee",
        url: "/Sales/GetSalesOrderHeaderRecords",
        datatype: 'json', //xml if xml data 
        mtype: "GET",
        width: 900,
        height: 500,
        rowNum: 5,
        rowList: [5, 10, 30, 60, 90, 100],
        hidegrid: false,
        loadonce: false,
        rownumbers: true,
        viewrecords: true,
        pager: "#IDDivPager",
        sortname: "SalesOrderID",
        //sortorder:"desc",
        colModel: [
           {
               label: 'SalesOrder ID',
               name: 'SalesOrderID',
               align: 'center',
               index: "SalesOrderID",
               width: 30,
               resizable: true,
               sortable: true,
               search: true,
               searchoptions: {
                   sopt: ["eq", "ne", "lt", "gt", "cn"]
               }
           },
        {
            label: 'TotalDue',
            name: 'TotalDue',
            align: 'center',
            width: 30,
            resizable: true,
            sortable: true,
            search: true,
            searchoptions: {
                sopt: ["eq", "ne", "lt", "gt", "cn"]
            }
        },
        {
            label: 'OrderDate',
            name: 'OrderDate',
            align: 'center',
            width: 30,
            formatter: "date",
            resizable: true,
            sortable: true,
            search: true,
            searchoptions: {
                sopt: ["eq"],
                dataInit: function (elem) {
                    $(elem).datepicker({});
                },
            }
        }],

        loadComplete: function (data) {
            console.log("grid Load Completed"); console.log(data);
        },
        gridComplete: function () {
            console.log("Grid Completed")
        },
        loadError: function (xhr, status, error) {
            console.log("Code:" + xhr.status);
            console.log("Status:" + status);
            console.log("Msg:" + error);
        },
        jsonReader: {
            root: "root",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
        },

        subGrid: true,
        subGridRowExpanded: function (subgrid_id, row_id) {

            var salesOrderID = $("#IDTblDeptGrid").getCell(row_id, "SalesOrderID");

            var subgrid_table_id, pager_id;

            subgrid_table_id = subgrid_id + "_t";

            pager_id = "p_" + subgrid_table_id;

            $("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");


            jQuery("#" + subgrid_table_id).jqGrid({
                url: "/Sales/GetSalesOrderDetailerRecords?SalesOrderID=" + salesOrderID,
                datatype: "json",
                colNames: ['SalesOrderID', 'SalesOrderDetailID', 'ProductID', 'UnitPrice', 'OrderQtyl', 'LineTotal'],
                colModel: [
                    {
                        name: "SalesOrderID",
                        sortable: true,
                        search: true,
                        searchoptions: {
                            sopt: ["eq", "ne", "lt", "gt", "cn"]
                        }
                    },
                    {
                        name: "SalesOrderDetailID",
                        sortable: true,
                        search: true,
                        searchoptions: {
                            sopt: ["eq", "ne", "lt", "gt", "cn"]
                        }
                    },
                    {
                        name: "ProductID",
                        sortable: true,
                        search: true,
                        searchoptions: {
                            sopt: ["eq", "ne", "lt", "gt", "cn"]
                        }
                    },
                    {
                        name: "UnitPrice",
                        sortable: true,
                        search: true,
                        searchoptions: {
                            sopt: ["eq", "ne", "lt", "gt", "cn"]
                        }
                    },
                    {
                        name: "OrderQty",
                        sortable: true,
                        search: true,
                        searchoptions: {
                            sopt: ["eq", "ne", "lt", "gt", "cn"]
                        }
                    },
                    {
                        name: "LineTotal",
                        sortable: true,
                        search: true,
                        searchoptions: {
                            sopt: ["eq", "ne", "lt", "gt", "cn"]
                        }
                    },
                ],
                rowNum: 5,
                sortname: "SalesOrderID",
                pager: pager_id,
                height: '100%',
                width: 750,
                loadonce: false,
                rownumbers: true,
                viewrecords: true,
                jsonReader: {
                    root: "root",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                },
            });
            jQuery("#" + subgrid_table_id).jqGrid('navGrid', "#" + pager_id, { edit: false, add: false, del: false, search: true, refresh: true });

            $("#"+subgrid_table_id).filterToolbar({
                stringResult: true,
                searchOnEnter: true,
                searchOperators: true
            });
        },
    });

    $("#IDTblDeptGrid").filterToolbar({
        stringResult: true,
        searchOnEnter: true,
        searchOperators: true
    });


    //AdvancedSearch/SimpleSearch
    $("#IDTblDeptGrid").navGrid("#IDDivPager", {}, {}, {}, {}, {
        multipleSearch: true,
        multipleGroup: true,
        showQuery: true
    });


}

