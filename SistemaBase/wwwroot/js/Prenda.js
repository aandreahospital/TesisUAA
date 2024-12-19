document.addEventListener('DOMContentLoaded', () => {

    addEventListeners();
});


const addEventListeners = () => {

    //var NombreAcreedor = document.getElementById('MostrarAcreedor');
    //var NombreDeudor = document.getElementById('MostrarDeudor');

    const nroEntradaInput = document.getElementById('nroEntradaInput');
    if (nroEntradaInput) {
        nroEntradaInput.addEventListener("keydown", (event) => {
            if (event.key === 'Enter' || event.keyCode === 13) {
                event.preventDefault();
                getDatosFormulario(nroEntradaInput.value);
            }
        });
    } else {
        console.log('El elemento no fue encontrado.');
    }

    const acreedorInput = document.getElementById('acreedor');
    if (acreedorInput) {
        acreedorInput.addEventListener("keydown", (event) => {
            if (event.key === 'Enter' || event.keyCode === 13) {
                event.preventDefault();
                fetchAndDisplayNombreAcreedor(acreedorInput.value);
            }
        });
    } else {
        console.log('El elemento no fue encontrado.');
    }
    const nroBoletaInput = document.getElementById('nroBoleta');

    if (nroBoletaInput) {
        nroBoletaInput.addEventListener("keydown", (event) => {
            if (event.key === 'Enter' || event.keyCode === 13) {
                event.preventDefault();
                const nroBoleta = document.getElementById('nroBoleta');
                BuscarImagen(nroBoleta.value);
                //cargarDistrito(nroBoleta.value);
                //cargarPropietario(nroBoleta.value);

            }
        });
    } else {
        console.log('El elemento no fue encontrado.');
    }
    const deudorInput = document.getElementById('deudor');

    if (deudorInput) {
        deudorInput.addEventListener("keydown", (event) => {
            if (event.key === 'Enter' || event.keyCode === 13) {
                event.preventDefault();
                fetchAndDisplayNombreDeudor(deudorInput.value);
            }
        });
    } else {
        console.log('El elemento no fue encontrado.');
    }

    var distritoSelect = document.getElementById('TipoDistrito');
    distritoSelect.addEventListener('change', function () {
        // Obtener el valor seleccionado del tipo de operacion
        var selectedDistrito = distritoSelect.value;
        getDepartamento(selectedDistrito);
    });

    BuscarImagen(nroBoletaInput.value);

};

function getDepartamento(selectedDistrito) {
    const params = {
        CodDistrito: selectedDistrito,
    }
    axios({
        method: "get",
        url: "/InscripcionCustom/GetDepartamento",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            if (response.data.success) {
                const mostrarNombreTitular = document.getElementById('Departamento');
                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = response.data.nombre;

                }
            }
        }
    }).catch(function (response) {
        console.log("Error al intentar obtener nombre de persona");
    });
}

const getDatosFormulario = (nroEntrada) => {
    const params = {
        nEntrada: nroEntrada,
    }
    const queryString = new URLSearchParams(params).toString();
    axios({
        method: "get",
        url: "/Prenda/Get",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        if (response.data.success == false) {
            swal({
                icon: 'error',
                title: 'Error...',
                text: response.data.errorMessage
            });
            //loader.style.display = 'none';
        } else {
            console.log({ 'then ': response });
            if (response.status == 200) {
                console.log({ 'reload ': response })
                if (response.data) {
                    const tempElement = document.createElement('div');
                    tempElement.innerHTML = response.data;
                    // Obtener solo la parte relevante (en este caso, la tabla)
                    const newform = tempElement.querySelector('#formPrenda');
                    // Reemplazar el contenido actual de listbody con la nueva tabla
                    const form = document.getElementById('formPrenda');
                    form.innerHTML = '';
                    form.appendChild(newform);
                    addEventListeners();

                    const nroBoleta = document.getElementById('nroBoleta');
                    BuscarImagen(nroBoleta.value);
                    bloquearFormulario(datosFormulario);

                }
                //else {
                //    //Limpiar campos
                //    document.getElementById("montoLiquidacion").value = "1111111";
                //}
            }
        }
    })
        .catch(function (response) {
            console.log("error al intentar obtener datos para autocompletar prenda")
        }).finally(() => {
            refrestjsfunction();
            //addEventListeners();
        });
}

