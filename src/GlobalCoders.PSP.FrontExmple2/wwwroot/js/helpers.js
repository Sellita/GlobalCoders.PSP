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

function createSelect(id, title, options = [], selectedValue = '') {
    let optionsHtml = options.map(option => {
        const isSelected = option.value === selectedValue ? 'selected' : '';
        return `<option value="${option.value}" ${isSelected}>${option.label}</option>`;
    }).join('');

    return `<div class="row g-3 align-items-center mb-3">
                <div class="col-auto">
                    <label for="${id}" class="col-form-label">${title}:</label>
                </div>
                <div class="col">
                    <select id="${id}" name="${id}" class="form-select w-100">
                        ${optionsHtml}
                    </select>
                </div>
            </div>`;
}

function daySelectOnChangeHandler(checkbox) {
    console.log(checkbox)
    console.log(checkbox.checked)

    $(`.${checkbox.className}-input`).prop('disabled', !checkbox.checked);

}

function createDaysOfWeekTable(tableId, schedules = []) {

    let rows = '';

    const daysSchedules = [...daysOfWeek];

    for (let i = 0; i < daysSchedules.length; i++) {

        const schedule = schedules.find((element) => element['dayOfWeek'] === i);
        
        daysSchedules[i] = {
            id: i,
            name: daysSchedules[i],
            startTime: schedule?.startTime,
            endTime: schedule?.endTime
        };
    }

    daysSchedules.forEach(function (daySchedule, index) {

        rows += `<input type="hidden" class="${daySchedule.name}-day-input" value="${daySchedule.id}" disabled/>`;

        const startTime = daySchedule.startTime || '';
        const endTime = daySchedule.endTime || '';

        const checked = startTime !== '' || endTime !== '' ? 'checked' : '';
        const disabled = !checked ? 'disabled' : '';

        rows += `<tr>
                    <td><input type="checkbox" class="${daySchedule.name}-day" onchange="daySelectOnChangeHandler(this)" ${checked}/></td>
                    <td>${daySchedule.name}</td>
                    <td><input id="${daySchedule.name}-dayOfWeek-startime" type="text" class="${daySchedule.name}-day-input day-input" value="${startTime}" ${disabled}/></td>
                    <td><input id="${daySchedule.name}-dayOfWeek-endtime" type="text" class="${daySchedule.name}-day-input day-input" value="${endTime}" ${disabled}/></td>
                 </tr>`;

    });


    return `<table id="${tableId}" class="table border-1">
<thead>
<tr>
<td></td></td><td>Day</td><td>Start Time</td><td>End Time</td>
</tr>
</thead>
<tbody>
${rows}
</tbody>
</table>`;
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}