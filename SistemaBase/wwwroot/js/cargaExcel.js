//document.getElementById("btnCargar").addEventListener("click", function () {
//    let archivo = document.getElementById("archivoExcel").files[0];
//    if (!archivo) {
//        //alert("Seleccione un archivo Excel primero.");
//        swal({
//            icon: 'warning',
//            title: 'Atención',
//            text: 'Seleccione un archivo Excel primero.',
//        });
//        return;
//    }

//    let reader = new FileReader();
//    reader.readAsBinaryString(archivo);
//    reader.onload = function (e) {
//        let data = e.target.result;
//        let workbook = XLSX.read(data, { type: "binary" });
//        let hoja = workbook.SheetNames[0]; // Tomar la primera hoja
//        let datos = XLSX.utils.sheet_to_json(workbook.Sheets[hoja], { raw: false });

      
//        let tabla = document.querySelector("#tablaAlumnos tbody");
//        tabla.innerHTML = ""; // Limpiar tabla antes de cargar nuevos datos
//        let errores = [];

//        datos.forEach((fila) => {

//            // Convertir FechaNacimiento a formato YYYY-MM-DD
//            if (fila.FechaNacimiento) {
//                let fecha = new Date(fila.FechaNacimiento);
//                fila.FechaNacimiento = fecha.toISOString().split("T")[0]; // Formato 'YYYY-MM-DD'
//            }

//            let tr = document.createElement("tr");
//            let codPersona = fila.CodPersona ? fila.CodPersona.trim() : "";
//            tr.setAttribute("data-codpersona", codPersona);
//                tr.innerHTML = `
//                <td>${fila.CodPersona}</td>
//                <td>${fila.Nombre}</td>
//                <td>${fila.Sexo}</td>
//                <td>${fila.FechaNacimiento}</td>
//                <td>${fila.EstadoCivil}</td>
//                <td>${fila.Email}</td>
//                <td>${fila.Clave}</td>
//                <td>${fila.CodGrupo}</td>
//                `;
//            tabla.appendChild(tr);
//        });

//        document.getElementById("btnProcesar").disabled = false;
//        localStorage.setItem("datosExcel", JSON.stringify(datos)); // Guardar temporalmente
//    };
//});

document.getElementById("btnCargar").addEventListener("click", function () {
    let archivo = document.getElementById("archivoExcel").files[0];
    if (!archivo) {
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
        let hoja = workbook.SheetNames[0];
        let datos = XLSX.utils.sheet_to_json(workbook.Sheets[hoja], { raw: false });

        let tabla = document.querySelector("#tablaAlumnos tbody");
        tabla.innerHTML = "";
        let errores = [];

        datos.forEach((fila, index) => {
            let codPersona = fila.CodPersona ? fila.CodPersona.trim() : "";
            let nombre = fila.Nombre ? fila.Nombre.trim() : "";
            let codGrupo = fila.CodGrupo ? fila.CodGrupo.trim() : "";
            let email = fila.Email ? fila.Email.trim() : "";
            let fechaStr = fila.FechaNacimiento;
            let fechaValida = true;
            let codPersonaValida = true;
            let emailValido = true;
            let nombreValido = true;
            let codGrupoValido = true;
            let fechaFormateada = "";


            // Validar Fecha no vacía y con formato válido
            if (!fechaStr) {
                fechaValida = false;
            } else {
                let fecha = new Date(fechaStr);
                if (isNaN(fecha.getTime())) {
                    fechaValida = false;
                } else {
                    fechaFormateada = fecha.toISOString().split("T")[0];
                    fila.FechaNacimiento = fechaFormateada;
                }
            }

            //if (fechaStr) {
            //    let fecha = new Date(fechaStr);
            //    if (isNaN(fecha.getTime())) {
            //        fechaValida = false;
            //    } else {
            //        fechaFormateada = fecha.toISOString().split("T")[0];
            //    }
            //} else {
            //    fechaValida = false;
            //}

            //if (fechaValida) {
            //    fila.FechaNacimiento = fechaFormateada;
            //}

            if (!codPersona) codPersonaValida = false;
            if (!nombre) nombreValido = false;
            if (!codGrupo) codGrupoValido = false;

            // Validar Email no vacío y con formato correcto
            if (!email) {
                emailValido = false;
            } else {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(email)) {
                    emailValido = false;
                }
            }

            //const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            //if (!emailRegex.test(email)) {
            //    emailValido = false;
            //}

            let tr = document.createElement("tr");
            tr.setAttribute("data-codpersona", codPersona);

            if (!fechaValida || !codPersonaValida || !emailValido || !nombreValido || !codGrupoValido) {
                tr.classList.add("fila-error");
            }

            // Acumular mensajes de error
            if (!codPersonaValida) errores.push(`Fila ${index + 2}: CodPersona vacío`);
            if (!nombreValido) errores.push(`Fila ${index + 2}: Nombre vacío`);
            if (!codGrupoValido) errores.push(`Fila ${index + 2}: CodGrupo vacío`);
            if (!emailValido) errores.push(`Fila ${index + 2}: Email inválido o vacío (${email})`);
            if (!fechaValida) errores.push(`Fila ${index + 2}: Fecha de nacimiento inválida o vacía (${fechaStr})`);


            //// Marcar fila con error si la fecha es inválida
            //if (!fechaValida) {
            //    tr.classList.add("fila-error");
            //    errores.push(`Fila ${index + 2}: Fecha de nacimiento inválida (${fechaStr})`);
            //}

            tr.innerHTML = `
                <td>${fila.CodPersona}</td>
                <td>${fila.Nombre}</td>
                <td>${fila.Sexo}</td>
                <td>${fila.FechaNacimiento || ''}</td>
                <td>${fila.EstadoCivil}</td>
                <td>${fila.Email}</td>
                <td>${fila.Clave}</td>
                <td>${fila.CodGrupo}</td>
            `;
            tabla.appendChild(tr);
        });

        document.getElementById("btnProcesar").disabled = false;;
        localStorage.setItem("datosExcel", JSON.stringify(datos));

        if (errores.length > 0) {
            swal({
                icon: 'error',
                title: 'Errores encontrados',
                text: errores.join('\n'),
                buttons: true
            });
        }
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
    let todasLasPersonas = JSON.parse(datos);

    //  Filtrar solo las filas válidas (que no tengan la clase fila-error)
    let filasValidas = [];
    document.querySelectorAll("#tablaAlumnos tbody tr").forEach((tr, index) => {
        if (!tr.classList.contains("fila-error")) {
            filasValidas.push(todasLasPersonas[index]); // Usamos el mismo índice
        }
    });

    if (filasValidas.length === 0) {
        swal({
            icon: 'error',
            title: 'Sin datos válidos',
            text: 'No hay filas válidas para procesar.',
        });
        return;
    }


   // let personas = JSON.parse(datos);

    fetch("/AutoCarga/ProcesarDatos", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            personas: filasValidas,
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
