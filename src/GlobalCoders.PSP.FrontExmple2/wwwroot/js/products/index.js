const productsDataTableId = "products-data-table";

$(async function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("products");

    function init() {
        const dataTableBody = $(`#${productsDataTableId} tbody`);

        dataTableBody.on('click', 'tr button.edit-product', function () {
            const rowId = $(this).attr('data-id');
            editProduct(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-product', function () {
            const rowId = $(this).attr('data-id');
            deleteProduct(rowId);
        });
    }

    const dt = $(`#${productsDataTableId}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function (data, callback, settings) {

            const pageInfo = $(`#${productsDataTableId}`).DataTable().page.info();

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
                text: 'New Product',
                attr: {
                    id: 'btn-product-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createProduct();
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
            {name: "stock", data: "stock"},
            {name: "taxName", data: "taxName"},
            {name: "taxValue", data: "taxValue"},
            {name: "category", data: "category"},
            {name: "price", data: "price"},
            {
                data: "productState",
                render: function (data, type, row) {
                    return productStates.find((productState) => productState.value === row['productState'])?.label;
                }
            },
            {name: "merchant", data: "merchant"},
            {name: "creationDate", data: "creationDate"},
            {name: "lastUpdateDate", data: "lastUpdateDate"},
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createEditProductButton(data, row);
                    actions += createDeleteProductButton(data, row);

                    return actions;
                }
            }
        ]
    });

    function createEditProductButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary edit-product me-1 mb-1",
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
            "class": "btn btn-sm btn-secondary delete-product me-1 mb-1",
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
            const rowId = "new-product";

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const productTypeOptions = await GetSelects(`${DOMAIN_URL}producttype/All`, "id", "displayName");

            console.log(organizationOptions);

            let inputs = createInput("displayName", "text", "DisplayName", "DisplayName", "", true);
            inputs += createInput("description", "text", "Description", "Description", "");
            inputs += createInput("price", "text", "Price", "Price", "");
            
            inputs += createSelect("productState", "State", productStates, '');
            inputs += createSelect("productTypeId", "Type", productTypeOptions, productTypeOptions[0]);

            inputs += createSelect("merchantId", "Organization", organizationOptions, organizationOptions[0]);
            

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

                    $(`#${productsDataTableId}`).DataTable().ajax.reload();

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

    function editProduct(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const productTypeOptions = await GetSelects(`${DOMAIN_URL}producttype/All`, "id", "displayName");

            console.log(organizationOptions);

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
            inputs += createInput("description", "text", "Description", "Description", product['description'], true);
            inputs += createInput("price", "text", "Price", "Price", product['price']);
            inputs += createSelect("productState", "State", productStates, product['productState']);
            inputs += createSelect("productTypeId", "Type", productTypeOptions, product['productTypeId']);

            inputs += createSelect("merchantId", "Organization", organizationOptions, product['merchantId']);

            inputs += createInput("id", "hidden", "", "", rowId);

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

                    const updateProductResponse = await fetch(`${DOMAIN_URL}Product/Update`, {
                        method: "PUT",
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

                    $(`#${productsDataTableId}`).DataTable().ajax.reload();

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

                    $(`#${productsDataTableId}`).DataTable().ajax.reload();

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