function fetchAndDisplayNombreAcreedor(codPersona) {
    const params = {
        codPersona: codPersona,
    }
    axios({
        method: "get",
        url: "/InscripcionCustom/GetNombreByCodPersona",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            if (response.data.success) {
                const mostrarNombreTitular = document.getElementById('nombreAcreedor');

                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = response.data.nombre;

                }
            } else {
                const mostrarNombreTitular = document.getElementById('nombreAcreedor');
                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = "Persona no encontrada";
                }
            }
        }
    }).catch(function (response) {
        console.log("Error al intentar obtener nombre de persona");
    });
}
function fetchAndDisplayNombreDeudor(codPersona) {
    const params = {
        codPersona: codPersona,
    }
    axios({
        method: "get",
        url: "/InscripcionCustom/GetNombreByCodPersona",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            if (response.data.success) {
                const mostrarNombreTitular = document.getElementById('nombreDeudor');
               
                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = response.data.nombre;
                
                }
            } else {
                const mostrarNombreTitular = document.getElementById('nombreDeudor');
                if (mostrarNombreTitular) {
                    mostrarNombreTitular.value = "Persona no encontrada";
                }
            }
        }
    }).catch(function (response) {
        console.log("Error al intentar obtener nombre de persona");
    });
}


const guardarDatos =  (e) => {
    e.preventDefault();
    //document.getElementById('btnSaved').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('formPrenda');
    // Mostrar el loader
    var loader = document.getElementById('spinnerLoader');
    loader.style.display = 'inline-block';
    const formData = new FormData();
    formData.append('Libro', document.querySelector('input[name="Libro"]').value)
    formData.append('Folio', document.querySelector('input[name="Folio"]').value)
    formData.append('NroEntrada', document.querySelector('input[name="NroEntrada"]').value)
    formData.append('FechaInscripcion', document.querySelector('input[name="FechaInscripcion"]').value)
    formData.append('Acreedor', document.querySelector('input[name="Acreedor"]').value)
    formData.append('Deudor', document.querySelector('input[name="Deudor"]').value)
    formData.append('FechaOperacion', document.querySelector('input[name="FechaOperacion"]').value)
    formData.append('FechaVencimiento', document.querySelector('input[name="FechaVencimiento"]').value)
    formData.append('MontoDeJusticia', document.querySelector('input[name="MontoDeJusticia"]').value)
    formData.append('MontoPrenda', document.querySelector('input[name="MontoPrenda"]').value)
    formData.append('NroBoleta', document.querySelector('input[name="NroBoleta"]').value)
    formData.append('NroBoletaSenal', document.querySelector('input[name="NroBoletaSenal"]').value)
    formData.append('CodDistrito', document.querySelector('select[name="CodDistrito"]').value)
    formData.append('Instrumento', document.querySelector('select[name="Instrumento"]').value)
    formData.append('Comentario', document.getElementById('getComentarioInput').value)
    formData.append('Departamento', document.getElementById("Departamento").value)

    axios({
        method: "post",
        url: "/Prenda/Create",
        data: formData,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    })
        .then(function (response) {
            console.log({ 'then ': response });
            if (response.status == 200) {
                console.log({ 'reload ': response });
                // En caso de error, manejar el mensaje del servidor si está presente
                window.location.replace(`/Prenda`);
              // location.reload();
                return;
            }

        })
        .catch(function (response) {

            desbloquearFormulario(datosFormulario);
            document.getElementById('btnSaved').disabled = false
            loader.style.display = 'none';
        })

}
const recuperarDatosParaPDF = async () => {
    const formData = new FormData();
    formData.append('Libro', document.querySelector('input[name="Libro"]').value)
    formData.append('Folio', document.querySelector('input[name="Folio"]').value)
    formData.append('NroEntrada', document.querySelector('input[name="NroEntrada"]').value)
    formData.append('Acreedor', document.querySelector('input[name="Acreedor"]').value)
    formData.append('Deudor', document.querySelector('input[name="Deudor"]').value)
    formData.append('CodDistrito', document.querySelector('select[name="CodDistrito"]').value)
    formData.append('Instrumento', document.querySelector('select[name="Instrumento"]').value)
    formData.append('Comentario', document.getElementById('getComentarioInput').value)

    await generarPDF(formData);
};

let pdfUrl; // Declara pdfUrl aquí

const generarPDF = async (formData) => {

    await axios({
        method: "post",
        url: "/Prenda/GenerarPdf", // Cambia la ruta según la ubicación de tu controlador
        data: formData,
        responseType: 'blob', // Indica que esperamos un archivo binario como respuesta
    })
        .then(function (response) {
            const blob = new Blob([response.data], { type: 'application/pdf' });
            //const url = window.URL.createObjectURL(blob);
            pdfUrl = window.URL.createObjectURL(blob); // Asigna la URL del PDF a pdfUrl

            /*const a = document.createElement('a');
            a.href = url;
            // Generar el nombre del archivo con el formato deseado
            const fileName = `EntradaPorLiquidación-Nro${formData.get('NumeroEntrada')}-${formatDate(new Date())}.pdf`;
            a.download = fileName;
            a.click();*/

            // Abre el PDF en una ventana emergente
            var pdfWindow = window.open(pdfUrl, '_blank');

            // Espera a que la ventana emergente se abra y luego recarga la página
            window.onfocus = function () {
                console.log('Ventana enfocada, recargando la página.');
                //location.reload();
            };

        })
        .catch(function (error) {
            console.error("Error al generar el PDF:", error);
            desbloquearFormulario(datosFormulario);
            document.getElementById('btnSaved').disabled = false
            loader.style.display = 'none';
        });
};

