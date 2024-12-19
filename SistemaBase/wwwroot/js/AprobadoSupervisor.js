document.addEventListener('DOMContentLoaded', () => {
    addEventListeners();
});

// Agregar una función para abrir el PDF en una nueva pestaña
const abrirPDFEnNuevaPestana = (url) => {
    return new Promise((resolve, reject) => {
        const nuevaPestana = window.open(url, '_blank');
        nuevaPestana.onload = () => {
            resolve();
        };
    });
}
function addEventListeners() {
    document.querySelector(".btnAbm").addEventListener("click", function () {
        // Mostrar el spinner de carga
        var loader = document.getElementById("spinnerLoader");
        loader.style.display = 'inline-block';
        // Obtener los valores del formulario
      /*  const codigoOficina = document.querySelector("select[name='CodigoOficina']").value;*/
        const fechaDesde = document.querySelector("input[name='FechaDesde']").value;
        const fechaHasta = document.querySelector("input[name='FechaHasta']").value;
        const entradaDesde = document.querySelector("#entradaDesdeInput").value;
        const entradaHasta = document.querySelector("#entradaHastaInput").value;
        // Validar si las fechas están vacías
        if (fechaDesde === '' || fechaHasta === '') {
            // Ocultar el spinner de carga
            loader.style.display = 'none';
            // Mostrar mensaje de error
            swal({
                icon: 'error',
                title: 'Error...',
                text: 'Debe ingresar la fecha'
            });
            return; // Detener la ejecución
        }
        // Crear un objeto con los datos del formulario
        const formData = new FormData();
        formData.append("Oficina", document.querySelector("select[name='Oficina']").value);
        //formData.append("CodigoOficina", codigoOficina);
        formData.append("FechaDesde", fechaDesde);
        formData.append("FechaHasta", fechaHasta);
        formData.append("NumeroEntradaDesde", entradaDesde);
        formData.append("NumeroEntradaHasta", entradaHasta);
        formData.append("Usuario", document.querySelector("input[name='Usuario']").value);

        // Realizar la solicitud POST utilizando Axios
        axios({
            method: "post", // Cambiado a POST
            url: "/AprobadoSupervisores/GenerarPdf",
            data: formData, // Enviar los datos en el cuerpo de la solicitud
            headers: {
                "Content-Type": "application/pdf", // Corregir el tipo de contenido
            },
            responseType: 'blob',
        })
            .then(function (response) {
                // Ocultar el spinner de carga
                document.getElementById("spinnerLoader").style.display = "none";

                const blob = new Blob([response.data], { type: 'application/pdf' });
                const urlPDF = URL.createObjectURL(blob);
                // Abrir el PDF en una nueva pestaña y esperar a que cargue
                abrirPDFEnNuevaPestana(urlPDF).then(() => {
                    // Una vez que la pestaña del PDF ha cargado, recargar la página
                    window.location.reload();
                });
            })
            .catch(function (error) {
                // Manejar errores
                console.error(error);
                // Ocultar el spinner de carga en caso de error
                document.getElementById("spinnerLoader").style.display = "none";
                swal({
                    icon: 'error',
                    title: 'Error...',
                    text: 'No se encontraron datos en ese rango de fecha'
                });
            });
    });
}