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
            /*
            {
            url: `${DOMAIN_URL}Organization/All`,
            type: "POST",
            crossDomain: true,
            headers: {
                Authorization: "Bearer " + localStorage.getItem("token")
            },
            data: JSON.stringify({
                ""
            })
            
            
            {
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "displayName": "string"
    }
  ],
  "page": 0,
  "totalPages": 0,
  "totalItems": 0
}
        }*/
            
            $.ajax({
                url: `${DOMAIN_URL}Organization/All`,
                type: "POST",
                crossDomain: true,
                contentType: "application/json",
                headers: {
                    Authorization: "Bearer " + localStorage.getItem("token")
                },
                data: JSON.stringify({
                    "page": 1,
                    "itemsPerPage": 100,
                    "displayName": '',
                }),
                success: function (data) {
                    console.log(data);
                    
                    const tmpJson = {
                        recordsTotal: data.totalItems | 0,
                        recordsFiltered: data.totalPages | 0,
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

});