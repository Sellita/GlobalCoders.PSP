const ordersDataTableId = "orders-data-table";

$(async function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("orders");

    function init() {
        const dataTableBody = $(`#${ordersDataTableId} tbody`);

        dataTableBody.on('click', 'tr button.edit-order', function () {
            const rowId = $(this).attr('data-id');
            editOrder(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-order', function () {
            const rowId = $(this).attr('data-id');
            deleteOrder(rowId);
        });
        
        dataTableBody.on('click', 'tr button.status-order', function () {
            const rowId = $(this).attr('data-id');
            statusOrder(rowId);
        });      
        
        dataTableBody.on('click', 'tr button.look-order', function () {
            const rowId = $(this).attr('data-id');
            lookOrder(rowId);
        });     
        dataTableBody.on('click', 'tr button.tip-change-order', function () {
            const rowId = $(this).attr('data-id');
            tipChangeOrder(rowId);
        });     
        dataTableBody.on('click', 'tr button.payment-order', function () {
            const rowId = $(this).attr('data-id');
            paymentOrder(rowId);
        });       
        dataTableBody.on('click', 'tr button.product-change-order-q', function () {
            const rowId = $(this).attr('data-id');
            productChangeOrder(rowId);
        });
        dataTableBody.on('click', 'tr button.resume-payment', function () {
            const rowId = $(this).attr('data-id');
            resumePaymentOrder(rowId);
        });
    }

    const dt = $(`#${ordersDataTableId}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function (data, callback, settings) {

            const pageInfo = $(`#${ordersDataTableId}`).DataTable().page.info();

            console.log(pageInfo)

            $.ajax({
                url: `${DOMAIN_URL}Orders/All`,
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
                text: 'New Order',
                attr: {
                    id: 'btn-order-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createOrder();
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
            {name: "client", data: "client"},
            {name: "date", data: "date"},
            {name: "merchant", data: "merchant"},
            {name: "status", data: "status",
                render: function (data, type, row) {
                    return orderStates.find((orderState) => orderState.value === row['status'])?.label;
                }
            },
            {
                render: function (data, type, row) {
                    let actions = "";

                    actions += createResumePaymentButton(data, row);
                    actions += createViewButton(data, row);
                    actions += createChangePaymentButton(data, row);
                    actions += createChangeTipButton(data, row);
                    actions += createChangeProductButton(data, row);
                    actions += createChangeStatusButton(data, row);
                    actions += createEditProductButton(data, row);
                    actions += createDeleteProductButton(data, row);

                    return actions;
                }
            }
        ]
    });

    function createResumePaymentButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary resume-payment me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-solid fa-arrow-rotate-right"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createViewButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary look-order me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-solid fa-eye"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }   
    function createChangePaymentButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary payment-order me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-solid fa-cash-register"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }   
    
    function createChangeTipButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary tip-change-order me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-solid fa-dollar-sign"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    } 
    
    function createChangeProductButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary product-change-order-q me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-solid fa-cart-shopping"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    } 
    
    function createChangeStatusButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary status-order me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-s"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    } 
    
    function createEditProductButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary edit-order me-1 mb-1",
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
            "class": "btn btn-sm btn-secondary delete-order me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }

    function createOrder() {
        (async () => {
            const rowId = "new-order";

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");
            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");
            const discountsOptions = await GetSelects(`${DOMAIN_URL}Discount/All`, "id", "displayName");
            discountsOptions.unshift({value: "", label: "none discount"});
            console.log(organizationOptions);

//   "discounts": [
//     {
//       "discountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
//     }
//   ],

            let inputs = createInput("clientName", "text", "DisplayName", "Client name", "", true);
            inputs += createSelect("employeeId", "Employee", employeeOptions, employeeOptions[0]);
            inputs += createSelect("merchantId", "Organization", organizationOptions, organizationOptions[0]);
            inputs += createMultiSelect("discounts", "Discounts", discountsOptions, []);
            //todo add discount
            
            await Swal.fire({
                title: "New Order",
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

                    formObject['discounts'] = [];
                    formArray.forEach((value, index) => {

                        if (!value.value && $(`#${value.name}`).attr('required')) {
                            errors += `Field "${capitalizeFirstLetter(value.name)}" is required<br>`
                            return;
                        }
                        if(value.name === 'discounts') {

                            if(value.value !== ""){
                                formObject['discounts'].push( {discountId: value.value})
                            }
                        }
                        else
                        {
                            formObject[value.name] = value.value;
                        }
                            
                    });

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const saveOrderResponse = await fetch(`${DOMAIN_URL}orders/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveOrderResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new order",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function editOrder(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");
            const discountsOptions = await GetSelects(`${DOMAIN_URL}Discount/All`, "id", "displayName");
            discountsOptions.unshift({value: "", label: "none discount"});
            console.log(organizationOptions);

            const orderResponse = await fetch(`${DOMAIN_URL}Orders/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!orderResponse.ok) {
                return false;
            }

            const order = await orderResponse.json();
            
            let inputs = createInput("clientName", "text", "DisplayName", "Client name", order['clientName'], true);
            inputs += createSelect("employeeId", "Employee", employeeOptions, order['employeeId']);
            inputs += createSelect("merchantId", "Organization", organizationOptions, order['merchantId']);
            inputs += createMultiSelect("discounts", "Discounts", discountsOptions, order['discounts'].map((discount) => discount.discountId));

            inputs += createInput("id", "hidden", "", "", rowId);
            //todo add discount
            await Swal.fire({
                
                title: "Edit Order",
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
                    formObject['discounts'] = [];

                    formArray.forEach((value, index) => {

                        if (!value.value && $(`#${value.name}`).attr('required')) {
                            errors += `Field "${capitalizeFirstLetter(value.name)}" is required<br>`
                            return;
                        }

                        if(value.name === 'discounts') {
                            if(value.value !== ""){
                                formObject['discounts'].push( {discountId: value.value})
                            }
                        }
                        else
                        {
                            formObject[value.name] = value.value;
                        }
                    });

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const updateOrderResponse = await fetch(`${DOMAIN_URL}Orders/Update`, {
                        method: "PUT",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateOrderResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

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

    function deleteOrder(rowId) {
        (async () => {

            await Swal.fire({
                title: `Delete order`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteOrderResponse = await fetch(`${DOMAIN_URL}Orders/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteOrderResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success order deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }

    function statusOrder(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");

            console.log(organizationOptions);

            const orderResponse = await fetch(`${DOMAIN_URL}Orders/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!orderResponse.ok) {
                return false;
            }

            const order = await orderResponse.json();

            let inputs =  createSelect("newStatus", "Status", orderStates, order['status']);
            inputs += createInput("orderId", "hidden", "", "", rowId);
            //todo add discount
            await Swal.fire({

                title: "Edit Order",
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

                    const updateOrderResponse = await fetch(`${DOMAIN_URL}Orders/ChangeStatus`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateOrderResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

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
    
    function tipChangeOrder(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");

            console.log(organizationOptions);

            const orderResponse = await fetch(`${DOMAIN_URL}Orders/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!orderResponse.ok) {
                return false;
            }

            const order = await orderResponse.json();

            let inputs =  createInput("value", "number", "Tips", "amount", order['tips']);
            inputs += createInput("orderId", "hidden", "", "", rowId);
            //todo add discount
            await Swal.fire({

                title: "Edit Order",
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

                    const updateOrderResponse = await fetch(`${DOMAIN_URL}Orders/ChangeTips`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateOrderResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

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
    
    function paymentOrder(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");

            console.log(organizationOptions);

            const orderResponse = await fetch(`${DOMAIN_URL}Orders/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!orderResponse.ok) {
                return false;
            }

            const order = await orderResponse.json();

            let inputs =  createInput("amount", "number", "Value", "amount", order['totalPrice'] - order['paidSum']);
            inputs += createInput("orderId", "hidden", "", "", rowId);
            inputs += createInput("type", "hidden", "", "", 1);
            //todo add discount
            await Swal.fire({

                title: "Add Order Payment",
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

                    const updateOrderResponse = await fetch(`${DOMAIN_URL}Orders/MakePayment`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateOrderResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }
                    updateOrderResponse.json().then((data) => {
                        console.log(data.paymentUrl);
                        if(data.paymentUrl){
                            window.location.href = data.paymentUrl;
                        }
                    });

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

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
    
    function resumePaymentOrder(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");

            console.log(organizationOptions);

            const orderResponse = await fetch(`${DOMAIN_URL}Orders/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!orderResponse.ok) {
                return false;
            }

            const order = await orderResponse.json();
            const notProcessedPayments = order.payments.filter((payment) => !payment.isPaid).map((payment) => {
                return {
                    value: payment.id,
                    label: payment.amount
                    }
                    });
            console.log(notProcessedPayments)

            let inputs =  createInput("orderId", "hidden", "", "", rowId);
            inputs += createSelect("paymentId", "Select payment", notProcessedPayments, notProcessedPayments[0]);
            //todo add discount
            await Swal.fire({

                title: "Add Order Payment",
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

                    const updateOrderResponse = await fetch(`${DOMAIN_URL}Orders/ResumePayment`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateOrderResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }
                    updateOrderResponse.json().then((data) => {
                        console.log(data.paymentUrl);
                        if(data.paymentUrl){
                            window.location.href = data.paymentUrl;
                        }
                    });

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

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
    
    function productChangeOrder(rowId) {
        (async () => {

            const productOptions = await GetSelects(`${DOMAIN_URL}product/All`, "id", "displayName");



            const orderResponse = await fetch(`${DOMAIN_URL}Orders/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!orderResponse.ok) {
                return false;
            }

            const order = await orderResponse.json();

            let inputs =  createInput("quantity", "number", "Quantity", "amount", 0);
            inputs += createInput("orderId", "hidden", "", "", rowId);
            inputs += createSelect("productId", "Product", productOptions, order['productId']);

            await Swal.fire({

                title: "Change Products in Order",
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

                    const updateOrderResponse = await fetch(`${DOMAIN_URL}Orders/ChangeProductQuantity`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateOrderResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${ordersDataTableId}`).DataTable().ajax.reload();

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
    
    function lookOrder(rowId) {
        (async () => {

            const organizationOptions = await GetSelects(`${DOMAIN_URL}Organization/All`, "id", "displayName");

            const employeeOptions = await GetSelects(`${DOMAIN_URL}Employee/All`, "employeeId", "name");

            console.log(organizationOptions);

            const orderResponse = await fetch(`${DOMAIN_URL}Orders/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!orderResponse.ok) {
                return false;
            }

            const order = await orderResponse.json();

            const stateName = orderStates.find((productState) => productState.value === order['status'])?.label;
            
            
            
            
            //   "discounts": [
            //     {
            //       "discountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
            //       "name": "string",
            //       "value": 0,
            //       "type": 1
            //     }
            //   ],
            //   "date": "2024-12-19T04:16:07.339Z"
      
            await Swal.fire({
                width: "100%",
                title: "View Order",
                html: `<div class="container">
                        <div class="row">                  
                            <div class="col-12">Organization: ${order.merchantName}</div>
                            <div class="col-12">Employee: ${order.employeeName}</div>
                            <div class="col-12">Client: ${order.clientName}</div>
                            <div class="col-12">Total tax: ${order.totalTax}</div>
                            <div class="col-12">Discount: ${order.discount}</div>
                            <div class="col-12">Price: ${order.price}</div>
                            <div class="col-12">Price with vat: ${order.priceWithTax}</div>
                            <div class="col-12">Total: ${order.totalPrice}</div>
                            <div class="col-12">Paid: ${order.paidSum}</div>
                            <div class="col-12">Tips: ${order.tips}</div>
                            <div class="col-12">Status: ${stateName}</div>
                        </div>
                        <div class="row">
                            <div class="col-12">Products</div>
                            <div class="col-12">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Product</th>
                                            <th>Quantity</th>
                                            <th>Price</th>
                                            <th>Tax</th>
                                            <th>Discount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        ${order.products.map((product) => {
                                            return `<tr>
                                                        <td>${product.productName}</td>
                                                        <td>${product.quantity}</td>
                                                        <td>${product.price}</td>
                                                        <td>${JSON.stringify(product.tax)}</td>
                                                        <td>${product.discount}</td>
                                                    </tr>`
                                        })}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">Payments</div>
                            <div class="col-12">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Amount</th>
                                            <th>Is paid</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        ${order.payments.map((payment) => {
                                            return `<tr>
                                                        <td>${payment.amount}</td>
                                                        <td>${payment.isPaid}</td>
                                                    </tr>`
                                        })}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12">Discounts</div>
                            <div class="col-12">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Name</th>
                                            <th>Value</th>
                                            <th>Type</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        ${order.discounts.map((discount) => {
                                            return `<tr>
                                                        <td>${discount.name}</td>
                                                        <td>${discount.value}</td>
                                                        <td>${discount.type}</td>
                                                    </tr>`
                                        })}
                                    </tbody>
                                </table>
                            </div>
                            
                        </div>`,
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    
                }
            });
        })();
    }

});