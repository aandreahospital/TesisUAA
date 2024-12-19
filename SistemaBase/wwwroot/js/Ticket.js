const cargarTitulares = (transaccion) => {
    var parametros = {
        idTransaccion: transaccion
    };
    $.ajax({
        type: "GET",
        url: "/TransaccionCustom/CargarTitulares", // Reemplaza "TuControlador" con el nombre de tu controlador
        dataType: "json",
        data: parametros,
        success: function (data) {
            // Los datos del servidor (array) están en la variable "data
            addTitularMarcaTrans(data);
        },
        error: function () {
            console.log("Error al obtener datos del servidor.");
        }
    });
}

const addTitularMarcaTrans = (datos) => {
    var table;
    if ($.fn.DataTable.isDataTable('#titularesMarcaTable')) {
        table = $('#titularesMarcaTable').DataTable();
    }
    else {
        table = $('#titularesMarcaTable').DataTable({
            searching: false, // Oculta la barra de búsqueda
            paging: false,
            info: false,
            columns: [
                { width: '10%' },  // Ancho de la primera columna
                { width: '50%' },  // Ancho de la segunda columna
                { width: '30%' },  // Ancho de la segunda columna
                { width: '10%' }
            ],
            columnDefs: [
                {
                    targets: 0, // La tercera columna (índice 2)
                    render: function (data, type, row) {
                        row[0] = "EL/LA SEÑOR/A";
                        row[2] = "CON DOC. IDENT. IDENT. NRO";
                        return row;
                    }
                },
                {
                    visible: false, // Hacer esta columna invisible
                    render: function (data, type, row) {
                        return data; // Renderizar normalmente
                    }
                }
            ],
            select: {
                style: 'os', // Estilo de selección (puede ser 'single', 'multi', 'os', etc.)
                selector: '.select-checkbox' // Selector de los checkboxes
            },
        });
    }
    if (datos != null) {

        datos.forEach((item) => {
            let titularData = Object.values(item);
            table.row.add(titularData).draw();
        })
    }
    else {
        const titularDataForm = document.getElementById("FormModalCreate");
        titularData = new FormData(titularDataForm);
    }
};