const productTypesDataTable = "product-types-data-table";
$(function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("product-types")

    function init() {
        const dataTableBody = $(`#${productTypesDataTable} tbody`);

        dataTableBody.on('click', 'tr button.edit-product-type', function () {
            const rowId = $(this).attr('data-id');
            editProductType(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-product-type', function () {
            const rowId = $(this).attr('data-id');
            deleteProductType(rowId);
        });
    }

    const dt = $(`#${productTypesDataTable}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function(data, callback,settings) {
            
            const pageInfo = $(`#${productTypesDataTable}`).DataTable().page.info();
            
            console.log(pageInfo)
            
            $.ajax({
                url: `${DOMAIN_URL}productType/All`,
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
                text: 'New Product type',
                attr: {
                    id: 'btn-product-type-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createProductType();
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

                    actions += createeditProductTypeButton(data, row);
                    actions += createdeleteProductTypeButton(data, row);

                    return actions;
                }
            }
        ]
    });

    function createeditProductTypeButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary edit-product-type me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-pen-to-square"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createdeleteProductTypeButton(data, row) {
        const $deleteButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-secondary delete-product-type me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }

    function createProductType(){
        (async () => {
            const rowId = "new-product-type";

            // const organizationsResponse = await fetch(`${DOMAIN_URL}Organization/All`, {
            //     method: "POST",
            //     headers: {
            //         Authorization: "Bearer " + localStorage.getItem("token"),
            //         'Content-Type': 'application/json',
            //     },
            //     body: JSON.stringify({
            //         "page": 1,
            //         "itemsPerPage": 100
            //     })
            // });
            //
            // if(!organizationsResponse.ok){
            //     Swal.fire({
            //         title: "Status",
            //         text: "Oops...Something was wrong",
            //         icon: "error",
            //         confirmButtonColor: "#70757d"
            //     })
            //     return;
            // }
            //
            // const organizations = await organizationsResponse.json();
            // let organizationOptions = []
            // organizations.items.map(organization => {
            //    
            //     organizationOptions.push({
            //         value: organization.id,
            //         label: organization.displayName
            //     });
            // });
            // debugger;
            
            let inputs = createInput("displayName", "text", "Display Name", "Display Name", "", true);
            //inputs += createSelect("productState", "State", organizationOptions, organizationOptions[0]);
            
            await Swal.fire({
                title: "New Product Type",
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

                    const saveProductTypeResponse = await fetch(`${DOMAIN_URL}producttype/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveProductTypeResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${productTypesDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new product type",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
    
    function editProductType(rowId) {
        (async () => {
            const productTypeResponse = await fetch(`${DOMAIN_URL}producttype/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!productTypeResponse.ok) {
                return false;
            }

            const productType = await productTypeResponse.json();

            let inputs = createInput("displayName", "text", "Display Name", "Display Name", productType["displayName"], true);
            inputs += createInput("id", "hidden", "", "", rowId);
            
            await Swal.fire({
                title: "Edit Product type",
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

                    const updateProductTypeResponse = await fetch(`${DOMAIN_URL}producttype/Update`, {
                        method: "PUT",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateProductTypeResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try update");

                        return;
                    }

                    $(`#${productTypesDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success updated product type",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
            
        })();
    }
    
    function deleteProductType(rowId) {
        (async () => {
            
            await Swal.fire({
                title: `Delete product type`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteProductTypeResponse = await fetch(`${DOMAIN_URL}producttype/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteProductTypeResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${productTypesDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success product type deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
});