const inventoryDataTableId = "inventory-data-table";

$(async function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("inventory");

    function init() {
        const dataTableBody = $(`#${inventoryDataTableId} tbody`);

        dataTableBody.on('click', 'tr button.edit-inventory', function () {
            const rowId = $(this).attr('data-id');
            const merchantId = $(this).attr('data-merchant');
            editProduct(rowId, merchantId);
        });

        dataTableBody.on('click', 'tr button.delete-inventory', function () {
            const rowId = $(this).attr('data-id');
            deleteProduct(rowId);
        });
    }

    const dt = $(`#${inventoryDataTableId}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function (data, callback, settings) {

            const pageInfo = $(`#${inventoryDataTableId}`).DataTable().page.info();

            console.log(pageInfo)

            $.ajax({
                url: `${DOMAIN_URL}Product/All`,
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
                success: async function (data) {
                    console.log(data);
                    let items = [];
                    for (let i = 0; i < data.items.length; i++) {
                        let q = (await fetch(`${DOMAIN_URL}inventory/quantity/${data.items[i].merchantId}/${data.items[i].id}`, {
                            method: "Get",
                            headers: {
                                Authorization: "Bearer " + localStorage.getItem("token"),
                                'Content-Type': 'application/json',
                            }
                        }));
                        
                        q = await q.json();

                        items.push({
                            id: data.items[i].id,
                            displayName: data.items[i].displayName,
                            merchant: data.items[i].merchant,
                            quantity: q,
                            merchantId: data.items[i].merchantId
                        });
                    }
                    
                    const tmpJson = {
                        recordsTotal: data.totalItems | 0,
                        recordsFiltered: data.totalItems | 0,
                        data: items,
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
            // {
            //     text: 'Add/Remove itemt',
            //     attr: {
            //         id: 'btn-inventory-new',
            //         class: 'btn btn-primary'
            //     },
            //     action: function (e, dt, node, config) {
            //         changeProductQuantity();
            //     }
            // }
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
            {name: "merchant", data: "merchant"},
            {name: "quantity", data: "quantity"},
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createEditProductButton(data, row);
                    //actions += createDeleteProductButton(data, row);

                    return actions;
                }
            }
        ]
    });

    
    function createEditProductButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "data-merchant": row.merchantId,
            "class": "btn btn-sm btn-primary edit-inventory me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-pen-to-square"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createDeleteProductButton(data, row) {
        const $deleteButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-secondary delete-inventory me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }

    function createProduct() {
        (async () => {
            const rowId = "new-inventory";

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");
            
            console.log(organizationOptions);

            let inputs = createInput("displayName", "text", "DisplayName", "DisplayName", "", true);
            inputs += createInput("description", "text", "Description", "Description", "");
            inputs += createInput("quantityChange", "text", "Price", "Price", "");
            inputs += createSelect("productTypeId", "Type", productTypeOptions, productTypeOptions[0]);
            inputs += createSelect("organizationId", "Organization", organizationOptions, organizationOptions[0]);

            await Swal.fire({
                title: "New Product",
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

                    const saveProductResponse = await fetch(`${DOMAIN_URL}Product/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveProductResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${inventoryDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new product",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function editProduct(rowId, merchantId) {
        (async () => {

           

            const productResponse = await fetch(`${DOMAIN_URL}Product/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!productResponse.ok) {
                return false;
            }

            const product = await productResponse.json();


            let inputs = createInput("displayName", "text", "DisplayName", "DisplayName", product['displayName'], true);
            inputs += createInput("quantityChange", "number", "Add/Remove items", "+/-0", 0, true);
            inputs += createInput("productId", "hidden", "", "", rowId);
            inputs += createInput("organizationId", "hidden", "", "", merchantId);

            await Swal.fire({
                title: "Edit Product",
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

                    const updateProductResponse = await fetch(`${DOMAIN_URL}inventory/add`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateProductResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${inventoryDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success updated product",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function deleteProduct(rowId) {
        (async () => {

            await Swal.fire({
                title: `Delete product`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteProductResponse = await fetch(`${DOMAIN_URL}Product/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteProductResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${inventoryDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success product deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
});