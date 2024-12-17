const organizationsDataTableId = "organizations-data-table";
$(function () {

    $.fn.dataTable.ext.errMode = "none";

    console.log("organizations")

    function init() {
        const dataTableBody = $(`#${organizationsDataTableId} tbody`);

        dataTableBody.on('click', 'tr button.edit-organization', function () {
            const rowId = $(this).attr('data-id');
            editOrganization(rowId);
        });

        dataTableBody.on('click', 'tr button.delete-organization', function () {
            const rowId = $(this).attr('data-id');
            deleteOrganization(rowId);
        });
    }

    const dt = $(`#${organizationsDataTableId}`).DataTable({
        initComplete: function () {
            init();
        },
        dom: '<B"row" <"col-sm-5" l><"col-sm-7 px-0"<r"d-flex justify-content-end">>><"row dt-row mb-2" t><"row"<"col-sm-6"i><"col-sm-6"p>><"row"<"col-sm-6">>',
        "stripeClasses": ['odd-row', 'even-row'],
        ajax: function(data, callback,settings) {
            
            const pageInfo = $(`#${organizationsDataTableId}`).DataTable().page.info();
            
            console.log(pageInfo)
            
            $.ajax({
                url: `${DOMAIN_URL}Organization/All`,
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
                text: 'New Organization',
                attr: {
                    id: 'btn-organization-new',
                    class: 'btn btn-primary'
                },
                action: function (e, dt, node, config) {
                    createOrganization();
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

    function createEditOrganizationButton(data, row) {
        const $editButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-primary edit-organization me-1 mb-1",
            "title": "Edit"
        });

        const $editButtonIcon = $('<em>', {
            "class": "fa-regular fa-pen-to-square"
        });

        $editButton.append($editButtonIcon);

        return $editButton.get(0).outerHTML;
    }

    function createDeleteOrganizationButton(data, row) {
        const $deleteButton = $('<button>', {
            "role": "button",
            "data-id": row.id,
            "class": "btn btn-sm btn-secondary delete-organization me-1 mb-1",
            "title": "Delete"
        });

        const $deleteButtonIcon = $('<em>', {
            "class": "fa-solid fa-trash"
        });

        $deleteButton.append($deleteButtonIcon);

        return $deleteButton.get(0).outerHTML;
    }

    function createOrganization(){
        (async () => {
            const rowId = "new-organization";
            
            let inputs = createInput("displayName", "text", "Display Name", "Display Name", "", true);
            inputs += createInput("legalName", "text", "Legal Name", "Legal Name", "");
            inputs += createInput("address", "text", "Address", "Address", "");
            inputs += createInput("email", "text", "E-mail", "E-mail", "");
            inputs += createInput("mainPhoneNumber", "text", "Phone Number", "Phone number", "");
            inputs += createInput("secondaryPhoneNumber", "text", "Second Phone Number", "Second phone number", "");

            inputs += createDaysOfWeekTable('scheduleTableId',[]);
            
            await Swal.fire({
                title: "New Organization",
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
                    
                    formObject['workingSchedule'] = getworkingSchedule();

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const saveOrganizationResponse = await fetch(`${DOMAIN_URL}Organization/Create`, {
                        method: "POST",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!saveOrganizationResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try save");

                        return;
                    }

                    $(`#${organizationsDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success saved new organization",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
    
    function editOrganization(rowId) {
        (async () => {
            const organizationResponse = await fetch(`${DOMAIN_URL}Organization/Id/${rowId}`, {
                method: "GET",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token"),
                    'Content-Type': 'application/json',
                }
            })

            if (!organizationResponse.ok) {
                return false;
            }

            const organization = await organizationResponse.json();

            let inputs = createInput("displayName", "text", "Display Name", "Display Name", organization["displayName"], true);
            inputs += createInput("legalName", "text", "Legal Name", "Legal Name", organization["legalName"]);
            inputs += createInput("address", "text", "Address", "Address", organization["address"]);
            inputs += createInput("email", "text", "E-mail", "E-mail", organization["email"]);
            inputs += createInput("mainPhoneNumber", "text", "Phone Number", "Phone number", organization["mainPhoneNumber"]);
            inputs += createInput("secondaryPhoneNumber", "text", "Second Phone Number", "Second phone number", organization["secondaryPhoneNumber"]);
            inputs += createInput("id", "hidden", "", "", rowId);

            inputs += createDaysOfWeekTable('scheduleTableId',organization["workingSchedule"]);
            
            await Swal.fire({
                title: "Edit Organization",
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

                    formObject['workingSchedule'] = getworkingSchedule();

                    if (errors) {
                        Swal.showValidationMessage(errors);
                        return false;
                    }

                    const updateOrganizationResponse = await fetch(`${DOMAIN_URL}Organization/Update`, {
                        method: "PUT",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(formObject)
                    });

                    if (!updateOrganizationResponse.ok) {

                        Swal.showValidationMessage("Something was wrong when try update");

                        return;
                    }

                    $(`#${organizationsDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success updated organization",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
            
        })();
    }
    
    function deleteOrganization(rowId) {
        (async () => {
            
            await Swal.fire({
                title: `Delete organization`,
                text: 'Are you sure want to delete?',
                icon: "warning",
                focusConfirm: false,
                allowOutsideClick: false,
                confirmButtonColor: "#70757d",
                showCancelButton: true,
                preConfirm: async () => {

                    const deleteOrganizationResponse = await fetch(`${DOMAIN_URL}Organization/Delete/${rowId}`, {
                        method: "DELETE",
                        headers: {
                            Authorization: "Bearer " + localStorage.getItem("token"),
                            'Content-Type': 'application/json',
                        }
                    });

                    if (!deleteOrganizationResponse.ok) {

                        Swal.fire({
                            title: "Status",
                            text: "Oops...Something was wrong",
                            icon: "error",
                            confirmButtonColor: "#70757d"
                        });

                        return;
                    }

                    $(`#${organizationsDataTableId}`).DataTable().ajax.reload();

                    Swal.fire({
                        title: "Status",
                        text: "Success organization deleted",
                        icon: "success",
                        confirmButtonColor: "#70757d"
                    });
                }
            });
        })();
    }
    
    function getworkingSchedule() {
        
        const workingSchedule = [];

        daysOfWeek.forEach(function (day, index){

            if(!$(`.${day}-day`).prop('checked')) {
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