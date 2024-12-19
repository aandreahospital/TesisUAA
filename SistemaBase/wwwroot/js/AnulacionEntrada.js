document.addEventListener('DOMContentLoaded', () => {

    addEventListeners();
});


const addEventListeners = () => {
    // Este código se ejecutará cuando todos los elementos del DOM estén disponibles
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


};


const getDatosFormulario = (nroEntrada) => {
    const params = {
        nEntrada: nroEntrada,
    }
    const queryString = new URLSearchParams(params).toString();
    axios({
        method: "get",
        url: "/AnulacionEntrada/Get",
        params: params,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    }).then(function (response) {
        console.log({ 'then ': response });
        if (response.status == 200) {
            console.log({ 'reload ': response })
            if (response.data) {
                const tempElement = document.createElement('div');
                tempElement.innerHTML = response.data;
                // Obtener solo la parte relevante (en este caso, la tabla)
                const newform = tempElement.querySelector('#formAnulacion');
                // Reemplazar el contenido actual de listbody con la nueva tabla
                const form = document.getElementById('formAnulacion');
                form.innerHTML = '';
                form.appendChild(newform);
                //bloquearFormulario(datosFormulario);
            }
            //else {
            //    //Limpiar campos
            //    document.getElementById("montoLiquidacion").value = "1111111";
            //}
        }
    }).catch(function (response) {
        console.log("error al intentar obtener datos para autocompletar mesa salida")
    }).finally(() => {
        refrestjsfunction();
        addEventListeners();
    });
}
//
const refrestjsfunction = () => {
    const refreshjs = document.querySelectorAll("script");
    refreshjs.forEach((item) => {
        var src = item.src;
        item.remove();
        var script,
            scriptTag;
        script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = src;
        scriptTag = document.getElementsByTagName('script')[0];
        scriptTag.parentNode.insertBefore(script, scriptTag);
    })
};

const guardarAnulacion = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnSaved').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('formAnulacion');
    // Mostrar el loader
    var loader = document.getElementById('spinnerLoader');
    loader.style.display = 'inline-block';
    const formData = new FormData();
    formData.append('NroEntrada', document.getElementById('nroEntradaInput').value)
    formData.append('NroFormulario', document.querySelector('input[name="NroFormulario"]').value)
    formData.append('FechaAnulacion', document.querySelector('input[name="FechaAnulacion"]').value)
    formData.append('CodOficina', document.querySelector('select[name="CodOficina"]').value)
    formData.append('TipoSolicitud', document.querySelector('select[name="TipoSolicitud"]').value)
    formData.append('EstadoEntrada', document.querySelector('select[name="EstadoEntrada"]').value)
    formData.append('NombreTit', document.querySelector('input[name="NombreTit"]').value)
    formData.append('IdMotivo', document.querySelector('select[name="IdMotivo"]').value)
    axios({
        method: "post",
        url: "/AnulacionEntrada/Create",
        data: formData,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    })
        .then(function (response) {
            console.log({ 'then ': response });
            if (response.status == 200) {
                swal({
                    icon: 'success',
                    title: 'Listo',
                    text: 'La operación se realizó correctamente.'
                }).then(() => {
                    // Código para actualizar la pantalla después de cerrar el modal
                    location.reload();
                });
            }

        })
        .catch(function (response) {

            desbloquearFormulario(datosFormulario);
            document.getElementById('btnSaved').disabled = false
            loader.style.display = 'none';
        })

}