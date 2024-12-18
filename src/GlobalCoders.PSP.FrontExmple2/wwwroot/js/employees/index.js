const employeesDataTableId = "employees-data-table";

$(function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("employees");

    function init() {
        const dataTableBody = $(`#${employeesDataTableId} tbody`);

        dataTableBody.on('click', 'tr button.edit-employee', function () {
            const rowId = $(this).attr('data-id');
            editEmployee(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-employee', function () {
            const rowId = $(this).attr('data-id');
            deleteEmployee(rowId);
        });
    }

    const dt = $(`#${employeesDataTableId}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function(data, callback,settings) {

            const pageInfo = $(`#${employeesDataTableId}`).DataTable().page.info();

            console.log(pageInfo)

            $.ajax({
                url: `${DOMAIN_URL}Employee/All`,
                type: "POST",
                crossDomain: true,
                contentType: "application/json",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token")
                },
                data: JSON.stringify({
                    "page": pageInfo.page + 1,
                    "itemsPerPage": pageInfo.length,
                    "displayName": '',
                }),
                success: function (data) {
                    console.log(data);

                    const tmpJson = {
                        recordsTotal: data.totalItems | 0,
                        recordsFiltered: data.totalItems | 0,
                        data: data.items,
                        currentPage: data.page,
                    };

                    callback(tmpJson);
                }
            })
        },
        paging: true,
        bPaginate: true,
        info: true,
        serverSide: true,
        response: true,
        buttons: [
            {
                text: 'New Employee',
                attr: {
                    id: 'btn-employee-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createEmployee();
                }
            }
        ],
        columnDefs: [
            {
                "className": "dt-center",
                "targets": "_all",
            },
            {
                orderable: false,
                targets: [0],
                visible: false
            }],
        columns: [
            {name: "id", data: "id"},
            {name: "displayName", data: "displayName"},
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createEditOrganizationButton(data, row);
                    actions += createDeleteOrganizationButton(data, row);

                    return actions;
                }
            }
        ]
    });
});