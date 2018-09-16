var idEstudiante = 0;
class Estudiantes{
    constructor() {

    }
    //...data (resto de parametros) representa todos los parametros que le pasemos
    guardarEstudiante(id, fucion,...data) {
        var action = data[0];
        var response = new Array({
            apellidos: data[3], matricula: data[1], direccion: data[8], cedula: data[5], email: data[6],
            estado: data[9], fechaNacimiento: data[4], nombres: data[2], id: id, telefono: data[7]
        });
        if (data[1] == "") {
            document.getElementById("Matricula").focus()
        } else {
            if (data[2] == "") {
                document.getElementById("Nombre").focus()
            } else {
                if (data[3] == "") {
                    document.getElementById("Apellido").focus()
                } else {
                    if (data[4] == "") {
                        document.getElementById("FechaNacimiento").focus()
                    } else {
                        if (data[5] == "") {
                            document.getElementById("Cedula").focus()
                        } else {
                            if (data[6] == "") {
                                document.getElementById("Email").focus()
                            } else {
                                if (data[7] == "") {
                                    document.getElementById("Telefono").focus()
                                } else {
                                    if (data[8] == "") {
                                        document.getElementById("Direccion").focus()
                                    } else {
                                        $.post(
                                            action,
                                            {
                                                response, funcion
                                            },
                                            (response) => {
                                                if ("1" == response[0].code) {
                                                    this.restablecer();
                                                } else {
                                                    document.getElementById("mensaje").innerHTML = "No se puede guardar el estudiante";
                                                }
                                            }
                                        );
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    filtrarEstudiantes(numPag, valor, order, action) {
        valor = (valor == "") ? "null" : valor;
        $.post(
            action,
            { valor, numPag, order },
            (response) => {

                $('#resultSearch').html(response[0][0]);
                $('#paginado').html(response[0][1]);
            });
    }

    getEstudiante(id, funcion, action) {
        $.post(
            action,
            { id },
            (response) => {
                console.log(response);
                if (funcion == 1) {
                    idEstudiante = response[0].id;
                    document.getElementById("Matricula").value = response[0].matricula;
                    document.getElementById("Nombre").value = response[0].nombres;
                    document.getElementById("Apellido").value = response[0].apellidos;
                    document.getElementById("FechaNacimiento").value = response[0].fechaNacimiento;
                    document.getElementById("Cedula").value = response[0].cedula;
                    document.getElementById("Email").value = response[0].email;
                    document.getElementById("Telefono").value = response[0].telefono;
                    document.getElementById("Direccion").value = response[0].direccion;
                    document.getElementById("Estado").checked = response[0].estado;
                }
               var action = 'Estudiantes/guardarEstudiantes';
               this.editarEstudiante(response, funcion, action);
            });
    }

    editarEstudiante(response, funcion, action) {

        $.post(
            action,
            { response, funcion },
            (response) => {
                if (funcion == 0) {
                    this.restablecer();
                }
                console.log(response);
            });

    }


    eliminarEstudiante(id, action) {
        $.post(
            action,
            { id },
            (response) => {
                console.log(response);
                this.restablecer();
            });
    }

    restablecer() {
        document.getElementById("Matricula").value = "";
        document.getElementById("Nombre").value = "";
        document.getElementById("Apellido").value = "";
        document.getElementById("FechaNacimiento").value = "";
        document.getElementById("Cedula").value = "";
        document.getElementById("Email").value = "";
        document.getElementById("Telefono").value = "";
        document.getElementById("Direccion").value = "";
        document.getElementById("Estado").checked = false;
        filtrarEstudiantes(1, "nombre");
        $("#modalNE").modal('hide');
        $("#modalEE").modal('hide');
    }
}