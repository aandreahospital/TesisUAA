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
            let codPersona = fila.CodPersona ? fila.CodPersona.trim() : "";
            tr.setAttribute("data-codpersona", codPersona);
                tr.innerHTML = `
                <td>${fila.CodPersona}</td>
                <td>${fila.Nombre}</td>
                <td>${fila.Sexo}</td>
                <td>${fila.FechaNacimiento}</td>
                <td>${fila.EstadoCivil}</td>
                <td>${fila.Email}</td>
                <td>${fila.Clave}</td>
                <td>${fila.CodGrupo}</td>
                `;
            tabla.appendChild(tr);
        });

        document.getElementById("btnProcesar").disabled = false;
        localStorage.setItem("datosExcel", JSON.stringify(datos)); // Guardar temporalmente
    };
});


document.getElementById("btnProcesar").addEventListener("click", function () {
    let datos = localStorage.getItem("datosExcel");
    let idCarrera = document.getElementById("Idcarrera").value;

    if (!datos) {
        swal({
            icon: 'warning',
            title: 'Atención',
            text: 'No hay datos cargados.',
        });
        return;
    }

    let personas = JSON.parse(datos);

    fetch("/AutoCarga/ProcesarDatos", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            personas: personas,
            carrera: parseFloat(idCarrera)
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.errores && data.errores.length > 0) {
                // Limpiar marcas anteriores
                document.querySelectorAll("#tablaAlumnos tbody tr").forEach(row => {
                    row.classList.remove("fila-error");
                    row.querySelectorAll("td").forEach(td => {
                        td.title = "";
                    });
                });

                // Marcar filas con error
                data.errores.forEach(error => {
                    //let row = document.querySelector(`#tablaAlumnos tbody tr[data-codpersona="${error.codPersona}"]`);
                   // let codPersona = error.codPersona ? error.codPersona.trim() : "";
                    let codPersona = (error.codPersona !== undefined && error.codPersona !== null)
                        ? error.codPersona.trim()
                        : "";

                    // Buscar la fila: si codPersona está vacío, buscar por filas con data-codpersona=""
                    let selector = codPersona === ""
                        ? '#tablaAlumnos tbody tr[data-codpersona=""]'
                        : `#tablaAlumnos tbody tr[data-codpersona="${codPersona}"]`;
                    let row = document.querySelector(selector);
                    if (row) {
                        row.classList.add("fila-error");
                        row.querySelectorAll("td").forEach(td => {
                            td.title = error.error; // mostrar el mensaje como tooltip
                        });
                    }
                   
                });

                swal({
                    icon: 'warning',
                    title: 'Carga parcial',
                    text: 'Algunas filas no se procesaron. Revisa los errores en la tabla.',
                });
            } else {
                swal({
                    icon: 'success',
                    title: 'Listo',
                    text: 'La operación se realizó correctamente.'
                }).then(() => {
                    localStorage.removeItem("datosExcel");
                    document.querySelector("#tablaAlumnos tbody").innerHTML = "";
                    document.getElementById("btnProcesar").disabled = true;
                });
            }
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
