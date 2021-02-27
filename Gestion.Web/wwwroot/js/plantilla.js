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
    var mdlSaldo = document.getElementById('mdlSaldo');
    
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
    mPrecio.value = Precio.toFixed(2);    
    mImputado.value = Imputado.toFixed(2);
    mImputadoPorcentaje.value = ImputadoPorcentaje.toFixed(2);
    mImporte.value = importe.toFixed(2);
    mdlDisponible.value = mpagado.value;

    mdlSaldo.value = (Precio.toFixed(2) - Imputado.toFixed(2)).toFixed(2);
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
            Precio: mImporte,
            Imputado: mImputado,
            ImputadoPorcentaje: mImputadoPorcentaje,
            Importe: mImporte

        },
        success: function (data) {
            //$.each(data, function (i, item) {
            //__doPostBack();
            //})
            //CierraModal();
            

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
    document.getElementById("myForm").submit();
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
            Precio: -mImputado,
            Imputado: mImputado,
            ImputadoPorcentaje: mImputadoPorcentaje,
            Importe: mImporte

        },
        success: function (data) {
            

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
    document.getElementById("myForm").submit();
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
            

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
    document.getElementById("myForm").submit();
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
        },
        error: function (ex) {
            alert('Fallo');
        }              
    });
    document.getElementById("myForm").submit();
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
            

        },
        error: function (ex) {
            alert('Fallo');
        }
    });

    document.getElementById("myForm").submit();
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
            

        },
        error: function (ex) {
            alert('Fallo');
        }
    });
    document.getElementById("myForm").submit();
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

function FiltrarEnteros(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
};

function CierraModal() {
    $("#modal-default").modal('hide');//ocultamos el modal
    $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
    $('.modal-backdrop').remove();//eliminamos el backdrop del modal;
}


$(document).ready(function () {

    $('#dTableP').DataTable({
        "processing": "true",
        "retrieve": "true",
        "pageLength": 50,
        "language": {
            "processing": "Procesando... Espere por favor",
            "lengthMenu": "Mostrar _MENU_ registros por pagina",
            "zeroRecords": "Nada encontrado",
            "info": "Mostrando página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros disponibles",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sSearch": "Buscar:",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            }
        }
    });
});
        


function cargarDatosFiscales(Id, TipoComprobanteFiscal,LetraFiscal,PtoVentaFiscal,NumeroFiscal) 
{
    var mId = document.getElementById('mdlId');
    var mTipoComprobanteFiscal = document.getElementById('mdlTipoComprobanteFiscal');
    var mLetraFiscal = document.getElementById('mdlLetraFiscal');
    var mPtoVentaFiscal = document.getElementById('mdlPtoVentaFiscal');
    var mNumeroFiscal = document.getElementById('mdlNumeroFiscal');
    
    $("#mId").text('');
    $("#mTipoComprobanteFiscal").text('');
    $("#mLetraFiscal").text('');
    $("#mPtoVentaFiscal").text('');
    $("#mNumeroFiscal").text('');
    
    mId.value = Id;
    mTipoComprobanteFiscal.value = TipoComprobanteFiscal;
    mLetraFiscal.value = LetraFiscal;
    mPtoVentaFiscal.value = PtoVentaFiscal;
    mNumeroFiscal.value = NumeroFiscal;    
}; 

