// Write your JavaScript code.

$('#modalEdit').on('shown.bs.modal', function () {
  $('#myInput').focus()
})

$('#modalPF').on('shown.bs.modal', function () {
    $('#Nombre').focus()
})

function getUsuario(id, action) {
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            mostrarUsuario(response);
        }
    });
}

var items;
var j = 0;

//propiedades usuario
var id;
var userName;
var email;
var phoneNumber;
var role;
var selectRole;

var accessFiledCount;
var concurrencyStamp;
var emailConfirmed;
var lockoutEnabled;
var lockoutEnd;
var normalizedUserName;
var normalizedEmail;
var passwordHash;
var phoneNumberConfirmed;
var securityStamp;
var twoFactorEnabled;

function mostrarUsuario(response) {
    items = response;
    j = 0;
    for (var i = 0; i < 3; i++) {
        var x = document.getElementById('Select');
        x.remove(i);
    }

    $.each(items, function (index, val) {
        $('Input[name=Id]').val(val.id);
        $('Input[name=UserName]').val(val.userName);
        $('Input[name=Email]').val(val.email);
        $('Input[name=PhoneNumber]').val(val.phoneNumber);
        document.getElementById('Select').options[0] = new Option(val.role, val.roleId);

        //Mostrar detalles
        $("#dEmail").text(val.email);
        $("#dUserName").text(val.userName);
        $("#dPhoneNumber").text(val.phoneNumber);
        $("#dRole").text(val.role);

        //Datos Usuarios a eliminar
        $("#EUsuario").text(val.email);
        $('input[name=EIdUsuario]').val(val.id);
    });
}

function getRoles(action) {
    $.ajax({
        type: "POST",
        url: action,
        data: {},
        success: function (response) {
            if (j == 0) {
                for (var i = 0; i < response.length; i++) {
                    document.getElementById('Select').options[i] = new Option(response[i].text, response[i].value);
                    document.getElementById('SelectNuevo').options[i] = new Option(response[i].text, response[i].value);
                }
                j = 1;
            }   
        }
    });
}

function editarUsuario(action) {
    //obtener datos formulario
    id = $('Input[name=Id]')[0].value;
    email = $('Input[name=Email]')[0].value;
    phoneNumber = $('Input[name=PhoneNumber]')[0].value;
    role = document.getElementById('Select');
    selectRole = role.options[role.selectedIndex].text;

    $.each(items, function (index, val) {   
        accessFiledCount = val.accessFiledCount;
        concurrencyStamp = val.concurrencyStamp;
        emailConfirmed = val.emailConfirmed;
        lockoutEnabled = val.lockoutEnabled;
        lockoutEnd = val.lockoutEnd;
        userName = val.userName;
        normalizedUserName = val.normalizedUserName;
        normalizedEmail = val.normalizedEmail;
        passwordHash = val.passwordHash;
        phoneNumberConfirmed = val.emailConfirmed;
        securityStamp = val.securityStamp;
        twoFactorEnabled = val.twoFactorEnabled;
    });


    $.ajax({
        type: "POST",
        url: action,
        data: {
            id, userName, email, phoneNumber, accessFiledCount,
            concurrencyStamp, emailConfirmed, lockoutEnabled, lockoutEnd,
            normalizedUserName, normalizedEmail, passwordHash, phoneNumberConfirmed,
            securityStamp, twoFactorEnabled, selectRole
        },
        success: function (response) {
            if (response === "Save") {
                window.location.href = "Usuarios";
            } else {
                alert("No se pueden editar los datos del usuario");
            }
        }
        });
}

function ocultarDetalleusuario() {
    $("#modalDetalle").modal("hide");
}

function eliminarUsuario(action) {
    var id = $('input[name=EIdUsuario]')[0].value;
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            if (response === "Delete") {
                window.location.href = "Usuarios";
            } else {
                alert("No se puede eliminar el registro");
            }
        }
    });
}

