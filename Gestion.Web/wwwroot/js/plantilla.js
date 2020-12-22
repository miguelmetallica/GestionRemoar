/*===============================
		MENU ANIMACION            
===============================*/
  	
$('.sidebar-menu').tree();


/*===============================
		Modal Imputacion de productos
===============================*/
function cargarDatosImputa(Id, ComprobanteId, ProductoId, ProductoCodigo, ProductoNombre, Precio, Cantidad, Imputado, ImputadoPorcentaje) {
    var mId = document.getElementById('mdlId');
    var mComprobanteId = document.getElementById('mdlComprobanteId');
    var mProductoId = document.getElementById('mdlProductoId');
    var mProductoCodigo = document.getElementById('mdlProductoCodigo');
    var mProductoNombre = document.getElementById('mdlProductoNombre');
    var mPrecio = document.getElementById('mdlPrecio');    
    var mImputado = document.getElementById('mdlImputado');
    var mImputadoPorcentaje = document.getElementById('mdlImputadoPorcentaje');
    var mImporte = document.getElementById('mdlImporte');
    var mpagado = document.getElementById('pagado');
    var mdlDisponible = document.getElementById('mdlDisponible');
    
    var importe = 0;    

    $("#mId").text('');
    $("#mComprobanteId").text('');
    $("#mProductoId").text('');
    $("#mProductoCodigo").text('');
    $("#mProductoNombre").text('');
    $("#mPrecio").text('');
    $("#mImputado").text('');
    $("#mImputadoPorcentaje").text('');
    $("#mImporte").text('');

    mId.value = Id;
    mComprobanteId.value = ComprobanteId;
    mProductoId.value = ProductoId;
    mProductoCodigo.value = ProductoCodigo;
    mProductoNombre.value = ProductoNombre;
    mPrecio.value = Precio.toFixed(2);;    
    mImputado.value = Imputado.toFixed(2);
    mImputadoPorcentaje.value = ImputadoPorcentaje.toFixed(2);
    mImporte.value = importe.toFixed(2);
    mdlDisponible.value = mpagado.value;
};  

function validarFormularioModal() {
    
    var btn = document.getElementById('btnGuardar');
    var mId = document.getElementById('mdlId').value;
    var mComprobanteId = document.getElementById('mdlComprobanteId').value;
    var mProductoId = document.getElementById('mdlProductoId').value;
    var mProductoCodigo = document.getElementById('mdlProductoCodigo').value;
    var mProductoNombre = document.getElementById('mdlProductoNombre').value;
    var mPrecio = document.getElementById('mdlPrecio').value;
    var mImputado = document.getElementById('mdlImputado').value;
    var mImputadoPorcentaje = document.getElementById('mdlImputadoPorcentaje').value;
    var mImporte = document.getElementById('mdlImporte').value;
    var pagado = document.getElementById('pagado').value;

    btn.disabled = true;

    var esOk = true;
    $("#mId").text('');
    $("#mComprobanteId").text('');
    $("#mProductoId").text('');
    $("#mProductoCodigo").text('');
    $("#mProductoNombre").text('');
    $("#mPrecio").text('');
    $("#mImputado").text('');
    $("#mImputadoPorcentaje").text('');
    $("#mImporte").text('');
    
    //campo obligatorio
    if (mPrecio == null || mPrecio.length == 0 || /^\s+$/.test(mPrecio)) {
        $("#mPrecio").text('Debes ingresar un Importe valido');
        //return false;
        esOk = false;
    }

    if (Number(mPrecio) <= 0) {
        $("#mPrecio").text('El Importe debe ser mayor a Cero');
        //return false;
        esOk = false;
    }
    //---------------------------------

    //campo obligatorio
    if (mImporte == null || mImporte.length == 0 || /^\s+$/.test(mImporte)) {
        $("#mImporte").text('Debes ingresar un Importe valido');
        //return false;
        esOk = false;
    }

    if (Number(mImporte) <= 0) {
        $("#mImporte").text('El Importe debe ser mayor a Cero');
        //return false;
        esOk = false;
    }
    //---------------------------------
    if (pagado - mImporte < 0 ) {
        $("#mImporte").text('El Importe ingresado excede el monto Pagado');
        //return false;
        esOk = false;
    }
    //---------------------------------pagado
    if (mPrecio - mImporte - mImputado < 0 ) {
        $("#mImporte").text('El Importe ingresado excede el precio del producto');
        //return false;
        esOk = false;
    }
        
    //---------------------------------
    if (esOk == false) {
        btn.disabled = false;
        return false;
    }
    //---------------------------------

    guardarFormularioModal(mId, mComprobanteId, mProductoId, mProductoCodigo, mProductoNombre, mPrecio, mImputado, mImputadoPorcentaje, mImporte);

};

