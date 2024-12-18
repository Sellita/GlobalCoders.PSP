const employeesDataTableId = "employees-data-table";

$(async function () {

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

    const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

    const dt = $(`#${employeesDataTableId}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function (data, callback, settings) {

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
                    "itemsPerPage": pageInfo.length
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
            {name: "employeeId", data: "employeeId"},
            {name: "name", data: "name"},
            {name: "email", data: "email"},
            {name: "phone", data: "phone"},
            {name: "role", data: "role"},
            {
                data: "merchantId",
                render: function (data, type, row) {
                    
                    console.log(organizationOptions)
                    
                    return  organizationOptions.find((element) => element['value'] === row['merchantId'])?.label || '';
                }
            },

            {name: "createTime", data: "createTime"},
            {name: "isActive", data: "isActive"},
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createEditEmployeeButton(data, row);
                    actions += createDeleteEmployeeButton(data, row);

                    return actions;
                }
            }
        ]
    });

    function createEditEmployeeButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row['employeeId'],
            "class": "btn btn-sm btn-primary edit-employee me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-pen-to-square"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createDeleteEmployeeButton(data, row) {
        const $deleteButton = $('<button>', {
            "role": "button",
            "data-id": row['employeeId'],
            "class": "btn btn-sm btn-secondary delete-employee me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }

    function createEmployee() {
        (async () => {
            const rowId = "new-employee";

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            console.log(organizationOptions);

            let inputs = createInput("name", "text", "Name", "Name", "", true);
            inputs += createInput("email", "text", "E-mail", "E-mail", "");
            inputs += createInput("phoneNumber", "text", "Phone", "Phone", "");

            inputs += createSelect("role", "Role", [
                {value: 'Admin', label: 'Admin'},
                {value: 'Owner', label: 'Owner'},
                {value: 'Employee', label: 'Employee'}
            ], '');
            
            inputs += createSelect("isActive", "Enabled", [
                {value: true, label: 'Yes'},
                {value: false, label: 'No'}
            ], "true");
            
            inputs += createSelect("organizationId", "Organization", organizationOptions, organizationOptions[0]);

            inputs += createDaysOfWeekTable('workingScheduleTableId', []);

            await Swal.fire({
                title: "New Employee",
                html: `<div class="container">
                        <form id="form-${rowId}">${inputs}</form>
                        </div>`,
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const formArray = $(`#form-${rowId}`).serializeArray();
                    const formObject = {};

                    let errors = '';

                    formArray.forEach((value, index) => {

                        if (!value.value && $(`#${value.name}`).attr('required')) {
                            errors += `Field "${capitalizeFirstLetter(value.name)}" is required<br>`
                            return;
                        }

                        formObject[value.name] = value.value;
                    });

                    formObject['workingSchedule'] = getworkingSchedule();

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const saveEmployeeResponse = await fetch(`${DOMAIN_URL}Employee/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveEmployeeResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${employeesDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new employee",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function editEmployee(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const emploeeResponse = await fetch(`${DOMAIN_URL}Employee/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!emploeeResponse.ok) {
                return false;
            }

            const employee = await emploeeResponse.json();

            let inputs = createInput("name", "text", "Name", "Name", employee['name'], true);
            inputs += createInput("email", "text", "E-mail", "E-mail", employee['email']);
            inputs += createInput("phoneNumber", "text", "Phone", "Phone", employee['phone']);

            inputs += createSelect("role", "Role", [
                {value: 'Admin', label: 'Admin'},
                {value: 'Owner', label: 'Owner'},
                {value: 'Employee', label: 'Employee'}
            ], employee['role']);
            
            inputs += createSelect("isActive", "Enabled", [
                {value: true, label: 'Yes'},
                {value: false, label: 'No'}
            ], employee['isActive'] ? 'true' : 'false');

            inputs += createSelect("organizationId", "Organization", organizationOptions, employee['merchantId']);

            inputs += createDaysOfWeekTable('workingScheduleTableId', employee['workingSchedule']);

            inputs += createInput("id", "hidden", "", "", rowId);

            await Swal.fire({
                title: "Edit Employee",
                html: `<div class="container">
                        <form id="form-${rowId}">${inputs}</form>
                        </div>`,
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const formArray = $(`#form-${rowId}`).serializeArray();
                    const formObject = {};

                    let errors = '';

                    formArray.forEach((value, index) => {

                        if (!value.value && $(`#${value.name}`).attr('required')) {
                            errors += `Field "${capitalizeFirstLetter(value.name)}" is required<br>`
                            return;
                        }

                        formObject[value.name] = value.value;
                    });

                    formObject['workingSchedule'] = getworkingSchedule();

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const updateEmployeeResponse = await fetch(`${DOMAIN_URL}Employee/Update`, {
                        method: "PUT",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateEmployeeResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try update");

                        return;
                    }

                    $(`#${employeesDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success updated employee",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function deleteEmployee(rowId) {
        (async () => {

            await Swal.fire({
                title: `Delete Employee`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteEmployeeResponse = await fetch(`${DOMAIN_URL}Employee/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteEmployeeResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${employeesDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success employee deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function getworkingSchedule() {

        const workingSchedule = [];

        daysOfWeek.forEach(function (day, index) {

            if (!$(`.${day}-day`).prop('checked')) {
                return
            }

            const schedule = {
                DayOfWeek: index,
                StartTime: $(`#${day}-dayOfWeek-startime`).val(),
                EndTime: $(`#${day}-dayOfWeek-endtime`).val()
            };

            workingSchedule.push(schedule);
        })

        return workingSchedule;
    }
});