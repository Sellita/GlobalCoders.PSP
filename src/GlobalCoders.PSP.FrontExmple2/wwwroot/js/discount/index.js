const discountDataTable = "discounts-data-table";
const discountTypes = [
    {value: 1, label: "Percentage"},
    {value: 2, label: "Value"}
]
$(async function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("discounts")

    function init() {
        const dataTableBody = $(`#${discountDataTable} tbody`);

        dataTableBody.on('click', 'tr button.edit-discount', function () {
            const rowId = $(this).attr('data-id');
            editDiscount(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-discount', function () {
            const rowId = $(this).attr('data-id');
            deleteDiscount(rowId);
        });
    }
    const productTypeOptions = await GetSelects(`${DOMAIN_URL}producttype/All`, "id", "displayName");
    const productOptions = await GetSelects(`${DOMAIN_URL}product/All`, "id", "displayName");

    const dt = $(`#${discountDataTable}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function(data, callback,settings) {
            
            const pageInfo = $(`#${discountDataTable}`).DataTable().page.info();
            
            console.log(pageInfo)
            
            $.ajax({
                url: `${DOMAIN_URL}discount/All`,
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
                text: 'New Discount',
                attr: {
                    id: 'btn-discount-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createDiscount();
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
            {name: "type", data: "type",
                render: function (data, type, row) {

                    console.log(discountTypes)

                    return  discountTypes.find((element) => element['value'] === row['type'])?.label || '';
                }},
            {name: "value", data: "value"},
            {name: "productTypeId", data: "productTypeId",
                render: function (data, type, row) {

                    console.log(productTypeOptions)

                    return  productTypeOptions.find((element) => element['value'] === row['productTypeId'])?.label || '';
                }},     
            {name: "productId", data: "productId",
                render: function (data, type, row) {

                    console.log(productOptions)

                    return  productOptions.find((element) => element['value'] === row['productId'])?.label || '';
                }},
            {name: "startDate", data: "startDate"},
            {name: "endDate", data: "endDate"},
            
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createeditDiscountButton(data, row);
                    actions += createdeleteDiscountButton(data, row);

                    return actions;
                }
            }
        ]
    });

    function createeditDiscountButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary edit-discount me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-pen-to-square"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createdeleteDiscountButton(data, row) {
        const $deleteButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-secondary delete-discount me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }

    function createDiscount(){
        (async () => {
            const rowId = "new-discount";
            
            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");
            const productTypeOptions = await GetSelects(`${DOMAIN_URL}producttype/All`, "id", "displayName");
            const productOptions = await GetSelects(`${DOMAIN_URL}product/All`, "id", "displayName");
            productTypeOptions.unshift({value: "", label: "all types"});
            productOptions.unshift({value: "", label: "all products"});
            

            
            let inputs = createInput("name", "text", "Display Name", "Display Name", "", true);
            inputs += createSelect("type", "Discount type", discountTypes, discountTypes[0]);
            inputs += createInput("value", "number", "Value", 0, 0);
            inputs += createInput("startDate", "date", "Start date", 0, "");
            inputs += createInput("endDate", "date", "End date", 0, "");

            inputs += createSelect("organizationId", "Organization", organizationOptions, organizationOptions[0]);
            inputs += createSelect("productTypeId", "Product type", productTypeOptions, productTypeOptions[0]);
            inputs += createSelect("productId", "Product", productOptions, productOptions[0]);
            
            
            await Swal.fire({
                title: "New Discount",
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

                    const saveDiscountResponse = await fetch(`${DOMAIN_URL}discount/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveDiscountResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${discountDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new discount",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
    
    function editDiscount(rowId) {
        (async () => {
            const discountResponse = await fetch(`${DOMAIN_URL}discount/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!discountResponse.ok) {
                return false;
            }

            const discount = await discountResponse.json();

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");
            const productTypeOptions = await GetSelects(`${DOMAIN_URL}producttype/All`, "id", "displayName");
            const productOptions = await GetSelects(`${DOMAIN_URL}product/All`, "id", "displayName");

            productTypeOptions.unshift({value: "", label: "all types"});
            productOptions.unshift({value: "", label: "all types"});
            console.log(discount['startDate'])

            let inputs =createInput("name", "text", "Display Name", "Display Name", discount['name'], true);
            inputs += createSelect("type", "Discount type", discountTypes, discount['type']);
            inputs += createInput("value", "number", "Value", 0, discount['value']);
            inputs += createInput("startDate", "date", "Start date", 0, discount['startDate']?.substring(0,10));
            inputs += createInput("endDate", "date", "End date", 0, discount['endDate']?.substring(0,10));

            inputs += createSelect("organizationId", "Organization", organizationOptions, discount['merchantId']);
            inputs += createSelect("productTypeId", "Product type", productTypeOptions, discount['productTypeId']);
            inputs += createSelect("productId", "Product", productOptions, discount['productId']);
            inputs += createInput("id", "hidden", "", "", rowId);


            await Swal.fire({
                title: "Edit Discount",
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

                    const updateDiscountResponse = await fetch(`${DOMAIN_URL}discount/Update`, {
                        method: "PUT",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateDiscountResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try update");

                        return;
                    }

                    $(`#${discountDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success updated discount",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
            
        })();
    }
    
    function deleteDiscount(rowId) {
        (async () => {
            
            await Swal.fire({
                title: `Delete discount`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteDiscountResponse = await fetch(`${DOMAIN_URL}discount/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteDiscountResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${discountDataTable}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success discount deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
});