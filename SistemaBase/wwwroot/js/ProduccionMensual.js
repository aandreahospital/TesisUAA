document.addEventListener('DOMContentLoaded', () => {
    addEventListeners();
});

const addEventListeners = () => {

    const usuarioInput = document.getElementById('usuario');

    if (usuarioInput) {
        usuarioInput.addEventListener("keydown", (event) => {
            if (event.key === 'Enter' || event.keyCode === 13) {
                event.preventDefault();
                getDatosUsuario(usuarioInput.value);
            }
        });
    } else {
        console.log('El elemento no fue encontrado.');
    }

    if (usuarioInput) {
        usuarioInput.addEventListener("keydown", (event) => {
            if (event.key === 'Enter' || event.keyCode === 13) {
                event.preventDefault();
                fetchAndDisplayNombreTitu(usuarioInput.value);
            }
        });
    } else {
        console.log('El elemento no fue encontrado.');
    }

};

// Agregar una función para abrir el PDF en una nueva pestaña
const abrirPDFEnNuevaPestana = (url) => {
    return new Promise((resolve, reject) => {
        const nuevaPestana = window.open(url, '_blank');
        nuevaPestana.onload = () => {
            resolve();
        };
    });
}
const generarReporte = (e) => {
    e.preventDefault();
    // Mostrar el spinner de carga
    var loader = document.getElementById('spinnerLoader');
    loader.style.display = 'inline-block';

    // Obtener los valores del formulario
    /*const codigoOficina = document.querySelector("select[name='CodigoOficina']").value;*/
    const fechaDesde = document.querySelector("input[name='FechaDesde']").value;
    const fechaHasta = document.querySelector("input[name='FechaHasta']").value;

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
    formData.append("Usuario", document.querySelector("input[name='Usuario']").value);
    formData.append("NombreUsuario", document.querySelector("input[name='NombreUsuario']").value);
    formData.append("FechaDesde", fechaDesde);
    formData.append("FechaHasta", fechaHasta);

    // Realizar la solicitud POST utilizando Axios
    axios({
        method: "post", // Cambiado a POST
        url: "/InformeProduccionMensual/GenerarPdf",
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
}

function fetchAndDisplayNombreTitu(codPersona) {
    const params = {
        usuario: codPersona,
    }
    axios({
        method: "get",
        url: "/TrabajosRecibidosPendientes/GetDatosUsuario",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            if (response.data.success) {
                const mostrarNombreTitular = document.getElementById('mostrarNombreTitu');
                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = response.data.nombre;

                }
            } else {
                const mostrarNombreTitular = document.getElementById('mostrarNombreTitu');
                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = "Persona no encontrada";
                }
            }
        }
    }).catch(function (response) {
        console.log("Error al intentar obtener nombre de persona");
    });
}

function getDatosUsuario(usuario) {
    const params = {
        usuario: usuario,
    }
    axios({
        method: "get",
        url: "/TrabajosRecibidosPendientes/GetDatosUsuario",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            if (response.data.success) {
                const mostrarNombreTitular = document.getElementById('nombreUsuario');
                //const mostrarEsTitular = document.getElementById('EsPropietario_2');

                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = response.data.nombre;

                }
            } else {
                const mostrarNombreTitular = document.getElementById('nombreUsuario');
                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = "Persona no encontrada";
                }
            }
        }
    }).catch(function (response) {
        console.log("Error al intentar obtener nombre de persona");
    });
}