function crearUsuario(action) {
    //obtener datos del modal registro

    email = $('input[name=EmailNuevo]')[0].value;
    phoneNumber = $('input[name=PhoneNumberNuevo]')[0].value;
    passwordHash = $('input[name=PasswordHashNuevo]')[0].value;
    role = document.getElementById('SelectNuevo');
    selectRole = role.options[role.selectedIndex].text;

    if (email == "") {
        $("#EmailNuevo").focus;
        alert("Ingrese el Email del Usuario");
    } else {
        if (passwordHash == "") {
            $("#PasswordHashNuevo").focus;
            alert("Ingrese la Contraseña del Usuario");
        } else {
            $.ajax({
                type: "POST",
                url: action,
                data: { email, phoneNumber, passwordHash, selectRole },
                success: function (response) {
                    if (response == "Save") {
                        window.location.href = "Usuarios";
                    } else {
                        $('#MensajeNuevo').html("No se puede guardar el usuario. </br>Seleccione un rol. </br> Ingrese un email correcto. </br> El password debe tener de 6-100 caracteres, al menos un caracter especial, una letra mayuscula y un numero");
                    }
                }
            });
        }
    }

}

//funcion cada vez que actualizamos nuestra pagina
//1 = numero de pagina; nombre= por el dato que vamos a ordenar
$().ready(() => {
   var URLactual = window.location;
    document.getElementById("filtrar").focus();
    switch (URLactual.pathname) {
       case "/Categorias":
            filtrarDatos(1, "nombre");
           break;
       case "/Cursos":
            getCategorias(0, 0);

            filtrarCurso(1, "nombre");
            break;
        case "/Estudiantes":
            filtrarEstudiantes(1, "nombre");
            break;
        case "/Profesors":
            filtrarProfesores(1, "nombre");
            break;
        case "/Inscripciones":
            filtrarEstudiantesInscripcion();
            filtrarCursoInscripcion();
            mostrarCursos();
            break;
        case "/MisCursos":
            filtrarMisCurso(1);
            break;
    }
});


//mostrar modal Nuevo Curso
$('#modalNC').on('shown.bs.modal', () => {
    $('#Nombre').focus();
});
$('#modalNE').on('shown.bs.modal', () => {
    $('#Matricula').focus();
});
$('#modalNP').on('shown.bs.modal', () => {
    $('#Asignacion').focus();
});


//Imprimir inscripciones
var ImprimirInscripcion = (id) => {
    var imprimirContenido = document.getElementById(id).innerHTML;
    var cuerpoReportes = document.body.innerHTML;
    document.body.innerHTML = imprimirContenido;
    window.print();
    document.body.innerHTML = cuerpoReportes;
}

var idCategoria, funcion = 0, idCurso;
var idEstudiante = 0;
var idProfesor = 0;
var asignacionID = 0;

/*Codigo Categoria*/
var agregarCategoria = () => {
    var nombre = document.getElementById("Nombre").value;
    var descripcion = document.getElementById("Descripcion").value;
    var estados = document.getElementById('Estado');
    var estado = estados.options[estados.selectedIndex].value;

    if (funcion == 0) {
        var action = 'Categorias/guardarCategoria';
    } else {
        var action = 'Categorias/editarCategoria';
    }

    var categoria = new Categoria(nombre, descripcion, estado, action);
    categoria.agregarCategoria(idCategoria, funcion);
    funcion = 0;
}

var filtrarDatos = (numPag, order) => {
    var valor = document.getElementById("filtrar").value;
    var action = 'Categorias/filtrarDatos';
    //Espacios en blanco porque solo va a filtrar esos dos datos
    var categoria = new Categoria(valor, "", "", action);
    categoria.filtrarDatos(numPag, order);
}

var editarEstado = (id, fun) => {
    idCategoria = id;
    funcion = fun;
    var action = 'Categorias/getCategoria';
    var categoria = new Categoria("", "", "", action);
    categoria.getCategoria(id, funcion);
}

var editarCategoria = () => {
    var action = 'Categorias/editarCategoria';
    var categoria = new Categoria("", "", "", action);
    categoria.editarCategoria(idCategoria, funcion);
}


/*codigo Cursos*/

var getCategorias = (id, fun) => {
    var action = 'Cursos/getCategorias';
    var cursos = new Cursos("", "", "", "", "", "", action);
    cursos.getCategorias(id, fun);
}

