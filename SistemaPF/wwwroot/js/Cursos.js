var promesa = new Promise((resolve, reject) => {

});
class Cursos {
    constructor(nombre, descripcion, creditos, costo, estado, categoria, action) {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.creditos = creditos;
        this.costo = costo;
        this.estado = estado;
        this.categoria = categoria;
        this.action = action;
    }
    //metodo
    getCategorias(id, funcion) {
        var action = this.action;
        //crear las posiciones desde la posicion 1
        var count = 1;

        //devolver los datos que me envie mi controlador o servidor
        //llenar el select
        $.ajax({
            type: "POST",
            url: action,
            data: {},
            success: (response) => {
                //console.log(response);
                document.getElementById('CategoriaCursos').options[0] = new Option("Seleccione Curso", 0);
                if (0 < response.length) {
                    for (var i = 0; i < response.length; i++) {
                        if (funcion == 0) {
                            document.getElementById('CategoriaCursos').options[count] = new Option(response[i].nombre, response[i].categoriaID);
                            count++;
                        }
                        else {
                            if (response[i].categoriaID == id) {
                                document.getElementById('CategoriaCursos').options[0] = new Option(response[i].nombre, response[i].categoriaID);
                                document.getElementById('CategoriaCursos').selectedIndex = 0;
                                break;
                            }
                        }
                    }
                }
            }

        });
    }

    agregarCurso(id, funcion) {
        if (this.nombre == "") {
            document.getElementById("Nombre").focus();
        } else {
            if (this.descripcion == "") {
                document.getElementById("Descripcion").focus();
            } else {
                if (this.creditos == "") {
                    document.getElementById("Creditos").focus();
                } else {
                    if (this.costo == ""){
                        document.getElementById("Costo").focus();
                    } else {
                        if (this.categoria == "0") {
                            document.getElementById("mensaje").innerHTML = "Seleccione una categoria";
                        } else {
                            var nombre = this.nombre;
                            var descripcion = this.descripcion;
                            var creditos = this.creditos;
                            var costo = this.costo;
                            var estado = this.estado;
                            var categoria = this.categoria;
                            var action = this.action;
                            $.ajax({
                                type: "POST",
                                url: action,
                                data: { id, nombre, descripcion, creditos, costo, estado, categoria, funcion },
                                success: (response) => {
                                    //console.log(response);
                                    if ("Save" == response[0].code) {
                                        this.restablecer();
                                    } else {
                                        document.getElementById("mensaje").innerHTML = "No se ha podido guardar el curso";
                                    }
                                }
                            });
                        }
                    }
                }
            }
        }
    }

    filtrarCurso(numPag, order) {
        var valor = this.nombre;
        var action = this.action;   

        if (valor == "") {
            valor = "null";
        }
        $.ajax({
            type: "POST",
            url: action,
            data: { valor, numPag, order },
            success: (response) => {
                $("#resultSearch").html(response[0][0]);
                $("#paginado").html(response[0][1]);
            }
        });
    }

    //para obetener los datos para poder editar los cursos
    getCursos(id, funcion) {
        var action = this.action;
        $.ajax({
            type: "POST",
            url: action,
            data: { id },
            success: (response) => {
                console.log(response)
                if (funcion == 0) {
                    if (response[0].estado) {
                        document.getElementById("cuerpoCurso").innerHTML = "Esta apunto de desactivar el curso " + response[0].nombre;
                    } else {
                        document.getElementById("cuerpoCurso").innerHTML = "Esta apunto de activar el curso " + response[0].nombre;
                    }
                
                    promesa = Promise.resolve({
                        id: response[0].cursoID,
                        nombre: response[0].nombre,
                        descripcion: response[0].descripcion,
                        creditos: response[0].creditos,
                        costo: response[0].costo,
                        estado: response[0].estado,
                        categoria: response[0].categoriaID
                    });
                } else {
                    document.getElementById("Nombre").value = response[0].nombre;
                    document.getElementById("Descripcion").value = response[0].descripcion;
                    document.getElementById("Creditos").value = response[0].creditos;
                    document.getElementById("Costo").value = response[0].costo;
                    getCategorias(response[0].categoriaID, 1);
                    if (response[0].estado) {
                        document.getElementById("Estado").checked = true;
                    } else {
                        document.getElementById("Estado").checked = false;
                    }
                }
                if (funcion == 2 || funcion == 3) {
                    document.getElementById("cursoTitle").innerHTML = response[0].nombre;
                }
            }
        });
    }


    editarEstadoCurso(id, funcion) {
        var nombre, descripcion, creditos, costo, estado, categoria;
        var action = this.action;

        //obeter los datos de nuestra promesa
        promesa.then(data => {
            //id = data.id;
            nombre = data.nombre;
            descripcion = data.descripcion;
            creditos = data.creditos;
            costo = data.costo;
            estado = data.estado;
            categoria = data.categoria;

            $.ajax({
                type: "POST",
                url: action,
                data: { id, nombre, descripcion, estado, creditos, costo, categoria, funcion },
                success: (response) => {
                    if (response[0].code == "Save") {
                        this.restablecer();
                    } else {
                        document.getElementById("cuerpoCurso").innerHTML = response[0].descripcion;
                    }
                }

            });
        });
    }



    getProfesores(profesor, fun, action) {
        var count = 1;
        $.post(
            action,
            {},
            (response) => {
                document.getElementById('profesorCursos').options[0] = new Option("Seleccione un Profesor", 0);
                if (0 < response.length) {
                    for (var i = 0; i < response.length; i++) {
                        if (fun == 3) {
                            document.getElementById('profesorCursos').options[count] = new Option(response[i].nombres, response[i].id);
                            count++;
                        } else {
                            if (profesor == response[i].id) {
                                document.getElementById('profesorCursos').options[0] = new Option(response[i].nombres, response[i].id);
                                document.getElementById('profesorCursos').selectedIndex = 0;
                            } else {
                                document.getElementById('profesorCursos').options[count] = new Option(response[i].nombres, response[i].id);
                                count++;
                            }
                        }
                    }
                }
            });
    }

    profesorCursos(asignacionID, idCurso, profesorID, fecha, action) {
        var asignacion = new Array({
            asignacionID: asignacionID,
            cursoID: idCurso,
            profesorID: profesorID,
            fecha: fecha
        });
        $.post(
            action,
            { asignacion },
            (response) => {
                console.log(response);
                if (response[0].code == "Save") {
                    this.restablecer();
                } else {
                    document.getElementById("cursoTitle").innerHTML = response[0].description;
                }
            });
    }

    restablecer() {
        document.getElementById("Nombre").value = "";
        document.getElementById("Descripcion").value = "";
        document.getElementById("Creditos").value = "";
        document.getElementById("Costo").value = "";
        document.getElementById("Estado").checked = false;
        document.getElementById('CategoriaCursos').selectedIndex = 0;
        document.getElementById("mensaje").innerHTML = "";
        filtrarCurso(1, "nombre");
        $('#modalNC').modal('hide');
        $('#modalEstadoCurso').modal('hide');
        $('.bs-example-modal-sm').modal('hide');
    }
}