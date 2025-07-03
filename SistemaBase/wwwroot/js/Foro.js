


const modaldelete = (IdForoDebate) => {
    axios({
        url: `/ForoControl/Delete?IdForoDebate=${IdForoDebate}`, 
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
        url: "ForoControl/" + url,
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


const abrirForo = (e) => {
    e.preventDefault();
    axios({
        baseURL: "/ForoControl/AbrirForo",
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

const AddForo = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnAddForo').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();

    // Para el campo 'AnexoPDF'
    var anexoInput = $('#Adjunto')[0];
    //if (anexoInput.files.length === 0 || anexoInput.files[0].size === 0) {
    //    swal({
    //        icon: 'warning',
    //        title: 'Archivo requerido',
    //        text: 'Debe adjuntar un archivo válido.'
    //    });
    //   // document.getElementById('btnAddForo').disabled = false;
    //    return;
    //}
    // Asegúrate de que se ha seleccionado un archivo
    if (anexoInput.files.length > 0) {
        formData.append('ArchivoPDF', anexoInput.files[0]);
    }
    //formData.append('CodUsuario', document.getElementById('usuario').value)
    formData.append('Titulo', document.getElementById('Titulo').value)
    formData.append('Descripcion', document.getElementById('Descripcion').value)
   // formData.append('Adjunto', document.getElementById('Adjunto').value)
    axios({
        method: "post",
        url: "/ForoControl/AddForo",
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
            }else {
            swal({
                icon: 'error',
                title: 'Error',
                text: response.data.ErrorMessage || 'Ocurrió un error inesperado.'
            });
            document.getElementById('btnAddForo').disabled = false;
            }
        })
        .catch(function (response) {

            desbloquearFormulario(datosFormulario);
            document.getElementById('btnAddLaboral').disabled = false
            loader.style.display = 'none';
        })

}

const abrirEditForo = (id) => {
    const params = {
        id: id,
    }
    axios({
        baseURL: "/ForoControl/AbrirEditForo",
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


const AddEditForo = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnAddEditForo').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();

    // Para el campo 'AnexoPDF'
    var anexoInput = $('#Adjunto')[0];
    //if (anexoInput.files.length === 0 || anexoInput.files[0].size === 0) {
    //    swal({
    //        icon: 'warning',
    //        title: 'Archivo requerido',
    //        text: 'Debe adjuntar un archivo válido.'
    //    });
    //    ///document.getElementById('btnAddForo').disabled = false;
    //    return;
    //}
    // Asegúrate de que se ha seleccionado un archivo
    if (anexoInput.files.length > 0) {
        formData.append('ArchivoPDF', anexoInput.files[0]);
    }

    formData.append('CodUsuario', document.getElementById('CodUsuario').value)
    formData.append('IdForoDebate', document.getElementById('IdForoDebate').value)
    formData.append('Titulo', document.getElementById('Titulo').value)
    formData.append('Descripcion', document.getElementById('Descripcion').value)
    ///formData.append('Adjunto', document.getElementById('Adjunto').value)
    formData.append('Estado', document.getElementById('Estado').value)
    axios({
        method: "post",
        url: "/ForoControl/AddEditForo",
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


const verComentarios = (idForo) => {

    const params = {
        idForo: idForo,
    };
    window.location.replace(`/ComentarioForo/VerComentarios?idForo=${encodeURIComponent(idForo)}`);

}