var agregarCurso = () => {
    if (funcion == 0) {
        var action = 'Cursos/agregarCurso';
    } else {
        var action = 'Cursos/editarCurso';
    }
    var nombre = document.getElementById("Nombre").value;
    var descripcion = document.getElementById("Descripcion").value;
    var creditos = document.getElementById("Creditos").value;
    var costo = document.getElementById("Costo").value;
    var estado = document.getElementById("Estado").checked;
    var categorias = document.getElementById('CategoriaCursos');
    var categoria = categorias.options[categorias.selectedIndex].value;
    var cursos = new Cursos(nombre, descripcion, creditos, costo, estado, categoria, action);
    cursos.agregarCurso(idCurso, funcion);
    funcion = 0;
}

var filtrarCurso = (numPag, order) => {
    var valor = document.getElementById("filtrar").value;
    var action = 'Cursos/filtrarCurso';
    var cursos = new Cursos(valor, "", "", "", "", "", action);
    cursos.filtrarCurso(numPag, order);
}

var editarEstadoCurso = (id, fun) => {
    funcion = fun;
    idCurso = id;
    var action = 'Cursos/getCursos';
    var cursos = new Cursos("", "", "", "", "", "", action);
    cursos.getCursos(id, fun);
}

var editarEstadoCurso1 = () => {
    var action = 'Cursos/editarCurso';
    var cursos = new Cursos("", "", "", "", "", "", action);
    cursos.editarEstadoCurso(idCurso, funcion);
}

var restablecer = () => {
    var cursos = new Cursos("", "", "", "", "", "", "");
    cursos.restablecer();
}

var getProfesorCurso = (asignacion, curso, profesor, fun) => {
    idCurso = curso;
    asignacionID = asignacion;
    var action = 'Cursos/getCursos';
    var cursos = new Cursos("", "", "", "", "", "", action);
    cursos.getCursos(curso, fun);
    var action = 'Cursos/getProfesores';
    cursos.getProfesores(profesor, fun, action);

}

var profesorCurso = () => {
    let action = 'Cursos/profesorCurso';
    let profesores = document.getElementById('profesorCursos');
    let profesor = profesores.options[profesores.selectedIndex].value;
    let fecha = document.getElementById("Fecha").value;
    var cursos = new Cursos("", "", "", "", "", "", "");
    cursos.profesorCursos(asignacionID, idCurso, profesor, fecha, action);
    asignacionID = 0;
    idCurso = 0;
}

/*Codigo Estudiantes*/
var estudiante = new Estudiantes();
var guardarEstudiante = () => {

        var action = 'Estudiantes/guardarEstudiantes';

    var matricula = document.getElementById("Matricula").value;
    var nombre = document.getElementById("Nombre").value;
    var apellido = document.getElementById("Apellido").value;
    var fecha = document.getElementById("FechaNacimiento").value;
    var cedula = document.getElementById("Cedula").value;
    var email = document.getElementById("Email").value;
    var telefono = document.getElementById("Telefono").value;
    var direccion = document.getElementById("Direccion").value;
    var estado = document.getElementById("Estado").checked;
    estudiante.guardarEstudiante(idEstudiante, funcion, action, matricula, nombre, apellido, fecha, cedula, email, telefono, direccion, estado);
    idEstudiante = 0;
}

var filtrarEstudiantes = (numPag, order) => {
    var valor = document.getElementById("filtrar").value;
    var action = 'Estudiantes/filtrarEstudiantes';
    estudiante.filtrarEstudiantes(numPag, valor, order, action);
}

var editarEstudiante = (id, fun) => {
    idEstudiante = id;
    funcion = fun;
    var action = 'Estudiantes/getEstudiantes';
    estudiante.getEstudiante(id, fun, action);
}

var eliminarEstudiante = (id) =>{
    idEstudiante = id;
}

var eliminarEstudiantes = () => {
    action = 'Estudiantes/eliminarEstudiante';
    estudiante.eliminarEstudiante(idEstudiante, action);
    idEstudiante = 0;
}

