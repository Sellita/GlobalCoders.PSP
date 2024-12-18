async function GetSelects(url, idName, ValueName){
    const organizationsResponse = await fetch(url, {
    method: "POST",
    headers: {
        Authorization: "Bearer " + localStorage.getItem("token"),
        'Content-Type': 'application/json',
    },
    body: JSON.stringify({
        "page": 1,
        "itemsPerPage": 100
    })
});

if(!organizationsResponse.ok){
    Swal.fire({
        title: "Status",
        text: "Oops...Something was wrong",
        icon: "error",
        confirmButtonColor: "#70757d"
    })
    return [];
}

const organizations = await organizationsResponse.json();
let organizationOptions = []
organizations.items.map(organization => {

    organizationOptions.push({
        value: organization[idName],
        label: organization[ValueName]
    });
});

return organizationOptions;
}