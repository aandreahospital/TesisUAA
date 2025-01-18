const EditarDatos = () => {
    document.getElementById('btnGuardar').disabled = false;
    document.getElementById('btnEditar').disabled = true;
    //document.getElementById('CodPersona').disabled = false;
    document.getElementById('Nombre').disabled = false;
    document.getElementById('Correo').disabled = false;
    document.getElementById('FecNacimiento').disabled = false;
    document.getElementById('Sexo').disabled = false;
    document.getElementById('DireccionParticular').disabled = false;
    document.getElementById('SitioWeb').disabled = false;
    
}


const verifyCheckbox = (id) => {
    var checkBox = document.getElementById(id + "_2");
    var text = document.getElementById(id);
    if (checkBox.checked == true) {
        text.value = "S";
    } else {
        text.value = "N";
    }
};

const GuardarDatos = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnGuardar').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('formDatosAlumno');
    const formData = new FormData();
    formData.append('CodPersona', document.getElementById('CodPersona').value)
    formData.append('Nombre', document.querySelector('input[name="Nombre"]').value)
    formData.append('Email', document.querySelector('input[name="Correo"]').value)
    formData.append('FecNacimiento', document.querySelector('input[name="FecNacimiento"]').value)
    formData.append('Sexo', document.querySelector('input[name="Sexo"]').value)
    formData.append('DireccionParticular', document.querySelector('input[name="DireccionParticular"]').value)
    formData.append('SitioWeb', document.querySelector('input[name="SitioWeb"]').value)
    axios({
        method: "post",
        url: "/DatosAlumno/GuardarDatos",
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

const AddLaboral = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnAddLaboral').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();
    formData.append('CodUsuario', document.getElementById('usuario').value)
    formData.append('Lugartrabajo', document.getElementById('Lugartrabajo').value)
    formData.append('CargoIdcargo', document.getElementById('CargoIdcargo').value)
    formData.append('Direccionlaboral', document.getElementById('Direccionlaboral').value)
    formData.append('Antiguedad', document.getElementById('Antiguedad').value)
    formData.append('Herramientas', document.getElementById('Herramientas').value)
    axios({
        method: "post",
        url: "/DatosAlumno/GuardarLaboral",
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
            document.getElementById('btnAddLaboral').disabled = false
            loader.style.display = 'none';
        })

}

const abrirLaboral = (e) => {
    e.preventDefault();
    axios({
        baseURL: "/DatosAlumno/AbrirLaboral",
        method: 'Get',
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
        
    });
}

const abrirAcademico = (e) => {
    e.preventDefault();
    axios({
        baseURL: "/DatosAlumno/AbrirAcademico",
        method: 'Get',
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

    });
}


const AddAcademico = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnAddAcademico').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();
    formData.append('CodUsuario', document.getElementById('CodUsuario').value)
    formData.append('Idcentroestudio', document.getElementById('Idcentroestudio').value)
    formData.append('Idcarrera', document.getElementById('Idcarrera').value)
    formData.append('Fechainicio', document.getElementById('Fechainicio').value)
    formData.append('Fechafin', document.getElementById('Fechafin').value)
    formData.append('Estado', document.getElementById('Estado').value)
    axios({
        method: "post",
        url: "/DatosAlumno/GuardarEducacion",
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
            document.getElementById('btnAddAcademico').disabled = false
            loader.style.display = 'none';
        })

}