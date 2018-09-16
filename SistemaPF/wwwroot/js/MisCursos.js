
var inscripcionID = 0;

class MisCursos {
    constructor() {

    }

    filtrarMisCurso(numPag, valor) {

        var funcion = 1;
        if (valor == "")
            valor = "null";
        $.post(
            "MisCursos/filtrarMisCurso",
            { valor, numPag, funcion },
            (response) => {
                console.log(response);
                $("#resultMisCursos").html(response[0][0]);

            });
    }


    actualizarMisCursos() {

        let curso = document.getElementById("Curso").value;
        let estudiante = document.getElementById("Estudiante").value;
        let profesor = document.getElementById("Profesor").value;
        let grado = document.getElementById("Grado").value;
        let pago = document.getElementById("Pago").value;
        let fecha = document.getElementById("Fecha").value;

        if (curso == "") {
            document.getElementById("Curso").focus();
        } else {
            if (estudiante == "") {
                document.getElementById("Estudiate").focus();
            } else {
                if (profesor == "") {
                    document.getElementById("Profesor").focus();
                } else {
                    if (grado == "") {
                        document.getElementById("Grado").focus();
                    } else {
                        if (pago == "") {
                            document.getElementById("Pago").focus();
                        } else {
                            if (fecha == "") {
                                document.getElementById("Fecha").focus();
                            } else {
                                let listCursos = new Array({
                                    inscripcionID,
                                    curso,
                                    estudiante,
                                    profesor,
                                    grado,
                                    pago,
                                    fecha
                                });
                                //convertimos la coleccion de datos para poder enviarla al servidor
                                let data = JSON.stringify(listCursos);
                                $.post(
                                    'MisCursos/actualizarMisCursos',
                                    { data },
                                    (response) => {
                                        if (response[0].code == "Save") {
                                            this.restablecer();
                                        }
                                        console.log(response);
                                    });
                            }
                        }
                    }
                }
            }
        }
    }


    //llenado los campos del modal con el arreglo
    getMisCurso(curso, id) {
        document.getElementById("Curso").value = curso[0];
        document.getElementById("Estudiante").value = curso[1];
        document.getElementById("Profesor").value = curso[2];
        document.getElementById("Grado").value = curso[3];
        document.getElementById("Pago").value = curso[4];
        document.getElementById("Fecha").value = curso[5];
        inscripcionID = id;
    }

    reportesCursos() {
        var valor = "null";
        var order = "nombre";
        var funcion = 2;

        $.post(
            'MisCursos/reportesCursos',
            { valor, order, funcion },
            (response) => {
                console.log(response);
                document.getElementById("titleReportes").innerHTML = "Cursos Inscritos";
                $("#resultReportes").html(response[0][0]);
                $("#thead").html(response[1][0]);
            });
    }

    restablecer() {
        $('#modalMisCurso').modal('hide');
        filtrarMisCurso(1);
    }

}
