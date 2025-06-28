

const modaldelete = (IdOfertaAcademica) => {
    axios({
        url: `/Vitrina/Delete?IdOfertaAcademica=${IdOfertaAcademica}`,
        method: 'get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'X-Response-View': 'Json'
        }
    }).then(response => {
        document.getElementById("details").innerHTML = response.data;
        document.getElementById("detailsview").click(); // Esto dispara el modal
    });
};


const submitforms = (e, url, id) => {
    e.preventDefault();
    document.getElementById("loader_inv").style.visibility = "visible";
    const form = document.getElementById(id);
    const formData = new FormData(form);

    axios({
        method: "post",
        url: "Vitrina/" + url,
        data: formData,
        headers: {
            "Content-Type": "multipart/form-data",
            'X-Response-View': 'Json'
        }
    })
        .then(function (response) {
            location.reload();

        })
        .catch(function (response) {
            console.log(response);
        })
        .finally(() => {
            document.getElementById("loader_inv").style.visibility = "hidden";
            refrestjsfunction();
        });
};



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
}





const AbrirOferta = (e) => {
    e.preventDefault();
    axios({
        baseURL: "/Vitrina/AbrirOferta",
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

const AddOferta = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnAddOferta').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();

    // Para el campo 'AnexoPDF'
    var anexoInput = $('#Adjunto')[0];
    if (anexoInput.files.length === 0 || anexoInput.files[0].size === 0) {
        swal({
            icon: 'warning',
            title: 'Archivo requerido',
            text: 'Debe adjuntar un archivo válido.'
        });
        ///document.getElementById('btnAddForo').disabled = false;
        return;
    }

    // Asegúrate de que se ha seleccionado un archivo
    if (anexoInput.files.length > 0) {
        formData.append('ArchivoPDF', anexoInput.files[0]);
    }
    //formData.append('CodUsuario', document.getElementById('usuario').value)
    formData.append('Titulo', document.getElementById('Titulo').value)
    formData.append('Descripcion', document.getElementById('Descripcion').value)
  // formData.append('FechaCreacion', document.getElementById('FechaCreacion').value)
    formData.append('FechaCierre', document.getElementById('FechaCierre').value)
    // formData.append('Adjunto', document.getElementById('Adjunto').value)
    axios({
        method: "post",
        url: "/Vitrina/AddOferta",
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

const abrirEditOferta = (id) => {
    const params = {
        id: id,
    }
    axios({
        baseURL: "/Vitrina/AbrirEditOferta",
        method: 'Get',
        params: params,
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


const AddEditOferta = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnAddEditOferta').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();

    // Para el campo 'AnexoPDF'
    var anexoInput = $('#Adjunto')[0];
    if (anexoInput.files.length === 0 || anexoInput.files[0].size === 0) {
        swal({
            icon: 'warning',
            title: 'Archivo requerido',
            text: 'Debe adjuntar un archivo válido.'
        });
        //document.getElementById('btnAddForo').disabled = false;
        return;
    }

    // Asegúrate de que se ha seleccionado un archivo
    if (anexoInput.files.length > 0) {
        formData.append('ArchivoPDF', anexoInput.files[0]);
    }

    formData.append('CodUsuario', document.getElementById('CodUsuario').value)
    formData.append('IdOfertaAcademica', document.getElementById('IdOfertaAcademica').value)
    formData.append('Titulo', document.getElementById('Titulo').value)
    formData.append('Descripcion', document.getElementById('Descripcion').value)
    formData.append('FechaCierre', document.getElementById('FechaCierre').value)
   // formData.append('Adjunto', document.getElementById('Adjunto').value)
    formData.append('Estado', document.getElementById('Estado').value)
    axios({
        method: "post",
        url: "/Vitrina/AddEditOferta",
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



