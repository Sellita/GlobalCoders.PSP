const servicesDataTableId = "services-data-table";

$(async function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("services");

    function init() {
        const dataTableBody = $(`#${servicesDataTableId} tbody`);

        dataTableBody.on('click', 'tr button.edit-service', function () {
            const rowId = $(this).attr('data-id');
            editService(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-service', function () {
            const rowId = $(this).attr('data-id');
            deleteService(rowId);
        });
    }

    const dt = $(`#${servicesDataTableId}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function (data, callback, settings) {

            const pageInfo = $(`#${servicesDataTableId}`).DataTable().page.info();

            console.log(pageInfo)

            $.ajax({
                url: `${DOMAIN_URL}Service/All`,
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
                text: 'New Service',
                attr: {
                    id: 'btn-service-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createService();
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
            {name: "description", data: "description"},
            {name: "durationMin", data: "durationMin"},
            {name: "price", data: "price"},
            {
                data: "serviceState",
                render: function (data, type, row) {
                    return serviceStates.find((serviceState) => serviceState.value === row['serviceState'])?.label;
                }
            },
            {name: "employee", data: "employee"},
            {name: "creationDate", data: "creationDate"},
            {name: "lastUpdateDate", data: "lastUpdateDate"},
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createEditServiceButton(data, row);
                    actions += createDeleteServiceButton(data, row);

                    return actions;
                }
            }
        ]
    });

    function createEditServiceButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary edit-service me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-pen-to-square"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createDeleteServiceButton(data, row) {
        const $deleteButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-secondary delete-service me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }


    function createService() {
        (async () => {
            const rowId = "new-service";

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");

            console.log(employeeOptions);

            let inputs = createInput("displayName", "text", "DisplayName", "DisplayName", "", true);
            inputs += createInput("description", "text", "Description", "Description", "");
            inputs += createInput("durationMin", "number", "Duration, min", "min", "");
            inputs += createInput("price", "text", "Price", "Price", "");
            inputs += createSelect("serviceState", "State", serviceStates, serviceStates[0].value);
            inputs += createSelect("employeeId", "Employee", employeeOptions, employeeOptions[0]);

            await Swal.fire({
                title: "New Service",
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

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const saveServiceResponse = await fetch(`${DOMAIN_URL}Service/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveServiceResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${servicesDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new service",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function editService(rowId) {
        (async () => {

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");

            console.log(employeeOptions);

            const serviceResponse = await fetch(`${DOMAIN_URL}Service/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!serviceResponse.ok) {
                return false;
            }

            const service = await serviceResponse.json();

            let inputs = createInput("displayName", "text", "DisplayName", "DisplayName", service['displayName'], true);
            inputs += createInput("description", "text", "Description", "Description", service['description']);
            inputs += createInput("durationMin", "number", "Duration, min", "min", service['durationMin']);
            inputs += createInput("price", "text", "Price", "Price", service['price']);
            inputs += createSelect("serviceState", "State", serviceStates, service['serviceState']);
            inputs += createSelect("employeeId", "Employee", employeeOptions, service['employeeId']);
            inputs += createInput("id", "hidden", "", "", rowId);

            await Swal.fire({
                title: "New Service",
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

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const saveServiceResponse = await fetch(`${DOMAIN_URL}Service/Update`, {
                        method: "PUT",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveServiceResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${servicesDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success updated service",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function deleteService(rowId) {
        (async () => {

            await Swal.fire({
                title: `Delete service`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteServiceResponse = await fetch(`${DOMAIN_URL}Service/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteServiceResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${servicesDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success service deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
    
});