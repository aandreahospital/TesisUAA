
const abrirComentario = (e) => {
    e.preventDefault();
    axios({
        baseURL: "/ComentarioForo/AbrirAddComentario",
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


const AddComentario = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnAddComen').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();
    formData.append('IdForoDebate', document.getElementById('IdForoDebate').value)
    formData.append('CodUsuario', document.getElementById('CodUsuario').value)
    formData.append('Descripcion', document.getElementById('Descripcion').value)
    axios({
        method: "post",
        url: "/ComentarioForo/AddComentario",
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
                    //console.log({ 'reload ': response });
                    location.reload();
                    return;
                });
            }

        })
        .catch(function (response) {

            desbloquearFormulario(datosFormulario);
            document.getElementById('btnAddComen').disabled = false
            loader.style.display = 'none';
        })

}

const abrirEditForo = (id) => {
    const params = {
        id: id,
    }
    axios({
        baseURL: "/ComentarioForo/AbrirEditComentario",
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


const DeleteComentario = (id) => {
    const params = { id: id };

    axios({
        baseURL: "/ComentarioForo/DeleteComentario",
        method: 'get',
        params: params,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
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
                    //console.log({ 'reload ': response });
                    location.reload();
                    return;
                });
            }

        })
};