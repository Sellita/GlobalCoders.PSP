function createInput(id, type, title, placeholder, value, required = false) {
    
    const label = type !== 'hidden' ? `<div class="col-auto">
                <label for="${id}" class="col-form-label">${title}:</label>
            </div>` : '';
    
    return `<div class="row g-3 align-items-center mb-3">
            ${label}
            <div class="col">
                <input type="${type}" id="${id}" name="${id}" class="form-control w-100" autocomplete="off" placeholder="${(placeholder) ? placeholder : title}" value="${value}" ${required ? 'required' : ''}>
            </div>
           </div>`;
}

function daySelectOnChangeHandler(checkbox){
    console.log(checkbox)
    console.log(checkbox.checked)
    
    $(`.${checkbox.className}-input`).prop('disabled', !checkbox.checked);
    
}
function createDaysOfWeekTable(tableId, schedule = []){

    let rows = '';
    
    daysOfWeek.forEach(function (day, index) {
        console.log(day, index);
        
        rows += `<input type="hidden" class="${day}-day-input" value="${index}" disabled/>`;

        if(!schedule[index]){
            rows += `<tr>
                        <td><input type="checkbox" class="${day}-day" onchange="daySelectOnChangeHandler(this)"/></td>
                        <td>${day}</td>
                        <td><input id="${day}-dayOfWeek-startime" type="text" class="${day}-day-input day-input" disabled/></td>
                        <td><input id="${day}-dayOfWeek-endtime" type="text" class="${day}-day-input day-input" disabled/></td>
                     </tr>`;
            
            return;
        }
        
        const startTime = schedule[index]['startTime'] || '';
        const endTime = schedule[index]['endTime'] || '';
        
        rows += `<tr>
                    <td><input type="checkbox" class="${day}-day" onchange="daySelectOnChangeHandler(this)" checked/></td>
                    <td>${day}</td>
                    <td><input id="${day}-dayOfWeek-startime" type="text" class="${day}-day-input day-input" value="${startTime}"/></td>
                    <td><input id="${day}-dayOfWeek-endtime" type="text" class="${day}-day-input day-input" value="${endTime}"/></td>
                 </tr>`;

    });
    
    
    const table = `<table id="${tableId}" class="table border-1">
<thead>
<tr>
<td></td></td><td>Day</td><td>Start Time</td><td>End Time</td>
</tr>
</thead>
<tbody>
${rows}
</tbody>
</table>`;
    
    return table;
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}