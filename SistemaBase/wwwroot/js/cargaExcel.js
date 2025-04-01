document.getElementById("btnCargar").addEventListener("click", function () {
    let archivo = document.getElementById("archivoExcel").files[0];
    if (!archivo) {
        //alert("Seleccione un archivo Excel primero.");
        swal({
            icon: 'warning',
            title: 'Atención',
            text: 'Seleccione un archivo Excel primero.',
        });
        return;
    }

    let reader = new FileReader();
    reader.readAsBinaryString(archivo);
    reader.onload = function (e) {
        let data = e.target.result;
        let workbook = XLSX.read(data, { type: "binary" });
        let hoja = workbook.SheetNames[0]; // Tomar la primera hoja
        let datos = XLSX.utils.sheet_to_json(workbook.Sheets[hoja], { raw: false });

        let tabla = document.querySelector("#tablaAlumnos tbody");
        tabla.innerHTML = ""; // Limpiar tabla antes de cargar nuevos datos
        datos.forEach((fila) => {

            // Convertir FechaNacimiento a formato YYYY-MM-DD
            if (fila.FechaNacimiento) {
                let fecha = new Date(fila.FechaNacimiento);
                fila.FechaNacimiento = fecha.toISOString().split("T")[0]; // Formato 'YYYY-MM-DD'
            }

            let tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${fila.CodPersona}</td>
                <td>${fila.Nombre}</td>
                <td>${fila.Sexo}</td>
                <td>${fila.FechaNacimiento}</td>
                <td>${fila.EstadoCivil}</td>
                <td>${fila.Email}</td>
                <td>${fila.Clave}</td>
                <td>${fila.CodGrupo}</td>
                <td>${fila.Carrera}</td>
            `;
            tabla.appendChild(tr);
        });

        document.getElementById("btnProcesar").disabled = false;
        localStorage.setItem("datosExcel", JSON.stringify(datos)); // Guardar temporalmente
    };
});

document.getElementById("btnProcesar").addEventListener("click", function () {
    let datos = localStorage.getItem("datosExcel");
    if (!datos) {
        swal({
            icon: 'warning',
            title: 'Atención',
            text: 'No hay datos cargados.',
        });
        return;
    }

    fetch("/AutoCarga/ProcesarDatos", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: datos
    })
        .then(response => response.json())
        .then(data => {
            swal({
                icon: 'success',
                title: 'Listo',
                text: 'La operación se realizó correctamente.'
            }).then(() => {
                localStorage.removeItem("datosExcel");
                document.querySelector("#tablaAlumnos tbody").innerHTML = "";
                document.getElementById("btnProcesar").disabled = true;
            });
        })
        .catch(error => {
            swal({
                icon: 'error',
                title: 'Error',
                text: 'Ocurrió un error al procesar los datos.',
            });
            console.error("Error:", error);
        });
});