var selectedButton = null;
const procesarPrenda = async (e, accion) => {
    e.preventDefault();
    e.stopPropagation();
    document.getElementById("btnSaved").disabled = true;
    //document.getElementById("btnObservar").disabled = true;
    //document.getElementById("btnNotaNegativa").disabled = true;
    var comentarioInput = document.getElementById("observaciones");
    var idInput = document.getElementById("nroEntradaInput");
    var getComentario = document.getElementById("getComentarioInput").value;

    var id = idInput.value;
    var comentario = comentarioInput.value;
    var comentario2 = getComentario;
    if (comentario != "") {
        if (selectedButton == accion && accion != "aprobar") {
            switch (accion) {
                case "observar":
                    //peticion Axios para cambiar estado de entrada
                    var nuevoEstado = "Observado/Registrador";
                    console.log("Seleccionaste la opción notaNegativa");
                    break;
                case "notaNegativa":
                    //peticion Axios para cambiar estado de entrada
                    var nuevoEstado = "Nota Negativa/Registrador";
                    console.log("Seleccionaste la opción notaNegativa");
                    break;

                default:
                    console.log("Opción no reconocida");

            }
            await actualizarEstado(id, nuevoEstado, comentario);
            //guardarDatos(e);

            // Construir la URL del PDF
            var urlPDF = '/Informe/GenerarPdf?nEntrada=' + id + '&comentario=' + encodeURIComponent(comentario);

            // Abrir el PDF en una nueva pestaña y esperar a que cargue
            abrirPDFEnNuevaPestana(urlPDF).then(() => {
                // Una vez que la pestaña del PDF ha cargado, recargar la página
                window.location.reload();
            });
        }
    }
    if (accion == "notaNegativa" || accion =="observar") {
        comentarioInput.disabled = false;
        comentarioInput.placeholder = "Ingrese un observacion para " + accion;
        selectedButton = accion;
    }

    if (accion == "aprobar") {
        var nuevoEstado = "Aprobado/Registrador";
        await actualizarEstado(id, nuevoEstado, comentario);
         guardarDatos(e);

        return;
    }


    return;
}

// Agregar una función para abrir el PDF en una nueva pestaña
const abrirPDFEnNuevaPestana = (url) => {
    return new Promise((resolve, reject) => {
        const nuevaPestana = window.open(url, '_blank');
        nuevaPestana.onload = () => {
            resolve();
        };
    });
}

