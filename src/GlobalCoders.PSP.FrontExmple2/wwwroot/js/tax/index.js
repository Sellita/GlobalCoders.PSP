const taxDataTable = "taxes-data-table";
$(function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("taxes")

    function init() {
        const dataTableBody = $(`#${taxDataTable} tbody`);

        dataTableBody.on('click', 'tr button.edit-tax', function () {
            const rowId = $(this).attr('data-id');
            editTax(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-tax', function () {
            const rowId = $(this).attr('data-id');
            deleteTax(rowId);
        });
    }

    const dt = $(`#${taxDataTable}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function(data, callback,settings) {
            
            const pageInfo = $(`#${taxDataTable}`).DataTable().page.info();
            
            console.log(pageInfo)
            
            $.ajax({
                url: `${DOMAIN_URL}tax/All`,
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
                text: 'New Tax',
                attr: {
                    id: 'btn-tax-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createTax();
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
            {name: "type", data: "type"},
            {name: "value", data: "value"},
            {name: "productTypeId", data: "productTypeId"},
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createeditTaxButton(data, row);
                    actions += createdeleteTaxButton(data, row);

                    return actions;
                }
            }
        ]
    });

    function createeditTaxButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary edit-tax me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-pen-to-square"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createdeleteTaxButton(data, row) {
        const $deleteButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-secondary delete-tax me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }

    function createTax(){
        (async () => {
            const rowId = "new-tax";
            
            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");
            const productTypeOptions = await GetSelects(`${DOMAIN_URL}producttype/All`, "id", "displayName");
            
            let inputs = createInput("name", "text", "Display Name", "Display Name", "", true);
            inputs += createSelect("organizationId", "Organization", organizationOptions, organizationOptions[0]);
            inputs += createSelect("productTypeId", "Product type", productTypeOptions, productTypeOptions[0]);
            
            await Swal.fire({
                title: "New Tax",
                html: `<div class="container">
                        <form id="form-${rowId}">${inputs}</form>
                        </div>`,
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async() => {

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

                    const saveTaxResponse = await fetch(`${DOMAIN_URL}tax/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveTaxResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${taxDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new tax",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
    
    function editTax(rowId) {
        (async () => {
            const taxResponse = await fetch(`${DOMAIN_URL}tax/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!taxResponse.ok) {
                return false;
            }

            const tax = await taxResponse.json();

            let inputs = createInput("displayName", "text", "Display Name", "Display Name", tax["displayName"], true);
            inputs += createInput("id", "hidden", "", "", rowId);
            
            await Swal.fire({
                title: "Edit Tax",
                html: `<div class="container">
                        <form id="form-${rowId}">${inputs}</form>
                        </div>`,
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async() => {

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

                    //formObject['workingSchedule'] = getworkingSchedule();

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const updateTaxResponse = await fetch(`${DOMAIN_URL}tax/Update`, {
                        method: "PUT",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateTaxResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try update");

                        return;
                    }

                    $(`#${taxDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success updated tax",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
            
        })();
    }
    
    function deleteTax(rowId) {
        (async () => {
            
            await Swal.fire({
                title: `Delete tax`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteTaxResponse = await fetch(`${DOMAIN_URL}tax/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteTaxResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${taxDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success tax deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
});