function guardarFormularioModal(mId, mComprobanteId, mProductoId, mProductoCodigo, mProductoNombre, mPrecio, mImputado, mImputadoPorcentaje, mImporte)
{
    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/ImputaProducto',
        dataType: 'json',
        data: {
            Id: mId,
            ComprobanteId: mComprobanteId,
            ProductoId: mProductoId,
            ProductoCodigo: mProductoCodigo,
            ProductoNombre: mProductoNombre,
            Precio: mPrecio,
            Imputado: mImputado,
            ImputadoPorcentaje: mImputadoPorcentaje,
            Importe: mImporte

        },
        success: function (data) {
            //$.each(data, function (i, item) {
            //__doPostBack();
            //})
            //CierraModal();
            document.getElementById("myForm").submit();

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
}

function cancelarFormularioModal(mId,mComprobanteId,mProductoId,mProductoCodigo,mProductoNombre,mPrecio,mCantidad,mImputado,mImputadoPorcentaje) {
    var mImporte = -1 * mImputado;

    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/ImputaProducto',
        dataType: 'json',
        data: {
            Id: mId,
            ComprobanteId: mComprobanteId,
            ProductoId: mProductoId,
            ProductoCodigo: mProductoCodigo,
            ProductoNombre: mProductoNombre,
            Precio: mPrecio,
            Imputado: mImputado,
            ImputadoPorcentaje: mImputadoPorcentaje,
            Importe: mImporte

        },
        success: function (data) {
            document.getElementById("myForm").submit();

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
}

function entregarFormularioModal(mId, mComprobanteId, mProductoId, mProductoCodigo, mProductoNombre, mPrecio, mCantidad, mImputado, mImputadoPorcentaje) {
    var mImporte = mImputado;

    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/EntregaProducto',
        dataType: 'json',
        data: {
            Id: mId,
            ComprobanteId: mComprobanteId,
            ProductoId: mProductoId,
            ProductoCodigo: mProductoCodigo,
            ProductoNombre: mProductoNombre,
            Precio: mPrecio,
            Imputado: mImputado,
            ImputadoPorcentaje: mImputadoPorcentaje,
            Importe: mImporte

        },
        success: function (data) {
            document.getElementById("myForm").submit();

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
}

function entregarAnulaFormularioModal(mId, mComprobanteId, mProductoId, mProductoCodigo, mProductoNombre, mPrecio, mCantidad, mImputado, mImputadoPorcentaje) {
    var mImporte = mImputado;

    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/EntregaAnulaProducto',
        dataType: 'json',
        data: {
            Id: mId,
            ComprobanteId: mComprobanteId,
            ProductoId: mProductoId,
            ProductoCodigo: mProductoCodigo,
            ProductoNombre: mProductoNombre,
            Precio: mPrecio,
            Imputado: mImputado,
            ImputadoPorcentaje: mImputadoPorcentaje,
            Importe: mImporte

        },
        success: function (data) {
            document.getElementById("myForm").submit();

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
}

function autorizaFormularioModal(mId) {
    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/AutorizaProducto',
        dataType: 'json',
        data: {
            Id: mId
        },
        success: function (data) {
            document.getElementById("myForm").submit();

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
}

function autorizaAnulaFormularioModal(mId) {
    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/AutorizaAnulaProducto',
        dataType: 'json',
        data: {
            Id: mId
        },
        success: function (data) {
            document.getElementById("myForm").submit();

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
}



function FiltrarDecimal(evt, input) {
    // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
    var key = window.Event ? evt.which : evt.keyCode;
    var chark = String.fromCharCode(key);
    var tempValue = input.value + chark;
    if (key >= 48 && key <= 57) {
        if (filter(tempValue) === false) {
            return false;
        } else {
            return true;
        }
    } else {
        if (key == 8 || key == 13 || key == 0) {
            return true;
        } else if (key == 46) {
            if (filter(tempValue) === false) {
                return false;
            } else {
                return true;
            }
        } else {
            return false;
        }
    }
}

function filter(__val__) {
    var preg = /^([0-9]+\.?[0-9]{0,2})$/;
    if (preg.test(__val__) === true) {
        return true;
    } else {
        return false;
    }

};

function CierraModal() {
    $("#modal-default").modal('hide');//ocultamos el modal
    $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
    $('.modal-backdrop').remove();//eliminamos el backdrop del modal;
}