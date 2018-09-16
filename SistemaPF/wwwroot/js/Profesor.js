var idProfesor = 0;

//Especialidad profesor
class Profesores {
    constructor() {

    }

    guardarProfesor(id, funcion,...data) {
        var action = data[0];
        var response = new Array({
            id: id, especialidad: data[1], nombres: data[2], apellidos: data[3], fechaNacimiento: data[4],
            cedula: data[5], email: data[6], telefono: data[7], direccion: data[8], estado: data[9]
        });
        if (data[1] == "") {
            document.getElementById("Especialidad").focus()
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
                                                    document.getElementById("mensaje").innerHTML = "No se puede guardar el profesor";
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

    filtrarProfesores(numPag, valor, order, action) {
        valor = (valor == "") ? "null" : valor;
        $.post(
            action,
            { valor, numPag, order },
            (response) => {

                $('#resultSearch').html(response[0][0]);
                $('#paginado').html(response[0][1]);
            });
    }

    getProfesor(id, funcion, action) {
        $.post(
            action,
            { id },
            (response) => {
                console.log(response);
                if (funcion == 1) {
                    idProfesor = response[0].id;
                    document.getElementById("Especialidad").value = response[0].especialidad;
                    document.getElementById("Nombre").value = response[0].nombres;
                    document.getElementById("Apellido").value = response[0].apellidos;
                    document.getElementById("FechaNacimiento").value = response[0].fechaNacimiento;
                    document.getElementById("Cedula").value = response[0].cedula;
                    document.getElementById("Email").value = response[0].email;
                    document.getElementById("Telefono").value = response[0].telefono;
                    document.getElementById("Direccion").value = response[0].direccion;
                    document.getElementById("Estado").checked = response[0].estado;
                }
                var action = 'Profesors/guardarProfesor';
                this.editarProfesor(response, funcion, action);
            });
    }

     editarProfesor(response, funcion, action) {
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

    eliminarProfesor(id, action) {
        $.post(
            action,
            { id },
            (response) => {
                console.log(response);
                this.restablecer();
            });
    }

    restablecer() {
        document.getElementById("Especialidad").value = "";
        document.getElementById("Nombre").value = "";
        document.getElementById("Apellido").value = "";
        document.getElementById("FechaNacimiento").value = "";
        document.getElementById("Cedula").value = "";
        document.getElementById("Email").value = "";
        document.getElementById("Telefono").value = "";
        document.getElementById("Direccion").value = "";
        document.getElementById("Estado").checked = false;
        filtrarProfesores(1, "nombre");
        $("#modalNP").modal('hide');
        $("#modalEP").modal('hide');
    }


    }