let map;
let plots;
window.onload = async function () {
    initialize();
}
function goToPopUp(longitude, latitude) {
    map.setView([longitude, latitude], 18);
}
async function initialize() {
    map = new L.map('map').setView([50.041187, 21.999121], 18);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap'
    }).addTo(map);
}

async function getPlots(searchText) {
    if (!searchText) {
        searchText = "";
    }
    plots = await fetch(`/plots?searchText=${searchText}`).then(response => response.json()).then(data => {
        let areaSum = 0;
        const plotTable = document.querySelector(".userPlots");
        plotTable.innerHTML = "";
        data.forEach(function (plot, idx) {
            // Create marker
            var marker = new L.marker([plot.longitude, plot.latitude])
                .bindPopup(`<div class="text-center">${plot.city} ${plot.plotNumber} <br> ${plot.tillage} <br> ${plot.area}ha</div>`).addTo(map);
            let plotArea = parseFloat(plot.area);


            if (plot.area === null) {
                plot.area = 0;
            }

            if (plot.tillage === null) {
                plot.tillage = "";
            }

            if (plotArea === null || isNaN(plotArea) === true || plotArea === undefined) {
                plotArea = 0;
            }

            areaSum += plotArea;

            // Create table row with plot
            const plotRow = document.createElement("tr");
            plotRow.classList.add("plotListItemStyle");
            plotRow.addEventListener("click", (() => goToPopUp(plot.longitude, plot.latitude)));
            plotRow.innerHTML = `
              <th class="align-middle" scope="row">${idx + 1}</th>
              <td class="align-middle">${plot.city}</td>
              <td class="align-middle">${plot.plotNumber}</td>
              <td class="align-middle">${plot.area}</td>
              <td class="align-middle">${plot.tillage}</td>
              <td class="align-middle">
                <form class="d-flex justify-content-center" method="post" action="/plot/${plot.id}">
                    <a href="/plot/${plot.id}/edit" class="btn btn-sm btn-primary">Edit</a>
                    <button type="submit" class="btn btn-sm btn-danger">Delete</button>
                </form>
              </td>
            `;
            plotTable.appendChild(plotRow);
            goToPopUp(plot.longitude, plot.latitude);
        })
        // Create summary table row
        areaSum = areaSum.toFixed(3);
        document.querySelector(".summary").innerHTML = `<div class="d-flex justify-content-between border-bottom"><p class="mb-0">Suma: </p><p class="mb-0">${areaSum} ha</p></div>`;
    })
        .catch(error => console.error("Unable to get items.", error));
}
function showForm() {
    document.querySelector(".plotForm").classList.toggle("hidden");
}