//Para Actualizar los Estados
async function actualizarEstado(id, nuevoEstado, comentario) {
    try {
        const response = await fetch(`/Prenda/CambiarEstado?id=${id}&nuevoEstado=${nuevoEstado}&comentario=${comentario}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            // Aquí puedes manejar el caso en que la solicitud fue exitosa
            console.log('Estado actualizado correctamente.');
            //location.reload();
        } else {
            // Aquí puedes manejar el caso en que la solicitud no fue exitosa
            console.error('Error al actualizar el estado.');
        }
    } catch (error) {
        console.error('Error en la solicitud:', error);
    }
}

const abrirRepPersona = (e) => {
    e.preventDefault();
    var rep = document.getElementById("acreedor");
    axios({
        baseURL: "/InscripcionCustom/EditPersona",
        method: 'Get',
        params: {
            idPersona: rep.value
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        //var nuevaVista = response.data;
        //window.open(nuevaVista);
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();

        const Acreedor = document.getElementById('acreedor');
        if (Acreedor) {
            Acreedor.addEventListener("keydown", (event) => {
                if (event.key === 'Enter' || event.keyCode === 13) {
                    event.preventDefault();
                    fetchAndDisplayNombreAcreedor(Deudor.value);
                }
            });
        } else {
            console.log('El elemento no fue encontrado.');
        }

        const Deudor = document.getElementById('deudor');
        if (Deudor) {
            Deudor.addEventListener("keydown", (event) => {
                if (event.key === 'Enter' || event.keyCode === 13) {
                    event.preventDefault();
                    fetchAndDisplayNombreDeudor(Deudor.value);
                }
            });
        } else {
            console.log('El elemento no fue encontrado.');
        }
    });
}

const abrirTelPersona = (e) => {
    e.preventDefault();
    var acreedor = document.getElementById("acreedor");
    axios({
        baseURL: "/InscripcionCustom/EditTelPersona",
        method: 'Get',
        params: {
            idPersona: rep.value
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
        var codAcreedor = document.getElementById("codAcreedorModalTel");
        codAcreedor.value = rep.value;

    });

    e.preventDefault();
    var deudor = document.getElementById("deudor");
    axios({
        baseURL: "/InscripcionCustom/EditTelPersona",
        method: 'Get',
        params: {
            idPersona: rep.value
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
        var codDeudor = document.getElementById("codDeudorModalTel");
        codDeudor.value = deudor.value;

    });

}

const abrirIdenPersona = (e) => {
    e.preventDefault();
    var acreedor = document.getElementById("acreedor");

    axios({
        baseURL: "/InscripcionCustom/AddPersonaIden",
        method: 'Get',
        params: {
            idPersona: rep.value
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
        document.getElementById("CodPersona").value = acreedor.value;
    });

    e.preventDefault();
    var deudor = document.getElementById("deudor");

    axios({
        baseURL: "/InscripcionCustom/AddPersonaIden",
        method: 'Get',
        params: {
            idPersona: rep.value
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
        document.getElementById("CodPersona").value = deudor.value;
    });

}

const abrirRepDirPersona = (e) => {
    e.preventDefault();
    var acreedor = document.getElementById("acreedor");
    axios({
        baseURL: "/InscripcionCustom/EditDirPersona",
        method: 'Get',
        params: {
            idPersona: rep.value
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();

    });

    e.preventDefault();
    var deudor = document.getElementById("deudor");
    axios({
        baseURL: "/InscripcionCustom/EditDirPersona",
        method: 'Get',
        params: {
            idPersona: rep.value
        },
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();

    });

}


const titularmodalcreate = (e) => {
    e.preventDefault();
    axios({
        baseURL: "/InscripcionCustom/AddTitular",
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click();
        const acreedor = document.getElementById('acreedor');
        if (acreedor) {
            acreedor.addEventListener("keydown", (event) => {
                if (event.key === 'Enter' || event.keyCode === 13) {
                    event.preventDefault();
                    fetchAndDisplayNombreAcreedor(acreedor.value);
                }
            });
        } else {
            console.log('El elemento no fue encontrado.');
        }

        const deudor = document.getElementById('deudor');
        if (deudor) {
            deudor.addEventListener("keydown", (event) => {
                if (event.key === 'Enter' || event.keyCode === 13) {
                    event.preventDefault();
                    fetchAndDisplayNombreDeudor(deudor.value);
                }
            });
        } else {
            console.log('El elemento no fue encontrado.');
        }
    });
};

const fetchImagenMarca = (data) => {

    axios({
        method: "get",
        url: "/Informe/BuscarImagen",
        params: {
            nroBoleta: data
        },
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            if (response.data.success) {
                const imagenMarca = document.getElementById('imgmarca');
                const imagenSenhal = document.getElementById('imgsenhal');
                if (imagenMarca) {
                    imagenMarca.setAttribute("src", response.data.srcmarca);
                }
                if (imagenSenhal) {
                    imagenSenhal.setAttribute("src", response.data.srcsenhal);
                }
                const distrito = document.getElementById('DescDistrtito');
                if (distrito) {
                    distrito.value = response.data.distrito;
                }
                const departamento = document.getElementById('Departamento');
                if (departamento) {
                    departamento.value = response.data.departamento;
                }
            }
        }
    }).catch(function (response) {
        console.log("Error al intentar imagen Marca");
    });
}


function BuscarImagen(nroBoleta) {
    const params = {
        nroBoleta: nroBoleta,
    }
    axios({
        method: "get",
        url: "/Informe/BuscarImagen",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            if (response.data.success) {
                const imagenMarca = document.getElementById('imgmarca');
                const imagenSenhal = document.getElementById('imgsenhal');
                if (imagenMarca) {
                    imagenMarca.setAttribute("src", response.data.srcmarca);
                }
                if (imagenSenhal) {
                    imagenSenhal.setAttribute("src", response.data.srcsenhal);
                }
                //const distrito = document.getElementById('DescDistrtito');
                //if (distrito) {
                //    distrito.value = response.data.distrito;
                //}
                //const departamento = document.getElementById('Departamento');
                //if (departamento) {
                //    departamento.value = response.data.departamento;
                //}
            }
        }
    }).catch(function (response) {
        console.log("Error al intentar imagen Marca");
    });
}



// Función para formatear la fecha como 'dd-MM-yyyy'
const formatDate = (date) => {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    return `${day}-${month}-${year}`;
};

// Asociar la función recuperarDatosParaPDF a un evento (por ejemplo, clic en un botón)
const botonGenerarPDF = document.getElementById('btnAbm');
botonGenerarPDF.addEventListener('click', recuperarDatosParaPDF);