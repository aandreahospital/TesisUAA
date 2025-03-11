
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

const AbrirEditComen = (id) => {
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


const EditComentario = (e) => {
    e.preventDefault();// Evita la propagación del evento
    document.getElementById('btnEditComen').disabled = true;
    // Bloquear el formulario
    var datosFormulario = document.getElementById('FormModalCreate');
    const formData = new FormData();
    formData.append('IdComentario', document.getElementById('IdComentario').value)
    formData.append('CodUsuario', document.getElementById('CodUsuario').value)
    formData.append('Descripcion', document.getElementById('Descripcion').value)
    axios({
        method: "post",
        url: "/ComentarioForo/EditComentario",
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
//function AbrirAdjunto(IdForoDebate) {
//    let url = '@Url.Action("AbrirAdjunto", "ComentarioForo")?IdForoDebate=' + IdForoDebate;

//    fetch(url, { method: "HEAD" })  // Solo verifica si el archivo está disponible
//        .then(response => {
//            if (response.ok) {
//                // Si el archivo existe, abrir en una nueva pestaña
//                let newTab = window.open();
//                newTab.location.href = url;
//            } else {
//                // Si no se puede abrir, forzar la descarga
//                let downloadLink = document.createElement("a");
//                downloadLink.href = url;
//                downloadLink.download = "archivo_descargado"; // Nombre genérico
//                document.body.appendChild(downloadLink);
//                downloadLink.click();
//                document.body.removeChild(downloadLink);
//            }
//        })
//        .catch(error => {
//            console.error("Error al verificar el archivo:", error);
//        });
//}

//function AbrirAdjunto(IdForoDebate) {
//    let url = '@Url.Action("AbrirAdjunto", "ComentarioForo")?IdForoDebate=' + IdForoDebate;

//    fetch(url, { method: "HEAD" })  // Primero obtenemos los headers sin descargar el archivo
//        .then(response => {
//            let contentType = response.headers.get("Content-Type");

//            // Si el archivo es PDF o imagen, abrir en una nueva pestaña
//            if (contentType.includes("pdf") || contentType.includes("image")) {
//                window.open(url, '_blank');
//            } else {
//                // Descargar el archivo sin cambiar la página
//                fetch(url)
//                    .then(res => res.blob())
//                    .then(blob => {
//                        let link = document.createElement("a");
//                        link.href = window.URL.createObjectURL(blob);

//                        // Intentar obtener el nombre del archivo desde Content-Disposition
//                        let contentDisposition = response.headers.get("Content-Disposition");
//                        let fileName = "archivo_descargado";
//                        if (contentDisposition) {
//                            let match = contentDisposition.match(/filename="?(.+)"?/);
//                            if (match && match[1]) {
//                                fileName = match[1];
//                            }
//                        }

//                        link.download = fileName;
//                        document.body.appendChild(link);
//                        link.click();
//                        document.body.removeChild(link);
//                    })
//                    .catch(error => console.error("Error al descargar el archivo:", error));
//            }
//        })
//        .catch(error => console.error("Error al obtener la información del archivo:", error));
//}