function validarFiscalModal() {

    //var btn = document.getElementById('btnGuardar');
    var mId = document.getElementById('mdlId').value;
    var mTipoComprobanteFiscal = document.getElementById('mdlTipoComprobanteFiscal').value;
    var mLetraFiscal = document.getElementById('mdlLetraFiscal').value;
    var mPtoVentaFiscal = document.getElementById('mdlPtoVentaFiscal').value;
    var mNumeroFiscal = document.getElementById('mdlNumeroFiscal').value;

    //btn.disabled = true;

    var esOk = true;
    $("#mId").text('');
    $("#mTipoComprobanteFiscal").text('');
    $("#mLetraFiscal").text('');
    $("#mPtoVentaFiscal").text('');
    $("#mNumeroFiscal").text('');


    //campo obligatorio
    if (mTipoComprobanteFiscal == null || mTipoComprobanteFiscal.length == 0 || /^\s+$/.test(mTipoComprobanteFiscal)) {
        $("#mTipoComprobanteFiscal").text('Debes ingresar un Tipo de Comprobante valido');
        //return false;
        esOk = false;
    }

    if (mTipoComprobanteFiscal.toUpperCase() != "FA" && mTipoComprobanteFiscal.toUpperCase() != "ND" && mTipoComprobanteFiscal.toUpperCase() != "NC" && mTipoComprobanteFiscal.toUpperCase() != "RE") {
        $("#mTipoComprobanteFiscal").text('Debes ingresar un Tipo de Comprobante valido');
        //return false;
        esOk = false;
    }

    //campo obligatorio
    if (mLetraFiscal == null || mLetraFiscal.length == 0 || /^\s+$/.test(mLetraFiscal)) {
        $("#mLetraFiscal").text('Debes ingresar una Letra valida');
        //return false;
        esOk = false;
    }

    if (mLetraFiscal.toUpperCase() != "X" && mLetraFiscal.toUpperCase() != "A" && mLetraFiscal.toUpperCase() != "B" && mLetraFiscal.toUpperCase()  != "C") {
        $("#mLetraFiscal").text('Debes ingresar una Letra valida');
        //return false;
        esOk = false;
    }

    //campo obligatorio
    if (mPtoVentaFiscal == null || mPtoVentaFiscal.length == 0 || /^\s+$/.test(mPtoVentaFiscal)) {
        $("#mPtoVentaFiscal").text('Debes ingresar un Pto de Venta valido');
        //return false;
        esOk = false;
    }

    if (Number(mPtoVentaFiscal) <= 0) {
        $("#mPtoVentaFiscal").text('El Pto de Venta debe ser mayor a Cero');
        //return false;
        esOk = false;
    }

    //campo obligatorio
    if (mNumeroFiscal == null || mNumeroFiscal.length == 0 || /^\s+$/.test(mNumeroFiscal)) {
        $("#mNumeroFiscal").text('Debes ingresar un Numero valido');
        //return false;
        esOk = false;
    }

    if (Number(mNumeroFiscal) <= 0) {
        $("#mNumeroFiscal").text('El Numero debe ser mayor a Cero');
        //return false;
        esOk = false;
    }
    //---------------------------------

    
    //---------------------------------
    if (esOk == false) {
        //btn.disabled = false;
        return false;
    }
    //---------------------------------

    guardarFiscalModal(mId, mTipoComprobanteFiscal.toUpperCase(), mLetraFiscal.toUpperCase(), mPtoVentaFiscal, mNumeroFiscal);

};

function guardarFiscalModal(mId, mTipoComprobanteFiscal, mLetraFiscal, mPtoVentaFiscal, mNumeroFiscal) {
    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/InsertaDatosFiscales',
        dataType: 'json',
        data: {
            Id: mId,
            TipoComprobanteFiscal: mTipoComprobanteFiscal,
            LetraFiscal: mLetraFiscal,
            PtoVentaFiscal: mPtoVentaFiscal,
            NumeroFiscal: mNumeroFiscal
        },
        success: function (data) {
            
        },
        error: function (ex) {
            alert('Fallo');
        }
    });
    document.getElementById("CCForm").submit();
}


function CargarcodigoAutorizacion(Id, CodigoAutorizacion) {
    var mId = document.getElementById('mdlId');
    var mCodigoAutorizacion = document.getElementById('mdlCodigoAutorizacion');
    
    $("#mId").text('');
    $("#mCodigoAutorizacion").text('');
    
    mId.value = Id;
    mCodigoAutorizacion.value = CodigoAutorizacion;    
};  

function validarCodigoAutorizacion() {

    //var btn = document.getElementById('btnGuardar');
    var mId = document.getElementById('mdlId').value;
    var mCodigoAutorizacion = document.getElementById('mdlCodigoAutorizacion').value;
    
    var esOk = true;
    $("#mId").text('');
    $("#mCodigoAutorizacion").text('');
    
    //campo obligatorio
    if (mCodigoAutorizacion == null || mCodigoAutorizacion.length == 0 || /^\s+$/.test(mCodigoAutorizacion)) {
        $("#mCodigoAutorizacion").text('Debes ingresar una Codigo de Autorizacion');
        //return false;
        esOk = false;
    }

    //---------------------------------
    if (esOk == false) {
        return false;
    }
    //---------------------------------

    guardarCodigoAutorizacionModal(mId, mCodigoAutorizacion.toUpperCase());

};


function guardarCodigoAutorizacionModal(mId, mCodigoAutorizacion) {
    $.ajax({
        async: false,
        type: 'POST',
        url: '/Comprobantes/InsertaCodigoAutorizacion',
        dataType: 'json',
        data: {
            Id: mId,
            CodigoAutorizacion: mCodigoAutorizacion
        },
        success: function (data) {},
        error: function (ex) {
            alert('Fallo');
        }        
    });

    document.getElementById("CCForm").submit();
}