/*Codigo Profesor*/
var profesor = new Profesores();
var guardarProfesor = () => {

    var action = 'Profesors/guardarProfesor';

    var especialidad = document.getElementById("Especialidad").value;
    var nombre = document.getElementById("Nombre").value;
    var apellido = document.getElementById("Apellido").value;
    var fecha = document.getElementById("FechaNacimiento").value;
    var cedula = document.getElementById("Cedula").value;
    var email = document.getElementById("Email").value;
    var telefono = document.getElementById("Telefono").value;
    var direccion = document.getElementById("Direccion").value;
    var estado = document.getElementById("Estado").checked;
    profesor.guardarProfesor(idProfesor, funcion, action, especialidad, nombre, apellido, fecha, cedula, email, telefono, direccion, estado);
    idProfesor = 0;
}

var filtrarProfesores = (numPag, order) => {
    var valor = document.getElementById("filtrar").value;
    var action = 'Profesors/filtrarProfesores';
    profesor.filtrarProfesores(numPag, valor, order, action);
}

var editarProfesor = (id, fun) => {
    idEstudiante = id;
    funcion = fun;
    var action = 'Profesors/getProfesores';
    profesor.getProfesor(id, fun, action);
}

var eliminarProfesor = (id) => {
    idProfesor = id;
}

var eliminarProfesores = () => {
    action = 'Profesors/eliminarProfesor';
    profesor.eliminarProfesor(idProfesor, action);
    idProfesor = 0;
}

/*Codigo Inscripciones*/
var inscripciones = new Inscripciones();

var filtrarEstudiantesInscripcion = () => {
    var action = 'Inscripciones/filtrarEstudiantes';
    var valor = document.getElementById("filtrar").value;
    inscripciones.filtrarDatosInscripcion(valor, action, 1);
}

var getEstudiante = () => {
    let count = 0, id;
    let chkbox = document.getElementsByName('chkboxEstudiante[]');
    for (var i = 0; i < chkbox.length; i++) {
        if (chkbox[i].checked) {
            id = chkbox[i].value;
            count++;
        }
    }
    if (count > 1) {
        document.getElementById("mensajeEstudiante").innerHTML = "No puede seleccionar mas de un estudiante";
    } else {
        var action = 'Inscripciones/getEstudiante';
        inscripciones.getDatos(id, action, 1);
    }
}

var filtrarCursoInscripcion = () => {
    var action = 'Inscripciones/filtrarCursos';
    var valor = document.getElementById('filtrarCurso').value;
    inscripciones.filtrarDatosInscripcion(valor, action, 2);
}

var getCurso = () => {
    let count = 0, id;
    let chkbox = document.getElementsByName('chkboxCurso[]');
    for (var i = 0; i < chkbox.length; i++) {
        if (chkbox[i].checked) {
            id = chkbox[i].value;
            count++;
        }
    }
    if (count > 1) {
        document.getElementById("mensajeCurso").innerHTML = "No puede seleccionar mas de un curso";
    } else {
        var action = 'Inscripciones/getCurso';
        inscripciones.getDatos(id, action, 2);
    }
}

var addCursos = () => {
    var estudiante = document.getElementById("Estudiante").value;
    var curso = document.getElementById("InscripcionCurso").value;
    var grado = document.getElementById("grado").value;
    var costo = document.getElementById("CostoCurso").value;

    inscripciones.addCursos(estudiante, curso, grado, costo);
}

var restablecer = () => {
    inscripciones.eliminarDatos();
}

var mostrarCursos = () => {
    inscripciones.mostrarCursos();
}

var eliminarCurso = (id) => {
    inscripciones.eliminarCurso(id);
}

var guardarCursos = () => {
    inscripciones.guardarCursos();
}

/*Codigo Mis Cursos*/
var misCursos = new MisCursos();
var filtrarMisCurso = (pagina) => {
    var valor = document.getElementById("filtrar").value;
    misCursos.filtrarMisCurso(pagina, valor);
}

var getMisCursos = (curso, id) => {
    misCursos.getMisCurso(curso, id);
}

var actualizarMisCursos = () => {
    misCursos.actualizarMisCursos();
}

var reportesCursos = () => {
    funcion = 1;
    misCursos.reportesCursos(1);
}